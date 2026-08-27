ALTER TABLE [dbo].[ArchiveStorage]  WITH CHECK ADD  CONSTRAINT [FK_ArchiveStorage_RecordingStorage] FOREIGN KEY([recordingstorage_id])
REFERENCES [dbo].[RecordingStorage] ([pkid])
GO
ALTER TABLE [dbo].[ArchiveStorage] CHECK CONSTRAINT [FK_ArchiveStorage_RecordingStorage]
GO
ALTER TABLE [dbo].[ClientProfileRoleRelation]  WITH CHECK ADD  CONSTRAINT [FK_ClientProfileRoleRelation_ClientProfile] FOREIGN KEY([IDClientProfile])
REFERENCES [dbo].[ClientProfile] ([IDClientProfile])
GO
ALTER TABLE [dbo].[ClientProfileRoleRelation] CHECK CONSTRAINT [FK_ClientProfileRoleRelation_ClientProfile]
GO
ALTER TABLE [dbo].[ClientProfileRoleRelation]  WITH CHECK ADD  CONSTRAINT [FK_ClientProfileRoleRelation_SecurityRole] FOREIGN KEY([IDRole])
REFERENCES [dbo].[SecurityRoles] ([IDRole])
GO
ALTER TABLE [dbo].[ClientProfileRoleRelation] CHECK CONSTRAINT [FK_ClientProfileRoleRelation_SecurityRole]
GO
ALTER TABLE [dbo].[DataCollectorConfiguration]  WITH CHECK ADD  CONSTRAINT [FK_DataCollectorConfiguration_DataCollector] FOREIGN KEY([data_collector_id])
REFERENCES [dbo].[DataCollector] ([pkid])
GO
ALTER TABLE [dbo].[DataCollectorConfiguration] CHECK CONSTRAINT [FK_DataCollectorConfiguration_DataCollector]
GO
ALTER TABLE [dbo].[DataCollectorInstalledServices]  WITH CHECK ADD  CONSTRAINT [FK_DataCollectorInstalledServices_DataCollector] FOREIGN KEY([data_collector_id])
REFERENCES [dbo].[DataCollector] ([pkid])
GO
ALTER TABLE [dbo].[DataCollectorInstalledServices] CHECK CONSTRAINT [FK_DataCollectorInstalledServices_DataCollector]
GO
ALTER TABLE [dbo].[DataCollectorInstalledServices]  WITH CHECK ADD  CONSTRAINT [FK_DataCollectorInstalledServices_ServerType] FOREIGN KEY([service_name])
REFERENCES [dbo].[ServerType] ([name])
GO
ALTER TABLE [dbo].[DataCollectorInstalledServices] CHECK CONSTRAINT [FK_DataCollectorInstalledServices_ServerType]
GO
ALTER TABLE [dbo].[DeviceDataDistribution]  WITH CHECK ADD  CONSTRAINT [FK_DeviceDataDistribution_Devices] FOREIGN KEY([device_id])
REFERENCES [dbo].[Devices] ([IDDevice])
GO
ALTER TABLE [dbo].[DeviceDataDistribution] CHECK CONSTRAINT [FK_DeviceDataDistribution_Devices]
GO
ALTER TABLE [dbo].[DeviceDataDistribution]  WITH CHECK ADD  CONSTRAINT [FK_DeviceDataDistribution_DeviceTrack] FOREIGN KEY([track_id])
REFERENCES [dbo].[DeviceTrack] ([IDTrack])
GO
ALTER TABLE [dbo].[DeviceDataDistribution] CHECK CONSTRAINT [FK_DeviceDataDistribution_DeviceTrack]
GO
ALTER TABLE [dbo].[DeviceDataDistribution]  WITH CHECK ADD  CONSTRAINT [FK_DeviceDataDistribution_Recorders] FOREIGN KEY([recorder_id])
REFERENCES [dbo].[Recorders] ([IDRecorder])
GO
ALTER TABLE [dbo].[DeviceDataDistribution] CHECK CONSTRAINT [FK_DeviceDataDistribution_Recorders]
GO
ALTER TABLE [dbo].[DeviceGroupMembers]  WITH CHECK ADD  CONSTRAINT [FK_DeviceGroupMembers_DeviceGroups] FOREIGN KEY([IDDeviceGroup])
REFERENCES [dbo].[DeviceGroups] ([IDDeviceGroup])
GO
ALTER TABLE [dbo].[DeviceGroupMembers] CHECK CONSTRAINT [FK_DeviceGroupMembers_DeviceGroups]
GO
ALTER TABLE [dbo].[DeviceGroupMembers]  WITH CHECK ADD  CONSTRAINT [FK_DeviceGroupMembers_Devices] FOREIGN KEY([IDDevice])
REFERENCES [dbo].[Devices] ([IDDevice])
GO
ALTER TABLE [dbo].[DeviceGroupMembers] CHECK CONSTRAINT [FK_DeviceGroupMembers_Devices]
GO
ALTER TABLE [dbo].[DeviceGroups]  WITH CHECK ADD  CONSTRAINT [FK_DeviceGroups_ParentDeviceGroup] FOREIGN KEY([IDParentDeviceGroup])
REFERENCES [dbo].[DeviceGroups] ([IDDeviceGroup])
GO
ALTER TABLE [dbo].[DeviceGroups] CHECK CONSTRAINT [FK_DeviceGroups_ParentDeviceGroup]
GO
ALTER TABLE [dbo].[DeviceLinks]  WITH CHECK ADD  CONSTRAINT [FK_DeviceLinks_Devices] FOREIGN KEY([IDDevice])
REFERENCES [dbo].[Devices] ([IDDevice])
GO
ALTER TABLE [dbo].[DeviceLinks] CHECK CONSTRAINT [FK_DeviceLinks_Devices]
GO
ALTER TABLE [dbo].[DeviceLinks]  WITH CHECK ADD  CONSTRAINT [FK_DeviceLinks_Devices_Linked] FOREIGN KEY([IDDeviceLinked])
REFERENCES [dbo].[Devices] ([IDDevice])
GO
ALTER TABLE [dbo].[DeviceLinks] CHECK CONSTRAINT [FK_DeviceLinks_Devices_Linked]
GO
ALTER TABLE [dbo].[DeviceMetadataInfo]  WITH CHECK ADD  CONSTRAINT [FK_DeviceMetadataInfo_Devices] FOREIGN KEY([IDDevice])
REFERENCES [dbo].[Devices] ([IDDevice])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DeviceMetadataInfo] CHECK CONSTRAINT [FK_DeviceMetadataInfo_Devices]
GO
ALTER TABLE [dbo].[DevicePrivacyMask]  WITH CHECK ADD  CONSTRAINT [FK_DevicePrivacyMask_Devices] FOREIGN KEY([IDDevice])
REFERENCES [dbo].[Devices] ([IDDevice])
GO
ALTER TABLE [dbo].[DevicePrivacyMask] CHECK CONSTRAINT [FK_DevicePrivacyMask_Devices]
GO
ALTER TABLE [dbo].[DevicePtzSettingData]  WITH CHECK ADD  CONSTRAINT [FK_DevicePtzSettingData_Devices] FOREIGN KEY([IDDevice])
REFERENCES [dbo].[Devices] ([IDDevice])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DevicePtzSettingData] CHECK CONSTRAINT [FK_DevicePtzSettingData_Devices]
GO
ALTER TABLE [dbo].[Devices]  WITH CHECK ADD  CONSTRAINT [FK_Devices_Hardware] FOREIGN KEY([IDHardware])
REFERENCES [dbo].[Hardware] ([IDHardware])
GO
ALTER TABLE [dbo].[Devices] CHECK CONSTRAINT [FK_Devices_Hardware]
GO
ALTER TABLE [dbo].[Devices]  WITH CHECK ADD  CONSTRAINT [FK_Devices_RegisteredDeviceTypes] FOREIGN KEY([DeviceType])
REFERENCES [dbo].[RegisteredDeviceTypes] ([DeviceType])
GO
ALTER TABLE [dbo].[Devices] CHECK CONSTRAINT [FK_Devices_RegisteredDeviceTypes]
GO
ALTER TABLE [dbo].[DeviceTrack]  WITH CHECK ADD  CONSTRAINT [FK_DeviceTrack_Devices] FOREIGN KEY([IDDevice])
REFERENCES [dbo].[Devices] ([IDDevice])
GO
ALTER TABLE [dbo].[DeviceTrack] CHECK CONSTRAINT [FK_DeviceTrack_Devices]
GO
ALTER TABLE [dbo].[DeviceTrack]  WITH CHECK ADD  CONSTRAINT [FK_DeviceTrack_TrackDefinition] FOREIGN KEY([IDTrackDefinition])
REFERENCES [dbo].[DeviceTrackDefinition] ([IDTrackDefinition])
GO
ALTER TABLE [dbo].[DeviceTrack] CHECK CONSTRAINT [FK_DeviceTrack_TrackDefinition]
GO
ALTER TABLE [dbo].[Hardware]  WITH CHECK ADD  CONSTRAINT [FK_Hardware_Definition] FOREIGN KEY([C18NDigest])
REFERENCES [dbo].[HardwareDefinition] ([C18NDigest])
GO
ALTER TABLE [dbo].[Hardware] CHECK CONSTRAINT [FK_Hardware_Definition]
GO
ALTER TABLE [dbo].[Hardware]  WITH CHECK ADD  CONSTRAINT [FK_Hardware_HostedHardware] FOREIGN KEY([HostedHardware_id])
REFERENCES [dbo].[HostedHardware] ([pkid])
GO
ALTER TABLE [dbo].[Hardware] CHECK CONSTRAINT [FK_Hardware_HostedHardware]
GO
ALTER TABLE [dbo].[Hardware]  WITH CHECK ADD  CONSTRAINT [FK_Hardware_Recorders] FOREIGN KEY([IDRecorder])
REFERENCES [dbo].[Recorders] ([IDRecorder])
GO
ALTER TABLE [dbo].[Hardware] CHECK CONSTRAINT [FK_Hardware_Recorders]
GO
ALTER TABLE [dbo].[HardwareBlock]  WITH CHECK ADD  CONSTRAINT [FK_HardwareBlock_Hardware] FOREIGN KEY([hardware_id])
REFERENCES [dbo].[Hardware] ([IDHardware])
GO
ALTER TABLE [dbo].[HardwareBlock] CHECK CONSTRAINT [FK_HardwareBlock_Hardware]
GO
ALTER TABLE [dbo].[HardwareDriverRecorder]  WITH CHECK ADD  CONSTRAINT [FK_HardwareDriverRecorder_RecordingServerService] FOREIGN KEY([IDRecordingServerService])
REFERENCES [dbo].[RecordingServerService] ([pkid])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[HardwareDriverRecorder] CHECK CONSTRAINT [FK_HardwareDriverRecorder_RecordingServerService]
GO
ALTER TABLE [dbo].[HostedHardware]  WITH CHECK ADD  CONSTRAINT [FK_HostedHardware_HostingServer] FOREIGN KEY([hosting_server_id])
REFERENCES [dbo].[HostingServer] ([pkid])
GO
ALTER TABLE [dbo].[HostedHardware] CHECK CONSTRAINT [FK_HostedHardware_HostingServer]
GO
ALTER TABLE [dbo].[MarkedDataDevices]  WITH CHECK ADD  CONSTRAINT [FK_MarkedDataDevices_Devices] FOREIGN KEY([IDDevice])
REFERENCES [dbo].[Devices] ([IDDevice])
GO
ALTER TABLE [dbo].[MarkedDataDevices] CHECK CONSTRAINT [FK_MarkedDataDevices_Devices]
GO
ALTER TABLE [dbo].[MarkedDataDevices]  WITH CHECK ADD  CONSTRAINT [FK_MarkedDataDevices_MarkedData] FOREIGN KEY([IDMarkedData])
REFERENCES [dbo].[MarkedData] ([IDMarkedData])
GO
ALTER TABLE [dbo].[MarkedDataDevices] CHECK CONSTRAINT [FK_MarkedDataDevices_MarkedData]
GO
ALTER TABLE [dbo].[MetadataSearchClassCandidates]  WITH CHECK ADD  CONSTRAINT [FK_MetadataSearchClassCandidates_MetadataSearchCategories_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[MetadataSearchCategories] ([Id])
GO
ALTER TABLE [dbo].[MetadataSearchClassCandidates] CHECK CONSTRAINT [FK_MetadataSearchClassCandidates_MetadataSearchCategories_CategoryId]
GO
ALTER TABLE [dbo].[MetadataSearchFilters]  WITH CHECK ADD  CONSTRAINT [FK_MetadataSearchFilters_MetadataSearchCategories_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[MetadataSearchCategories] ([Id])
GO
ALTER TABLE [dbo].[MetadataSearchFilters] CHECK CONSTRAINT [FK_MetadataSearchFilters_MetadataSearchCategories_CategoryId]
GO
ALTER TABLE [dbo].[MetadataSearchPaths]  WITH CHECK ADD  CONSTRAINT [FK_MetadataSearchPaths_MetadataSearchObjectDefinitions_ObjectDefinitionId] FOREIGN KEY([ObjectDefinitionId])
REFERENCES [dbo].[MetadataSearchObjectDefinitions] ([Id])
GO
ALTER TABLE [dbo].[MetadataSearchPaths] CHECK CONSTRAINT [FK_MetadataSearchPaths_MetadataSearchObjectDefinitions_ObjectDefinitionId]
GO
ALTER TABLE [dbo].[MetadataSearchPaths]  WITH CHECK ADD  CONSTRAINT [FK_MetadataSearchPaths_MetadataSearchProperties] FOREIGN KEY([PropertyId])
REFERENCES [dbo].[MetadataSearchProperties] ([Id])
GO
ALTER TABLE [dbo].[MetadataSearchPaths] CHECK CONSTRAINT [FK_MetadataSearchPaths_MetadataSearchProperties]
GO
ALTER TABLE [dbo].[MetadataSearchProperties]  WITH CHECK ADD  CONSTRAINT [FK_MetadataSearchProperties_MetadataSearchFilters_FilterId] FOREIGN KEY([FilterId])
REFERENCES [dbo].[MetadataSearchFilters] ([Id])
GO
ALTER TABLE [dbo].[MetadataSearchProperties] CHECK CONSTRAINT [FK_MetadataSearchProperties_MetadataSearchFilters_FilterId]
GO
ALTER TABLE [dbo].[PatrollingProfiles]  WITH CHECK ADD  CONSTRAINT [FK_PatrollingProfiles_Devices] FOREIGN KEY([IDDevice])
REFERENCES [dbo].[Devices] ([IDDevice])
GO
ALTER TABLE [dbo].[PatrollingProfiles] CHECK CONSTRAINT [FK_PatrollingProfiles_Devices]
GO
ALTER TABLE [dbo].[PerformanceCounterInstance]  WITH CHECK ADD  CONSTRAINT [FK_PerformanceCounterInstance_DataCollector] FOREIGN KEY([data_collector_id])
REFERENCES [dbo].[DataCollector] ([pkid])
GO
ALTER TABLE [dbo].[PerformanceCounterInstance] CHECK CONSTRAINT [FK_PerformanceCounterInstance_DataCollector]
GO
ALTER TABLE [dbo].[PerformanceCounterStateOverride]  WITH CHECK ADD  CONSTRAINT [FK_PerformanceCounterStateOverride_PerformanceCounterStateSource] FOREIGN KEY([source_id])
REFERENCES [dbo].[PerformanceCounterStateSource] ([source_id])
GO
ALTER TABLE [dbo].[PerformanceCounterStateOverride] CHECK CONSTRAINT [FK_PerformanceCounterStateOverride_PerformanceCounterStateSource]
GO
ALTER TABLE [dbo].[PerformanceCounterStateSource]  WITH CHECK ADD  CONSTRAINT [FK_PerformanceCounterStateSource_PerformanceCounterStateRule] FOREIGN KEY([counter_state_rule_id])
REFERENCES [dbo].[PerformanceCounterStateRule] ([counter_state_rule_id])
GO
ALTER TABLE [dbo].[PerformanceCounterStateSource] CHECK CONSTRAINT [FK_PerformanceCounterStateSource_PerformanceCounterStateRule]
GO
ALTER TABLE [dbo].[PerformanceCounterValue]  WITH CHECK ADD  CONSTRAINT [FK_PerformanceCounterValue_PerformanceCounterInstance] FOREIGN KEY([instance_id])
REFERENCES [dbo].[PerformanceCounterInstance] ([pkid])
GO
ALTER TABLE [dbo].[PerformanceCounterValue] CHECK CONSTRAINT [FK_PerformanceCounterValue_PerformanceCounterInstance]
GO
ALTER TABLE [dbo].[RecorderStatusData]  WITH CHECK ADD  CONSTRAINT [FK_RecorderStatusData_Recorders] FOREIGN KEY([IDRecorder])
REFERENCES [dbo].[Recorders] ([IDRecorder])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RecorderStatusData] CHECK CONSTRAINT [FK_RecorderStatusData_Recorders]
GO
ALTER TABLE [dbo].[RecordingServerServiceConfigurable]  WITH CHECK ADD  CONSTRAINT [FK_RecordingServerServiceConfigurable_RecordingServerService] FOREIGN KEY([PKID])
REFERENCES [dbo].[RecordingServerService] ([pkid])
GO
ALTER TABLE [dbo].[RecordingServerServiceConfigurable] CHECK CONSTRAINT [FK_RecordingServerServiceConfigurable_RecordingServerService]
GO
ALTER TABLE [dbo].[RecordingServerServiceRecorder]  WITH CHECK ADD  CONSTRAINT [FK_RecordingServerServiceRecorder_Recorder] FOREIGN KEY([recorder_id])
REFERENCES [dbo].[Recorders] ([IDRecorder])
GO
ALTER TABLE [dbo].[RecordingServerServiceRecorder] CHECK CONSTRAINT [FK_RecordingServerServiceRecorder_Recorder]
GO
ALTER TABLE [dbo].[RecordingServerServiceRecorder]  WITH CHECK ADD  CONSTRAINT [FK_RecordingServerServiceRecorder_RecordingServerService] FOREIGN KEY([recording_server_service_id])
REFERENCES [dbo].[RecordingServerService] ([pkid])
GO
ALTER TABLE [dbo].[RecordingServerServiceRecorder] CHECK CONSTRAINT [FK_RecordingServerServiceRecorder_RecordingServerService]
GO
ALTER TABLE [dbo].[RecordingServerServiceRecorder]  WITH CHECK ADD  CONSTRAINT [FK_RecordingServerServiceRecorder_RecordingServerServiceType] FOREIGN KEY([type])
REFERENCES [dbo].[RecordingServerServiceType] ([pkid])
GO
ALTER TABLE [dbo].[RecordingServerServiceRecorder] CHECK CONSTRAINT [FK_RecordingServerServiceRecorder_RecordingServerServiceType]
GO
ALTER TABLE [dbo].[RecordingStorage]  WITH CHECK ADD  CONSTRAINT [FK_RecordingStorage_Recorders] FOREIGN KEY([recordingserver_id])
REFERENCES [dbo].[Recorders] ([IDRecorder])
GO
ALTER TABLE [dbo].[RecordingStorage] CHECK CONSTRAINT [FK_RecordingStorage_Recorders]
GO
ALTER TABLE [dbo].[RecordingStorageDevice]  WITH CHECK ADD  CONSTRAINT [FK_RecordingStorageDevice_Devices] FOREIGN KEY([device_id])
REFERENCES [dbo].[Devices] ([IDDevice])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RecordingStorageDevice] CHECK CONSTRAINT [FK_RecordingStorageDevice_Devices]
GO
ALTER TABLE [dbo].[RecordingStorageDevice]  WITH CHECK ADD  CONSTRAINT [FK_RecordingStorageDevice_RecordingStorage] FOREIGN KEY([recordingstorage_id])
REFERENCES [dbo].[RecordingStorage] ([pkid])
GO
ALTER TABLE [dbo].[RecordingStorageDevice] CHECK CONSTRAINT [FK_RecordingStorageDevice_RecordingStorage]
GO
ALTER TABLE [dbo].[RestrictedMediaDevice]  WITH CHECK ADD  CONSTRAINT [FK_RestrictedMediaDevice_Devices] FOREIGN KEY([IDDevice])
REFERENCES [dbo].[Devices] ([IDDevice])
GO
ALTER TABLE [dbo].[RestrictedMediaDevice] CHECK CONSTRAINT [FK_RestrictedMediaDevice_Devices]
GO
ALTER TABLE [dbo].[RestrictedMediaDevice]  WITH CHECK ADD  CONSTRAINT [FK_RestrictedMediaDevice_RestrictedMedia] FOREIGN KEY([IDRestrictedMedia])
REFERENCES [dbo].[RestrictedMedia] ([IDRestrictedMedia])
GO
ALTER TABLE [dbo].[RestrictedMediaDevice] CHECK CONSTRAINT [FK_RestrictedMediaDevice_RestrictedMedia]
GO
ALTER TABLE [dbo].[RestrictedMediaLive]  WITH CHECK ADD  CONSTRAINT [FK_RestrictedMediaLive_Devices] FOREIGN KEY([IDDevice])
REFERENCES [dbo].[Devices] ([IDDevice])
GO
ALTER TABLE [dbo].[RestrictedMediaLive] CHECK CONSTRAINT [FK_RestrictedMediaLive_Devices]
GO
ALTER TABLE [dbo].[SecurityAccessEntry]  WITH CHECK ADD  CONSTRAINT [FK_SecurityObject_SecurityIdentity] FOREIGN KEY([user_sid])
REFERENCES [dbo].[SecurityIdentity] ([SID])
GO
ALTER TABLE [dbo].[SecurityAccessEntry] CHECK CONSTRAINT [FK_SecurityObject_SecurityIdentity]
GO
ALTER TABLE [dbo].[SecurityAccessEntry]  WITH CHECK ADD  CONSTRAINT [FK_SecurityObject_SecurityObject] FOREIGN KEY([security_object_id])
REFERENCES [dbo].[SecurityObject] ([security_object_id])
GO
ALTER TABLE [dbo].[SecurityAccessEntry] CHECK CONSTRAINT [FK_SecurityObject_SecurityObject]
GO
ALTER TABLE [dbo].[SecurityAccessEntryTimeProfile]  WITH CHECK ADD  CONSTRAINT [FK_SecurityAccessEntry_TimeProfile] FOREIGN KEY([access_entry_id])
REFERENCES [dbo].[SecurityAccessEntryTimeProfile] ([access_entry_id])
GO
ALTER TABLE [dbo].[SecurityAccessEntryTimeProfile] CHECK CONSTRAINT [FK_SecurityAccessEntry_TimeProfile]
GO
ALTER TABLE [dbo].[SecurityAccessEntryTimeProfile]  WITH CHECK ADD  CONSTRAINT [FK_SecurityAccessEntryTimeProfile_TimeProfile] FOREIGN KEY([timeprofile_id])
REFERENCES [dbo].[TimeProfile] ([pkid])
GO
ALTER TABLE [dbo].[SecurityAccessEntryTimeProfile] CHECK CONSTRAINT [FK_SecurityAccessEntryTimeProfile_TimeProfile]
GO
ALTER TABLE [dbo].[SecurityAction]  WITH CHECK ADD  CONSTRAINT [FK_SecurityNamespace_SecurityAction] FOREIGN KEY([namespace_id])
REFERENCES [dbo].[SecurityNamespace] ([namespace_id])
GO
ALTER TABLE [dbo].[SecurityAction] CHECK CONSTRAINT [FK_SecurityNamespace_SecurityAction]
GO
ALTER TABLE [dbo].[SecurityMembership]  WITH CHECK ADD  CONSTRAINT [FK_SecurityIdentity_SecurityMembership_Group] FOREIGN KEY([Container])
REFERENCES [dbo].[SecurityIdentity] ([SID])
GO
ALTER TABLE [dbo].[SecurityMembership] CHECK CONSTRAINT [FK_SecurityIdentity_SecurityMembership_Group]
GO
ALTER TABLE [dbo].[SecurityMembership]  WITH CHECK ADD  CONSTRAINT [FK_SecurityIdentity_SecurityMembership_Identity] FOREIGN KEY([MemberSID])
REFERENCES [dbo].[SecurityIdentity] ([SID])
GO
ALTER TABLE [dbo].[SecurityMembership] CHECK CONSTRAINT [FK_SecurityIdentity_SecurityMembership_Identity]
GO
ALTER TABLE [dbo].[SecurityMembershipCache]  WITH CHECK ADD  CONSTRAINT [FK_SecurityMemberCache_SecurityIdentity_Group] FOREIGN KEY([GroupSID])
REFERENCES [dbo].[SecurityIdentity] ([SID])
GO
ALTER TABLE [dbo].[SecurityMembershipCache] CHECK CONSTRAINT [FK_SecurityMemberCache_SecurityIdentity_Group]
GO
ALTER TABLE [dbo].[SecurityMembershipCache]  WITH CHECK ADD  CONSTRAINT [FK_SecurityMemberCache_SecurityIdentity_Member] FOREIGN KEY([MemberSID])
REFERENCES [dbo].[SecurityIdentity] ([SID])
GO
ALTER TABLE [dbo].[SecurityMembershipCache] CHECK CONSTRAINT [FK_SecurityMemberCache_SecurityIdentity_Member]
GO
ALTER TABLE [dbo].[SecurityObject]  WITH CHECK ADD  CONSTRAINT [FK_SecurityNamespace_SecurityObject] FOREIGN KEY([namespace_id])
REFERENCES [dbo].[SecurityNamespace] ([namespace_id])
GO
ALTER TABLE [dbo].[SecurityObject] CHECK CONSTRAINT [FK_SecurityNamespace_SecurityObject]
GO
ALTER TABLE [dbo].[SecurityPermissions]  WITH CHECK ADD  CONSTRAINT [FK_SecurityPermissions_SecurityAttributes] FOREIGN KEY([SecurityAttributeID])
REFERENCES [dbo].[SecurityAttributes] ([SecurityAttributeID])
GO
ALTER TABLE [dbo].[SecurityPermissions] CHECK CONSTRAINT [FK_SecurityPermissions_SecurityAttributes]
GO
ALTER TABLE [dbo].[SecurityPermissions]  WITH CHECK ADD  CONSTRAINT [FK_SecurityPermissions_SecurityRole] FOREIGN KEY([IDRole])
REFERENCES [dbo].[SecurityRoles] ([IDRole])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SecurityPermissions] CHECK CONSTRAINT [FK_SecurityPermissions_SecurityRole]
GO
ALTER TABLE [dbo].[SecurityPermissions]  WITH CHECK ADD  CONSTRAINT [FK_SecurityPermissions_TimeProfile] FOREIGN KEY([IDTimeProfile])
REFERENCES [dbo].[TimeProfile] ([pkid])
GO
ALTER TABLE [dbo].[SecurityPermissions] CHECK CONSTRAINT [FK_SecurityPermissions_TimeProfile]
GO
ALTER TABLE [dbo].[SecurityPermissionsDevice]  WITH CHECK ADD  CONSTRAINT [FK_SecurityPermissionsDevice_Devices] FOREIGN KEY([IDDevice])
REFERENCES [dbo].[Devices] ([IDDevice])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SecurityPermissionsDevice] CHECK CONSTRAINT [FK_SecurityPermissionsDevice_Devices]
GO
ALTER TABLE [dbo].[SecurityPermissionsDevice]  WITH CHECK ADD  CONSTRAINT [FK_SecurityPermissionsDevice_SecurityAttributes] FOREIGN KEY([SecurityAttributeID])
REFERENCES [dbo].[SecurityAttributes] ([SecurityAttributeID])
GO
ALTER TABLE [dbo].[SecurityPermissionsDevice] CHECK CONSTRAINT [FK_SecurityPermissionsDevice_SecurityAttributes]
GO
ALTER TABLE [dbo].[SecurityPermissionsDevice]  WITH CHECK ADD  CONSTRAINT [FK_SecurityPermissionsDevice_SecurityRole] FOREIGN KEY([IDRole])
REFERENCES [dbo].[SecurityRoles] ([IDRole])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SecurityPermissionsDevice] CHECK CONSTRAINT [FK_SecurityPermissionsDevice_SecurityRole]
GO
ALTER TABLE [dbo].[SecurityPermissionsDevice]  WITH CHECK ADD  CONSTRAINT [FK_SecurityPermissionsDevice_TimeProfile] FOREIGN KEY([IDTimeProfile])
REFERENCES [dbo].[TimeProfile] ([pkid])
GO
ALTER TABLE [dbo].[SecurityPermissionsDevice] CHECK CONSTRAINT [FK_SecurityPermissionsDevice_TimeProfile]
GO
ALTER TABLE [dbo].[SecurityPermissionsMatrix]  WITH CHECK ADD  CONSTRAINT [FK_SecurityPermissionsMatrix_Matrix] FOREIGN KEY([IDMatrix])
REFERENCES [dbo].[Matrix] ([IDMatrix])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SecurityPermissionsMatrix] CHECK CONSTRAINT [FK_SecurityPermissionsMatrix_Matrix]
GO
ALTER TABLE [dbo].[SecurityPermissionsMatrix]  WITH CHECK ADD  CONSTRAINT [FK_SecurityPermissionsMatrix_SecurityAttributes] FOREIGN KEY([SecurityAttributeID])
REFERENCES [dbo].[SecurityAttributes] ([SecurityAttributeID])
GO
ALTER TABLE [dbo].[SecurityPermissionsMatrix] CHECK CONSTRAINT [FK_SecurityPermissionsMatrix_SecurityAttributes]
GO
ALTER TABLE [dbo].[SecurityPermissionsMatrix]  WITH CHECK ADD  CONSTRAINT [FK_SecurityPermissionsMatrix_SecurityRole] FOREIGN KEY([IDRole])
REFERENCES [dbo].[SecurityRoles] ([IDRole])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SecurityPermissionsMatrix] CHECK CONSTRAINT [FK_SecurityPermissionsMatrix_SecurityRole]
GO
ALTER TABLE [dbo].[SecurityRoleClaim]  WITH CHECK ADD  CONSTRAINT [FK_SecurityRoleClaim_SecurityClaim] FOREIGN KEY([IDClaim])
REFERENCES [dbo].[SecurityClaim] ([IDClaim])
GO
ALTER TABLE [dbo].[SecurityRoleClaim] CHECK CONSTRAINT [FK_SecurityRoleClaim_SecurityClaim]
GO
ALTER TABLE [dbo].[SecurityRoleClaim]  WITH CHECK ADD  CONSTRAINT [FK_SecurityRoleClaim_SecurityRoles] FOREIGN KEY([IDRole])
REFERENCES [dbo].[SecurityRoles] ([IDRole])
GO
ALTER TABLE [dbo].[SecurityRoleClaim] CHECK CONSTRAINT [FK_SecurityRoleClaim_SecurityRoles]
GO
ALTER TABLE [dbo].[SecurityRolesIdentity]  WITH CHECK ADD  CONSTRAINT [FK_SecurityRolesIdentity_SecurityRoles] FOREIGN KEY([IDRole])
REFERENCES [dbo].[SecurityRoles] ([IDRole])
GO
ALTER TABLE [dbo].[SecurityRolesIdentity] CHECK CONSTRAINT [FK_SecurityRolesIdentity_SecurityRoles]
GO
ALTER TABLE [dbo].[ServerGroupMembership]  WITH CHECK ADD  CONSTRAINT [FK_ServerGroupMembership_ServerGroup] FOREIGN KEY([IDServerGroup])
REFERENCES [dbo].[ServerGroup] ([IDServerGroup])
GO
ALTER TABLE [dbo].[ServerGroupMembership] CHECK CONSTRAINT [FK_ServerGroupMembership_ServerGroup]
GO
ALTER TABLE [dbo].[Sites]  WITH CHECK ADD  CONSTRAINT [FK_Sites_SiteState] FOREIGN KEY([IDSiteState])
REFERENCES [dbo].[SiteState] ([IDSiteState])
GO
ALTER TABLE [dbo].[Sites] CHECK CONSTRAINT [FK_Sites_SiteState]
GO
ALTER TABLE [dbo].[Sites]  WITH CHECK ADD  CONSTRAINT [FK_Sites_SiteType] FOREIGN KEY([IDSiteType])
REFERENCES [dbo].[SiteType] ([IDSiteType])
GO
ALTER TABLE [dbo].[Sites] CHECK CONSTRAINT [FK_Sites_SiteType]
GO
ALTER TABLE [dbo].[SitesRelation]  WITH CHECK ADD  CONSTRAINT [FK_SitesRelationMaster_Site] FOREIGN KEY([IDSiteMaster])
REFERENCES [dbo].[Sites] ([IDSite])
GO
ALTER TABLE [dbo].[SitesRelation] CHECK CONSTRAINT [FK_SitesRelationMaster_Site]
GO
ALTER TABLE [dbo].[SitesRelation]  WITH CHECK ADD  CONSTRAINT [FK_SitesRelationSlave_Site] FOREIGN KEY([IDSiteSlave])
REFERENCES [dbo].[Sites] ([IDSite])
GO
ALTER TABLE [dbo].[SitesRelation] CHECK CONSTRAINT [FK_SitesRelationSlave_Site]
GO
ALTER TABLE [dbo].[TimeProfileDefaultRoleRelation]  WITH CHECK ADD  CONSTRAINT [FK_TimeProfileDefaultRoleRelation_SecurityRole] FOREIGN KEY([IDRole])
REFERENCES [dbo].[SecurityRoles] ([IDRole])
GO
ALTER TABLE [dbo].[TimeProfileDefaultRoleRelation] CHECK CONSTRAINT [FK_TimeProfileDefaultRoleRelation_SecurityRole]
GO
ALTER TABLE [dbo].[TimeProfileDefaultRoleRelation]  WITH CHECK ADD  CONSTRAINT [FK_TimeProfileDefaultRoleRelation_TimeProfile] FOREIGN KEY([IDTimeProfile])
REFERENCES [dbo].[TimeProfile] ([pkid])
GO
ALTER TABLE [dbo].[TimeProfileDefaultRoleRelation] CHECK CONSTRAINT [FK_TimeProfileDefaultRoleRelation_TimeProfile]
GO
ALTER TABLE [dbo].[VideoWallMonitor]  WITH CHECK ADD  CONSTRAINT [FK_VideoWallMonitor_VideoWall] FOREIGN KEY([videowall_id])
REFERENCES [dbo].[VideoWall] ([pkid])
GO
ALTER TABLE [dbo].[VideoWallMonitor] CHECK CONSTRAINT [FK_VideoWallMonitor_VideoWall]
GO
ALTER TABLE [dbo].[VideoWallMonitor]  WITH CHECK ADD  CONSTRAINT [FK_VideoWallMonitor_View] FOREIGN KEY([view_id])
REFERENCES [dbo].[VideoWallView] ([pkid])
GO
ALTER TABLE [dbo].[VideoWallMonitor] CHECK CONSTRAINT [FK_VideoWallMonitor_View]
GO
ALTER TABLE [dbo].[VideoWallMonitorPreset]  WITH CHECK ADD  CONSTRAINT [FK_VideoWallMonitorPreset_Monitor] FOREIGN KEY([monitor_id])
REFERENCES [dbo].[VideoWallMonitor] ([pkid])
GO
ALTER TABLE [dbo].[VideoWallMonitorPreset] CHECK CONSTRAINT [FK_VideoWallMonitorPreset_Monitor]
GO
ALTER TABLE [dbo].[VideoWallMonitorPreset]  WITH CHECK ADD  CONSTRAINT [FK_VideoWallMonitorPreset_VideoWallPreset] FOREIGN KEY([videowallpreset_id])
REFERENCES [dbo].[VideoWallPreset] ([pkid])
GO
ALTER TABLE [dbo].[VideoWallMonitorPreset] CHECK CONSTRAINT [FK_VideoWallMonitorPreset_VideoWallPreset]
GO
ALTER TABLE [dbo].[VideoWallPreset]  WITH CHECK ADD  CONSTRAINT [FK_VideoWallPreset_VideoWall] FOREIGN KEY([videowall_id])
REFERENCES [dbo].[VideoWall] ([pkid])
GO
ALTER TABLE [dbo].[VideoWallPreset] CHECK CONSTRAINT [FK_VideoWallPreset_VideoWall]
GO
ALTER TABLE [dbo].[VideoWallViewItem]  WITH CHECK ADD  CONSTRAINT [FK_VideoWallViewItem_View] FOREIGN KEY([view_id])
REFERENCES [dbo].[VideoWallView] ([pkid])
GO
ALTER TABLE [dbo].[VideoWallViewItem] CHECK CONSTRAINT [FK_VideoWallViewItem_View]
GO
