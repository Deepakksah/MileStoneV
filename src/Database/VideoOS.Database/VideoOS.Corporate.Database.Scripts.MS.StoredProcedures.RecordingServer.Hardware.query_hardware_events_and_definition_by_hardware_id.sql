-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_hardware_events_and_definition_by_hardware_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_hardware_events_and_definition_by_hardware_id]
GO

CREATE PROCEDURE [query_hardware_events_and_definition_by_hardware_id]
	@IDHardware [GuidType]
 AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	SELECT
		h.[IDHardware],
		h.[C18NDigest] AS [HardwareDefinitionDigest],
		h.[EventSettings],
		h.[LastModified],
		f.[Definition] AS [HardwareDefinition]
	FROM ([Hardware] h
	INNER JOIN [HardwareDefinition] f ON h.[C18NDigest] = f.[C18NDigest])
	WHERE h.[IDHardware] = @IDHardware
END
GO