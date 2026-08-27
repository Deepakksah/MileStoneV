-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_archive_storage_move]') AND type = 'P')
DROP PROCEDURE [update_archive_storage_move]
GO

CREATE PROCEDURE [update_archive_storage_move]
	@pkid [GuidType],
	@target_path NVARCHAR(512),
	@session_id [GuidType]
AS
BEGIN
	SET NOCOUNT ON

		-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @dataSetBefore XML
		DECLARE @dataSetAfter XML

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		BEGIN TRAN
	
		DECLARE @recordingstorage_id [GuidType]
		DECLARE @archive_storage_name [NameType]
		DECLARE @recording_storage_name [NameType]
		DECLARE @archive_mounted BIT
		DECLARE @source_path NVARCHAR(512)
		DECLARE @archive_folder [GuidType]
		DECLARE @source_bank_id UNIQUEIDENTIFIER
		DECLARE @target_bank_id UNIQUEIDENTIFIER
		DECLARE @recordingServerName [NameType]
		DECLARE	@recordingserver_id [GuidType]
		DECLARE @recorder_service_id [GuidType]

		SELECT  
			@recordingserver_id = r.[recordingserver_id],
			@recordingstorage_id = r.[pkid],
			@archive_storage_name = a.[name],
			@recording_storage_name = r.[name],
			@source_path = a.[path],
			@archive_folder = a.[folder],
			@source_bank_id = a.[bank_id],
			@archive_mounted = a.[mounted]
		FROM
			[ArchiveStorage] a WITH(UPDLOCK) 
			INNER JOIN [RecordingStorage] r WITH(UPDLOCK) ON a.[recordingstorage_id] = r.[pkid]
		WHERE
			a.[pkid] = @pkid
	
		-- Extract info for logging
	
		SELECT	@recordingServerName = r.[Name],
				@recorder_service_id = rssr.[recording_server_service_id]
		FROM	[Recorders] r
				INNER JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recorder_id] = r.[IDRecorder] AND rssr.[type] = 0
		WHERE	[IDRecorder] = @recordingserver_id
	
		IF (@recorder_service_id IS NULL)
		BEGIN
			;THROW 60000, @procedureName, 1;
		END

		IF (@archive_mounted = 0)
		BEGIN
			;THROW 60410, @procedureName, 1; -- Error like: Could not move the archive. The archive cannot be moved while it is unmounted.
		END

		
		-- Validate if a new Database.MoveArchive task can be created:
		-- No other database tasks on storage with same storage id.
		-- Exceptions: MoveStorage.
		IF EXISTS (	SELECT 1
					FROM   [TaskProperties] tp
					WHERE  [TaskPropertyName] = 'storage_id' AND
					       [TaskPropertyValue] = @recordingstorage_id AND
					       [TaskType] NOT IN ('Database.MoveStorage')
				  )
		BEGIN
			;THROW 60133, @procedureName, 1;
		END

		-- Get DataSet before changes
		SET @dataSetBefore = dbo.archiveStorageAsDataSet(@pkid)
	
		SET @target_bank_id = NEWID()

		UPDATE	[ArchiveStorage]
		SET		[path] = @target_path,
				[folder] = @pkid,
				[bank_id] = @target_bank_id
		WHERE	[pkid]= @pkid
	
		-- Get DataSet after changes
		SET @dataSetAfter = dbo.archiveStorageAsDataSet(@pkid)

		-- Call the create task procedure to create diffgram update task
		EXEC [create_task_transaction]
							'diffgram_update',
							@recorder_service_id,
							@dataSetBefore,
							@dataSetAfter,
							@CurrentUtcTime


		DECLARE @task_type NVARCHAR(200) = 'Database.MoveArchive'

		-- First create the new task row - with default task data
		INSERT [Task] (
			[type],
			[target],
			[data_a],
			[data_b],
			[percent_completed],
			[status_details], 
			[task_status],
			[start_time],
			[end_time]
		)
		VALUES (
			@task_type,
			@recorder_service_id,
			'',
			NULL,
			0,
			'',
			0,
			@CurrentUtcTime,
			NULL
		)

		DECLARE @task_id BIGINT
		SET @task_id = @@IDENTITY

		-- Now we update the task data with correct values
		DECLARE @data_a_xml AS XML
		SET @data_a_xml = '<task/>'
	
		SET @data_a_xml.modify('insert attribute type {sql:variable("@task_type")} as first into (/task)[1]')
		SET @data_a_xml.modify('insert attribute id {sql:variable("@task_id")} as last into (/task)[1]')

		SET @data_a_xml.modify('insert element target {sql:variable("@recorder_service_id")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element idRecorder {sql:variable("@recordingserver_id")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element idStorage {sql:variable("@recordingstorage_id")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element idArchive {sql:variable("@pkid")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element archiveName {sql:variable("@archive_storage_name")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element pathSource {sql:variable("@source_path")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element folderSource {sql:variable("@archive_folder")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element pathTarget {sql:variable("@target_path")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element folderTarget {sql:variable("@pkid")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element bankIdSource {sql:variable("@source_bank_id")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element bankIdTarget {sql:variable("@target_bank_id")} as last into (/task)[1]')

		-- Update the task xml in the table
		UPDATE	[Task]
		SET		[data_a] = @data_a_xml
		WHERE	[pkid] = @task_id
	

		EXEC create_pending_log @session_id, 20007, @pkid, @recording_storage_name, @recordingserver_id, @recordingServerName, @archive_storage_name, @source_path, @target_path


		SELECT @task_id AS task_id
	
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
