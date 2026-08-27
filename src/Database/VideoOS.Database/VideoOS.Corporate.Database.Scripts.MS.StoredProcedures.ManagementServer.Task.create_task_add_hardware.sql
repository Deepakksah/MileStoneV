-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_task_add_hardware]') AND type = 'P')
DROP PROCEDURE [create_task_add_hardware]
GO

CREATE PROCEDURE [create_task_add_hardware]
	@recording_server_id [GuidType],
	@address [UriTypeShort],
	@user_name [UserIdType],
	@password [PasswordType],
	@driver_number INT,
	@definition_xml NVARCHAR(MAX),
	@customdata NVARCHAR(MAX),
	@location_ip_address NVARCHAR(MAX),
	@user_domain_name NVARCHAR(MAX),
	@user_sid NVARCHAR(MAX)
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

		DECLARE @recorder_name [NameType]
		SELECT
			@recorder_name = r.[Name]
		FROM
			[Recorders] r
		WHERE
			r.[IDRecorder] = @recording_server_id
	
		IF(@recorder_name IS NULL)
		BEGIN
			;THROW 60251, @procedureName, 1;
		END

		-- First create the new task row - with default task data
		DECLARE @task_type NVARCHAR(200)
		SET @task_type = 'System.AddHardware'

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
		SET @data_a_xml.modify('insert element idRecorder {sql:variable("@recording_server_id")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element address {sql:variable("@address")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element userName {sql:variable("@user_name")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element password {sql:variable("@password")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element driverNumber {sql:variable("@driver_number")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element definitionXml {sql:variable("@definition_xml")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element recordingServerName {sql:variable("@recorder_name")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element customdata {sql:variable("@customdata")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element callcontext {} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element locationipaddress {sql:variable("@location_ip_address")} as last into (/task/callcontext)[1]')
		SET @data_a_xml.modify('insert element userdomainaccountname {sql:variable("@user_domain_name")} as last into (/task/callcontext)[1]')
		SET @data_a_xml.modify('insert element usersid {sql:variable("@user_sid")} as last into (/task/callcontext)[1]')

		-- Update the task xml in the table
		UPDATE [Task]
		SET [data_a] = @data_a_xml
		WHERE [pkid] = @task_id

		SELECT @task_id AS task_id
	
		COMMIT TRANSACTION
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