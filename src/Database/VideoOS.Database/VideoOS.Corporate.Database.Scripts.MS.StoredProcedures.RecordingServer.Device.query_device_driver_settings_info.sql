-- This header is used by the dbupdate tool]
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on] The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes]
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_driver_settings_info]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_driver_settings_info]
GO

CREATE PROCEDURE [query_device_driver_settings_info]
	@device_id [GuidType]
 AS
BEGIN
	SET NOCOUNT ON

	SELECT
		d.[LastModified] AS [last_modified],
		d.[DeviceIndex] AS [device_index],
		d.[DeviceType] AS [device_type],
		d.[Settings] AS [settings_xml],
		d.[IDHardware] AS [hardware_id],
		h.[C18NDigest] AS [definition_digest]
	FROM 
		[Devices] d
		INNER JOIN [Hardware] h ON h.[IDHardware] = d.[IDHardware]
	WHERE d.[IDDevice] = @device_id
END
GO
