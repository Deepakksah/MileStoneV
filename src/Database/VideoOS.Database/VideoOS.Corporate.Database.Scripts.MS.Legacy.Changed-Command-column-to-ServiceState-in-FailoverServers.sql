-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[FailoverServers]') AND name = N'ServiceState')
BEGIN
	ALTER TABLE [FailoverServers] ADD
	[ServiceState] INT NOT NULL DEFAULT 1
END
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[FailoverServers]') AND name = N'Command')
BEGIN
	ALTER TABLE [FailoverServers] DROP COLUMN
	[Command]
END
GO
