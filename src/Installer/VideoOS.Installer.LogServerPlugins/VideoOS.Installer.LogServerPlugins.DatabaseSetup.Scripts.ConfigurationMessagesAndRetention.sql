CREATE TABLE [LogServer].[Configuration](
	[Name] [nvarchar](256) PRIMARY KEY,
	[Value] [nvarchar](max) NULL
)
GO

CREATE TABLE [LogServer].[Language](
	[Id] smallint IDENTITY(1,1) PRIMARY KEY,
	[Code] nvarchar(10) NOT NULL,
	CONSTRAINT [C_Language_Code] UNIQUE([Code]),  
)
GO

-- This contains both Source Type and Resource Type
CREATE TABLE [LogServer].[SourceType](
	[Id] nvarchar(400) PRIMARY KEY
)
GO

CREATE TABLE [LogServer].[SourceTypeText](
	[SourceTypeId] nvarchar(400) NOT NULL,
	[LanguageId] smallint NOT NULL,
	[Text] nvarchar(400),
	CONSTRAINT [FK_SourceTypeText_SourceType] FOREIGN KEY (SourceTypeId) REFERENCES [LogServer].[SourceType] (Id),
	CONSTRAINT [FK_SourceTypeText_Language] FOREIGN KEY (LanguageId) REFERENCES [LogServer].[Language] (Id),
	CONSTRAINT [PK_SourceTypeText] PRIMARY KEY CLUSTERED
	(
		[SourceTypeId] ASC,
		[LanguageId] ASC
	)
)
GO

CREATE TABLE [LogServer].[Category](
	[Id] nvarchar(400) PRIMARY KEY
)
GO

CREATE TABLE [LogServer].[CategoryText](
	[CategoryId] nvarchar(400) NOT NULL,
	[LanguageId] smallint NOT NULL,
	[Text] nvarchar(400),
	CONSTRAINT [FK_CategoryText_Category] FOREIGN KEY (CategoryId) REFERENCES [LogServer].[Category] (Id),
	CONSTRAINT [FK_CategoryText_Language] FOREIGN KEY (LanguageId) REFERENCES [LogServer].[Language] (Id),
	CONSTRAINT [PK_CategoryText] PRIMARY KEY CLUSTERED
	(
		[CategoryId] ASC,
		[LanguageId] ASC
	)
)
GO

CREATE TABLE [LogServer].[MessageType](
	[Id] uniqueidentifier PRIMARY KEY,
	[LogTypeId] tinyint NOT NULL,
	[SourceTypeId] [nvarchar](400),
	[CategoryId] [nvarchar](400),
	CONSTRAINT [FK_MessageType_SourceType] FOREIGN KEY (SourceTypeId) REFERENCES [LogServer].[SourceType] (Id),
	CONSTRAINT [FK_MessageType_Category] FOREIGN KEY (CategoryId) REFERENCES [LogServer].[Category] (Id),
)
GO

CREATE NONCLUSTERED INDEX [IX_Language_Code] ON [LogServer].[Language]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE TABLE [LogServer].[MessageText](
	[MessageTypeId] uniqueidentifier NOT NULL,
	[LanguageId] smallint NOT NULL,
	[Text] nvarchar(max),
	CONSTRAINT [FK_MessageText_MessageType] FOREIGN KEY (MessageTypeId) REFERENCES [LogServer].[MessageType] (Id),
	CONSTRAINT [FK_MessageText_Language] FOREIGN KEY (LanguageId) REFERENCES [LogServer].[Language] (Id),
	CONSTRAINT [PK_MessageText] PRIMARY KEY CLUSTERED
	(
		[LanguageId] ASC, -- LanguageId first in key gives best performance when joining in read function (using pk index)
		[MessageTypeId] ASC
	)
)
GO

CREATE TABLE [LogServer].[MessageTypeIdMapping](
	[LegacyId] nvarchar(400) PRIMARY KEY,
	[Id] uniqueidentifier NOT NULL,
	[LogLevel] smallint,
	CONSTRAINT MessageTypeIdMapping_Id UNIQUE([Id]),  
)
GO

CREATE TABLE [LogServer].[RetentionConfiguration](
	[LogTypeId] [tinyint] PRIMARY KEY,
	[RetentionDays] [int] NOT NULL
)
GO