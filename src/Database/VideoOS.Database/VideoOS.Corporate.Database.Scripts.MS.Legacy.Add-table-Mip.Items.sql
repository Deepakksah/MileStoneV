-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mip].[Items]') AND type = 'U')
CREATE TABLE [Mip].[Items]
(
	[ItemId] UNIQUEIDENTIFIER NOT NULL,
	[Name] NVARCHAR(256),
	[Kind] UNIQUEIDENTIFIER NOT NULL,
	[ParentId] UNIQUEIDENTIFIER NOT NULL,
	[ServerId] UNIQUEIDENTIFIER NOT NULL,
	[ServerHost] NVARCHAR(256) NOT NULL,
	[ServerPort] INTEGER NOT NULL,
	[ServerType] NVARCHAR(256) NULL,
	[Enabled] BIT NOT NULL,
	[Properties] NVARCHAR(MAX),
	[GisPoint] GEOGRAPHY NOT NULL,
	[LastModified] DATETIME NOT NULL,
	PRIMARY KEY (ItemId, Kind)
)

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[Mip].[Items]') AND name = N'IX_Items_ItemId')
BEGIN
	CREATE INDEX [IX_Items_ItemId] ON [Mip].[Items]([ItemId])
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[Mip].[Items]') AND name = N'IX_Items_Kind')
BEGIN
	CREATE INDEX [IX_Items_Kind] ON [Mip].[Items]([Kind])
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[Mip].[Items]') AND name = N'IX_Items_GisPoint')
BEGIN
	CREATE SPATIAL INDEX [IX_Items_GisPoint] ON [Mip].[Items]([GisPoint])
END
GO

