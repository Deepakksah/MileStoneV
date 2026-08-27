-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF EXISTS(SELECT * FROM [SystemSettings] WHERE [Name] = 'MediaDBCertificate')
BEGIN
	DELETE FROM SystemSettings WHERE [Name] = 'MediaDBCertificate'
END
GO