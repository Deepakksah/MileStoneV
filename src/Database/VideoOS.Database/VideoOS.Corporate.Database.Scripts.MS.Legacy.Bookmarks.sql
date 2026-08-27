-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Bookmarks]') AND type in (N'U'))
CREATE TABLE [Bookmarks]
(
	[IDBookmark] [GuidType] NOT NULL,
	[IDDevice] [GuidType] NOT NULL,
	[Reference] [NameType] NOT NULL,
	[User] [NameType] NOT NULL,
	[Header] [LongNameType] NOT NULL,
	[Description] NVARCHAR(MAX) NOT NULL,
	[TimeBegin] DATETIME NOT NULL,
	[TimeTrigged] DATETIME NOT NULL,
	[TimeEnd] DATETIME NOT NULL,

	CONSTRAINT [PK_Bookmarks] PRIMARY KEY NONCLUSTERED
	(
		[IDBookmark] ASC
	),
	
	CONSTRAINT [FK_Bookmarks_Devices] FOREIGN KEY([IDDevice]) REFERENCES [Devices] ([IDDevice])

) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Bookmarks]') AND name = N'IX_Bookmarks_IDDevice_ TimeTrigger')
CREATE CLUSTERED INDEX [IX_Bookmarks_IDDevice_ TimeTrigger] ON [dbo].[Bookmarks] ( [IDDevice] ASC, [TimeTrigged] ASC ) 
GO



