-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF  EXISTS (
	SELECT * 
	FROM sys.columns 
	WHERE object_id = OBJECT_ID(N'[MarkedData]') AND  name = N'IDRetention')
BEGIN
	EXEC('ALTER TABLE [MarkedData] DROP COLUMN [IDRetention]')
END
GO

IF  EXISTS (
	SELECT * 
	FROM sys.columns 
	WHERE object_id = OBJECT_ID(N'[MarkedData]') AND  name = N'Protected')
BEGIN
	EXEC('ALTER TABLE [MarkedData] DROP COLUMN [Protected]')
END
GO

