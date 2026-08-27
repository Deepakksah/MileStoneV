-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_recording_storage_set_default]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_recording_storage_set_default]
GO

CREATE PROCEDURE [update_recording_storage_set_default]
	@pkid [GuidType]
 AS
BEGIN
	SET NOCOUNT ON;

		-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @dataSetBefore XML
		DECLARE @dataSetAfter XML
		DECLARE @recorder_service_id [GuidType]

		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		-- Make sure the recording storage exists
		IF NOT EXISTS (
			SELECT [pkid]
			FROM [RecordingStorage] WITH(UPDLOCK)
			WHERE [pkid] = @pkid)
		BEGIN
			;THROW 60100, @procedureName, 1;
		END

		-- Get recording server id
		DECLARE @IDRecordingServer [GuidType]
		SELECT @IDRecordingServer = [recordingserver_id]
		FROM [RecordingStorage]
		WHERE [pkid] = @pkid
	
		-- Get current default storage (there can be only one)
		DECLARE @currentDefault AS [GuidType]
		SELECT @currentDefault = [pkid]
		FROM [RecordingStorage] WITH(UPDLOCK)
		WHERE [is_default] = 1 AND 
			  [recordingserver_id] = @IDRecordingServer

		-- Get service id for task		
		SELECT @recorder_service_id = rssr.[recording_server_service_id]
		FROM [RecordingServerServiceRecorder] rssr
		WHERE rssr.[recorder_id] = @IDRecordingServer AND rssr.[type] = 0

		IF(@recorder_service_id IS NULL)
		BEGIN
			;THROW 60000, @procedureName, 1;
		END

		-- Get DataSet before changes
		SET @dataSetBefore = dbo.recordingStorageAsDataSet2(@currentDefault, @pkid)

		-- Clear current default recording storage
		UPDATE [RecordingStorage]
		SET [is_default] = 0,
			[last_modified] = @CurrentUtcTime
		WHERE
			[pkid] = @currentDefault

		-- Set new recording storage as default
		UPDATE [RecordingStorage]
		SET [is_default] = 1,
			[last_modified] = @CurrentUtcTime
		WHERE [pkid] = @pkid

		-- Get DataSet after changes
		set @dataSetAfter = dbo.recordingStorageAsDataSet2(@currentDefault, @pkid)

		-- Task creation
	
		-- Call the create task procedure to create diffgram update task
		EXEC [create_task_transaction]
								'diffgram_update',
								@recorder_service_id,
								@dataSetBefore,
								@dataSetAfter,
								@CurrentUtcTime

		COMMIT TRAN

		RETURN 0

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
