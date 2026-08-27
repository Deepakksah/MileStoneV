-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[get_service_applications]') AND type in (N'P', N'PC'))
DROP PROCEDURE [get_service_applications]
GO

CREATE PROCEDURE [get_service_applications]
	@data_collector_id INT
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	SELECT [name], [upgrade_code], CAST(0 AS BIT) AS [is_installed]
	FROM [ServerType]
END
GO