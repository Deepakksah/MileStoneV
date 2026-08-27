-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[RegisteredServers]') AND name = N'Enabled')
BEGIN
	ALTER TABLE [RegisteredServers]
	ADD [Enabled] BIT NULL
END
GO

UPDATE [RegisteredServers]
SET [Enabled] = 1
WHERE IsNull([Enabled], 1) = 1
GO
