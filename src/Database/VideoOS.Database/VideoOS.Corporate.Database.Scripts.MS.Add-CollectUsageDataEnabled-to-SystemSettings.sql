-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM [SystemSettings] WHERE [Name] = 'CollectUsageDataEnabled')
BEGIN
	INSERT INTO [SystemSettings] ([LastModified], [Name], [Setting])
	VALUES (getdate(), 'CollectUsageDataEnabled', 'False')
END
GO