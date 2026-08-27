-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Add FailoverServers->DatabaseLocation
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[FailoverServers]') AND name = N'DatabaseLocation')
BEGIN
	ALTER TABLE [FailoverServers] ADD
	[DatabaseLocation] DescriptionType NULL
END
GO
