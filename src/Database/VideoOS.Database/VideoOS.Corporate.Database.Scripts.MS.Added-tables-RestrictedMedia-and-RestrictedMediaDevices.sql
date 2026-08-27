-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[RestrictedMedia]') AND [type] IN (N'U'))
CREATE TABLE [RestrictedMedia]
(
	[LastModified] DATETIME2 NOT NULL,
	[IDRestrictedMedia] UNIQUEIDENTIFIER NOT NULL,
	[Header] [LongNameType] NOT NULL,
	[Description] NVARCHAR(MAX) NOT NULL,
	[UserName] [NameType] NOT NULL,
	[StartTime] DATETIME2 NOT NULL,	
	[EndTime] DATETIME2 NOT NULL,

	CONSTRAINT [PK_RestrictedMedia] PRIMARY KEY NONCLUSTERED
	(
		[IDRestrictedMedia] ASC
	),
) ON [PRIMARY]
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[RestrictedMediaDevice]') AND [type] IN (N'U'))
CREATE TABLE [RestrictedMediaDevice]
(
	[IDRestrictedMedia] UNIQUEIDENTIFIER NOT NULL,
	[IDDevice] [GuidType] NOT NULL,

	CONSTRAINT [PK_RestrictedMediaDevice] PRIMARY KEY NONCLUSTERED
	(
		[IDRestrictedMedia], [IDDevice] ASC
	),
	
	CONSTRAINT [FK_RestrictedMediaDevice_RestrictedMedia] FOREIGN KEY([IDRestrictedMedia]) REFERENCES [RestrictedMedia] ([IDRestrictedMedia]),
	CONSTRAINT [FK_RestrictedMediaDevice_Devices] FOREIGN KEY([IDDevice]) REFERENCES [Devices] ([IDDevice])

) ON [PRIMARY]
GO



