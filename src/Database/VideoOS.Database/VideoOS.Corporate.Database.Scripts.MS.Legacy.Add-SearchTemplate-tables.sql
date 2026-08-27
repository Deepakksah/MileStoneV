-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'Search')
	EXECUTE('CREATE SCHEMA[Search]')
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Search].[SearchTemplates]') AND type in (N'U'))
CREATE TABLE [Search].[SearchTemplates] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Description] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Availability] nvarchar(40) NOT NULL,
    [IncludesScopeItems] bit NOT NULL,
    [SearchQuery] varbinary(max) NOT NULL,
    [Owner] [SIDType] NOT NULL,
    CONSTRAINT [PK_SearchTemplates] PRIMARY KEY ([Id])
);
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[Search].[SearchTemplates]') AND name = N'IX_SearchTemplates_Name')
CREATE INDEX [IX_SearchTemplates_Name] ON [Search].[SearchTemplates] ([Name] ASC) 
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Search].[SearchTemplatesFind]') AND type in (N'U'))
CREATE TABLE [Search].[SearchTemplatesFind] (
    [Id] uniqueidentifier NOT NULL,
    [Count] int NOT NULL,
    [StartIndex] int NOT NULL,
    [KeywordFilter] nvarchar(256),
    [PrivateOnly] bit NOT NULL,
	[CreateDate] datetime2 NOT NULL,
    [Owner] [SIDType] NOT NULL,
    CONSTRAINT [PK_FindSearchTemplates] PRIMARY KEY ([Id])
);
GO
