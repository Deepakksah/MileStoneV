-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Create the new type (nvarchar(1024))
IF NOT EXISTS (SELECT 1 FROM dbo.systypes WHERE [name] = N'UriTypeShort')
EXEC sp_addtype N'UriTypeShort', N'NVARCHAR(1024)', N'not null'
GO

-- Update table
ALTER TABLE [Hardware] ALTER COLUMN [Uri] [UriTypeShort] NOT NULL
GO