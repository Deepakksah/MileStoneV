-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[FailoverServers]') AND name = N'x64')
BEGIN
	ALTER TABLE FailoverServers
	ADD [x64] BIT NOT NULL CONSTRAINT [DF_FailoverServer_x64] DEFAULT (0)
END
GO
