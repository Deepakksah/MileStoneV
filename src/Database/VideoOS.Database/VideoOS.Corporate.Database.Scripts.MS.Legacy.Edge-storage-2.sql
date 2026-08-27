---- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (
	SELECT * 
	FROM sys.columns 
	WHERE object_id = OBJECT_ID(N'[RecordingStorageDevice]') AND 
		name = N'edgestorage_enabled')
BEGIN
	ALTER TABLE [RecordingStorageDevice] ADD [edgestorage_enabled] BIT NULL
END
GO

UPDATE [RecordingStorageDevice] SET [edgestorage_enabled] = 0 WHERE [edgestorage_enabled] IS NULL
GO

ALTER TABLE [RecordingStorageDevice] ALTER COLUMN [edgestorage_enabled] BIT NOT NULL
GO
