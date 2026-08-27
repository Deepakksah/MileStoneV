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

-- Removes the function and assembly as it is no longer needed
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EncryptData]') AND [type] = 'FS')
DROP FUNCTION [dbo].[EncryptData]
GO

IF EXISTS(SELECT * FROM sys.assemblies asms WHERE asms.name = N'VideoOS.SqlServer.SqlTypes')
DROP ASSEMBLY [VideoOS.SqlServer.SqlTypes]
GO

-- Disable TRUSTWORTHY to disallow load of 'Unsafe' assembly
-- This was required by the "C18NDigest" user-defined function
 ALTER DATABASE [{DatabaseName}] SET TRUSTWORTHY OFF
 GO

EXEC sp_configure 'clr enabled' , '0';
RECONFIGURE;
