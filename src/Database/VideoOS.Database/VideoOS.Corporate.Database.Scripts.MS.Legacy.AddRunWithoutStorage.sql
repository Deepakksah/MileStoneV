-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- The SUPPORTS_TRANSACTION property specifies if the scripts supports
-- running in multi-statement transaction or if it should be running
-- using implicit transactions
-- =========================================================================
-- TARGET: Surveillance
-- SUPPORTS_TRANSACTION: TRUE
-- =========================================================================

-- Add column for running with offline storage
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[RecordingServerServiceConfigurable]') AND name = N'ShutdownOnStorageFailure')
ALTER TABLE [RecordingServerServiceConfigurable]
  ADD [ShutdownOnStorageFailure] [Bit] NULL
GO

UPDATE	[RecordingServerServiceConfigurable]
	SET	[ShutdownOnStorageFailure] = 0
	WHERE [ShutdownOnStorageFailure] IS NULL
GO

ALTER TABLE [RecordingServerServiceConfigurable] ALTER COLUMN [ShutdownOnStorageFailure] BIT NOT NULL
GO
