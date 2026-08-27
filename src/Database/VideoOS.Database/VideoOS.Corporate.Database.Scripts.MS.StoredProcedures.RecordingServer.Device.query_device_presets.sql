-- This header is used by the dbupdate tool]
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on] The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes]
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_presets]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_presets]
GO

CREATE PROCEDURE [query_device_presets]
	@IDDevice [GuidType]
 AS
BEGIN
	SET NOCOUNT ON

	SELECT
		d.[IDDevice],
		d.[Presets],
		d.[Settings],
		d.[LastModified],
		h.[IDHardware],
		h.[IDRecorder]
	FROM [Devices] d
	INNER JOIN [Hardware] h ON h.[IDHardware] = d.[IDHardware]
	WHERE d.[IDDevice] = @IDDevice
END
GO
