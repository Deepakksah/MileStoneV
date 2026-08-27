-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM [SystemSettings] WHERE [IDSystemSetting] = 'E02516C8-4E4B-405A-88D3-308308C659B5')
BEGIN
	INSERT [SystemSettings]
	VALUES(GETUTCDATE(), 'E02516C8-4E4B-405A-88D3-308308C659B5', 'LocalIPRanges', '')
END
GO
