-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dataset_device_privacy_mask_select_by_device_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dataset_device_privacy_mask_select_by_device_ids]
GO

CREATE PROCEDURE [dataset_device_privacy_mask_select_by_device_ids]
	@ids XML
AS
BEGIN
	SELECT [LastModified], [IDDevice], [PrivacyMask], [Enabled]
	FROM [DevicePrivacyMask] dpm
	INNER JOIN XmlToIdTable(@ids) ids ON dpm.[IDDevice] = ids.[ID]
END
GO