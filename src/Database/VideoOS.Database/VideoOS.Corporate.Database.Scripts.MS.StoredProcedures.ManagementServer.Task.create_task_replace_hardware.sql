-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT 1 FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[create_task_replace_hardware]') AND [type] = 'P')
DROP PROCEDURE [create_task_replace_hardware]
GO

CREATE PROCEDURE [create_task_replace_hardware]
	@hardware_id                        [GuidType],
	@hardware_address                   [UriTypeShort],
	@port                               INT,
	@use_https                          BIT,
	@https_port                         INT,
	@user_name                          [UserIdType],
	@password                           [PasswordType],
	@driver_number                      INT,
	@custom_driver_data                 NVARCHAR(1024),
	@allow_deleting_disabled_devices    BIT,
	@acting_user_sid                    [SIDType],
	@acting_user_domain_account_name    [UserIdType],
	@acting_user_location_ip_address    [UriTypeShort]
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @status INT

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + DB_NAME() + '..' + OBJECT_NAME(@@PROCID)

	DECLARE @CurrentUtcTime DATETIME
	SET @CurrentUtcTime = GETUTCDATE()

	BEGIN TRAN
	
	DECLARE @server_service_id [GuidType] = '909749AE-88C4-4780-A381-383D691E98FA'
	
	DECLARE @min_vps_driver_number INT = 20001
	DECLARE @max_vps_driver_number INT = 20001
	DECLARE @min_icp_driver_number INT = 30000
	DECLARE @max_icp_driver_number INT = 39999

	DECLARE @hardware_name [NameType]
	DECLARE @recorder_name [NameType]
	DECLARE @recording_server_id [GuidType]
	DECLARE @current_driver_number INT
	DECLARE @hosted_hardware_id INT

	-- Check hardware existence and resolve current recording server
	SELECT
		@hardware_name = h.[Name],
		@recorder_name = r.[Name],
		@recording_server_id = rssr.[recorder_id],
		@current_driver_number = h.[DriverNumber],
		@hosted_hardware_id = h.[HostedHardware_id]
	FROM
		[Hardware] h
		INNER JOIN [Recorders] r ON r.[IDRecorder] = h.[IDRecorder]
		INNER JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recorder_id] = r.[IDRecorder] AND rssr.[type] = 0  -- Primary recorder type
	WHERE
		h.[IDHardware] = @hardware_id

	IF (@recorder_name IS NULL OR @hardware_name IS NULL)
	BEGIN
	  ROLLBACK TRAN
		;THROW 60045, @procedureName, 1; -- Error like: The hardware does not exist.
	END

	-- Check that the 'replace hardware' is supported by the hardware type
	DECLARE @old_driver_is_vps BIT = CASE WHEN (@current_driver_number >= @min_vps_driver_number AND @current_driver_number <= @max_vps_driver_number) THEN 1 ELSE 0 END
	DECLARE @new_driver_is_vps BIT = CASE WHEN (@driver_number >= @min_vps_driver_number AND @driver_number <= @max_vps_driver_number) THEN 1 ELSE 0 END
	IF (@old_driver_is_vps = 1 OR @new_driver_is_vps = 1)
	BEGIN
		ROLLBACK TRAN
		;THROW 60382, @procedureName, 1; -- Error like: Hardware replacement is not supported for hardware using a VPS driver.
	END

	IF (@hosted_hardware_id IS NOT NULL)
	BEGIN
		ROLLBACK TRAN
		;THROW 60383, @procedureName, 1; -- Error like: Hardware replacement is not supported for hardware using 'Remote connect services'.
	END


	IF (@current_driver_number >= @min_icp_driver_number AND @current_driver_number <= @max_icp_driver_number)
	BEGIN
		ROLLBACK TRAN
		;THROW 60384, @procedureName, 1; -- Error like: Hardware replacement is not supported for hardware using an interconnected product driver.
	END

	IF (@driver_number >= @min_icp_driver_number AND @driver_number <= @max_icp_driver_number)
	BEGIN
		ROLLBACK TRAN
		;THROW 60385, @procedureName, 1; -- Error like: Changing from an ordinary driver type to an interconnected product driver is not supported.
	END



	-- First create the new task row - with default task data
	DECLARE @task_type NVARCHAR(200)
	SET @task_type = 'System.ReplaceHardware'

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

	SET @status = @@ERROR
	IF(@status <> 0)
	BEGIN
		ROLLBACK TRAN
		;THROW 60000, @procedureName, 1;
	END
	
	DECLARE @task_id BIGINT
	SET @task_id = SCOPE_IDENTITY()
	
	
	DECLARE @use_https_value NVARCHAR(10)
	SELECT @use_https_value = CAST(@use_https AS NVARCHAR(10))

	DECLARE @allow_deleting_disabled_devices_value NVARCHAR(10)
	SELECT @allow_deleting_disabled_devices_value = CAST(@allow_deleting_disabled_devices AS NVARCHAR(10))

	-- Now we update the task data with correct values
	DECLARE @data_a_xml XML
	SET @data_a_xml = '<task/>'
	
	SET @data_a_xml.modify('insert attribute type {sql:variable("@task_type")} as first into (/task)[1]')
	SET @data_a_xml.modify('insert attribute id {sql:variable("@task_id")} as last into (/task)[1]')

	SET @data_a_xml.modify('insert element target {sql:variable("@server_service_id")} as last into (/task)[1]')
	SET @data_a_xml.modify('insert element hardwareId {sql:variable("@hardware_id")} as last into (/task)[1]')
	SET @data_a_xml.modify('insert element hardwareName {sql:variable("@hardware_name")} as last into (/task)[1]')
	SET @data_a_xml.modify('insert element recordingServerId {sql:variable("@recording_server_id")} as last into (/task)[1]')
	SET @data_a_xml.modify('insert element recordingServerName {sql:variable("@recorder_name")} as last into (/task)[1]')

	SET @data_a_xml.modify('insert element hardwareAddress {sql:variable("@hardware_address")} as last into (/task)[1]')
	SET @data_a_xml.modify('insert element port {sql:variable("@port")} as last into (/task)[1]')
	SET @data_a_xml.modify('insert element useHttps {sql:variable("@use_https_value")} as last into (/task)[1]')
	SET @data_a_xml.modify('insert element httpsPort {sql:variable("@https_port")} as last into (/task)[1]')
	SET @data_a_xml.modify('insert element userName {sql:variable("@user_name")} as last into (/task)[1]')
	SET @data_a_xml.modify('insert element password {sql:variable("@password")} as last into (/task)[1]')
	SET @data_a_xml.modify('insert element driverNumber {sql:variable("@driver_number")} as last into (/task)[1]')
	SET @data_a_xml.modify('insert element customDriverData {sql:variable("@custom_driver_data")} as last into (/task)[1]')
	SET @data_a_xml.modify('insert element allowDeletingDisabledDevices {sql:variable("@allow_deleting_disabled_devices_value")} as last into (/task)[1]')
	SET @data_a_xml.modify('insert element actingUserSid {sql:variable("@acting_user_sid")} as last into (/task)[1]')
	SET @data_a_xml.modify('insert element actingUserDomainAccountName {sql:variable("@acting_user_domain_account_name")} as last into (/task)[1]')
	SET @data_a_xml.modify('insert element actingUserLocationIPAddress {sql:variable("@acting_user_location_ip_address")} as last into (/task)[1]')

	-- Update the task xml in the table
	UPDATE [Task]
	SET    [data_a] = @data_a_xml
	WHERE  [pkid] = @task_id

	SELECT @task_id AS task_id
	
	COMMIT TRAN

	RETURN 0
END
GO
