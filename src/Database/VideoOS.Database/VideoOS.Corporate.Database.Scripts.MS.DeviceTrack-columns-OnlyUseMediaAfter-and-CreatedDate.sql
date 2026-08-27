-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[DeviceTrack]') AND name = N'OnlyUseMediaAfter')
ALTER TABLE [DeviceTrack]
  ADD [OnlyUseMediaAfter] DATETIME2 NULL,
      [CreatedDate] DATETIME2 NULL
GO

DECLARE @CurrentUtcTime DATETIME2 = SYSUTCDATETIME()

UPDATE	[DeviceTrack]
	SET	[OnlyUseMediaAfter] = @CurrentUtcTime, [CreatedDate] = @CurrentUtcTime
	WHERE [OnlyUseMediaAfter] IS NULL
GO

ALTER TABLE [DeviceTrack] ALTER COLUMN [OnlyUseMediaAfter] DATETIME2 NOT NULL
ALTER TABLE [DeviceTrack] ALTER COLUMN [CreatedDate] DATETIME2 NOT NULL

GO
