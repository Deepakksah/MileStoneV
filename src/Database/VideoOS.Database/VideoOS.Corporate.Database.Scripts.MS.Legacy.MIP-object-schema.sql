-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[MIPObject]') AND type in (N'U'))
DROP TABLE MIPObject
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[MIPObject]') AND type in (N'U'))
CREATE TABLE [MIPObject]
(
	[IDMIPObject] [UNIQUEIDENTIFIER] NOT NULL,
	[LastModified] [DATETIME] NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL,
	[Name] [NameType] NOT NULL,
	[Kind] [UNIQUEIDENTIFIER] NOT NULL,
	[SubKind] [UNIQUEIDENTIFIER] NOT NULL,
	[PluginId] [UNIQUEIDENTIFIER] NOT NULL,
	[CustomData] NVARCHAR(MAX)

	CONSTRAINT [PK_MIPObject] PRIMARY KEY NONCLUSTERED
	(
		[IDMIPObject] ASC
	),
) ON [PRIMARY]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ServerGroupKind]') AND type in (N'U'))
DROP TABLE ServerGroupKind
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ServerGroupKind]') AND type in (N'U'))
CREATE TABLE [ServerGroupKind]
(
	[IDServerGroup] [GuidType] NOT NULL,
	[Kind] [GuidType] NOT NULL,
	[PluginId] [GuidType] NOT NULL

	CONSTRAINT [PK_ServerGroupKind] PRIMARY KEY NONCLUSTERED
	(
		[IDServerGroup] ASC
	),
) ON [PRIMARY]
GO



