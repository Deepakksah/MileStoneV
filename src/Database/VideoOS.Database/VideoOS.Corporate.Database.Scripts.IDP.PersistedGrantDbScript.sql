IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE MigrationId = N'20191017180132_InitialIdentityServerPersistedGrantDbMigration')
	BEGIN

	CREATE TABLE [PersistedGrants] (
		[Key] nvarchar(200) NOT NULL,
		[Type] nvarchar(50) NOT NULL,
		[SubjectId] nvarchar(200) NULL,
		[ClientId] nvarchar(200) NOT NULL,
		[CreationTime] datetime2 NOT NULL,
		[Expiration] datetime2 NULL,
		[Data] nvarchar(max) NOT NULL,
		CONSTRAINT [PK_PersistedGrants] PRIMARY KEY ([Key])
	);

	CREATE INDEX [IX_PersistedGrants_SubjectId_ClientId_Type] ON [PersistedGrants] ([SubjectId], [ClientId], [Type]);

	INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
	--VALUES (N'20191018081530_InitialIdentityServerPersistedGrantDbMigration', N'2.2.6-servicing-10079');
	VALUES (N'20191017180132_InitialIdentityServerPersistedGrantDbMigration', N'2.2.6-servicing-10079');
END

GO

