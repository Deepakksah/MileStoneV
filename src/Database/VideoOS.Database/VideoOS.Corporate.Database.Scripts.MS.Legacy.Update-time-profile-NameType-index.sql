-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[TimeProfile]') AND name = 'IDX_NameType')
DROP INDEX IDX_NameType ON TimeProfile
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[TimeProfile]') AND name = 'IDX_NameType')
CREATE UNIQUE CLUSTERED INDEX IDX_NameType ON TimeProfile ([name]);
GO

