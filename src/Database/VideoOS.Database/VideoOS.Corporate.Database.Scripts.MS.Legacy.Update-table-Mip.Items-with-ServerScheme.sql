-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[Mip].[Items]') AND name = N'ServerScheme')
ALTER TABLE [Mip].[Items]
	ADD [ServerScheme] NVARCHAR(256) NULL
GO

UPDATE [Mip].[Items]
	SET [ServerScheme] = 'http' WHERE [ServerScheme] IS NULL
GO

ALTER TABLE [Mip].[Items] ALTER COLUMN [ServerScheme] NVARCHAR(256) NOT NULL
GO

