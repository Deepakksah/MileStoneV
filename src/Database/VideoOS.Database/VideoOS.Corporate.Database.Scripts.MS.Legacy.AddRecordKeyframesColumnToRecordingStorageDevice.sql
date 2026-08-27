-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[RecordingStorageDevice]') AND name = N'record_keyframes')
BEGIN
	ALTER TABLE [RecordingStorageDevice]
	ADD [record_keyframes] BIT NULL
END
GO

UPDATE [RecordingStorageDevice]
SET [record_keyframes] = 0
WHERE [record_keyframes] IS NULL
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[RecordingStorageDevice]') AND name = N'record_keyframes')
BEGIN
	ALTER TABLE [RecordingStorageDevice] ALTER COLUMN [record_keyframes] BIT NOT NULL
END
GO

-- Fixing an older bug from 6.0.00
IF EXISTS (SELECT [edgestorage_playback] FROM [RecordingStorageDevice] WHERE [edgestorage_playback] IS NULL)
BEGIN
	UPDATE [RecordingStorageDevice]
	SET [edgestorage_playback] = 0
	WHERE [edgestorage_playback] IS NULL
END
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[RecordingStorageDevice]') AND name = N'edgestorage_playback')
BEGIN
	ALTER TABLE [RecordingStorageDevice] ALTER COLUMN [edgestorage_playback] BIT NOT NULL
END
GO