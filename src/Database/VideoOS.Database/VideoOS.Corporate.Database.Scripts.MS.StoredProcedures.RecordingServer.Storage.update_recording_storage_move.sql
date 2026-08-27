-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_recording_storage_move]') AND type = 'P')
DROP PROCEDURE [update_recording_storage_move]
GO

CREATE PROCEDURE [update_recording_storage_move]
	@pkid [GuidType],
	@target_path NVARCHAR(512),
	@session_id [GuidType]
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

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		BEGIN TRAN
	
		DECLARE @storage_name [NameType]
		DECLARE @source_path NVARCHAR(512)
		DECLARE @source_bank_id UNIQUEIDENTIFIER
		DECLARE @target_bank_id UNIQUEIDENTIFIER
		DECLARE @recordingserver_name [NameType]
		DECLARE	@recordingServer_id [GuidType]

		SELECT 
			@recordingServer_id = r.[recordingServer_id],
			@storage_name = r.[name],
			@source_path = r.[path],
			@source_bank_id = r.[bank_id]
		FROM
			[RecordingStorage] r
		WITH(UPDLOCK) 
		WHERE
			r.[pkid] = @pkid
	
		-- Extract info for logging
		SELECT
			@recordingserver_name = r.[Name],
			@recorder_service_id = rssr.[recording_server_service_id]
		FROM
			[Recorders] r
			INNER JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recorder_id] = r.[IDRecorder] AND rssr.[type] = 0
		WHERE
			r.[IDRecorder] = @recordingserver_id
	
		IF(@recorder_service_id IS NULL)
		BEGIN
			;THROW 60000, @procedureName, 1;
		END

		-- Get DataSet before changes
		SET @dataSetBefore = dbo.recordingStorageAsDataSet(@pkid)
	
		SET @target_bank_id = NEWID()

		UPDATE
			[RecordingStorage]
		SET
			[path] = @target_path,
			[bank_id] = @target_bank_id
		WHERE
			[pkid]= @pkid
	
		-- Get DataSet after changes
		SET @dataSetAfter = dbo.recordingStorageAsDataSet(@pkid)

		-- Call the create task procedure to create diffgram update task
		EXEC [create_task_transaction]
									'diffgram_update',
									@recorder_service_id,
									@dataSetBefore,
									@dataSetAfter,
									@CurrentUtcTime


		-- Validate if a new Database.MoveStorage task can be created:
		-- No other database tasks on storage with same storage id.
		-- Exceptions: DeleteArchive; MoveArchive.
		IF EXISTS (	SELECT 1
					FROM   [TaskProperties] tp
					WHERE  [TaskPropertyName] = 'storage_id' AND 
					       [TaskPropertyValue] = @pkid AND
						   [TaskType] NOT IN ('Database.DeleteArchive', 'Database.MoveArchive')
				  )
		BEGIN
			;THROW 60132, @procedureName, 1;
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
			'Database.MoveStorage',
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
		SET @task_type = 'Database.MoveStorage'

		-- Now we update the task data with correct values
		DECLARE @data_a_xml AS XML
		SET @data_a_xml = '<task/>'
	
		SET @data_a_xml.modify('insert attribute type {sql:variable("@task_type")} as first into (/task)[1]')
		SET @data_a_xml.modify('insert attribute id {sql:variable("@task_id")} as last into (/task)[1]')

		SET @data_a_xml.modify('insert element target {sql:variable("@recorder_service_id")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element idRecorder {sql:variable("@recordingserver_id")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element idStorage {sql:variable("@pkid")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element storageName {sql:variable("@storage_name")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element pathSource {sql:variable("@source_path")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element pathTarget {sql:variable("@target_path")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element bankIdSource {sql:variable("@source_bank_id")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element bankIdTarget {sql:variable("@target_bank_id")} as last into (/task)[1]')
	
		-- Update the task xml in the table
		UPDATE [Task]
		SET [data_a] = @data_a_xml
		WHERE [pkid] = @task_id
	
		EXEC create_pending_log @session_id, 20004, @pkid, @storage_name, @recordingServer_id, @recordingserver_name, @source_path, @target_path

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
