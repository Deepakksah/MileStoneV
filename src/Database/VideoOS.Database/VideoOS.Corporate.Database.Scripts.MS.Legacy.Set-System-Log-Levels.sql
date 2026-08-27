-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT Name FROM SystemSettings WHERE Name = 'LogSystemCanUseLevels')
BEGIN
	INSERT INTO SystemSettings
	VALUES (GetUtcDate(), NewId(), 'LogSystemCanUseLevels', 'True')
END

IF NOT EXISTS(SELECT Name FROM SystemSettings WHERE Name = 'LogSystemLevel')
BEGIN
	INSERT INTO SystemSettings
	VALUES (GetUtcDate(), NewId(), 'LogSystemLevel', '3')
END

GO
