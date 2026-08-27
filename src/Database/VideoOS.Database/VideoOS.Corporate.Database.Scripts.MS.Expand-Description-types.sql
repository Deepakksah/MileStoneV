-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM dbo.systypes WHERE [name] = N'DescriptionType2')
EXEC sp_addtype N'DescriptionType2', N'NVARCHAR(4000)', N'not null'
GO

ALTER TABLE dbo.RestrictedMedia
  ALTER COLUMN Description DescriptionType2;
GO