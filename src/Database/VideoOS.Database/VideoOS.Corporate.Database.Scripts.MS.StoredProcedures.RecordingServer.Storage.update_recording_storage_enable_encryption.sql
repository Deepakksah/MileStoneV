-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_recording_storage_enable_encryption]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_recording_storage_enable_encryption]
GO

CREATE PROCEDURE [update_recording_storage_enable_encryption]
	@pkid [GuidType],
	@encryption_method [InternalIdType],
	@password [PasswordType],
	@session_id [GuidType]
 AS
BEGIN
	SET NOCOUNT ON;

		-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
	DECLARE @rowCount INT
	DECLARE @dataSetBefore XML
	DECLARE @dataSetAfter XML
	DECLARE @recorder_service_id [GuidType]
	DECLARE @recordingstorage_id [GuidType]
	DECLARE @recordingstorage_name [NameType]
	DECLARE @recordingserver_id [GuidType]
	DECLARE @recordingserver_name [NameType]

	DECLARE @CurrentUtcTime DATETIME
	SET @CurrentUtcTime = GETUTCDATE()

	IF(@encryption_method = 'None')
	BEGIN
		;THROW 60092, @procedureName, 1;
	END

	BEGIN TRAN

	-- Make sure the Recording storage exists
	SELECT @recordingstorage_id = rs.[pkid],
			@recordingserver_id = rs.[recordingserver_id],
			@recordingstorage_name = rs.[name]
	FROM [RecordingStorage] rs WITH(UPDLOCK)
	WHERE rs.[pkid] = @pkid

	SELECT @rowCount = @@ROWCOUNT

	IF(@rowCount = 0)
	BEGIN
		;THROW 60093, @procedureName, 1;
	END
	
	-- Extract info for logging
	SELECT
		@recordingserver_name = r.[Name],
		@recorder_service_id = rssr.[recording_server_service_id]
	FROM
		[Recorders] r
		INNER JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recorder_id] = r.[IDRecorder] AND rssr.[type] = 0
	WHERE
		r.[IDRecorder] = @recordingserver_id


	-- Get DataSet before changes
	set @dataSetBefore = dbo.recordingStorageAsDataSet(@pkid)

	-- Set encryption method and password
	UPDATE [RecordingStorage]
	SET 
		[encryption_method] = @encryption_method,
		[encryption_password_new] = @password
	WHERE [pkid] = @pkid AND [encryption_method] = 'None'
	
	SELECT @rowCount = @@ROWCOUNT
	
	-- return error if encryption is already enabled because this is 
	-- esentially just a change of password which should be done with the change password procedure
	IF(@rowCount = 0)
	BEGIN
		;THROW 60094, @procedureName, 1;
	END
	
	-- Get DataSet after changes
	set @dataSetAfter = dbo.recordingStorageAsDataSet(@pkid)

	-- Call the create task procedure to create diffgram update task
	EXEC [create_task_transaction]
								'diffgram_update',
								@recorder_service_id,
								@dataSetBefore,
								@dataSetAfter,
								@CurrentUtcTime


	-- Validate if a both the new Database.SetEncryptionMethod and Database.SetEncryptionPassword task can be created:
	-- No other database tasks on storage with same storage id.
	-- Exceptions: None
	IF EXISTS (	SELECT 1
				FROM   [TaskProperties] tp
				WHERE  [TaskPropertyName] = 'storage_id' AND 
				       [TaskPropertyValue] = @pkid
			  )
	BEGIN
		;THROW 60095, @procedureName, 1;
	END


	-- Create task for setting encryption method
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
		'Database.SetEncryptionMethod',
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
	SET @task_type = 'Database.SetEncryptionMethod'

	DECLARE @data_a_xml AS XML
	SET @data_a_xml = '<task/>'

	SET @data_a_xml.modify('insert attribute type {sql:variable("@task_type")} as first into (/task)[1]')
	SET @data_a_xml.modify('insert attribute id {sql:variable("@task_id")} as last into (/task)[1]')

	SET @data_a_xml.modify('insert element target {sql:variable("@recorder_service_id")} as last into (/task)[1]')
	SET @data_a_xml.modify('insert element idRecorder {sql:variable("@recordingserver_id")} as last into (/task)[1]')
	SET @data_a_xml.modify('insert element idStorage {sql:variable("@pkid")} as last into (/task)[1]')
	SET @data_a_xml.modify('insert element storageName {sql:variable("@recordingstorage_name")} as last into (/task)[1]')
	SET @data_a_xml.modify('insert element encryptionMethod {sql:variable("@encryption_method")} as last into (/task)[1]')

	-- Update the task xml in the table
	UPDATE [Task]
	SET [data_a] = @data_a_xml
	WHERE [pkid] = @task_id

	-- Create task for setting encryption password
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
		'Database.SetEncryptionPassword',
		@recorder_service_id,
		'',
		NULL,
		0,
		'',
		0,
		@CurrentUtcTime,
		NULL
	)
	
	DECLARE @task2_id BIGINT
	SET @task2_id = @@IDENTITY
	
	DECLARE @task2_type NVARCHAR(200)
	SET @task2_type = 'Database.SetEncryptionPassword'
	
	SET @data_a_xml = '<task/>'
	SET @data_a_xml.modify('insert attribute type {sql:variable("@task2_type")} as first into (/task)[1]')
	SET @data_a_xml.modify('insert attribute id {sql:variable("@task2_id")} as last into (/task)[1]')

	SET @data_a_xml.modify('insert element target {sql:variable("@recorder_service_id")} as last into (/task)[1]')
	SET @data_a_xml.modify('insert element idRecorder {sql:variable("@recordingserver_id")} as last into (/task)[1]')
	SET @data_a_xml.modify('insert element idStorage {sql:variable("@pkid")} as last into (/task)[1]')
	SET @data_a_xml.modify('insert element storageName {sql:variable("@recordingstorage_name")} as last into (/task)[1]')
	SET @data_a_xml.modify('insert element encryptionPassword {sql:variable("@password")} as last into (/task)[1]')

	-- Update the task xml in the table
	UPDATE [Task]
	SET [data_a] = @data_a_xml
	WHERE [pkid] = @task2_id
	
	EXEC create_pending_log @session_id, 20002, @recordingstorage_id, @recordingstorage_name, @recordingserver_id, @recordingserver_name

	SELECT @task_id AS task_id
	UNION
	SELECT @task2_id AS task_id

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
