-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_archive_storage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_archive_storage]
GO

CREATE PROCEDURE [delete_archive_storage]
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
		DECLARE @archive_storage_name [NameType]
		DECLARE @recordingstorage_id [GuidType]
		DECLARE @recordingstorage_name [NameType]
		DECLARE @recordingserver_id [GuidType]
		DECLARE @recordingserver_name [NameType]
		DECLARE @recorder_service_id [GuidType]
		DECLARE @archive_path nvarchar(512)
		DECLARE @archive_folder [GuidType]
		DECLARE @archive_retain_minutes INT

		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		-- Is this a valid archive storage that we can delete?
		SELECT @archive_storage_name = arch.[Name], 
			@recordingstorage_id = arch.[recordingstorage_id], 
			@recordingserver_id = rs.[recordingserver_id],
			@archive_path = arch.[path],
			@archive_folder = arch.[folder],
			@archive_retain_minutes = arch.[retain_minutes],
			@recordingstorage_name = rs.[Name]
		FROM [ArchiveStorage] arch WITH(UPDLOCK)
		INNER JOIN [RecordingStorage] rs WITH(UPDLOCK) ON rs.[pkid] = arch.[recordingstorage_id]
		WHERE arch.[pkid] = @pkid
	
		-- Extract info for logging
		SELECT @recordingserver_name = r.[Name],
			   @recorder_service_id = rssr.[recording_server_service_id]
		FROM [Recorders] r
			INNER JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recorder_id] = r.[IDRecorder] AND rssr.[type] = 0
		WHERE r.[IDRecorder] = @recordingserver_id

		-- Ensure that user is only allowed to delete last archive storage
		IF EXISTS (	SELECT [pkid] 
					FROM [ArchiveStorage] a WITH(UPDLOCK)
					WHERE 
						a.[pkid] != @pkid
						AND a.[recordingstorage_id] = @recordingstorage_id
						AND a.[retain_minutes] > @archive_retain_minutes)
		BEGIN
			;THROW 60151, @procedureName, 1;
		END
	
		-- Does the archive storage exist?
		IF(@archive_storage_name IS NULL)
		BEGIN
			;THROW 60052, @procedureName, 1;
		END


		-- Check if the archive storage being deleted contains evidence locked data
		IF EXISTS (SELECT [pkid] FROM [StorageUsageInformation] WHERE [pkid] = @pkid AND [locked_size] > 0)
		BEGIN
			;THROW 60306, @procedureName, 1;
		END

	
		DECLARE @diffgram XML
		SELECT @diffgram = dbo.[archiveStorageAsDataSet](@pkid)

		-- Create diffgram
		EXEC [create_task_transaction]
			@type = 'diffgram_delete',
			@target = @recorder_service_id,
			@data_a = @diffgram,
			@current_time = @CurrentUtcTime
		


		-- Delete the archive database
		DELETE FROM [ArchiveStorage]
		WHERE [pkid] = @pkid

		-- Validate if a new Database.DeleteArchive task can be created:
		-- No other database tasks on storage with same storage id.
		-- Exceptions: MoveStorage
		IF EXISTS (	SELECT 1
					FROM   [TaskProperties] tp
					WHERE  [TaskPropertyName] = 'storage_id' AND
					       [TaskPropertyValue] = @recordingstorage_id AND
						   [TaskType] NOT IN ('Database.MoveStorage')
				  )
		BEGIN
			;THROW 60135, @procedureName, 1;
		END


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
			'Database.DeleteArchive',
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
	
		DECLARE @task_type NVARCHAR(200)
		SET @task_type = 'Database.DeleteArchive'

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
		SET @data_a_xml.modify('insert element archivePath {sql:variable("@archive_path")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element archiveFolder {sql:variable("@archive_folder")} as last into (/task)[1]')

		-- Update the task xml in the table
		UPDATE [Task]
		SET [data_a] = @data_a_xml
		WHERE [pkid] = @task_id

		EXEC create_pending_log @session_id, 20008, @recordingstorage_id, @recordingstorage_name, @recordingserver_id, @recordingserver_name, @archive_storage_name

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
