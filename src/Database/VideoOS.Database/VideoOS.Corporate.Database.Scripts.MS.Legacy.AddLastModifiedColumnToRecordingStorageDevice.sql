-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[RecordingStorageDevice]') AND name = N'last_modified')
BEGIN
	ALTER TABLE [RecordingStorageDevice]
	ADD [last_modified] DATETIME NULL
END
GO

UPDATE [RecordingStorageDevice]
SET [last_modified] = [Devices].[LastModified]
FROM [RecordingStorageDevice]
INNER JOIN
	[Devices]
ON
	[RecordingStorageDevice].[device_id] = [Devices].[IDDevice]
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[RecordingStorageDevice]') AND name = N'last_modified')
BEGIN
	ALTER TABLE [RecordingStorageDevice] ALTER COLUMN [last_modified] DATETIME NOT NULL
END
GO