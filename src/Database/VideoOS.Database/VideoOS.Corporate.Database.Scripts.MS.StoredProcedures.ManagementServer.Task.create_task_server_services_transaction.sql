-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_task_server_services_transaction]') AND type = 'P')
DROP PROCEDURE [create_task_server_services_transaction]
GO

CREATE PROCEDURE [create_task_server_services_transaction]
    @type VARCHAR(50),
    @data_a XML,
    @data_b XML = NULL,
    @current_time DATETIME
AS
BEGIN
    SET NOCOUNT ON

    -- Initialize the ProcedureName for error messages.
    DECLARE @procedureName SYSNAME
    SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
    
    -- Make sure that a current transaction is active
    IF(@@TRANCOUNT = 0)
    BEGIN
        ;THROW 60008, @procedureName, 1;
    END

    DECLARE @server_service_id [GuidType] = '909749AE-88C4-4780-A381-383D691E98FA'

	EXEC [create_task_transaction] @type, @server_service_id, @data_a, @data_b, @current_time
END
GO