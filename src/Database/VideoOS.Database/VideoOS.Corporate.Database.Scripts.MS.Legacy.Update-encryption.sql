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

UPDATE [RecordingStorage]
SET [encryption_password] = [dbo].[UpdateEncryption]([encryption_password], 'RC2-recordingstorage', '{MasterEncryptionKey}')
WHERE NULLIF([encryption_password],'') IS NOT NULL
GO


UPDATE [RecordingStorage]
SET [encryption_password_new] = [dbo].[UpdateEncryption]([encryption_password_new], 'RC2-recordingstorage', '{MasterEncryptionKey}')
WHERE NULLIF([encryption_password_new],'') IS NOT NULL
GO

UPDATE [ClientProfile]
SET [Settings] = [dbo].[UpdateClientProfileXMLEncryption](CONVERT(NVARCHAR(MAX), [Settings]), '{MasterEncryptionKey}')
WHERE NULLIF(CONVERT(NVARCHAR(MAX), [Settings]),'') IS NOT NULL
GO

UPDATE [Task]
SET data_a = [dbo].[UpdateTaskXMLDatabaseSetEncryptionPassword](CONVERT(NVARCHAR(MAX), [data_a]), '{MasterEncryptionKey}')
WHERE [type] = 'Database.SetEncryptionPassword'
GO

UPDATE [Task]
SET data_a = [dbo].[UpdateTaskXMLSystemChangeHardwarePassword](CONVERT(NVARCHAR(MAX), [data_a]), '{MasterEncryptionKey}')
WHERE [type] = 'System.ChangeHardwarePassword'
GO

DELETE FROM [Task]
WHERE [type] IN ('System.AddHardware','System.HardwareScan', 'System.HardwareScanExpress', 'System.ActivateLicense', 'diffgram_update', 'diffgram_insert', 'diffgram_delete')
GO

UPDATE [CustomSettings]
SET [SettingDataXml] = [dbo].[UpdateCustomSettingXMLEncryption]([SettingDataXml], '{MasterEncryptionKey}')
GO

UPDATE [Hardware]
SET [EncryptedPassword] = [dbo].[UpdateHardwarePasswordEncryption]([EncryptedPassword], [IDHardware], '{MasterEncryptionKey}')
WHERE NULLIF([EncryptedPassword],'') IS NOT NULL
GO

UPDATE [Hardware]
SET [RemoteAdminPassword] = [dbo].[UpdateEncryption]([RemoteAdminPassword], 'RC2-software', '{MasterEncryptionKey}')
WHERE NULLIF([RemoteAdminPassword],'') IS NOT NULL
GO

UPDATE [SystemSettings]
SET [Setting] = [dbo].[UpdateEncryptionActivationCredentials](CONVERT(NVARCHAR(MAX), [Setting]), '{MasterEncryptionKey}')
WHERE [Name] = 'ActivationCredentials' and NULLIF([Setting], '') IS NOT NULL
GO

UPDATE [SystemSettings]
SET [Setting] = [dbo].[UpdateEncryptionSystemSettings](CONVERT(NVARCHAR(MAX), [Setting]), '{MasterEncryptionKey}')
WHERE [Name] = 'SmtpPassword' and NULLIF([Setting], '') IS NOT NULL
GO

UPDATE [SystemSettings]
SET [Setting] = [dbo].[UpdateEncryptionSystemSettings](CONVERT(NVARCHAR(MAX), [Setting]), '{MasterEncryptionKey}')
WHERE [Name] = 'AVHSPass' and NULLIF([Setting], '') IS NOT NULL
GO

UPDATE [HostedHardware]
SET [password] = [dbo].[Encrypt](CONVERT(NVARCHAR(MAX), [password]), '{MasterEncryptionKey}')
WHERE NULLIF([password], '') IS NOT NULL
GO

UPDATE [HostingServer]
SET [password] = [dbo].[Encrypt](CONVERT(NVARCHAR(MAX), [password]), '{MasterEncryptionKey}')
WHERE NULLIF([password], '') IS NOT NULL
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_Hardware_Definition]') AND parent_object_id = OBJECT_ID(N'[Hardware]'))
BEGIN
	ALTER TABLE [Hardware] 
	DROP CONSTRAINT  FK_Hardware_Definition;
END
GO

DECLARE @ConstraintsName NVARCHAR(MAX)
SELECT @ConstraintsName = [Name]
FROM sys.key_constraints
WHERE [type] = 'PK'
AND [parent_object_id] = OBJECT_ID('[HardwareDefinition]');

IF (@ConstraintsName IS NOT NULL)
BEGIN
	DECLARE @DropConstraintsSQL NVARCHAR(MAX)
	SELECT @DropConstraintsSQL = 'ALTER TABLE [HardwareDefinition]'
					           + ' DROP CONSTRAINT [' + @ConstraintsName + ']'
		
	EXEC(@DropConstraintsSQL)
END
GO

-- Add unique index to Hardware.C18NDigest
IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'Hardware') AND name = N'IX_C18NDigest')
DROP INDEX [IX_C18NDigest] ON [Hardware] 
GO

ALTER TABLE [Hardware]
ALTER COLUMN [C18NDigest] CHAR(44) NOT NULL
GO

ALTER TABLE HardwareDefinition
ALTER COLUMN [C18NDigest] CHAR(44) NOT NULL
GO

DECLARE @tmpTable AS TABLE (
		[C18NDigest] CHAR(24),
		[NewC18NDigest] CHAR(44)
	)

INSERT @tmpTable (C18NDigest, [NewC18NDigest])
SELECT hd.C18NDigest, dbo.[C18NDigest](hd.[Definition])
FROM [HardwareDefinition] hd

UPDATE hd
SET hd.[C18NDigest] = tmp.[NewC18NDigest]
FROM [HardwareDefinition] hd
INNER JOIN @tmpTable tmp ON tmp.[C18NDigest] = hd.[C18NDigest]

UPDATE h
SET h.[C18NDigest] = tmp.[NewC18NDigest]
FROM [Hardware] h
INNER JOIN @tmpTable tmp ON tmp.[C18NDigest] = h.[C18NDigest]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[PK_Hardware_Definition]') AND parent_object_id = OBJECT_ID(N'[HardwareDefinition]'))
BEGIN
	ALTER TABLE [HardwareDefinition] WITH CHECK
	ADD CONSTRAINT [PK_Hardware_Definition]
	PRIMARY KEY([C18NDigest])
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_Hardware_Definition]') AND parent_object_id = OBJECT_ID(N'[Hardware]'))
BEGIN
	ALTER TABLE [Hardware] WITH CHECK
	ADD CONSTRAINT [FK_Hardware_Definition]
	FOREIGN KEY([C18NDigest])
	REFERENCES [HardwareDefinition] ([C18NDigest])
END
GO

-- Add unique index to Hardware.C18NDigest
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'Hardware') AND name = N'IX_C18NDigest')
CREATE NONCLUSTERED INDEX [IX_C18NDigest] ON [Hardware] ([C18NDigest]) ON [PRIMARY]
GO

DECLARE @LicenseIdOld [GuidType] = 'E145597A-ECAF-40A1-99E6-14BB9D45A09E';
UPDATE [Mip].[Items]
SET [Properties] = [dbo].[UpdateMIPLicensesXMLEncryption]([Properties], '{MasterEncryptionKey}'), [Encrypted] = 1
WHERE [ItemId] = @LicenseIdOld
GO

DECLARE @RegistrationIdOld [GuidType] = 'E125597A-ECAF-40A1-99E6-24BB9D45A09E';
UPDATE [Mip].[Items]
SET [Properties] = [dbo].[UpdateMIPRegistrationsXMLEncryption]([Properties], '{MasterEncryptionKey}'), [Encrypted] = 1
WHERE [ItemId] = @RegistrationIdOld
GO

EXEC sp_configure 'clr enabled' , '0';
RECONFIGURE;
GO