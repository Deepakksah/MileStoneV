-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Adds new MacAddress property under SystemSettings

IF NOT EXISTS(SELECT * FROM [SystemSettings] WHERE [IDSystemSetting] = 'B3C5A985-1C86-4E1A-8514-58209ACC04F5')
BEGIN
	INSERT [SystemSettings]
	VALUES(GETUTCDATE(), 'B3C5A985-1C86-4E1A-8514-58209ACC04F5', 'MacAddress', '')
END
GO
