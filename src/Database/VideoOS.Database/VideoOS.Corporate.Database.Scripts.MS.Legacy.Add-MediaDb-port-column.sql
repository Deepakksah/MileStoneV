-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS ( SELECT *  FROM sys.columns  WHERE object_id = OBJECT_ID(N'[RecordingServerService]') AND name = N'database_port')
BEGIN
	ALTER TABLE [RecordingServerService]
	ADD [database_port] INT NULL
END
GO

UPDATE [RecordingServerService]
SET [database_port] = 5210
WHERE [database_port] IS NULL
GO

ALTER TABLE [RecordingServerService]
ALTER COLUMN [database_port] INT NOT NULL
GO
