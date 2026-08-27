-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_device_data_distribution]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_device_data_distribution]
GO

CREATE PROCEDURE [delete_device_data_distribution]
	@recorder_id AS [GuidType],
	@track_id AS [UNIQUEIDENTIFIER]
 AS
BEGIN
	SET NOCOUNT ON

		-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		BEGIN TRAN

		IF EXISTS(SELECT * FROM [DeviceDataDistribution] WHERE [recorder_id] = @recorder_id AND [track_id] = @track_id)
		BEGIN
			-- Query device id based on track id
			DECLARE @device_id [GuidType]
			SELECT	@device_id = [device_id]
			FROM	[DeviceDataDistribution]
			WHERE	[track_id] = @track_id

			-- First create diffgram xml for delete difgram
			DECLARE @dataSetBefore XML
			SET @dataSetBefore = dbo.deviceDataDistributionAsDataSet(@recorder_id, @track_id)

			-- Query recorder id to send diffgram to
			DECLARE @CurrentRecorderId GuidType
			SELECT @CurrentRecorderId = h.IDRecorder FROM Hardware h
				INNER JOIN Devices d on h.IDHardware = d.IDHardware
			WHERE d.IDDevice = @device_id

			-- Create diffgram task for all services for recorder
			EXEC [create_task_recorder_services_transaction]
							@type = 'diffgram_delete',
							@data_a = @dataSetBefore,
							@current_time = @CurrentUtcTime,
							@recorder_id = @CurrentRecorderId
		
			-- Handle deletion of DeviceTrackInfo

			DECLARE @deviceTrackInfoBefore XML
			SET @deviceTrackInfoBefore = dbo.[deviceTrackInfoAsDataSet](@recorder_id, @track_id)

			-- Create diffgram task for all services for recorder
			EXEC [create_task_recorder_services_transaction]
							@type = 'diffgram_delete',
							@data_a = @deviceTrackInfoBefore,
							@current_time = @CurrentUtcTime,
							@recorder_id = @recorder_id
			
			-- Handle Deletion of DeviceTrackEvidenceLocks 

			DECLARE @deviceEvidenceLockBefore XML
			SET @deviceEvidenceLockBefore = dbo.[deviceTrackEvidenceLockAsDataSet](@recorder_id, @track_id, null, null)
	
			IF (@deviceEvidenceLockBefore IS NOT NULL)
			BEGIN
				-- Create diffgram task for all services for recorder
				EXEC [create_task_recorder_services_transaction]
								@type = 'diffgram_delete',
								@data_a = @deviceEvidenceLockBefore,	
								@current_time = @CurrentUtcTime,
								@recorder_id = @recorder_id
			END

			-- Now we delete the actual device data distribution
			DELETE FROM [DeviceDataDistribution]
			WHERE       (([device_id] = @device_id) AND ([recorder_id] = @recorder_id) AND ([track_id] = @track_id))
		END

		COMMIT TRAN
		RETURN 0;

	END TRY
	BEGIN CATCH 
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION
		END;
		DECLARE @shouldReThrowError BIT = dbo.[ShouldReThrowError](ERROR_NUMBER(), ERROR_SEVERITY())
		IF(@shouldReThrowError = 1)
			THROW;
		SET @ErrorArgs = dbo.[FormatErrorMessage](ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE())
		;THROW 60000, @ErrorArgs, 1;
    END CATCH;
END
GO
