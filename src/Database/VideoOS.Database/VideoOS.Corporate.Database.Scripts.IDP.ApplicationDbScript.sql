IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE MigrationId = N'20191017190554_InitialIdentityServerApplicationDbMigration')
BEGIN
	CREATE TABLE [SecurityKey] (
		[Id] int NOT NULL IDENTITY,
		[KeyId] nvarchar(200) NULL,
		[Expiration] datetime2 NULL,
		[Created] datetime2 NOT NULL,
		[D] varbinary(256) NULL,
		[DP] varbinary(128) NULL,
		[DQ] varbinary(128) NULL,
		[Exponent] varbinary(3) NULL,
		[InverseQ] varbinary(128) NULL,
		[Modulus] varbinary(256) NULL,
		[P] varbinary(128) NULL,
		[Q] varbinary(128) NULL,
		CONSTRAINT [PK_SecurityKey] PRIMARY KEY ([Id])
	);


	INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
	--VALUES (N'20191018081605_InitialIdentityServerApplicationDbMigration', N'2.2.6-servicing-10079');
	VALUES (N'20191017190554_InitialIdentityServerApplicationDbMigration', N'2.2.6-servicing-10079');
END
GO

