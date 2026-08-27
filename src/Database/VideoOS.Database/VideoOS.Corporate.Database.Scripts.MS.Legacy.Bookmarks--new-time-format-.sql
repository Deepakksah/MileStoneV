-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Bookmarks]') AND type = 'U')
CREATE TABLE [Bookmarks]
(
	[IDBookmark] [dbo].[GuidType] NOT NULL,
	[IDDevice] [dbo].[GuidType] NOT NULL,
	[Reference] [dbo].[NameType] NOT NULL,
	[User] [dbo].[NameType] NOT NULL,
	[Header] [dbo].[LongNameType] NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[TimeBegin] [bigint] NOT NULL,
	[TimeTrigged] [bigint] NOT NULL,
	[TimeEnd] [bigint] NOT NULL,

	CONSTRAINT [PK_Bookmarks] PRIMARY KEY NONCLUSTERED
	(
		[IDBookmark] ASC
	),
	
	CONSTRAINT [FK_Bookmarks_Devices] FOREIGN KEY([IDDevice]) REFERENCES [Devices] ([IDDevice]) ON DELETE CASCADE

) ON [PRIMARY]

GO


IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Bookmarks]') AND name = N'IX_Bookmarks_IDDevice_ TimeTrigger')
CREATE CLUSTERED INDEX [IX_Bookmarks_IDDevice_ TimeTrigger] ON [dbo].[Bookmarks] ( [IDDevice] ASC, [TimeTrigged] ASC ) 
GO



