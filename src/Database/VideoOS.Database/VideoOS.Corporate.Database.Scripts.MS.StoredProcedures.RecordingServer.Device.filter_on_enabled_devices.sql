-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[filter_on_enabled_devices]') AND type in (N'P', N'PC'))
DROP PROCEDURE [filter_on_enabled_devices]
GO

CREATE PROCEDURE [filter_on_enabled_devices]
	@object_ids tvp_StringTableGuidType READONLY
 AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	SELECT
		d.[IDDevice] as 'device_id'
	FROM [Devices] d
	INNER JOIN @object_ids ids ON d.[IDDevice] = ids.[ID] AND d.[Enabled] = '1'
END
GO
