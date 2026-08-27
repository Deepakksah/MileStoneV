IF NOT EXISTS (
	SELECT * 
	FROM sys.columns 
	WHERE object_id = OBJECT_ID(N'[LogAudit]') AND 
		name = N'ResourceType')
BEGIN
	ALTER TABLE [LogAudit]
	ADD [ResourceType] EnumType NOT NULL
	DEFAULT 'Unknown'
END

IF NOT EXISTS (
	SELECT * 
	FROM sys.columns 
	WHERE object_id = OBJECT_ID(N'[LogAudit]') AND 
		name = N'ResourceName')
BEGIN
	ALTER TABLE [LogAudit]
	ADD [ResourceName] NVARCHAR(512) NOT NULL
	DEFAULT ''
END

IF NOT EXISTS (
	SELECT * 
	FROM sys.columns 
	WHERE object_id = OBJECT_ID(N'[LogAudit]') AND 
		name = N'ResourceLocation')
BEGIN
	ALTER TABLE [LogAudit]
	ADD [ResourceLocation] NVARCHAR(512) NOT NULL
	DEFAULT ''
END

IF NOT EXISTS (
	SELECT * 
	FROM sys.columns 
	WHERE object_id = OBJECT_ID(N'[LogAudit]') AND 
		name = N'ResourceID')
BEGIN
	ALTER TABLE [LogAudit]
	ADD [ResourceID] GuidType NOT NULL
	DEFAULT '00000000-0000-0000-0000-000000000000'
END
GO

IF NOT EXISTS (
	SELECT * 
	FROM sys.columns 
	WHERE object_id = OBJECT_ID(N'[LogAudit]') AND 
		name = N'LogIDString')
BEGIN
	ALTER TABLE [LogAudit]
	ADD [LogIDString] nvarchar(512) NOT NULL
	DEFAULT ''
END

IF NOT EXISTS (
	SELECT * 
	FROM sys.columns 
	WHERE object_id = OBJECT_ID(N'[LogSystem]') AND 
		name = N'LogIDString')
BEGIN
	ALTER TABLE [LogSystem]
	ADD [LogIDString] nvarchar(512) NOT NULL
	DEFAULT ''
END
GO
