-- This header is used by the dbupdate tool.
-- The TARGET poperty is used to indicate which database the script is
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

 -- Drop references to assembly
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[C18NDigest]') AND [type] = 'FS')
DROP FUNCTION [dbo].[C18NDigest]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateTLSData]') AND [type] = 'FS')
DROP FUNCTION [dbo].[CreateTLSData]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EncryptData]') AND [type] = 'FS')
DROP FUNCTION [dbo].[EncryptData]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateXMLEncryption]') AND [type] = 'FS')
DROP FUNCTION [dbo].[UpdateXMLEncryption]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateCustomSettingXMLEncryption]') AND [type] = 'FS')
DROP FUNCTION [dbo].[UpdateCustomSettingXMLEncryption]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateClientProfileXMLEncryption]') AND [type] = 'FS')
DROP FUNCTION [dbo].[UpdateClientProfileXMLEncryption]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateTaskXMLDatabaseSetEncryptionPassword]') AND [type] = 'FS')
DROP FUNCTION [dbo].[UpdateTaskXMLDatabaseSetEncryptionPassword]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateTaskXMLSystemChangeHardwarePassword]') AND [type] = 'FS')
DROP FUNCTION [dbo].[UpdateTaskXMLSystemChangeHardwarePassword]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateEncryption]') AND [type] = 'FS')
DROP FUNCTION [dbo].[UpdateEncryption]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateHardwarePasswordEncryption]') AND [type] = 'FS')
DROP FUNCTION [dbo].[UpdateHardwarePasswordEncryption]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateEncryptionActivationCredentials]') AND [type] = 'FS')
DROP FUNCTION [dbo].[UpdateEncryptionActivationCredentials]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateMIPLicensesXMLEncryption]') AND [type] = 'FS')
DROP FUNCTION [dbo].[UpdateMIPLicensesXMLEncryption]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateMIPRegistrationsXMLEncryption]') AND [type] = 'FS')
DROP FUNCTION [dbo].[UpdateMIPRegistrationsXMLEncryption]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateEncryptionSystemSettings]') AND [type] = 'FS')
DROP FUNCTION [dbo].[UpdateEncryptionSystemSettings]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Encrypt]') AND [type] = 'FS')
DROP FUNCTION [dbo].[Encrypt]
GO

-- Removes the function and assembly as it is no longer needed
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EncryptData]') AND [type] = 'FS')
DROP FUNCTION [dbo].[EncryptData]
GO

-- Drop the assembly itself
IF EXISTS(SELECT * FROM sys.assemblies asms WHERE asms.name = N'VideoOS.SqlServer.SqlTypes')
DROP ASSEMBLY [VideoOS.SqlServer.SqlTypes]
GO

-- Disable TRUSTWORTHY to disallow load of 'Unsafe' assembly
-- This was required by the "C18NDigest" user-defined function
ALTER DATABASE [{DatabaseName}] SET TRUSTWORTHY OFF
GO

-- Disable CLR
EXEC sp_configure 'clr enabled' , '0';
RECONFIGURE;
GO