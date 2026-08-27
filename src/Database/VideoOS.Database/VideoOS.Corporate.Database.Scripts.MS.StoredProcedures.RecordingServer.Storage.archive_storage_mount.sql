-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[archive_storage_mount]') AND [type] = 'P')
DROP PROCEDURE [archive_storage_mount]
GO

CREATE PROCEDURE [archive_storage_mount]
	@pkid [GuidType],
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
	
		DECLARE @recorder_service_id [GuidType]
		DECLARE	@recording_server_id [GuidType]
		DECLARE @recording_server_name [NameType]
		DECLARE @recording_storage_id [GuidType]
		DECLARE @recording_storage_name [NameType]
		DECLARE @archive_name [NameType]
		DECLARE @archive_path NVARCHAR(512)
		DECLARE @archive_folder [GuidType]
		DECLARE @archive_bank_id UNIQUEIDENTIFIER
		DECLARE @archive_mounted BIT

		SELECT 
			@recording_server_id = r.[recordingserver_id],
			@recording_storage_id = r.[pkid],
			@recording_storage_name = r.[name],
			@archive_name = a.[name],
			@archive_path = a.[path],
			@archive_folder = a.[folder],
			@archive_bank_id = a.[bank_id],
			@archive_mounted = a.[mounted]
		FROM
			[ArchiveStorage] a WITH(UPDLOCK) INNER JOIN [RecordingStorage] r WITH(UPDLOCK) ON a.[recordingstorage_id] = r.[pkid]
		WHERE
			a.[pkid] = @pkid

		IF @archive_name IS NULL 
		BEGIN
			;THROW 60408, @procedureName, 1; -- Error like: Could not mount the archive. The archive does not exist.
		END


		-- Extract info for logging
		SELECT 
			@recording_server_name = r.[Name],
			@recorder_service_id = rssr.[recording_server_service_id]
		FROM 
			[Recorders] r
			INNER JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recorder_id] = r.[IDRecorder] AND rssr.[type] = 0
		WHERE
			[IDRecorder] = @recording_server_id
	
		IF(@recorder_service_id IS NULL)
		BEGIN
			;THROW 60408, @procedureName, 1; -- Error like: Could not mount the archive. The archive does not exist.
		END

		-- Check if archive is already mounted
		IF (@archive_mounted = 1)
		BEGIN
			;THROW 60397, @procedureName, 1; -- Error like: Could not mount the archive. The archive is already mounted.
		END


		-- Validate if a new Database.MountArchive task can be created:
		-- No other database tasks on storage with same storage id.
		-- Exceptions: None
		IF EXISTS (	SELECT 1
					FROM   [TaskProperties] tp
					WHERE  [TaskPropertyName] = 'storage_id' AND
					       [TaskPropertyValue] = @recording_storage_id
				  )
		BEGIN
			;THROW 60396, @procedureName, 1; -- Error like: Could not mount the archive. A conflicting task already exists.
		END

		-- Get DataSet before changes
		SET @dataSetBefore = dbo.archiveStorageAsDataSet(@pkid)
	
		UPDATE [ArchiveStorage]
		SET    [mounted] = 1,
		       [last_modified] = @CurrentUtcTime
		WHERE  [pkid]= @pkid
	
		-- Get DataSet after changes
		SET @dataSetAfter = dbo.archiveStorageAsDataSet(@pkid)

		-- Call the create task procedure to create diffgram update task
		EXEC [create_task_transaction]
							'diffgram_update',
							@recorder_service_id,
							@dataSetBefore,
							@dataSetAfter,
							@CurrentUtcTime


		DECLARE @task_type NVARCHAR(200) = 'Database.MountArchive'
		
		-- First create the new task row - with default task data
		INSERT	[Task] (
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

		DECLARE @task_id BIGINT = @@IDENTITY

		-- Now we update the task data with correct values
		DECLARE @data_a_xml AS XML
		SET @data_a_xml = '<task/>'
	
		SET @data_a_xml.modify('insert attribute type {sql:variable("@task_type")} as first into (/task)[1]')
		SET @data_a_xml.modify('insert attribute id {sql:variable("@task_id")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element target {sql:variable("@recorder_service_id")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element idRecorder {sql:variable("@recording_server_id")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element recorderName {sql:variable("@recording_server_name")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element idStorage {sql:variable("@recording_storage_id")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element storageName {sql:variable("@recording_storage_name")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element idArchive {sql:variable("@pkid")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element archiveName {sql:variable("@archive_name")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element archivePath {sql:variable("@archive_path")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element archiveFolder {sql:variable("@archive_folder")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element archiveBankId {sql:variable("@archive_bank_id")} as last into (/task)[1]')

		-- Update the task xml in the table
		UPDATE [Task]
		SET [data_a] = @data_a_xml
		WHERE [pkid] = @task_id

		EXEC create_pending_log @session_id, 20009, @pkid, @recording_storage_name, @recording_server_id, @recording_server_name, @archive_name

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
