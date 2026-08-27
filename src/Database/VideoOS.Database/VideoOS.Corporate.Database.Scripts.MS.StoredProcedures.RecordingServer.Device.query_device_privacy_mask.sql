-- This header is used by the dbupdate tool]
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on] The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes]
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_privacy_mask]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_privacy_mask]
GO

CREATE PROCEDURE [query_device_privacy_mask]
	@IDDevice [GuidType]
 AS
BEGIN
	SET NOCOUNT ON

	SELECT
		dpm.[IDDevice],
		dpm.[PrivacyMask],
		dpm.[LastModified],
		dpm.[Enabled],
		h.[IDHardware],
		h.[IDRecorder]
	FROM [DevicePrivacyMask] dpm
	INNER JOIN [Devices] d ON d.[IDDevice] = dpm.[IDDevice]
	INNER JOIN [Hardware] h ON h.[IDHardware] = d.[IDHardware]
	WHERE dpm.[IDDevice] = @IDDevice AND 
	dpm.[LastModified] = (SELECT MAX([LastModified]) FROM [DevicePrivacyMask] WHERE [IDDevice] = @IDDevice)
END
GO
