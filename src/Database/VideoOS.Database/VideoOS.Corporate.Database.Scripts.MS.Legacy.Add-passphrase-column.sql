-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS ( SELECT *  FROM sys.columns  WHERE object_id = OBJECT_ID(N'[RecordingServerService]') AND name = N'passphrase')
BEGIN
	ALTER TABLE [RecordingServerService]
	ADD [passphrase] NVARCHAR(MAX) NULL
END
GO

UPDATE [RecordingServerService]
SET [passphrase] = N''
WHERE [passphrase] IS NULL
GO

ALTER TABLE [RecordingServerService]
ALTER COLUMN [passphrase] NVARCHAR(MAX) NOT NULL
GO
