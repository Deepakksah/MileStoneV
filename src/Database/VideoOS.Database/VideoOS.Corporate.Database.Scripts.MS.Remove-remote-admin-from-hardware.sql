-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS (SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'[Hardware]') AND name = N'RemoteAdminUserID')
BEGIN
	ALTER TABLE [Hardware] DROP COLUMN [RemoteAdminUserID]
END
GO

IF EXISTS (SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'[Hardware]') AND name = N'RemoteAdminPassword')
BEGIN
	ALTER TABLE [Hardware] DROP COLUMN [RemoteAdminPassword]
END
GO