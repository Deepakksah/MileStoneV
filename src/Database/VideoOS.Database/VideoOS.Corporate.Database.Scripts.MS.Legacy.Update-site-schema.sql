-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Sites]') AND name = N'Version')
BEGIN
	ALTER TABLE [Sites]
	ADD [Version] NVARCHAR(15) NOT NULL CONSTRAINT [SITES_VERISON_DEFAULT] DEFAULT ('')
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Sites]') AND name = N'ServiceAccount')
BEGIN
	ALTER TABLE [Sites]
	ADD [ServiceAccount] NVARCHAR(256) NOT NULL CONSTRAINT [SITES_SERVICE_ACCOUNT_DEFAULT] DEFAULT ('')
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Sites]') AND name = N'SynchronizationStatus')
BEGIN
	ALTER TABLE [Sites]
	ADD [SynchronizationStatus] INT NOT NULL CONSTRAINT [SITES_SYNC_STATUS_DEFAULT] DEFAULT 0
END
GO
