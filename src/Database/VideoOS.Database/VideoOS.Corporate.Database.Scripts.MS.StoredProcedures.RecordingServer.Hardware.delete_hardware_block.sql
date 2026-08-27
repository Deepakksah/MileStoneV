-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_hardware_block]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_hardware_block]
GO

CREATE PROCEDURE [delete_hardware_block]
	@pkid INT
AS
BEGIN
	SET NOCOUNT ON

		-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @rowCount INT

		DECLARE @dataSetBefore XML

		DECLARE @hardware_id [GuidType]
		DECLARE @recorder_service_id [GuidType]

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		BEGIN TRAN
	
		-- Get hardware id so that we later can send find out which recorder(s) to send diffgram to
		SELECT @hardware_id = [hardware_id]
		FROM [HardwareBlock] WITH(UPDLOCK)
		WHERE [pkid] = @pkid

		SELECT @rowCount = @@ROWCOUNT
		IF(@rowCount = 0)
		BEGIN
			;THROW 60246, @procedureName, 1;
		END

		-- Get DataSet before changes
		SET @dataSetBefore = dbo.hardwareBlockAsDataSet(@pkid)

		-- Delete the entry
		DELETE [HardwareBlock] WHERE [pkid] = @pkid

		-- Get recording server service ids for task creation
		DECLARE recorder_cursor CURSOR FOR
			SELECT rssr.[recording_server_service_id]
			FROM [RecordingServerServiceRecorder] rssr INNER JOIN [Hardware] hw ON hw.[IDRecorder] = rssr.recorder_id
			WHERE hw.[IDHardware] = @hardware_id AND rssr.[type] IN (0, 2) -- RECORDER or HOT_STANDBY

		OPEN recorder_cursor
		BEGIN TRY
			FETCH NEXT FROM recorder_cursor
			INTO @recorder_service_id

			WHILE(@@FETCH_STATUS = 0)
			BEGIN
				-- Call the create task procedure to create diffgram update task
				EXEC [create_task_transaction]
					@type = 'diffgram_delete',
					@target = @recorder_service_id,
					@data_a = @dataSetBefore,
					@current_time = @CurrentUtcTime

				FETCH NEXT FROM recorder_cursor
				INTO @recorder_service_id
			END
		END TRY
		BEGIN CATCH
			CLOSE recorder_cursor
			DEALLOCATE recorder_cursor
			;THROW
		END CATCH

		CLOSE recorder_cursor
		DEALLOCATE recorder_cursor

		COMMIT TRAN

		SELECT @pkid
	END TRY
	BEGIN CATCH 
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION
		END;
		THROW; -- Used from ServerService and IIS timer, where Error 60000 is NOT expected
	END CATCH;
END
GO