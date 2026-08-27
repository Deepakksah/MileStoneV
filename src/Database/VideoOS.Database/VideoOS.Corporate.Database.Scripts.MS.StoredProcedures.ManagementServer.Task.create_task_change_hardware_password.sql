-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_task_change_hardware_password]') AND type = 'P')
DROP PROCEDURE [create_task_change_hardware_password]
GO

CREATE PROCEDURE [create_task_change_hardware_password]	
	@hardware_id [GuidType],
	@user_name [UserIdType],
	@password [PasswordType],
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
		DECLARE @recording_server_id [GuidType]
		DECLARE @recorder_name [NameType]

		-- Check hardware existence and resolve current recording server
		SELECT
			@hardware_name = h.[Name],
			@recorder_name = r.[Name],
			@recording_server_id = rssr.[recorder_id]
		FROM
			[Hardware] h
			INNER JOIN [Recorders] r ON r.[IDRecorder] = h.[IDRecorder]
			INNER JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recorder_id] = r.[IDRecorder] AND rssr.[type] = 0  -- Primary recorder type
		WHERE
			h.[IDHardware] = @hardware_id
	
		IF(@recorder_name IS NULL OR @hardware_name IS NULL)
		BEGIN
			;THROW 60045, @procedureName, 1;
		END

		-- First create the new task row - with default task data
		DECLARE @task_type NVARCHAR(200)
		SET @task_type = 'System.ChangeHardwarePassword'

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
		SET @task_id = @@IDENTITY

		-- Now we update the task data with correct values
		DECLARE @data_a_xml XML
		SET @data_a_xml = '<task/>'
	
		SET @data_a_xml.modify('insert attribute type {sql:variable("@task_type")} as first into (/task)[1]')
		SET @data_a_xml.modify('insert attribute id {sql:variable("@task_id")} as last into (/task)[1]')

		SET @data_a_xml.modify('insert element target {sql:variable("@server_service_id")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element recordingServerId {sql:variable("@recording_server_id")} as last into (/task)[1]')	
		SET @data_a_xml.modify('insert element recordingServerName {sql:variable("@recorder_name")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element hardwareId {sql:variable("@hardware_id")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element hardwareName {sql:variable("@hardware_name")} as last into (/task)[1]')

		SET @data_a_xml.modify('insert element userName {sql:variable("@user_name")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element password {sql:variable("@password")} as last into (/task)[1]')		

		-- Update the task xml in the table
		UPDATE [Task]
		SET [data_a] = @data_a_xml
		WHERE [pkid] = @task_id

		SELECT @task_id AS task_id
	
		COMMIT TRANSACTION

		EXEC create_pending_log @session_id, 3620, @hardware_id, @hardware_name, @recording_server_id, @recorder_name
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