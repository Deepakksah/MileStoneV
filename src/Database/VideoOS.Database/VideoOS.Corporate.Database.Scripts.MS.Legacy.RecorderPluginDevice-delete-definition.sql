-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

ALTER TABLE [RecorderPluginDevice]
DROP COLUMN [Definition]
GO

ALTER TABLE [RecorderPluginDevice] 
ADD [Definition] NVARCHAR(1) NULL
GO

UPDATE [RecorderPluginDevice]
SET [Definition] = ''
GO

ALTER TABLE [RecorderPluginDevice]
ALTER COLUMN [Definition] NVARCHAR(1) NOT NULL
GO
