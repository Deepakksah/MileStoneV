-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- The SUPPORTS_TRANSACTION property specifies if the scripts supports
-- running in multi-statement transaction or if it should be running
-- using implicit transactions
-- =========================================================================
-- TARGET: Surveillance
-- SUPPORTS_TRANSACTION: FALSE
-- =========================================================================

EXEC sp_configure 'clr enabled' , '1';
RECONFIGURE;
GO

UPDATE [dbo].[Hardware] 
SET EncryptedPassword = [dbo].[EncryptData](EncryptedPassword, IDHardware)
WHERE LEN([dbo].[EncryptData](EncryptedPassword, IDHardware)) < 256
GO

EXEC sp_configure 'clr enabled' , '0';
RECONFIGURE;
