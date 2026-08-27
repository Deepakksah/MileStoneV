-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_task_move_hardware]') AND type = 'P')
DROP PROCEDURE [create_task_move_hardware]
GO

CREATE PROCEDURE [create_task_move_hardware]
	@hardware_id [GuidType],
	@destination_recording_server_id [GuidType],
	@destination_storage_id [GuidType],
	@ignore_source_recording_server BIT,
	@session_id [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		BEGIN TRANSACTION
	
		DECLARE @server_service_id [GuidType]
		SET @server_service_id = '909749AE-88C4-4780-A381-383D691E98FA'

		DECLARE @hardware_name [NameType]

		DECLARE @source_recorder_name [NameType]
		DECLARE @source_recording_server_id [GuidType]

		DECLARE @destination_recorder_name [NameType]
		DECLARE @destination_storage_name [NameType]

		-- TODO: Validate task against creation rules (see create_task_truncate_device_storage for an example)

		-- Check hardware existence and resolve current recording server
		SELECT
			@hardware_name = h.[Name],
			@source_recorder_name = r.[Name],
			@source_recording_server_id = rssr.[recorder_id]
		FROM
			[Hardware] h
			INNER JOIN [Recorders] r ON r.[IDRecorder] = h.[IDRecorder]
			INNER JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recorder_id] = r.[IDRecorder] AND rssr.[type] = 0  -- Primary recorder type
		WHERE
			h.[IDHardware] = @hardware_id
	
		IF(@source_recorder_name IS NULL OR @hardware_name IS NULL)
		BEGIN
			;THROW 60247, @procedureName, 1;
		END

		-- Check source and destination recorders are different
		IF(@source_recording_server_id = @destination_recording_server_id)
		BEGIN
			;THROW 60297, @procedureName, 1;
		END

		-- Check destination recorder existence
		SELECT	@destination_recorder_name = r.[Name]
		FROM	[Recorders] r
				INNER JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recorder_id] = r.[IDRecorder] AND rssr.[type] = 0  -- Primary recorder type
		WHERE	r.[IDRecorder] = @destination_recording_server_id
	
		IF(@destination_recorder_name IS NULL)
		BEGIN
			;THROW 60248, @procedureName, 1;
		END

		-- Check destination storage existence
		SELECT	@destination_storage_name = rs.[name]
		FROM	[RecordingStorage] rs
				INNER JOIN [Recorders] r ON r.[IDRecorder] = rs.[recordingserver_id]
		WHERE	rs.[pkid] = @destination_storage_id
	
		IF (@destination_storage_name IS NULL)
		BEGIN
			;THROW 60249, @procedureName, 1;
		END

		-- First create the new task row - with default task data
		DECLARE @task_type NVARCHAR(200)
		SET @task_type = 'System.MoveHardware'

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
			@server_service_id,
			'',
			NULL,
			0,
			'',
			0,
			@CurrentUtcTime,
			NULL
		)
	
		DECLARE @task_id BIGINT
		SET @task_id = SCOPE_IDENTITY()

		DECLARE @ignore_source_recording_server_value NVARCHAR(10)
		SELECT @ignore_source_recording_server_value = CAST(@ignore_source_recording_server AS NVARCHAR(10))

		-- Now we update the task data with correct values
		DECLARE @data_a_xml XML
		SET @data_a_xml = '<task/>'
	
		SET @data_a_xml.modify('insert attribute type {sql:variable("@task_type")} as first into (/task)[1]')
		SET @data_a_xml.modify('insert attribute id {sql:variable("@task_id")} as last into (/task)[1]')

		SET @data_a_xml.modify('insert element target {sql:variable("@server_service_id")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element hardwareId {sql:variable("@hardware_id")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element hardwareName {sql:variable("@hardware_name")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element sourceRecorderId {sql:variable("@source_recording_server_id")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element sourceRecorderName {sql:variable("@source_recorder_name")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element destinationRecorderId {sql:variable("@destination_recording_server_id")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element destinationRecorderName {sql:variable("@destination_recorder_name")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element destinationStorageId {sql:variable("@destination_storage_id")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element destinationStorageName {sql:variable("@destination_storage_name")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element ignoreSourceRecordingServer {sql:variable("@ignore_source_recording_server_value")} as last into (/task)[1]')

		-- Update the task xml in the table
		UPDATE [Task]
		SET [data_a] = @data_a_xml
		WHERE [pkid] = @task_id

		SELECT @task_id AS task_id
	
		COMMIT TRANSACTION

		EXEC create_pending_log @session_id, 23003, @hardware_id, @hardware_name, @source_recording_server_id, @source_recorder_name, @destination_recorder_name
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