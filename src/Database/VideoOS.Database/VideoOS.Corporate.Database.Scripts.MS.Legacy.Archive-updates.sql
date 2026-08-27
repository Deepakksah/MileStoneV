-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (
	SELECT * 
	FROM sys.columns 
	WHERE object_id = OBJECT_ID(N'[ArchiveStorage]') AND 
		name = N'folder')
BEGIN
	ALTER TABLE [ArchiveStorage]
	ADD [folder] [GuidType] NOT NULL
END

IF NOT EXISTS (
	SELECT * 
	FROM sys.columns 
	WHERE object_id = OBJECT_ID(N'[ArchiveStorage]') AND 
		name = N'skip_hours')
BEGIN
	ALTER TABLE [ArchiveStorage]
	ADD [skip_hours] INT NOT NULL
END

GO
