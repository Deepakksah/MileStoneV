-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Adds Encrypted column to Mip.[IntegrationRegistrations] table
IF NOT EXISTS (
	SELECT * 
	FROM sys.columns 
	WHERE object_id = OBJECT_ID(N'[Mip].[IntegrationRegistrations]') AND 
		name = N'AllowIntegrationRegistration')
BEGIN
	ALTER TABLE [Mip].[IntegrationRegistrations]
	ADD [AllowIntegrationRegistration] bit NOT NULL
	DEFAULT 0
END
GO