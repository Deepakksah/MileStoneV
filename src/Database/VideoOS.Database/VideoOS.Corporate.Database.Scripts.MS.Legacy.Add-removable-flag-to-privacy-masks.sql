-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS (SELECT * FROM [DevicePrivacyMask] WHERE [PrivacyMask].exist('/mask') = 1)
BEGIN
	UPDATE [DevicePrivacyMask]
	SET [PrivacyMask].modify('insert attribute removable {"false"} as last into (/mask/methods/solid)[1]')
	WHERE [PrivacyMask].exist('/mask/methods/solid/@removable') = 0
END
GO
