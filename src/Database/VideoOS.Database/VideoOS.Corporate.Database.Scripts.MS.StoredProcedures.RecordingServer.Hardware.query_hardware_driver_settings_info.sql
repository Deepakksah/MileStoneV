-- This header is used by the dbupdate tool]
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on] The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes]
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_hardware_driver_settings_info]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_hardware_driver_settings_info]
GO

CREATE PROCEDURE [query_hardware_driver_settings_info]
	@hardware_id [GuidType]
 AS
BEGIN
	SET NOCOUNT ON

	SELECT
		h.[LastModified] AS [last_modified],
		h.[IDHardware] AS [hardware_id],
		h.[C18NDigest] AS [definition_digest],
		h.[Settings] AS [settings_xml]
	FROM 
		[Hardware] h
	WHERE h.[IDHardware] = @hardware_id
END
GO
