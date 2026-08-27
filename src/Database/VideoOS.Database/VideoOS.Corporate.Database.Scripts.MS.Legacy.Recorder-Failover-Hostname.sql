-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Recorders]') AND name = N'FailoverHostName')
BEGIN
	ALTER TABLE Recorders ADD
	[FailoverHostName] [LongNameType] NOT NULL DEFAULT ''
END
GO

UPDATE Recorders
SET FailoverHostName = HostName
WHERE FailoverHostName = ''
GO