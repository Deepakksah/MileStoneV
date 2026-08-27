-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[DeviceTrack]') AND name = N'InUse')
ALTER TABLE [DeviceTrack]
  ADD [InUse] [Bit] NULL
GO

UPDATE	[DeviceTrack]
	SET	[InUse] = 1
	WHERE [InUse] IS NULL
GO

ALTER TABLE [DeviceTrack] ALTER COLUMN [InUse] BIT NOT NULL
GO
