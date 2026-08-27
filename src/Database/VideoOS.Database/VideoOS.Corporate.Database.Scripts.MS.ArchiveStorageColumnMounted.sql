-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE [object_id] = OBJECT_ID(N'[ArchiveStorage]') AND [name] = N'mounted')
ALTER TABLE [ArchiveStorage]
  ADD [mounted] BIT NULL
GO

UPDATE  [ArchiveStorage]
SET     [mounted] = 1
WHERE   [mounted] IS NULL
GO

ALTER TABLE [ArchiveStorage] ALTER COLUMN [mounted] BIT NOT NULL
GO
