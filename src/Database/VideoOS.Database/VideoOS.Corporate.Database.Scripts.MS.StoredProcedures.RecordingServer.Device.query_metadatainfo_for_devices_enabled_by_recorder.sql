-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_metadatainfo_for_devices_enabled_by_recorder]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_metadatainfo_for_devices_enabled_by_recorder]
GO

CREATE PROCEDURE [query_metadatainfo_for_devices_enabled_by_recorder]
	@IDRecorder [GuidType]
 AS
BEGIN
	SET NOCOUNT ON

	SELECT 
		m.[IDDevice], m.[ItemIndex], m.[IDReference], m.[Name], m.[IDDisplayReference], m.[DisplayName] 
	FROM
		[DeviceMetadataInfo] as m
		INNER JOIN [Devices] AS d ON m.[IDDevice] = d.[IDDevice]
		INNER JOIN [Hardware] AS h ON d.[IDHardware] = h.[IDHardware]
	WHERE
		(h.[IDRecorder] = @IDRecorder) AND (h.[Enabled] = 1) AND (d.[Enabled] = 1)

END
GO
