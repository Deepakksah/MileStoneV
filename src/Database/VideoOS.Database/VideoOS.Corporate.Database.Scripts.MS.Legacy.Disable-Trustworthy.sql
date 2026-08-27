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

-- Removed C18NDigest UDF - this is only used during upgrade
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[C18NDigest]') AND [type] = 'FS')
DROP FUNCTION [dbo].[C18NDigest]
GO

-- Change the permissions for the assembly
IF EXISTS (SELECT * FROM sys.assemblies WHERE name = 'VideoOS.SqlServer.SqlTypes')
ALTER ASSEMBLY [VideoOS.SqlServer.SqlTypes]
WITH PERMISSION_SET = SAFE
GO

-- Disable TRUSTWORTHY to disallow load of 'Unsafe' assembly
-- This was required by the "C18NDigest" user-defined function
ALTER DATABASE [{DatabaseName}] SET TRUSTWORTHY OFF
GO
