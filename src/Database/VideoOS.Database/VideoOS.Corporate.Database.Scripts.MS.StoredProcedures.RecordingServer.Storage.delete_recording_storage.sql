-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_recording_storage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_recording_storage]
GO

CREATE PROCEDURE [delete_recording_storage]
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
		DECLARE @default BIT
		DECLARE @recordingStorageName [NameType]
		DECLARE @recorder_service_id [GuidType]
		DECLARE @recordingserver_id [GuidType]
		DECLARE @recordingserver_name [NameType]
		DECLARE @recordingStoragePath nvarchar(512)

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		BEGIN TRAN

		-- Is this a valid recording storage that we can delete?
		SELECT @default = rs.[is_default], 
			@recordingStorageName = rs.[name],
			@recordingStoragePath = rs.[path],
			@recordingserver_id = rs.[recordingserver_id]
		FROM [RecordingStorage] rs WITH(UPDLOCK)
		WHERE rs.[pkid] = @pkid
	
		-- Extract info for logging
		SELECT @recordingserver_name = r.[Name],
			   @recorder_service_id = rssr.[recording_server_service_id]
		FROM [Recorders] r
		INNER JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recorder_id] = r.[IDRecorder] AND rssr.[type] = 0
		WHERE r.[IDRecorder] = @recordingserver_id

		-- Does the recording storage exist?
		IF(@@ROWCOUNT = 0)
		BEGIN
			;THROW 60028, @procedureName, 1;
		END

		-- Is this the default recording storage
		IF(@default = 1)
		BEGIN
			;THROW 60029, @procedureName, 1;
		END

		-- Is the recording storage used by any devices
		IF EXISTS(SELECT [device_id] FROM [RecordingStorageDevice] WITH(UPDLOCK) WHERE [recordingstorage_id] = @pkid)
		BEGIN
			SET @ErrorArgs = @procedureName + N': %%Name=' + @recordingStorageName + N'%%'
			;THROW 60103, @ErrorArgs, 1;
		END

		IF EXISTS (
				SELECT	[pkid]
				FROM	[StorageUsageInformation] 
				WHERE	( [pkid] = @pkid OR 
						  [pkid] IN (SELECT [pkid] FROM [ArchiveStorage] WHERE [recordingstorage_id] = @pkid)
						)
						AND [locked_size] > 0)
		BEGIN
			;THROW 60303, @procedureName, 1;
		END

		DECLARE @archivestorage_id [GuidType]
		DECLARE @archiveStorages_xml XML
		DECLARE @archiveStorages_text NVARCHAR(MAX)
		DECLARE @diffgram XML

		SELECT @archiveStorages_xml = COALESCE(T.archiveStorages_xml, '<archiveStorages />') FROM
			(SELECT [pkid] [@id], [name] [@name], [path] [@path], [folder] [@folder] FROM ArchiveStorage
			WHERE [recordingstorage_id] = @pkid
			ORDER BY [retain_minutes] ASC
			FOR XML PATH('archiveStorage'), ROOT('archiveStorages')) T (archiveStorages_xml)
		SET @archiveStorages_text = CAST(@archiveStorages_xml AS NVARCHAR(MAX))

		DECLARE archive_cursor CURSOR FOR 
			SELECT [pkid]
			FROM [ArchiveStorage]
			WHERE [recordingstorage_id] = @pkid
	
		OPEN archive_cursor
		BEGIN TRY
			FETCH NEXT FROM archive_cursor INTO @archivestorage_id

			-- Delete all associated archives
			-- and do audit logging
			WHILE @@FETCH_STATUS = 0
			BEGIN
				EXEC [delete_archive_storage_transaction] @archivestorage_id

				FETCH NEXT FROM archive_cursor INTO @archivestorage_id
			END
		END TRY
		BEGIN CATCH
			CLOSE archive_cursor
			DEALLOCATE archive_cursor
			;THROW
		END CATCH
	
		CLOSE archive_cursor
		DEALLOCATE archive_cursor
	
		SELECT @diffgram = dbo.[recordingStorageAsDataSet](@pkid)

		-- Create diffgram
		EXEC [create_task_transaction]
			@type = 'diffgram_delete',
			@target = @recorder_service_id,
			@data_a = @diffgram,
			@current_time = @CurrentUtcTime

		-- Delete the recording storage
		DELETE FROM [RecordingStorage]
		WHERE [pkid] = @pkid

		-- Validate if a new Database.DeleteStorage task can be created:
		-- No other database tasks on storage with same storage id.
		-- Exceptions: None
		IF EXISTS (	SELECT 1
					FROM   [TaskProperties] tp
					WHERE  [TaskPropertyName] = 'storage_id' AND
					       [TaskPropertyValue] = @pkid
				  )
		BEGIN
			;THROW 60134, @procedureName, 1;
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
			'Database.DeleteStorage',
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
		SET @task_type = 'Database.DeleteStorage'
	
		-- Now we update the task data with correct values
		DECLARE @data_a_xml AS XML
		SET @data_a_xml = '<task/>'
	
		SET @data_a_xml.modify('insert attribute type {sql:variable("@task_type")} as first into (/task)[1]')
		SET @data_a_xml.modify('insert attribute id {sql:variable("@task_id")} as last into (/task)[1]')

		SET @data_a_xml.modify('insert element target {sql:variable("@recorder_service_id")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element idRecorder {sql:variable("@recordingserver_id")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element idStorage {sql:variable("@pkid")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element storageName {sql:variable("@recordingStorageName")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element storagePath {sql:variable("@recordingStoragePath")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element archiveStorages {sql:variable("@archiveStorages_text")} as last into (/task)[1]')

		-- Update the task xml in the table
		UPDATE [Task]
		SET [data_a] = @data_a_xml
		WHERE [pkid] = @task_id
	
		EXEC create_pending_log @session_id, 20003, @pkid, @recordingStorageName, @recordingserver_id, @recordingserver_name

		SELECT @task_id AS task_id
	
		COMMIT TRAN
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