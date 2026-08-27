-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Add new column for ptz reserved name as anonymous 

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'PtzReservedAsAnonymous' and OBJECT_ID = OBJECT_ID(N'SecurityRoles'))
ALTER TABLE [SecurityRoles]
ADD [PtzReservedAsAnonymous] BIT NOT NULL DEFAULT 0
GO
