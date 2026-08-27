-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_task_delete_idp_user_transaction]') AND type = 'P')
DROP PROCEDURE [create_task_delete_idp_user_transaction]
GO

CREATE PROCEDURE [create_task_delete_idp_user_transaction]
	@id NVARCHAR(450)
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	DECLARE @CurrentUtcTime DATETIME
	SET @CurrentUtcTime = GETUTCDATE()

	DECLARE @server_service_id [GuidType]
	SET @server_service_id = '909749AE-88C4-4780-A381-383D691E98FA'

	-- First create the new task row - with default task data
	DECLARE @task_type NVARCHAR(200)
	SET @task_type = 'System.DeleteIdpUser'

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
	SET @data_a_xml.modify('insert element id {sql:variable("@id")} as last into (/task)[1]')

	-- Update the task xml in the table
	UPDATE [Task]
	SET [data_a] = @data_a_xml
	WHERE [pkid] = @task_id

	SELECT @task_id AS task_id
	
	RETURN 0
END
GO