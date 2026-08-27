-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF NOT EXISTS(SELECT * from [SystemSettings] WHERE [Name] = 'DeviceErrorTimeoutSec')
BEGIN
	INSERT INTO [SystemSettings] ([IDSystemSetting], [Name], [Setting], [LastModified])
	VALUES(NEWID(), 'DeviceErrorTimeoutSec', '0', GETUTCDATE())
END
GO
