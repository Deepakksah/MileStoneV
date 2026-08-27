-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_archive_storage_enable_framerate_reduction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_archive_storage_enable_framerate_reduction]
GO

CREATE PROCEDURE [update_archive_storage_enable_framerate_reduction]
	@pkid [GuidType],
	@enable BIT,
	@session_id [GuidType]
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
		DECLARE @dataSetAfter XML
		DECLARE @recordingstorage_id [GuidType]
		DECLARE @recordingstorage_name [NameType]
		DECLARE @recorder_service_id [GuidType]
		DECLARE @recordingserver_id [GuidType]
		DECLARE @recordingserver_name [NameType]
		DECLARE @archivestorage_name [NameType]

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		BEGIN TRAN

		-- Ensure that associated recording storage exists
		IF NOT EXISTS ( SELECT r.[pkid]
						FROM [RecordingStorage] r WITH(UPDLOCK) INNER JOIN [ArchiveStorage] a ON a.[recordingstorage_id] = r.[pkid]
						WHERE a.[pkid] = @pkid)
		BEGIN
			;THROW 60131, @procedureName, 1;
		END
	
		-- Extract info for logging
		SELECT @recordingstorage_id = r.[pkid], @archivestorage_name = a.[name]
		FROM [RecordingStorage] r INNER JOIN [ArchiveStorage] a ON a.[recordingstorage_id] = r.[pkid]
		WHERE a.[pkid] = @pkid

		SELECT @recordingserver_name = r.[Name],
			   @recordingserver_id = rs.[recordingserver_id],
			   @recordingstorage_name = rs.[Name],
			   @recorder_service_id = rssr.[recording_server_service_id]
		FROM [RecordingStorage] rs
		INNER JOIN [Recorders] r ON rs.[recordingserver_id] = r.[IDRecorder]
		INNER JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recorder_id] = r.[IDRecorder] AND rssr.[type] = 0
		WHERE rs.[pkid] = @recordingstorage_id
	
		-- Get DataSet before changes
		set @dataSetBefore = dbo.archiveStorageAsDataSet(@pkid)

		-- Set framerate_reduction_enabled if storage exist
		UPDATE [ArchiveStorage]
		SET [framerate_reduction_enabled] = @enable,
				[last_modified] = @CurrentUtcTime
		WHERE [pkid] = @pkid

		SELECT @rowCount = @@ROWCOUNT

		-- If storage does not exist return error
		IF(@rowCount = 0)
		BEGIN
			;THROW 60131, @procedureName, 1;
		END

		-- Get DataSet after changes
		set @dataSetAfter = dbo.archiveStorageAsDataSet(@pkid)

		-- Get recording server id for task creation
		SELECT @recordingserver_id = rs.[recordingserver_id] FROM [RecordingStorage] rs
		INNER JOIN [ArchiveStorage] archStor ON rs.[pkid] = archStor.[recordingstorage_id]
		WHERE archStor.[pkid] = @pkid

		-- Call the create task procedure to create diffgram update task
		EXEC [create_task_transaction]
							'diffgram_update',
							@recorder_service_id,
							@dataSetBefore,
							@dataSetAfter,
							@CurrentUtcTime
	
		EXEC create_pending_log @session_id, 20006, @recordingstorage_id, @recordingstorage_name, @recordingserver_id, @recordingserver_name, @archivestorage_name

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
