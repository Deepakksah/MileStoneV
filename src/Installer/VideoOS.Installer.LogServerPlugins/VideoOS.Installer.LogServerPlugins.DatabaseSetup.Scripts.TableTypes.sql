-- user-defined table types used for stored procedures

CREATE TYPE [LogServer].[MessageTextTable]
AS TABLE
(
	[LanguageCode] nvarchar(10) PRIMARY KEY,
	[MessageText] nvarchar(max) NOT NULL
)
GO


CREATE TYPE [LogServer].[MessageTypeTable]
AS TABLE
(
	[MessageTypeId] uniqueidentifier NOT NULL,
	[LogTypeId] tinyint NOT NULL,
	[LanguageCode] nvarchar(10) NOT NULL,
	[MessageText] nvarchar(max) NOT NULL,
	[SourceTypeId] nvarchar(400),
	[CategoryId] nvarchar(400)
	PRIMARY KEY
	(
		[MessageTypeId],
		[LanguageCode]
	)
)
GO

CREATE TYPE [LogServer].[SourceTypeTextTable]
AS TABLE
(
	[LanguageCode] nvarchar(10) PRIMARY KEY,
	[SourceTypeText] nvarchar(400) NOT NULL
)
GO


CREATE TYPE [LogServer].[SourceTypeTable]
AS TABLE
(
	[SourceTypeId] nvarchar(400) NOT NULL,
	[LanguageCode] nvarchar(10) NOT NULL,
	[SourceTypeText] nvarchar(400) NOT NULL,
	PRIMARY KEY
	(
		[SourceTypeId],
		[LanguageCode]
	)
)
GO

CREATE TYPE [LogServer].[CategoryTextTable]
AS TABLE
(
	[LanguageCode] nvarchar(10) PRIMARY KEY,
	[CategoryText] nvarchar(400) NOT NULL
)
GO


CREATE TYPE [LogServer].[CategoryTable]
AS TABLE
(
	[CategoryId] nvarchar(400) NOT NULL,
	[LanguageCode] nvarchar(10) NOT NULL,
	[CategoryText] nvarchar(400) NOT NULL,
	PRIMARY KEY
	(
		[CategoryId],
		[LanguageCode]
	)
)
GO

CREATE TYPE [LogServer].[LegacyMessageTypeTable]
AS TABLE
(
	[MessageTypeId] nvarchar(400) NOT NULL,
	[LogTypeId] tinyint NOT NULL,
	[LanguageCode] nvarchar(10) NOT NULL,
	[MessageText] nvarchar(max) NOT NULL,
	[SourceTypeId] nvarchar(400),
	[CategoryId] nvarchar(400),
	[LogLevel] smallint,
	PRIMARY KEY
	(
		[MessageTypeId],
		[LanguageCode]
	)
)
GO

CREATE TYPE [LogServer].[MessageParameterTable]
AS TABLE
(
	[Name] nvarchar(128) PRIMARY KEY,
	[Value] nvarchar(400) NOT NULL
)
GO

CREATE TYPE [LogServer].[MessageInstancePageTable] AS TABLE(
	[Id] [bigint] NOT NULL,
	[ClientTimestampUtc] [datetime] NOT NULL,
	[SourceTypeId] [nvarchar](400) NULL,
	[CategoryId] [nvarchar](400) NULL,
	[Active] [bit] NULL,
	PRIMARY KEY
	(
		[Id] ASC
	)
)
GO

CREATE TYPE [LogServer].[MessageFilterTable] AS TABLE(
	[Name] [nvarchar](50) NOT NULL,
	[Value] [nvarchar](400) NOT NULL
)
GO

CREATE TYPE [LogServer].[ConfigurationTable] AS TABLE(
	[Name] [nvarchar](256) NOT NULL,
	[Value] [nvarchar](max) NULL
)
GO
