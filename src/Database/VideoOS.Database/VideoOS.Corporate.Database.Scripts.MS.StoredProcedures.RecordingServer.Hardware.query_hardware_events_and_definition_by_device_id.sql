-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_hardware_events_and_definition_by_device_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_hardware_events_and_definition_by_device_id]
GO



CREATE PROCEDURE [query_hardware_events_and_definition_by_device_id]
	@IDDevice [GuidType]
 AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	SELECT
		d.[IDDevice],
		d.[DeviceIndex],
		h.[IDHardware],
		h.[C18NDigest] AS [HardwareDefinitionDigest],
		h.[EventSettings],
		h.[LastModified],
		f.[Definition] AS [HardwareDefinition]
	FROM (([Devices] d
	INNER JOIN [Hardware] h ON h.[IDHardware] = d.[IDHardware])
	INNER JOIN [HardwareDefinition] f ON h.[C18NDigest] = f.[C18NDigest])
	WHERE d.[IDDevice] = @IDDevice
END
GO
