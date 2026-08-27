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

DELETE FROM [License] WHERE IDLicense NOT IN ('94f58776-84bd-44db-b19b-851f1d53a832', '968FBC5E-99BD-4E08-B711-8A73D2A37461', 'b1cb8385-ec7e-492b-b046-1b84e378cb71')
GO

UPDATE [License]
SET [Data] = [dbo].[UpdateEncryption]([Data], 'RC2-software', '{MasterEncryptionKey}'),
	[LastModified] = CURRENT_TIMESTAMP
WHERE IDLicense = '94f58776-84bd-44db-b19b-851f1d53a832'
GO

UPDATE [License]
SET [Data] = [dbo].[UpdateEncryption]([Data], 'Rijndael', '{MasterEncryptionKey}'),
	[LastModified] = CURRENT_TIMESTAMP
WHERE IDLicense = '968FBC5E-99BD-4E08-B711-8A73D2A37461'
GO

UPDATE [License]
SET [Data] = [dbo].[Encrypt]([Data], '{MasterEncryptionKey}'),
	[LastModified] = CURRENT_TIMESTAMP
WHERE IDLicense = 'b1cb8385-ec7e-492b-b046-1b84e378cb71'
GO

EXEC sp_configure 'clr enabled' , '0';
RECONFIGURE;
GO