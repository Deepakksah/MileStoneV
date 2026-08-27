-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- ***************************************
-- Change obsolete types to current types
-- ***************************************
ALTER TABLE [Compressors]
ALTER COLUMN [ConfigData] VARBINARY(MAX)
GO

ALTER TABLE [CustomSettings]
ALTER COLUMN [SettingDataXml] NVARCHAR(MAX)
GO

ALTER TABLE [License]
ALTER COLUMN [Data] NVARCHAR(MAX)
GO

ALTER TABLE [NotificationSettings]
ALTER COLUMN [Settings] NVARCHAR(MAX)
GO

ALTER TABLE [PatrollingProfiles]
ALTER COLUMN [Definition] NVARCHAR(MAX)
GO

ALTER TABLE [SecurityAttributes]
ALTER COLUMN [Definition] NVARCHAR(MAX)
GO

ALTER TABLE [SystemSettings]
ALTER COLUMN [Setting] NVARCHAR(MAX)
GO

-- ***************************************
-- Remove unused stored procedues
-- ***************************************
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[client_get_viewgroupdata]') AND type = 'P')
DROP PROCEDURE [client_get_viewgroupdata]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[client_query_viewgroups]') AND type = 'P')
DROP PROCEDURE [client_query_viewgroups]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[client_query_viewgroup_by_id]') AND type = 'P')
DROP PROCEDURE [client_query_viewgroup_by_id]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[client_set_viewgroupdata]') AND type = 'P')
DROP PROCEDURE [client_set_viewgroupdata]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_legacy_delete_role_permissions]') AND type = 'P')
DROP PROCEDURE [security_legacy_delete_role_permissions]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_legacy_HasTrustedServerPermission]') AND type = 'P')
DROP PROCEDURE [security_legacy_HasTrustedServerPermission]
GO

-- ***************************************
-- Remove duplicate indexes
-- ***************************************
IF EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SecurityRoles]') AND name = N'IX_RoleName')
DROP INDEX [IX_RoleName] ON [SecurityRoles]
GO

IF EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[EventTypes]') AND name = N'IX_EventNameGeneratorType')
DROP INDEX [IX_EventNameGeneratorType] ON [EventTypes]
GO

IF EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[AuthorizationCookie]') AND name = N'UQ__Authoriz__09019C16DCF4C797')
ALTER TABLE [AuthorizationCookie] DROP CONSTRAINT [UQ__Authoriz__09019C166DD3710A]
GO

-- ***************************************
-- Index foreign keys
-- ***************************************
IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[RecordingStorageDevice]') AND name = N'IX_FK_RecordingStorageDevice_RecordingStorage')
CREATE INDEX [IX_FK_RecordingStorageDevice_RecordingStorage] ON [RecordingStorageDevice] ([recordingstorage_id])
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SecurityPermissionsDevice]') AND name = N'IX_FK_SecurityPermissionsDevice_Devices')
CREATE INDEX [IX_FK_SecurityPermissionsDevice_Devices] ON [SecurityPermissionsDevice] ([IDDevice])
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SecurityPermissionsDevice]') AND name = N'IX_FK_SecurityPermissionsDevice_TimeProfile')
CREATE INDEX [IX_FK_SecurityPermissionsDevice_TimeProfile] ON [SecurityPermissionsDevice] ([IDTimeProfile])
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SecurityPermissionsEnterpriseServer]') AND name = N'IX_FK_SecurityPermissionsEnterpriseServer_EnterpriseServers')
CREATE INDEX [IX_FK_SecurityPermissionsEnterpriseServer_EnterpriseServers] ON [SecurityPermissionsEnterpriseServer] ([IDEnterpriseServer])
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SecurityPermissionsEnterpriseServer]') AND name = N'IX_FK_SecurityPermissionsEnterpriseServer_SecurityRole')
CREATE INDEX [IX_FK_SecurityPermissionsEnterpriseServer_SecurityRole] ON [SecurityPermissionsEnterpriseServer] ([IDRole])
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[HostedHardware]') AND name = N'IX_FK_HostedHardware_HostingServer')
CREATE INDEX [IX_FK_HostedHardware_HostingServer] ON [HostedHardware] ([hosting_server_id])
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[Hardware]') AND name = N'IX_FK_Hardware_HostedHardware')
CREATE INDEX [IX_FK_Hardware_HostedHardware] ON [Hardware] ([HostedHardware_id])
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SecurityPermissionsMatrix]') AND name = N'IX_FK_SecurityPermissionsMatrix_Matrix')
CREATE INDEX [IX_FK_SecurityPermissionsMatrix_Matrix] ON [SecurityPermissionsMatrix] ([IDMatrix])
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SecurityPermissionsMatrix]') AND name = N'IX_FK_SecurityPermissionsMatrix_SecurityRole')
CREATE INDEX [IX_FK_SecurityPermissionsMatrix_SecurityRole] ON [SecurityPermissionsMatrix] ([IDRole])
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[RecordingServerServiceRecorder]') AND name = N'IX_FK_RecordingServerServiceRecorder_Recorder')
CREATE INDEX [IX_FK_RecordingServerServiceRecorder_Recorder] ON [RecordingServerServiceRecorder] ([recorder_id])
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[RecordingServerServiceRecorder]') AND name = N'IX_FK_RecordingServerServiceRecorder_RecordingServerServiceType')
CREATE INDEX [IX_FK_RecordingServerServiceRecorder_RecordingServerServiceType] ON [RecordingServerServiceRecorder] ([type])
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SecurityAccessEntry]') AND name = N'IX_FK_SecurityObject_SecurityObject')
CREATE INDEX [IX_FK_SecurityObject_SecurityObject] ON [SecurityAccessEntry] ([security_object_id])
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SecurityAccessEntry]') AND name = N'IX_FK_SecurityObject_SecurityIdentity')
CREATE INDEX [IX_FK_SecurityObject_SecurityIdentity] ON [SecurityAccessEntry] ([user_sid])
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SecurityPermissions]') AND name = N'IX_FK_SecurityPermissions_SecurityAttributes')
CREATE INDEX [IX_FK_SecurityPermissions_SecurityAttributes] ON [SecurityPermissions] ([SecurityAttributeID])
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SecurityPermissions]') AND name = N'IX_FK_SecurityPermissions_TimeProfile')
CREATE INDEX [IX_FK_SecurityPermissions_TimeProfile] ON [SecurityPermissions] ([IDTimeProfile])
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SitesRelation]') AND name = N'IX_FK_SitesRelationMaster_Site')
CREATE INDEX [IX_FK_SitesRelationMaster_Site] ON [SitesRelation] ([IDSiteMaster])
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[Sites]') AND name = N'IX_FK_Sites_SiteType')
CREATE INDEX [IX_FK_Sites_SiteType] ON [Sites] ([IDSiteType])
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[Sites]') AND name = N'IX_FK_Sites_SiteState')
CREATE INDEX [IX_FK_Sites_SiteState] ON [Sites] ([IDSiteState])
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[TimeProfileDefaultRoleRelation]') AND name = N'IX_FK_TimeProfileDefaultRoleRelation_TimeProfile')
CREATE INDEX [IX_FK_TimeProfileDefaultRoleRelation_TimeProfile] ON [TimeProfileDefaultRoleRelation] ([IDTimeProfile])
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[VideoWallMonitor]') AND name = N'IX_FK_VideoWallMonitor_View')
CREATE INDEX [IX_FK_VideoWallMonitor_View] ON [VideoWallMonitor] ([view_id])
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[VideoWallMonitorPreset]') AND name = N'IX_FK_VideoWallMonitorPreset_Monitor')
CREATE INDEX [IX_FK_VideoWallMonitorPreset_Monitor] ON [VideoWallMonitorPreset] ([monitor_id])
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[VideoWallMonitorPreset]') AND name = N'IX_FK_VideoWallMonitorPreset_VideoWallPreset')
CREATE INDEX [IX_FK_VideoWallMonitorPreset_VideoWallPreset] ON [VideoWallMonitorPreset] ([videowallpreset_id])
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[ClientProfileRoleRelation]') AND name = N'PK_ClientProfileRoleRelation')
ALTER TABLE [ClientProfileRoleRelation]
ADD CONSTRAINT [PK_ClientProfileRoleRelation] PRIMARY KEY CLUSTERED (IDClientProfile, IDRole)
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[ClientProfileRoleRelation]') AND name = N'IX_FK_ClientProfileRoleRelation_ClientProfile')
CREATE INDEX [IX_FK_ClientProfileRoleRelation_ClientProfile] ON [ClientProfileRoleRelation] ([IDClientProfile])
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[DataCollectorInstalledServices]') AND name = N'IX_FK_DataCollectorInstalledServices_ServerType')
CREATE INDEX [IX_FK_DataCollectorInstalledServices_ServerType] ON [DataCollectorInstalledServices] ([service_name])
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[DeviceGroups]') AND name = N'IX_FK_DeviceGroups_ParentDeviceGroup')
CREATE INDEX [IX_FK_DeviceGroups_ParentDeviceGroup] ON [DeviceGroups] ([IDParentDeviceGroup])
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[DeviceLinks]') AND name = N'IX_FK_DeviceLinks_Devices_Linked')
CREATE INDEX [IX_FK_DeviceLinks_Devices_Linked] ON [DeviceLinks] ([IDDeviceLinked])
GO

-- ***************************************
-- Remove unused definition column
-- ***************************************
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[RecorderPluginDevice]') AND name = N'Definition')
ALTER TABLE [RecorderPluginDevice]
DROP COLUMN [Definition]
GO






