-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[RecordingStorageDevice]') AND name = N'manual_timeout_enabled')
BEGIN
	ALTER TABLE [RecordingStorageDevice]
	ADD [manual_timeout_enabled] BIT NULL
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[RecordingStorageDevice]') AND name = N'manual_timeout')
BEGIN
	ALTER TABLE [RecordingStorageDevice]
	ADD [manual_timeout] INT NULL
END
GO

UPDATE [RecordingStorageDevice]
SET [manual_timeout_enabled] = 1, 
[manual_timeout] = 5
WHERE [manual_timeout] IS NULL OR [manual_timeout_enabled] IS NULL
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[RecordingStorageDevice]') AND name = N'manual_timeout_enabled')
BEGIN
	ALTER TABLE [RecordingStorageDevice] ALTER COLUMN [manual_timeout_enabled] BIT NOT NULL
END
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[RecordingStorageDevice]') AND name = N'manual_timeout')
BEGIN
	ALTER TABLE [RecordingStorageDevice] ALTER COLUMN [manual_timeout] INT NOT NULL
END
GO