-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


-- Add new Column to SecurityRoles table to allow logging on from mobile clients
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'AllowWebClientLogOn' and OBJECT_ID = OBJECT_ID(N'SecurityRoles'))
ALTER TABLE SecurityRoles
ADD [AllowWebClientLogOn] BIT NOT NULL DEFAULT 1
GO
