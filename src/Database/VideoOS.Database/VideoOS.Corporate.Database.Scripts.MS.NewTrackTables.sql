-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[DeviceTrackDefinition]') AND [type] IN (N'U'))
CREATE TABLE [DeviceTrackDefinition]
(
	[IDTrackDefinition] UNIQUEIDENTIFIER NOT NULL,
	[IsPrimary] BIT NOT NULL,	
	[IsEdge] BIT NOT NULL,	

	CONSTRAINT [PK_DeviceTrackDefinition] PRIMARY KEY NONCLUSTERED
	(
		[IDTrackDefinition] ASC
	),

) ON [PRIMARY]
GO


IF NOT EXISTS(SELECT [IDTrackDefinition] FROM [DeviceTrackDefinition])
BEGIN
	INSERT INTO [DeviceTrackDefinition] ([IDTrackDefinition], [IsPrimary], [IsEdge])
	VALUES('16CE3AA1-5F93-458A-ABE5-5C95D9ED1372', 1, 0)

	INSERT INTO [DeviceTrackDefinition] ([IDTrackDefinition], [IsPrimary], [IsEdge])
	VALUES('DEB2EB69-599C-416D-9557-3C7225D3F0E0', 0, 1)
END
GO


IF NOT EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[DeviceTrack]') AND [type] IN (N'U'))
CREATE TABLE [DeviceTrack]
(
	[IDDevice] GuidType NOT NULL,	
	[IDTrack] UNIQUEIDENTIFIER NOT NULL,
	[IDTrackDefinition] UNIQUEIDENTIFIER NOT NULL,
	[MergeEdge] BIT NOT NULL,	
	[DefaultPlayback] BIT NOT NULL,	

	CONSTRAINT [PK_DeviceTrack] PRIMARY KEY NONCLUSTERED
	(
		[IDTrack] ASC
	),

	CONSTRAINT [UC_TrackDefinition_Device] UNIQUE
	(
		[IDDevice],
		[IDTrackDefinition]
		
	),

	CONSTRAINT [FK_DeviceTrack_TrackDefinition] FOREIGN KEY([IDTrackDefinition]) REFERENCES [DeviceTrackDefinition] ([IDTrackDefinition]),
	CONSTRAINT [FK_DeviceTrack_Devices] FOREIGN KEY([IDDevice]) REFERENCES [Devices] ([IDDevice])

) ON [PRIMARY]
GO


IF EXISTS (SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'[Devices]') AND [name] = N'EdgeTrackId')
BEGIN
	EXEC sp_executesql N'INSERT INTO [DeviceTrack] ([IDDevice], [IDTrack], [IDTrackDefinition], [MergeEdge], [DefaultPlayback])
	SELECT [IDDevice], [IDDevice], ''16CE3AA1-5F93-458A-ABE5-5C95D9ED1372'', 1, 1 FROM [Devices]
	WHERE [EdgeTrackId] <> ''00000000-0000-0000-0000-000000000000'''

	EXEC sp_executesql N'INSERT INTO [DeviceTrack] ([IDDevice], [IDTrack], [IDTrackDefinition], [MergeEdge], [DefaultPlayback])
	SELECT [IDDevice], [EdgeTrackId], ''DEB2EB69-599C-416D-9557-3C7225D3F0E0'', 0, 0 FROM [Devices]
	WHERE [EdgeTrackId] <> ''00000000-0000-0000-0000-000000000000'''

	ALTER TABLE [Devices] DROP COLUMN [EdgeTrackId]
END
GO



