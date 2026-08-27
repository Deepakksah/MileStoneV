-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_archive_storage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_archive_storage]
GO

CREATE PROCEDURE [update_archive_storage]
	@pkid [GuidType],
	@name [NameType],
	@description NVARCHAR(MAX),
	@retain_minutes INT,
	@max_size_mb BIGINT,
	@target_framerate FLOAT,
	@framerate_reduction_enabled BIT,
	@schedule XML,
	@session_id [GuidType],
	@custom_properties XML = NULL
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
	
		DECLARE @retain_minutes_low_limit AS INT
		DECLARE @retain_minutes_high_limit AS INT

		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		-- Ensure that retain_minutes is not <= RecordingStorage.retain_minutes
		SELECT @retain_minutes_low_limit = r.[retain_minutes],
			   @recordingstorage_id = r.[pkid]
		FROM [RecordingStorage] r WITH(UPDLOCK) INNER JOIN [ArchiveStorage] a ON a.[recordingstorage_id] = r.[pkid]
		WHERE a.[pkid] = @pkid
		
		IF (@retain_minutes <= @retain_minutes_low_limit)
		BEGIN
			;THROW 60150, @procedureName, 1;
		END
	
		-- Extract info for logging
		SELECT @recordingserver_name = r.[Name],
			   @recordingserver_id = rs.[recordingserver_id],
			   @recordingstorage_name = rs.[Name],
			   @recorder_service_id = rssr.[recording_server_service_id]
		FROM [RecordingStorage] rs
		INNER JOIN [Recorders] r ON rs.[recordingserver_id] = r.[IDRecorder]
		INNER JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recorder_id] = r.[IDRecorder] AND rssr.[type] = 0
		WHERE rs.[pkid] = @recordingstorage_id
	
		-- Ensure that retain_minutes is not being changed to a value smaller than any archive storage retain_minutes
		-- 'before' this archive storeage in the archiving chain
		SELECT @retain_minutes_low_limit = MAX(a.[retain_minutes])
		FROM [ArchiveStorage] a WITH(UPDLOCK)
		WHERE 
			a.[pkid] != @pkid
			AND a.[recordingstorage_id] = (SELECT [recordingstorage_id] FROM [ArchiveStorage] WHERE [pkid] = @pkid)
			AND a.[retain_minutes] < (SELECT [retain_minutes] FROM [ArchiveStorage] WHERE [pkid] = @pkid)
		
		IF (@retain_minutes <= @retain_minutes_low_limit) -- if @retain_minutes_low_limit is null then condition is false and there is no problem
		BEGIN
			;THROW 60148, @procedureName, 1;
		END
		
		-- Ensure that retain_minutes is not being changed to a value larger than any archive storage retain_minutes
		-- 'after' this archive storeage in the archiving chain
		SELECT @retain_minutes_high_limit = MIN(a.[retain_minutes])
		FROM [ArchiveStorage] a WITH(UPDLOCK)
		WHERE 
			a.[pkid] != @pkid
			AND a.[recordingstorage_id] = (SELECT [recordingstorage_id] FROM [ArchiveStorage] WHERE [pkid] = @pkid)
			AND a.[retain_minutes] > (SELECT [retain_minutes] FROM [ArchiveStorage] WHERE [pkid] = @pkid)
	
		IF (@retain_minutes >= @retain_minutes_high_limit) -- if @retain_minutes_high_limit is null then condition is false and there is no problem
		BEGIN
			;THROW 60149, @procedureName, 1;
		END

		-- Get DataSet before changes
		set @dataSetBefore = dbo.archiveStorageAsDataSet(@pkid)

		-- Make sure the archive storage exists
		--IF NOT EXISTS (
		--	SELECT [IDArchiveDatabase]
		--	FROM [ArchiveDatabases] WITH(UPDLOCK)
		--	WHERE [IDArchiveDatabase] = @IDArchiveStorage)
		--BEGIN
		--	ROLLBACK TRAN
		--	DECLARE @IDStorage [GuidType]
		--	SET @IDStorage = @IDArchiveStorage
		--	;THROW 60051, @procedureName, 1;
		--END
		BEGIN TRY
			UPDATE [ArchiveStorage]
			SET
				[name] = @name,
				[description] = @description,
				[retain_minutes] = @retain_minutes,
				[max_size_mb] = @max_size_mb,
				[target_framerate] = @target_framerate,
				[framerate_reduction_enabled] = @framerate_reduction_enabled,
				[schedule] = @schedule,
				[last_modified] = @CurrentUtcTime
			WHERE [pkid] = @pkid

			SELECT @rowCount = @@ROWCOUNT
		END TRY
		BEGIN CATCH
			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
			BEGIN
				-- The name already exists on the recorder

				-- Lets find the recording storage name for the error message
				DECLARE @recordingStorageName [NameType]

				SELECT @recordingStorageName = rs.[name]
				FROM [RecordingStorage] rs
				INNER JOIN [ArchiveStorage] arch ON arch.[recordingstorage_id] = rs.[pkid]
				WHERE arch.[pkid] = @pkid

				SET @ErrorArgs = @procedureName + N': %%Name=' + @Name + N'%%; %%recordingServerName=' + @recordingStorageName + N'%%'
				;THROW 60050, @ErrorArgs, 1;
			END
			;THROW
		END CATCH

		IF(@rowCount = 0)
		BEGIN
			;THROW 60051, @procedureName, 1;
		END

		-- Update custom properties
		EXEC [update_custom_properties_transaction]
			@object_id = @pkid,
			@custom_properties = @custom_properties

		-- Get DataSet after changes
		set @dataSetAfter = dbo.archiveStorageAsDataSet(@pkid)

		-- Get recording server id for task creation
		SELECT @recordingserver_id = rs.[recordingserver_id]
		FROM [RecordingStorage] rs INNER JOIN [ArchiveStorage] a ON a.[recordingstorage_id] = rs.[pkid]
		WHERE a.[pkid] = @pkid

		-- Call the create task procedure to create diffgram update task
		EXEC [create_task_transaction] 
							'diffgram_update', 
							@recorder_service_id, 
							@dataSetBefore, 
							@dataSetAfter, 
							@CurrentUtcTime
	
		EXEC create_pending_log @session_id, 20006, @recordingstorage_id, @recordingstorage_name, @recordingserver_id, @recordingserver_name, @name

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
