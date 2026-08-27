-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[VideoWallMonitor]') AND name = N'monitor_state')
BEGIN
	ALTER TABLE [VideoWallMonitor]
	ADD [monitor_state] varchar(2000) NOT NULL
	DEFAULT ''
END
GO
