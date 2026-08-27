-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Adds column LastCredentialUpdate to Hardware table
IF NOT EXISTS (
	SELECT * 
	FROM sys.columns 
	WHERE object_id = OBJECT_ID(N'[Hardware]') AND 
		name = N'CredentialsLastModified')
BEGIN
	ALTER TABLE [Hardware]
		ADD [CredentialsLastModified] datetime NULL
END
GO