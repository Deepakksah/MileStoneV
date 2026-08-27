-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[DataCollector]') AND name = 'application_id')
BEGIN
	ALTER TABLE [DataCollector]
	ADD [application_id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT [DF_tempAppIdDefault] DEFAULT (NEWID())
	
	ALTER TABLE [DataCollector]
	DROP CONSTRAINT [DF_tempAppIdDefault]
END
GO
