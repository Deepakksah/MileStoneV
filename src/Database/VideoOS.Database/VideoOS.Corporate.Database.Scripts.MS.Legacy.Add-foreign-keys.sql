-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Change Description from NTEXT NULL to NVARCHAR(MAX) NOT NULL
UPDATE [DeviceGroups] SET [Description] = '' WHERE [Description] IS NULL
ALTER TABLE [DeviceGroups] ALTER COLUMN [Description] NVARCHAR(MAX) NOT NULL
UPDATE [DeviceGroups] SET [Description] = [Description]
GO

-- FK_DeviceGroups_ParentDeviceGroup
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_DeviceGroups_ParentDeviceGroup]') AND parent_object_id = OBJECT_ID(N'[DeviceGroups]'))
ALTER TABLE [DeviceGroups] DROP CONSTRAINT [FK_DeviceGroups_ParentDeviceGroup]
GO

DELETE
	[DeviceGroups] 
WHERE 
	[IDParentDeviceGroup] IS NOT NULL 
	AND [IDParentDeviceGroup] NOT IN (SELECT [IDDeviceGroup] FROM [DeviceGroups])
GO

ALTER TABLE [DeviceGroups]  WITH CHECK ADD  CONSTRAINT [FK_DeviceGroups_ParentDeviceGroup] FOREIGN KEY([IDParentDeviceGroup])
REFERENCES [DeviceGroups] ([IDDeviceGroup])
GO

ALTER TABLE [DeviceGroups] CHECK CONSTRAINT [FK_DeviceGroups_ParentDeviceGroup]
GO

-- MediaDatabases --
-- Change Description from NTEXT NULL to NVARCHAR(MAX) NOT NULL
UPDATE [MediaDatabases] SET [Description] = '' WHERE [Description] IS NULL
ALTER TABLE [MediaDatabases] ALTER COLUMN [Description] NVARCHAR(MAX) NOT NULL
UPDATE [MediaDatabases] SET [Description] = [Description]

-- Change Path from NTEXT to NVARCHAR(MAX)
ALTER TABLE [MediaDatabases] ALTER COLUMN [Path] NVARCHAR(MAX) NOT NULL
UPDATE [MediaDatabases] SET [Path] = [Path]
GO

-- FK_Devices_MediaDatabases
ALTER TABLE [Devices] ALTER COLUMN [IDMediaDatabase] [GuidType] NULL
GO

UPDATE [Devices] SET [IDMediaDatabase] = NULL WHERE [IDMediaDatabase] = '00000000-0000-0000-0000-000000000000'
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_Devices_MediaDatabases]') AND parent_object_id = OBJECT_ID(N'[Devices]'))
ALTER TABLE [Devices] DROP CONSTRAINT [FK_Devices_MediaDatabases]
GO

ALTER TABLE [Devices]  WITH CHECK ADD  CONSTRAINT [FK_Devices_MediaDatabases] FOREIGN KEY([IDMediaDatabase])
REFERENCES [MediaDatabases] ([IDMediaDatabase])
GO

ALTER TABLE [Devices] CHECK CONSTRAINT [FK_Devices_MediaDatabases]
GO

-- ArchiveDatabases --

-- Change Description from NTEXT NULL to NVARCHAR(MAX) NOT NULL
UPDATE [ArchiveDatabases] SET [Description] = '' WHERE [Description] IS NULL
ALTER TABLE [ArchiveDatabases] ALTER COLUMN [Description] NVARCHAR(MAX) NOT NULL
UPDATE [ArchiveDatabases] SET [Description] = [Description]

-- Change Definition from NTEXT to NVARCHAR(MAX)
ALTER TABLE [ArchiveDatabases] ALTER COLUMN [Definition] NVARCHAR(MAX) NOT NULL
UPDATE [ArchiveDatabases] SET [Definition] = [Definition]
GO

-- FK_Devices_ArchiveDatabases
ALTER TABLE [Devices] ALTER COLUMN [IDArchiveDatabase] [GuidType] NULL
GO

UPDATE [Devices] SET [IDArchiveDatabase] = NULL WHERE [IDArchiveDatabase] = '00000000-0000-0000-0000-000000000000'
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_Devices_ArchiveDatabases]') AND parent_object_id = OBJECT_ID(N'[Devices]'))
ALTER TABLE [Devices] DROP CONSTRAINT [FK_Devices_ArchiveDatabases]
GO

ALTER TABLE [Devices]  WITH CHECK ADD  CONSTRAINT [FK_Devices_ArchiveDatabases] FOREIGN KEY([IDArchiveDatabase])
REFERENCES [ArchiveDatabases] ([IDArchiveDatabase])
GO

ALTER TABLE [Devices] CHECK CONSTRAINT [FK_Devices_ArchiveDatabases]
GO
