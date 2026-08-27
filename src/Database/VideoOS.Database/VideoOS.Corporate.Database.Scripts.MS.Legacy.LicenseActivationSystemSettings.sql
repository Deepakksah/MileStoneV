-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


IF NOT EXISTS(SELECT Name FROM SystemSettings WHERE Name = 'ActivationCredentials')
BEGIN
	INSERT INTO SystemSettings
	VALUES (GetUtcDate(), 'ActivationCredentials', '')
END
GO

IF NOT EXISTS(SELECT Name FROM SystemSettings WHERE Name = 'EnableAutomaticActivation')
BEGIN
	INSERT INTO SystemSettings
	VALUES (GetUtcDate(), 'EnableAutomaticActivation', 'false')
END
GO

