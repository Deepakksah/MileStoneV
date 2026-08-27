-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


IF NOT EXISTS(SELECT * FROM [ServerType] WHERE [name] = 'LPRServer')
INSERT [ServerType] VALUES('LPRServer', '{CC1EABF7-B691-4E12-B4E0-DC801F57F8D6}')
GO
