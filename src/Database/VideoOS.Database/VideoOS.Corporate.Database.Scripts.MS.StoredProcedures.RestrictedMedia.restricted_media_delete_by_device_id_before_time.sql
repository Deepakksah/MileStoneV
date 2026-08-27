-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM   sys.objects WHERE  object_id = Object_id(N'[restricted_media_delete_by_device_id_before_time]') AND    type = 'P')
DROP PROCEDURE [restricted_media_delete_by_device_id_before_time]
GO

CREATE PROCEDURE [dbo].[restricted_media_delete_by_device_id_before_time]
	@IDDevice [GUIDTYPE],
	@BeforeTime [DATETIME2]
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY		
		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME = GETUTCDATE()

		-- Create diffgrams for evidence locks
		DECLARE @recorder_id	[GUIDTYPE]
		DECLARE @rmId			UNIQUEIDENTIFIER 
		DECLARE @dataSetBefore	XML

		DECLARE device_restricted_media_cursor CURSOR FORWARD_ONLY READ_ONLY FOR
			SELECT [IDRestrictedMedia], [IDRecorder] FROM [DeviceRestrictedMedia] WHERE  IDDevice = @IDDevice AND EndTime < @BeforeTime

		OPEN device_restricted_media_cursor
		BEGIN TRY
			WHILE (1 = 1)
			BEGIN
				FETCH device_restricted_media_cursor
				INTO  @rmId, @recorder_id
				IF(@@FETCH_STATUS <> 0)
				BEGIN
					BREAK
				END
				SET @dataSetBefore = dbo.[Devicerestrictedmediaasdataset](@rmId, @IDDevice)

				-- Create diffgram task for all services for recorder
				EXEC [create_task_recorder_services_transaction]
					@type = 'diffgram_delete',
					@data_a = @dataSetBefore,
					@current_time = @CurrentUtcTime,
					@recorder_id = @recorder_id
			END
		END TRY
		BEGIN CATCH
			CLOSE device_restricted_media_cursor
			DEALLOCATE device_restricted_media_cursor
			;THROW
		END CATCH

		CLOSE device_restricted_media_cursor
		DEALLOCATE device_restricted_media_cursor

		-- Start deleting RestrictedMedia and RestrictedMediaDevices
		DELETE		rmd
		FROM		restrictedmediadevice AS rmd
		INNER JOIN	restrictedmedia AS rm ON rm.idrestrictedmedia = rmd.idrestrictedmedia
		WHERE		rmd.iddevice = @IDDevice
		AND			rm.endtime < @BeforeTime

		DELETE rm
		FROM   [RestrictedMedia] AS rm
		WHERE  NOT EXISTS (	SELECT 1
							FROM   restrictedmediadevice AS rmd
							WHERE  rm.idrestrictedmedia = rmd.idrestrictedmedia)
		COMMIT TRAN
	END TRY
	BEGIN CATCH 
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION
		END;
		THROW; -- Used from ServerService, where Error 60000 is NOT expected
	END CATCH
END
GO