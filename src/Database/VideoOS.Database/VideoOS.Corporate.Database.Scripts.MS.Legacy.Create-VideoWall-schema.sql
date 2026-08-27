-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[VideoWall]') AND type in (N'U'))
BEGIN
	CREATE TABLE [VideoWall] (
		[pkid] [GuidType] NOT NULL,
		[name] [NameType] NOT NULL,
		[description] NVARCHAR(MAX),
		[width] FLOAT NOT NULL,
		[height] FLOAT NOT NULL,
		[created_date] DATETIME NOT NULL,
		[last_modified] DATETIME NOT NULL,
		
		CONSTRAINT [PK_VideoWall] PRIMARY KEY NONCLUSTERED
		(
			[pkid] ASC
		)
	) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[VideoWall]') AND name = N'IX_UniqueName')
CREATE UNIQUE CLUSTERED INDEX [IX_UniqueName] ON [VideoWall]
(
	[name] ASC
) ON [PRIMARY]
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[VideoWallPreset]') AND type in (N'U'))
BEGIN
	CREATE TABLE [VideoWallPreset] (
		[pkid] [GuidType] NOT NULL,
		[videowall_id] [GuidType] NOT NULL,
		[name] [NameType] NOT NULL,
		[description] NVARCHAR(MAX),
		[created_date] DATETIME NOT NULL,
		[last_modified] DATETIME NOT NULL,
		
		CONSTRAINT [PK_VideoWallPreset] PRIMARY KEY NONCLUSTERED
		(
			[pkid] ASC
		),
		
		CONSTRAINT [FK_VideoWallPreset_VideoWall] FOREIGN KEY
		(
			[videowall_id]
		)
		REFERENCES [VideoWall] ([pkid])

	) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[VideoWallPreset]') AND name = N'IX_UniqueName')
CREATE UNIQUE CLUSTERED INDEX [IX_UniqueName] ON [VideoWallPreset]
(
	[videowall_id] ASC,
	[name] ASC
) ON [PRIMARY]
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[VideoWallViewLayout]') AND type in (N'U'))
BEGIN
	CREATE TABLE [VideoWallViewLayout] (
		[pkid] [GuidType] NOT NULL,
		[name] [NameType] NOT NULL,
		[description] NVARCHAR(MAX) NULL,
		[created_date] DATETIME NOT NULL,
		[last_modified] DATETIME NOT NULL,
		[definition_xml] XML NOT NULL,

		CONSTRAINT [PK_VideoWallViewLayout] PRIMARY KEY NONCLUSTERED
		(
			[pkid] ASC
		)
	) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[VideoWallViewLayout]') AND name = N'IX_UniqueName')
CREATE UNIQUE CLUSTERED INDEX [IX_UniqueName] ON [VideoWallViewLayout]
(
	[name] ASC
) ON [PRIMARY]
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[VideoWallView]') AND type in (N'U'))
BEGIN
	CREATE TABLE [VideoWallView] (
		[pkid] [GuidType] NOT NULL,
		[name] [NameType] NOT NULL,
		[description] NVARCHAR(MAX) NULL,
		[created_date] DATETIME NOT NULL,
		[last_modified] DATETIME NOT NULL,
		[definition_xml] XML NOT NULL,

		CONSTRAINT [PK_VideoWallView] PRIMARY KEY NONCLUSTERED
		(
			[pkid] ASC
		)
	) ON [PRIMARY]
END
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[VideoWallViewItem]') AND type in (N'U'))
BEGIN
	CREATE TABLE [VideoWallViewItem] (
		[pkid] [GuidType] NOT NULL,
		[view_id] [GuidType] NOT NULL,
		[position] INT NOT NULL,
		[definition_xml] XML NOT NULL,

		CONSTRAINT [PK_VideoWallViewItem] PRIMARY KEY NONCLUSTERED
		(
			[pkid] ASC
		),

		CONSTRAINT [FK_VideoWallViewItem_View] FOREIGN KEY
		(
			[view_id]
		)
		REFERENCES [VideoWallView] ([pkid])
	) ON [PRIMARY]
END
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[VideoWallMonitor]') AND type in (N'U'))
BEGIN
	CREATE TABLE [VideoWallMonitor] (
		[pkid] [GuidType] NOT NULL,
		[videowall_id] [GuidType] NOT NULL,
		[view_id] [GuidType] NOT NULL,
		[name] [NameType] NOT NULL,
		[description] NVARCHAR(MAX),
		[monitor_size] REAL NOT NULL,
		[aspect_ratio] VARCHAR(50),
		[insertion_method] VARCHAR(50),
		[no_layout_method] VARCHAR(50),
		[empty_viewitems_method] VARCHAR(50),
		[location_x] FLOAT NOT NULL,
		[location_y] FLOAT NOT NULL,
		[created_date] DATETIME NOT NULL,
		[last_modified] DATETIME NOT NULL,

		CONSTRAINT [PK_VideoWallMonitor] PRIMARY KEY NONCLUSTERED
		(
			[pkid] ASC
		),

		CONSTRAINT [FK_VideoWallMonitor_VideoWall] FOREIGN KEY
		(
			[videowall_id]
		)
		REFERENCES [VideoWall] ([pkid]),
		
		CONSTRAINT [FK_VideoWallMonitor_View] FOREIGN KEY
		(
			[view_id]
		)
		REFERENCES [VideoWallView] ([pkid])

	) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[VideoWallMonitor]') AND name = N'IX_UniqueName')
CREATE UNIQUE CLUSTERED INDEX [IX_UniqueName] ON [VideoWallMonitor]
(
	[videowall_id] ASC,
	[name] ASC
) ON [PRIMARY]
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[VideoWallMonitorPreset]') AND type in (N'U'))
BEGIN
	CREATE TABLE [VideoWallMonitorPreset] (
		[pkid] [GuidType] NOT NULL,
		[monitor_id] [GuidType] NOT NULL,
		[videowallpreset_id] [GuidType] NOT NULL,
		[last_modified] DATETIME NOT NULL,
		[definition_xml] XML NOT NULL,

		CONSTRAINT [PK_VideoWallMonitorPreset] PRIMARY KEY NONCLUSTERED
		(
			[pkid] ASC
		),

		CONSTRAINT [FK_VideoWallMonitorPreset_Monitor] FOREIGN KEY
		(
			[monitor_id]
		)
		REFERENCES [VideoWallMonitor] ([pkid]),

		CONSTRAINT [FK_VideoWallMonitorPreset_VideoWallPreset] FOREIGN KEY
		(
			[videowallpreset_id]
		)
		REFERENCES [VideoWallPreset] ([pkid])

	) ON [PRIMARY]
END
GO
