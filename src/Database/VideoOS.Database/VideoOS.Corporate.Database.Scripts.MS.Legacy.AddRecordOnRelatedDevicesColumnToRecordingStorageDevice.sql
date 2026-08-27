-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[RecordingStorageDevice]') AND name = N'record_on_related_devices')
BEGIN
	ALTER TABLE [RecordingStorageDevice]
	ADD [record_on_related_devices] BIT NULL
END
GO

UPDATE [RecordingStorageDevice]
SET [record_on_related_devices] = 0
WHERE [record_on_related_devices] IS NULL
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[RecordingStorageDevice]') AND name = N'record_on_related_devices')
BEGIN
	ALTER TABLE [RecordingStorageDevice] ALTER COLUMN [record_on_related_devices] BIT NOT NULL
END
GO