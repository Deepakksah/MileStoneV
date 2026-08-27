-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dataset_device_data_distribution_delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dataset_device_data_distribution_delete]
GO

CREATE PROCEDURE [dataset_device_data_distribution_delete]
	@Original_device_id [GuidType],
	@Original_recorder_id [GuidType],
	@Original_track_id [UNIQUEIDENTIFIER]
AS
BEGIN
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	DECLARE @CurrentUtcTime DATETIME
	SET @CurrentUtcTime = GETUTCDATE()

	DECLARE @DeviceIsNowOnThisRecorder BIT

	-- Dataset insert or updates are handled before delete, so hardware changes are committed at this point (move hw issue)
	IF EXISTS(
		SELECT h.IDRecorder
		FROM Devices d
		INNER JOIN Hardware h ON d.IDHardware = h.IDHardware
		WHERE (d.IDDevice = @Original_device_id AND h.IDRecorder = @Original_recorder_id)
	)
		SET @DeviceIsNowOnThisRecorder = 1
	ELSE
		SET @DeviceIsNowOnThisRecorder = 0

	
	IF(@DeviceIsNowOnThisRecorder = 1)
	BEGIN
		BEGIN TRY
			-- Only delete the actual device data distribution
			DELETE FROM [DeviceDataDistribution]
			WHERE       (([device_id] = @Original_device_id) AND ([recorder_id] = @Original_recorder_id) AND ([track_id] = @Original_track_id))
		END TRY
		BEGIN CATCH
			;THROW 60000, @procedureName, 1;
		END CATCH
	END
	ELSE
	BEGIN
		-- Handle deletion of DeviceTrackInfo
		DECLARE @deviceTrackInfoBefore XML
		SET @deviceTrackInfoBefore = dbo.[deviceTrackInfoAsDataSet](@Original_recorder_id, @Original_track_id)

		BEGIN TRY
			-- Create diffgram task for all services for recorder
			EXEC [create_task_recorder_services_transaction]
				@type = 'diffgram_delete',
				@data_a = @deviceTrackInfoBefore,
				@current_time = @CurrentUtcTime,
				@recorder_id = @Original_recorder_id
		END TRY
		BEGIN CATCH
			;THROW
		END CATCH

		-- Handle Deletion of DeviceTrackEvidenceLocks 
		DECLARE @deviceEvidenceLockBefore XML
		SET @deviceEvidenceLockBefore = dbo.[deviceTrackEvidenceLockAsDataSet](@Original_recorder_id, @Original_track_id, null, null)

		IF (@deviceEvidenceLockBefore IS NOT NULL)
		BEGIN
			BEGIN TRY
				-- Create diffgram task for all services for recorder
				EXEC [create_task_recorder_services_transaction]
					@type = 'diffgram_delete',
					@data_a = @deviceEvidenceLockBefore,	
					@current_time = @CurrentUtcTime,
					@recorder_id = @Original_recorder_id
			END TRY
			BEGIN CATCH
				;THROW
			END CATCH
		END

		BEGIN TRY
			-- Now we delete the actual device data distribution
			DELETE FROM [DeviceDataDistribution]
			WHERE       (([device_id] = @Original_device_id) AND ([recorder_id] = @Original_recorder_id) AND ([track_id] = @Original_track_id))
		END TRY
		BEGIN CATCH
			;THROW 60000, @procedureName, 1;
		END CATCH
	END
END
GO