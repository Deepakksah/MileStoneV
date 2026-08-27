-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF NOT EXISTS(SELECT * FROM [SystemSettings] WHERE [IDSystemSetting] = 'BE5041BE-1B32-4BC2-8D25-AD5D01BE473C')
BEGIN
	INSERT INTO [SystemSettings] ([LastModified], [IDSystemSetting], [Name], [Setting])
	VALUES (getdate(), 'BE5041BE-1B32-4BC2-8D25-AD5D01BE473C', 'LogAuditPlaybackFramesBeforeLogging', '5')
END

IF NOT EXISTS(SELECT * FROM [SystemSettings] WHERE [IDSystemSetting] = '7433C74E-C751-4DF2-8550-4DB169523372')
BEGIN
	INSERT INTO [SystemSettings] ([LastModified], [IDSystemSetting], [Name], [Setting])
	VALUES (getdate(), '7433C74E-C751-4DF2-8550-4DB169523372', 'LogAuditPlaybackWindowSizeInSec', '60')
END
GO

IF NOT EXISTS(SELECT * FROM [SystemSettings] WHERE [IDSystemSetting] = 'D535C560-F6F6-4c48-B601-6783EE22417D')
BEGIN
	INSERT INTO [SystemSettings] ([LastModified], [IDSystemSetting], [Name], [Setting])
	VALUES (getdate(), 'D535C560-F6F6-4c48-B601-6783EE22417D', 'LogAuditEnableUserAccessLogging', 'false')
END
GO