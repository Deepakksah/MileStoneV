-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

ALTER TABLE [SecurityRoles]
ADD CONSTRAINT BuiltinDefault DEFAULT (0) FOR [Builtin]
GO

ALTER TABLE [SecurityRoles]
ADD CONSTRAINT EnabledDefault DEFAULT (1) FOR [Enabled]
GO

IF NOT EXISTS(SELECT * FROM sys.columns where object_id = OBJECT_ID('SecurityRoles') AND name = 'Type')
ALTER TABLE [SecurityRoles]
ADD [Type] TINYINT NULL
GO

UPDATE [SecurityRoles]
SET [Type] = 2
GO

ALTER TABLE [SecurityRoles]
ALTER COLUMN [Type] TINYINT NOT NULL
GO

-- Make sure the administrator role gets the correct type
UPDATE [SecurityRoles]
SET [Type] = 0
WHERE [IDRole] = '1f30d14b-ba91-4e8a-8838-eda17931f8fc'
GO

-- Delete orphaned users. This means users who are members of deleted groups.
DELETE [SecurityRolesIdentity]
WHERE [IDRole] NOT IN (SELECT [IDRole] FROM [SecurityRoles])
GO

-- Add foreign key constraints between SecurityRoles and SecurityRolesIdentity
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_SecurityRolesIdentity_SecurityRoles]') AND parent_object_id = OBJECT_ID(N'[SecurityRolesIdentity]'))
ALTER TABLE [SecurityRolesIdentity] WITH CHECK 
ADD CONSTRAINT [FK_SecurityRolesIdentity_SecurityRoles] FOREIGN KEY([IDRole])
REFERENCES [SecurityRoles] ([IDRole])
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SecurityRoles]') AND name = 'IX_SecurityRoles')
CREATE UNIQUE NONCLUSTERED INDEX [IX_SecurityRoles] ON [dbo].[SecurityRoles] 
(
	[Name] ASC
) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityMembership]') AND type = 'U')
BEGIN
	CREATE TABLE [SecurityMembership](
		[Container] VARCHAR(256) NOT NULL,
		[MemberSID] [SIDType] NOT NULL,

		CONSTRAINT [PK_SecurityMembership] PRIMARY KEY CLUSTERED
		(
			[Container] ASC,
			[MemberSID] ASC
		),

		CONSTRAINT [FK_SecurityIdentity_SecurityMembership_Group] FOREIGN KEY( [Container] )
		REFERENCES SecurityIdentity([SID]),

		CONSTRAINT [FK_SecurityIdentity_SecurityMembership_Identity] FOREIGN KEY( [MemberSID] )
		REFERENCES SecurityIdentity([SID])

	) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityMembershipCache]') AND type = 'U')
BEGIN
	CREATE TABLE [dbo].[SecurityMembershipCache](
		[MemberSID] [SIDType] NOT NULL,
		[GroupSID] [SIDType] NOT NULL,

		CONSTRAINT [PK_SecurityMemberCache] PRIMARY KEY CLUSTERED 
		(
			[MemberSID] ASC,
			[GroupSID] ASC
		),

		CONSTRAINT [FK_SecurityMemberCache_SecurityIdentity_Member] FOREIGN KEY( [MemberSID] )
		REFERENCES [dbo].[SecurityIdentity] ([SID]),

		CONSTRAINT [FK_SecurityMemberCache_SecurityIdentity_Group] FOREIGN KEY( [GroupSID] )
		REFERENCES [dbo].[SecurityIdentity] ([SID])

	) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityActions]') AND type = 'U')
BEGIN
	CREATE TABLE [dbo].[SecurityActions](
		[IDClass] [varchar](128) NOT NULL,
		[IDAction] [varchar](128) NOT NULL,
		[OrderNo] [tinyint] NOT NULL,
		[ShowAction] [bit] NOT NULL,

		CONSTRAINT [PK_SecurityActions] PRIMARY KEY CLUSTERED 
		(
			[IDClass] ASC,
			[IDAction] ASC
		),

		CONSTRAINT [UQ_SecurityActions] UNIQUE NONCLUSTERED
		(
			[IDClass] ASC,
			[OrderNo] ASC
		)
	) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityObjects]') AND type = 'U')
BEGIN
	CREATE TABLE [SecurityObjects](
		[IDSecurityObject] [GuidType] NOT NULL,
		[IDObject] [GuidType] NOT NULL,
		[IDClass] [varchar](128) NOT NULL,
		[IDInheritsFrom] [GuidType] NULL,

		CONSTRAINT [PK_SecurityObjects] PRIMARY KEY CLUSTERED 
		(
			[IDSecurityObject] ASC
		)
	) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityAccessEntries]') AND type = 'U')
BEGIN
	CREATE TABLE [dbo].[SecurityAccessEntries](
		[IDAccessEntry] [int] IDENTITY(1,1) NOT NULL,
		[IDSecurityObject] [GuidType] NOT NULL,
		[IDAction] [varchar](128) NOT NULL,
		[SID] [SIDType] NOT NULL,
		[AccessDeny] [bit] NOT NULL,
		[Deleted] [bit] NOT NULL,

		CONSTRAINT [PK_SecurityAccess] PRIMARY KEY CLUSTERED 
		(
			[IDAccessEntry] ASC
		),

		CONSTRAINT [IX_SecurityAccessConstraint] UNIQUE NONCLUSTERED 
		(
			[IDSecurityObject] ASC,
			[IDAction] ASC,
			[SID] ASC,
			[AccessDeny] ASC
		),

		CONSTRAINT [FK_SecurityAccess_SecurityIdentity] FOREIGN KEY( [SID] )
		REFERENCES [SecurityIdentity] ([SID]),

		CONSTRAINT [FK_SecurityAccess_SecurityObjects] FOREIGN KEY( [IDSecurityObject] )
		REFERENCES [SecurityObjects] ([IDSecurityObject])
	) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SecurityAccessEntries]') AND name = N'IX_SecurityAccessIDSecurityObject')
CREATE NONCLUSTERED INDEX [IX_SecurityAccessIDSecurityObject] ON [SecurityAccessEntries] 
(
	[IDSecurityObject] ASC
) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityAccessCache]') AND type = 'U')
BEGIN
	CREATE TABLE [SecurityAccessCache](
		[IDObject] [GuidType] NOT NULL,
		[IDAction] [varchar](128) NOT NULL,
		[SID] [SIDType] NOT NULL,
		[AllowedAccess] [bit] NOT NULL,

		CONSTRAINT [PK_SecurityAccessCache] PRIMARY KEY CLUSTERED 
		(
			[IDObject] ASC,
			[IDAction] ASC,
			[SID] ASC
		),

		CONSTRAINT [FK_SecurityAccessCache_SecurityIdentity] FOREIGN KEY( [SID] )
		REFERENCES [SecurityIdentity] ([SID])

	) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[TempSecurityIdentities]') AND type = 'U')
BEGIN
	CREATE TABLE [TempSecurityIdentities](
		[SPID] [int] NOT NULL,
		[SID] [SIDType] NOT NULL,
		[Type] [tinyint] NULL,
		[DisplayName] [nvarchar](512) NULL,
		[Description] [nvarchar](2048) NULL,
		[Domain] [nvarchar](512) NULL,
		[AccountName] [nvarchar](512) NULL,
		[DistinguishedName] [nvarchar](2044) NULL,

		CONSTRAINT [PK_TempSecurityIdentities] PRIMARY KEY CLUSTERED
		(
			[SPID] ASC,
			[SID] ASC
		)
	) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[TempSecurityDeletedIdentities]') AND type = 'U')
BEGIN
	CREATE TABLE [TempSecurityDeletedIdentities]
	(
		[SPID] [int] NOT NULL,
		[SID] [SIDType] NOT NULL,
		[used] [bit] NULL,

		CONSTRAINT [PK_TempSecurityDeletedIdentities] PRIMARY KEY CLUSTERED
		(
			[SPID] ASC,
			[SID] ASC
		)
	) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[TempSecurityAccessEntries]') AND type = 'U')
BEGIN
	CREATE TABLE [TempSecurityAccessEntries]
	(
		[SPID] [int] NOT NULL,
		[SID] [SIDType] NOT NULL,
		[IDAction] [varchar](128) NOT NULL,
		[AccessDeny] [bit] NOT NULL,
		[Change] [tinyint] NULL,

		CONSTRAINT [PK_TempSecurityAccessEntries] PRIMARY KEY CLUSTERED
		(
			[SPID] ASC,
			[SID] ASC,
			[IDAction] ASC,
			[AccessDeny] ASC
		)
	) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityTableLock]') AND type = 'U')
BEGIN
	CREATE TABLE [SecurityTableLock]
	(
		[LockLevel] [int] NOT NULL,
		[LastUpdated] [datetime] NULL,

		CONSTRAINT [PK_security_update_lock] PRIMARY KEY CLUSTERED 
		(
			[LockLevel] ASC
		)
	) ON [PRIMARY]
END
GO

IF NOT EXISTS(SELECT * FROM [SecurityTableLock] WHERE [LockLevel] = 0)
INSERT INTO [SecurityTableLock] ( [LockLevel] )
VALUES(0)
GO

IF NOT EXISTS(SELECT * FROM [SecurityTableLock] WHERE [LockLevel] = 1)
INSERT INTO [SecurityTableLock] ( [LockLevel] )
VALUES(1)
GO