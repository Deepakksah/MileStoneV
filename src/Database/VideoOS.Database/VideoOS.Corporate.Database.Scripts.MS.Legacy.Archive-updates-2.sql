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
		name = N'framerate_reduction_enabled')
BEGIN
	ALTER TABLE [ArchiveStorage]
	ADD [framerate_reduction_enabled] BIT NOT NULL
END

IF EXISTS (
	SELECT * 
	FROM sys.columns 
	WHERE object_id = OBJECT_ID(N'[ArchiveStorage]') AND 
		name = N'skip_hours')
BEGIN
	ALTER TABLE [ArchiveStorage]
	DROP COLUMN [skip_hours]
END

GO
