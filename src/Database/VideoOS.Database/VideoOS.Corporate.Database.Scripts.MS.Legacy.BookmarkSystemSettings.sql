-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


IF NOT EXISTS(SELECT * FROM [SystemSettings] WHERE [IDSystemSetting] = 'E4DF1399-14F5-4d9e-AC86-F7FF44FAA5D0')
BEGIN
	INSERT [SystemSettings]
	VALUES(GETUTCDATE(), 'E4DF1399-14F5-4d9e-AC86-F7FF44FAA5D0', 'BookmarkIdPrefix', 'no.')
END

IF NOT EXISTS(SELECT * FROM [SystemSettings] WHERE [IDSystemSetting] = 'E387EFC4-C517-4258-AFFA-A0C66977DD1C')
BEGIN
	INSERT [SystemSettings]
	VALUES(GETUTCDATE(), 'E387EFC4-C517-4258-AFFA-A0C66977DD1C', 'BookmarkPreTime', '3')
END

IF NOT EXISTS(SELECT * FROM [SystemSettings] WHERE [IDSystemSetting] = '503FC893-AD85-40c7-929A-2E49C88C99FE')
BEGIN
	INSERT [SystemSettings]
	VALUES(GETUTCDATE(), '503FC893-AD85-40c7-929A-2E49C88C99FE', 'BookmarkPostTime', '30')
END

GO

