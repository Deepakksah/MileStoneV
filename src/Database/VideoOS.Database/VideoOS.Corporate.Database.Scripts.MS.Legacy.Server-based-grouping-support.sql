-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

SET ANSI_PADDING ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ServerGroup]') AND type = 'U')
CREATE TABLE [ServerGroup](
	[IDServerGroup] [GuidType] NOT NULL,
	[Name] [NameType] NOT NULL,
	[Description] NVARCHAR(1024) NULL,
	[IDParentGroup] [GuidType] NOT NULL,
	[StructureType] VARCHAR(64) NOT NULL,
	[OrderNo] INT NOT NULL,
	[Level] INT NOT NULL,
	[Path] NVARCHAR(4000) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastModified] DATETIME NOT NULL,

	CONSTRAINT [PK_ServerGroup] PRIMARY KEY (
		[IDServerGroup] ASC
	)
) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[ServerGroup]') AND name = N'IX_ServerGroupParentName')
CREATE UNIQUE NONCLUSTERED INDEX [IX_ServerGroupParentName] ON [ServerGroup] 
(
	[IDParentGroup] ASC,
	[Name] ASC
) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ServerGroupMembership]') AND type = 'U')
CREATE TABLE [ServerGroupMembership](
	[IDServerGroup] [GuidType] NOT NULL,
	[IDObject] [GuidType] NOT NULL,
	[ObjectType] NVARCHAR(256) NOT NULL,

	CONSTRAINT [PK_ServerGroupMembership] PRIMARY KEY (
		[IDServerGroup] ASC,
		[IDObject] ASC
	),

	CONSTRAINT [FK_ServerGroupMembership_ServerGroup] FOREIGN KEY (
		[IDServerGroup]
	)
	REFERENCES [ServerGroup]([IDServerGroup])

) ON [PRIMARY]
GO
