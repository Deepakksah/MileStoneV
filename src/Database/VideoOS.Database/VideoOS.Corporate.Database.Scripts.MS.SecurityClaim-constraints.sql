-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SecurityClaim]') AND name = N'UQ_SecurityClaim_ClaimName')
BEGIN
	ALTER TABLE [SecurityClaim] DROP CONSTRAINT [UQ_SecurityClaim_ClaimName]
END
GO

IF EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SecurityClaim]') AND name = N'UQ_SecurityClaim_DisplayName')
BEGIN
	ALTER TABLE [SecurityClaim] DROP CONSTRAINT [UQ_SecurityClaim_DisplayName]
END
GO

ALTER TABLE [SecurityClaim] ALTER COLUMN [LoginProviderId] UNIQUEIDENTIFIER NOT NULL
ALTER TABLE [SecurityClaim] ALTER COLUMN [ClaimName]  NVARCHAR(450) COLLATE Latin1_General_100_BIN NOT NULL -- Same as before

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SecurityClaim]') AND name = N'UQ_SecurityClaim_ClaimName')
ALTER TABLE [SecurityClaim] WITH CHECK
ADD CONSTRAINT [UQ_SecurityClaim_ClaimName] UNIQUE NONCLUSTERED
(
	[ClaimName] ASC,
	[LoginProviderId] ASC
) ON [PRIMARY]
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SecurityClaim]') AND name = N'UQ_SecurityClaim_DisplayName')
ALTER TABLE [SecurityClaim] WITH CHECK
ADD CONSTRAINT [UQ_SecurityClaim_DisplayName] UNIQUE NONCLUSTERED
(
	[DisplayName] ASC,
	[LoginProviderId] ASC
) ON [PRIMARY]
GO


IF EXISTS(SELECT 1 FROM information_schema.table_constraints WHERE CONSTRAINT_TYPE = 'FOREIGN KEY' AND TABLE_NAME = 'SecurityRoleClaim' AND CONSTRAINT_NAME = 'FK_SecurityRoleClaim_SecurityClaim')
BEGIN
	ALTER TABLE [SecurityRoleClaim] DROP CONSTRAINT [FK_SecurityRoleClaim_SecurityClaim]
END
GO

IF EXISTS(SELECT 1 FROM information_schema.table_constraints WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'SecurityClaim' AND CONSTRAINT_NAME = 'PK_SecurityClaim')
BEGIN
	ALTER TABLE [SecurityClaim] DROP CONSTRAINT [PK_SecurityClaim]
END
GO


IF EXISTS(SELECT 1 FROM information_schema.table_constraints WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'SecurityRoleClaim' AND CONSTRAINT_NAME = 'PK_SecurityRoleClaim')
BEGIN
	ALTER TABLE [SecurityRoleClaim] DROP CONSTRAINT [PK_SecurityRoleClaim]
END
GO

ALTER TABLE [SecurityRoleClaim] ALTER COLUMN [IDRoleClaim] INT NOT NULL
ALTER TABLE [SecurityRoleClaim] ALTER COLUMN [IDClaim] INT NOT NULL
ALTER TABLE [SecurityClaim] ALTER COLUMN [IDClaim] INT NOT NULL
GO


IF NOT EXISTS(SELECT 1 FROM information_schema.table_constraints WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'SecurityRoleClaim' AND CONSTRAINT_NAME = 'PK_SecurityRoleClaim')
BEGIN
	ALTER TABLE [SecurityRoleClaim] ADD  CONSTRAINT [PK_SecurityRoleClaim] PRIMARY KEY CLUSTERED 
	(
		[IDRoleClaim] ASC
	) ON [PRIMARY]
END
GO

IF NOT EXISTS(SELECT 1 FROM information_schema.table_constraints WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'SecurityClaim' AND CONSTRAINT_NAME = 'PK_SecurityClaim')
BEGIN
	ALTER TABLE [SecurityClaim] ADD  CONSTRAINT [PK_SecurityClaim] PRIMARY KEY CLUSTERED 
	(
		[IDClaim] ASC
	) ON [PRIMARY]
END
GO

IF NOT EXISTS(SELECT 1 FROM information_schema.table_constraints WHERE CONSTRAINT_TYPE = 'FOREIGN KEY' AND TABLE_NAME = 'SecurityRoleClaim' AND CONSTRAINT_NAME = 'FK_SecurityRoleClaim_SecurityClaim')
BEGIN
	ALTER TABLE [SecurityRoleClaim]  WITH CHECK ADD  CONSTRAINT [FK_SecurityRoleClaim_SecurityClaim] FOREIGN KEY([IDClaim])
	REFERENCES [SecurityClaim] ([IDClaim])
END
GO

IF NOT EXISTS(SELECT 1 FROM information_schema.table_constraints WHERE CONSTRAINT_TYPE = 'FOREIGN KEY' AND TABLE_NAME = 'SecurityRoleClaim' AND CONSTRAINT_NAME = 'FK_SecurityRoleClaim_SecurityClaim')
BEGIN
	ALTER TABLE [SecurityRoleClaim] CHECK CONSTRAINT [FK_SecurityRoleClaim_SecurityClaim]
END
GO

