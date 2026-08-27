IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].dbo.[MigrateIdpBasicUsers]') AND type in (N'U'))
BEGIN
	IF NOT EXISTS(SELECT * FROM [{target}].sys.objects WHERE object_id = OBJECT_ID(N'[{target}].dbo.[TempCorporateBasicUsers]') AND type in (N'U'))
	BEGIN
		CREATE TABLE [{target}].dbo.[TempCorporateBasicUsers] 
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
	ELSE
	BEGIN
		DELETE FROM [{target}].dbo.[TempCorporateBasicUsers]
	END
END
GO

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].dbo.[MigrateIdpBasicUsers]') AND type in (N'U'))
BEGIN
	IF EXISTS(SELECT * FROM [{target}].sys.objects WHERE object_id = OBJECT_ID(N'[{target}].dbo.[TempCorporateBasicUsers]') AND type in (N'U'))
	BEGIN
		INSERT INTO [{target}].dbo.[TempCorporateBasicUsers] ([Id], [Sid], [Username], [PasswordHash], [EncryptionMethod], [SaltValue], [Description], [CanChangePassword],[CreatedDate], [ModifiedDate] ) 
		SELECT [Id], [Sid], [Username], [PasswordHash], [EncryptionMethod], [SaltValue], [Description], [CanChangePassword], [CreatedDate], [ModifiedDate]
		FROM [{source}].dbo.[MigrateIdpBasicUsers]
	END
END
GO

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].dbo.[MigrateIdpBasicUsers]') AND type in (N'U'))
BEGIN
	DROP TABLE [{source}].dbo.[MigrateIdpBasicUsers]
END
GO


IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].dbo.[MigrateIdpAdminIdentities]') AND type in (N'U'))
BEGIN
	IF NOT EXISTS(SELECT * FROM [{target}].sys.objects WHERE object_id = OBJECT_ID(N'[{target}].dbo.[TempCorporateAdminIdentities]') AND type in (N'U'))
	BEGIN
		CREATE TABLE [{target}].dbo.[TempCorporateAdminIdentities] 
		(
			[Identity] [varchar](450) NOT NULL
		)
	END
	ELSE
	BEGIN
		DELETE FROM [{target}].dbo.[TempCorporateAdminIdentities]
	END
END
GO

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].dbo.[MigrateIdpAdminIdentities]') AND type in (N'U'))
BEGIN
	IF EXISTS(SELECT * FROM [{target}].sys.objects WHERE object_id = OBJECT_ID(N'[{target}].dbo.[TempCorporateAdminIdentities]') AND type in (N'U'))
	BEGIN
		INSERT INTO [{target}].dbo.[TempCorporateAdminIdentities] ([Identity]) 
		SELECT [Identity]
		FROM [{source}].dbo.[MigrateIdpAdminIdentities]
	END
END
GO

IF EXISTS(SELECT * FROM [{source}].sys.objects WHERE object_id = OBJECT_ID(N'[{source}].dbo.[MigrateIdpAdminIdentities]') AND type in (N'U'))
BEGIN
	DROP TABLE [{source}].dbo.[MigrateIdpAdminIdentities]
END
GO

