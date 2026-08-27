-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mip].[Kinds]') AND type = 'U')
CREATE TABLE [Mip].[Kinds]
(
	[KindId] UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
	[Name] NVARCHAR(256) NOT NULL,
	[PluginId] UNIQUEIDENTIFIER NOT NULL,
	[PluginName] NVARCHAR(256),
	[DataVersion] INTEGER NOT NULL,
	[KindType] NVARCHAR(16) NOT NULL,
	[SecurityAction] NVARCHAR(256) NOT NULL,
	[DisplayOnGisMap] BIT NOT NULL,
	[ParentKindId] UNIQUEIDENTIFIER NOT NULL,
	[LastModified] DATETIME NOT NULL,
	[ModifiedUser] NVARCHAR(256) NOT NULL,
)

-- IdTable
IF NOT EXISTS (SELECT * FROM [sys].[table_types] WHERE user_type_id = Type_id(N'[Mip].[IdTable]'))
	CREATE TYPE [Mip].[IdTable]
	AS TABLE
	(
		Id UNIQUEIDENTIFIER NOT NULL
	)
GO

