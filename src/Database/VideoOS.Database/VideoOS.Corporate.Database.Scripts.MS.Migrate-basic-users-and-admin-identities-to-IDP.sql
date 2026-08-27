-- This header is used by the dbupdate tool.
-- The TARGET poperty is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[MigrateIdpBasicUsers]') AND type = 'U')
BEGIN
	CREATE TABLE [MigrateIdpBasicUsers] 
	(
		[Id] [nvarchar](256) NOT NULL,
		[Sid] [varchar](256) NOT NULL,
		[Username] [nvarchar](256) NOT NULL,
		[PasswordHash] [varbinary](max) NOT NULL,
		[EncryptionMethod] [nvarchar](50) NOT NULL,
		[SaltValue] [nvarchar](256) NOT NULL,
		[Description] [nvarchar](max) NOT NULL,
		[CanChangePassword] [bit] NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		[ModifiedDate] [datetime] NOT NULL,
    )
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[MigrateIdpBasicUsers]') AND type = 'U')
BEGIN
	DELETE FROM [MigrateIdpBasicUsers]
	
	INSERT INTO [MigrateIdpBasicUsers]([Id], [Sid], [Username], [PasswordHash], [EncryptionMethod], [SaltValue], [Description], [CanChangePassword],[CreatedDate], [ModifiedDate])
	SELECT [id], [sid], [user_name], [password_hash], [encryption_method], [salt_value], [description], [can_change_password], [created_date], [last_modified]
	FROM [BasicUser]
END
GO


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[MigrateIdpAdminIdentities]') AND type = 'U')
BEGIN
	CREATE TABLE [MigrateIdpAdminIdentities] 
	(
		[Identity] [varchar](450) NOT NULL		
    )
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[MigrateIdpAdminIdentities]') AND type = 'U')
BEGIN
	DELETE FROM [MigrateIdpAdminIdentities]
	
	INSERT INTO [MigrateIdpAdminIdentities]([Identity])
	SELECT [SID]
	FROM [SecurityRolesIdentity] 
	WHERE [IDRole] = '1f30d14b-ba91-4e8a-8838-eda17931f8fc'
END
GO


