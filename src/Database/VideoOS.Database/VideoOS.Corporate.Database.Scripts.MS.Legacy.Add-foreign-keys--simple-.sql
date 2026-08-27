-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- FK_DeviceGroupMembers_DeviceGroups
DELETE [DeviceGroupMembers] WHERE [IDDeviceGroup] NOT IN (SELECT [IDDeviceGroup] FROM [DeviceGroups])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_DeviceGroupMembers_DeviceGroups]') AND parent_object_id = OBJECT_ID(N'[DeviceGroupMembers]'))
ALTER TABLE [DeviceGroupMembers] DROP CONSTRAINT [FK_DeviceGroupMembers_DeviceGroups]
GO

ALTER TABLE [DeviceGroupMembers]  WITH CHECK ADD  CONSTRAINT [FK_DeviceGroupMembers_DeviceGroups] FOREIGN KEY([IDDeviceGroup])
REFERENCES [DeviceGroups] ([IDDeviceGroup])
GO

ALTER TABLE [DeviceGroupMembers] CHECK CONSTRAINT [FK_DeviceGroupMembers_DeviceGroups]
GO


-- FK_DeviceGroupMembers_Devices
DELETE [DeviceGroupMembers] WHERE [IDDevice] NOT IN (SELECT [IDDevice] FROM [Devices])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_DeviceGroupMembers_Devices]') AND parent_object_id = OBJECT_ID(N'[DeviceGroupMembers]'))
ALTER TABLE [DeviceGroupMembers] DROP CONSTRAINT [FK_DeviceGroupMembers_Devices]
GO

ALTER TABLE [DeviceGroupMembers]  WITH CHECK ADD  CONSTRAINT [FK_DeviceGroupMembers_Devices] FOREIGN KEY([IDDevice])
REFERENCES [Devices] ([IDDevice])
GO

ALTER TABLE [DeviceGroupMembers] CHECK CONSTRAINT [FK_DeviceGroupMembers_Devices]
GO


-- FK_MediaDatabases_Recorders
DELETE [MediaDatabases] WHERE [IDRecorder] NOT IN (SELECT [IDRecorder] FROM [Recorders])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_MediaDatabases_Recorders]') AND parent_object_id = OBJECT_ID(N'[MediaDatabases]'))
ALTER TABLE [MediaDatabases] DROP CONSTRAINT [FK_MediaDatabases_Recorders]
GO

ALTER TABLE [MediaDatabases]  WITH CHECK ADD  CONSTRAINT [FK_MediaDatabases_Recorders] FOREIGN KEY([IDRecorder])
REFERENCES [Recorders] ([IDRecorder])
GO

ALTER TABLE [MediaDatabases] CHECK CONSTRAINT [FK_MediaDatabases_Recorders]
GO


-- FK_ArchiveDatabases_Recorders
DELETE [ArchiveDatabases] WHERE [IDRecorder] NOT IN (SELECT [IDRecorder] FROM [Recorders])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_ArchiveDatabases_Recorders]') AND parent_object_id = OBJECT_ID(N'[ArchiveDatabases]'))
ALTER TABLE [ArchiveDatabases] DROP CONSTRAINT [FK_ArchiveDatabases_Recorders]
GO

ALTER TABLE [ArchiveDatabases]  WITH CHECK ADD  CONSTRAINT [FK_ArchiveDatabases_Recorders] FOREIGN KEY([IDRecorder])
REFERENCES [Recorders] ([IDRecorder])
GO

ALTER TABLE [ArchiveDatabases] CHECK CONSTRAINT [FK_ArchiveDatabases_Recorders]
GO

-- FK_Hardware_Recorders
DELETE [Hardware] WHERE [IDRecorder] NOT IN (SELECT [IDRecorder] FROM [Recorders])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_Hardware_Recorders]') AND parent_object_id = OBJECT_ID(N'[Hardware]'))
ALTER TABLE [Hardware] DROP CONSTRAINT [FK_Hardware_Recorders]
GO

ALTER TABLE [Hardware]  WITH CHECK ADD  CONSTRAINT [FK_Hardware_Recorders] FOREIGN KEY([IDRecorder])
REFERENCES [Recorders] ([IDRecorder])
GO

ALTER TABLE [Hardware] CHECK CONSTRAINT [FK_Hardware_Recorders]
GO


-- FK_Devices_Hardware
DELETE [Devices] WHERE [IDHardware] NOT IN (SELECT [IDHardware] FROM [Hardware])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_Devices_Hardware]') AND parent_object_id = OBJECT_ID(N'[Devices]'))
ALTER TABLE [Devices] DROP CONSTRAINT [FK_Devices_Hardware]
GO

ALTER TABLE [Devices]  WITH CHECK ADD  CONSTRAINT [FK_Devices_Hardware] FOREIGN KEY([IDHardware])
REFERENCES [Hardware] ([IDHardware])
GO

ALTER TABLE [Devices] CHECK CONSTRAINT [FK_Devices_Hardware]
GO


-- FK_PatrollingProfiles_Devices
DELETE [PatrollingProfiles] WHERE [IDDevice] NOT IN (SELECT [IDDevice] FROM [Devices])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_PatrollingProfiles_Devices]') AND parent_object_id = OBJECT_ID(N'[PatrollingProfiles]'))
ALTER TABLE [PatrollingProfiles] DROP CONSTRAINT [FK_PatrollingProfiles_Devices]
GO

ALTER TABLE [PatrollingProfiles]  WITH CHECK ADD  CONSTRAINT [FK_PatrollingProfiles_Devices] FOREIGN KEY([IDDevice])
REFERENCES [Devices] ([IDDevice])
GO

ALTER TABLE [PatrollingProfiles] CHECK CONSTRAINT [FK_PatrollingProfiles_Devices]
GO


-- FK_DeviceLinks_Devices
DELETE [DeviceLinks] WHERE [IDDevice] NOT IN (SELECT [IDDevice] FROM [Devices])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_DeviceLinks_Devices]') AND parent_object_id = OBJECT_ID(N'[DeviceLinks]'))
ALTER TABLE [DeviceLinks] DROP CONSTRAINT [FK_DeviceLinks_Devices]
GO

ALTER TABLE [DeviceLinks]  WITH CHECK ADD  CONSTRAINT [FK_DeviceLinks_Devices] FOREIGN KEY([IDDevice])
REFERENCES [Devices] ([IDDevice])
GO

ALTER TABLE [DeviceLinks] CHECK CONSTRAINT [FK_DeviceLinks_Devices]
GO

-- FK_DeviceLinks_Devices_Linked
DELETE [DeviceLinks] WHERE [IDDeviceLinked] NOT IN (SELECT [IDDevice] FROM [Devices])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_DeviceLinks_Devices_Linked]') AND parent_object_id = OBJECT_ID(N'[DeviceLinks]'))
ALTER TABLE [DeviceLinks] DROP CONSTRAINT [FK_DeviceLinks_Devices_Linked]
GO

ALTER TABLE [DeviceLinks]  WITH CHECK ADD  CONSTRAINT [FK_DeviceLinks_Devices_Linked] FOREIGN KEY([IDDeviceLinked])
REFERENCES [Devices] ([IDDevice])
GO

ALTER TABLE [DeviceLinks] CHECK CONSTRAINT [FK_DeviceLinks_Devices_Linked]
GO

-- FK_Devices_RegisteredDeviceTypes
--DELETE [Devices] WHERE [DeviceType] NOT IN (SELECT [DeviceType] FROM [RegisteredDeviceTypes])
--GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_Devices_RegisteredDeviceTypes]') AND parent_object_id = OBJECT_ID(N'[Devices]'))
ALTER TABLE [Devices] DROP CONSTRAINT [FK_Devices_RegisteredDeviceTypes]
GO

ALTER TABLE [Devices]  WITH CHECK ADD  CONSTRAINT [FK_Devices_RegisteredDeviceTypes] FOREIGN KEY([DeviceType])
REFERENCES [RegisteredDeviceTypes] ([DeviceType])
GO

ALTER TABLE [Devices] CHECK CONSTRAINT [FK_Devices_RegisteredDeviceTypes]
GO
