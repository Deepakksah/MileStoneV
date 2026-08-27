using System;

namespace VideoOS.Common.Integration;

public interface IAuditLogger
{
	void AuditBasicOwnerInfoCreated(string id, string name, string details, string userCredentials, string locationIPAddress);

	void AuditBasicOwnerInfoDelete(string id, string name, string userCredentials, string locationIPAddress);

	void AuditBasicOwnerInfoEdited(string id, string name, string details, string userCredentials, string locationIPAddress);

	void AuditBasicUserCreated(string sid, string userName, string userCredentials, string locationIPAddress);

	void AuditBasicUserDelete(string sid, string userName, string userCredentials, string locationIPAddress);

	void AuditBasicUserEdited(string sid, string userName, string userCredentials, string locationIPAddress);

	void AuditBasicUserPasswordChanged(string sid, string userName, string userCredentials, string locationIPAddress);

	void AuditBasicUserRenamed(string oldUserName, string newUserName, string sid, string userCredentials, string locationIPAddress);

	void AuditBookmarkCreated(string refId, string headline, DateTime beginTime, DateTime triggerTime, DateTime endTime, Guid deviceId, string deviceType, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditBookmarkDeleted(string refId, Guid deviceId, string deviceType, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditBookmarkEdited(string refId, string headline, DateTime beginTime, DateTime triggerTime, DateTime endTime, Guid deviceId, string deviceType, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditBookmarkViewed(string refIds, Guid deviceId, string deviceType, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditClientProfileCreated(Guid clientProfileId, string clientProfileName, string userCredentials, string locationIPAddress);

	void AuditClientProfileDelete(Guid clientProfileId, string clientProfileName, string userCredentials, string locationIPAddress);

	void AuditClientProfileEdited(Guid clientProfileId, string clientProfileName, string userCredentials, string locationIPAddress);

	void AuditClientProfileRenamed(string oldClientProfileName, string newClientProfileName, Guid clientProfileId, string userCredentials, string locationIPAddress);

	void AuditConfigurationExported(string locationIPAddress, string userCredentials);

	void AuditConfigurationRestored(string locationIPAddress, string userCredentials);

	void AuditDevice360LensSettingsChanged(Guid deviceID, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditDevice360LensSettingsDisabled(Guid deviceID, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditDevice360LensSettingsEnabled(Guid deviceID, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditDeviceAdded(Guid deviceID, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditDeviceDatabaseDelete(Guid deviceID, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditDeviceDatabaseMove(Guid deviceID, string deviceName, string deviceType, string databaseBankName, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditDeviceDisabled(Guid deviceID, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditDeviceEnabled(Guid deviceID, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditDeviceMotionDetectionSettingsChanged(Guid deviceID, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditDeviceRecordingDisabled(Guid deviceId, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditDeviceRecordingEdgeStorageDisabled(Guid deviceId, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditDeviceRecordingEdgeStorageEnabled(Guid deviceId, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditDeviceRecordingEdgeStoragePlaybackDisabled(Guid deviceId, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditDeviceRecordingEdgeStoragePlaybackEnabled(Guid deviceId, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditDeviceRecordingEnabled(Guid deviceId, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditDeviceRecordingPrebufferDisabled(Guid deviceId, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditDeviceRecordingPrebufferEnabled(Guid deviceId, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditDeviceRecordingSettingsChanged(Guid deviceID, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditDeviceRecordingStorageSelected(Guid deviceId, string deviceName, string deviceType, string storageName, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditDeviceRemoved(Guid deviceID, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditDeviceRenamed(Guid deviceID, string oldDeviceName, string newDeviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditDeviceReplaced(Guid deviceID, string oldDeviceName, string newDeviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditDeviceRequestEdgeStorageRetrieval(Guid deviceId, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditDeviceSettingsChanged(Guid deviceID, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditDeviceStreamSettingsChanged(Guid deviceID, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditFileUploadToRecorder(string fileId, string recorderName, string userCredentials, string locationIPAddress);

	void AuditHardwareAdded(Guid hardwareId, string hardwareName, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditHardwareDisabled(Guid hardwareId, string hardwareName, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditHardwareEnabled(Guid hardwareId, string hardwareName, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditHardwareRemoved(Guid hardwareId, string hardwareName, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditHardwareRenamed(Guid hardwareId, string newHardwareName, string oldHardwareName, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditHardwareReplaced(Guid hardwareId, string newHardwareName, string oldHardwareName, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditHardwarePreConfigurationStarted(string hardwareId, string hardwareName, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditHardwareSettingsChanged(Guid hardwareId, string hardwareName, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditHostedHardwareAdded(int hostedHardwareId, string hostedHardwareName, string userCredentials, string locationIPAddress);

	void AuditHostedHardwareDeleted(int hostedHardwareId, string hostedHardwareName, string userCredentials, string locationIPAddress);

	void AuditHostedHardwareModified(int hostedHardwareId, string hostedHardwareName, string userCredentials, string locationIPAddress);

	void AuditHostedHardwareRenamed(int hostedHardwareId, string newHostedHardwareName, string oldHostedHardwareName, string userCredentials, string locationIPAddress);

	void AuditHostingServerAdded(int hostingServerId, string hostingServerName, string userCredentials, string locationIPAddress);

	void AuditHostingServerDeleted(int hostingServerId, string hostingServerName, string userCredentials, string locationIPAddress);

	void AuditHostingServerModified(int hostingServerId, string hostingServerName, string userCredentials, string locationIPAddress);

	void AuditHostingServerRenamed(int hostingServerId, string newHostingServerName, string oldHostingServerName, string userCredentials, string locationIPAddress);

	void AuditLoginLogoutTimeout(string userCredentials, string locationIPAddress);

	void AuditLoginSuccessfully(LogEntryAuditLogger.LoginFrom loginFrom, string userCredentials, string locationIPAddress);

	void AuditLoginTimeBased(string userCredentials, string locationIPAddress);

	void AuditLoginUnSuccessfulIncorrectPassword(string userCredentials, string locationIPAddress);

	void AuditLoginUnsuccessfullyInsufficientRights(LogEntryAuditLogger.LoginFrom loginFrom, string userCredentials, string locationIPAddress);

	void AuditLoginUnSuccessfulUnknownUser(string userCredentials, string locationIPAddress);

	void AuditLogout(string userCredentials, string locationIPAddress);

	void AuditMarkedDataCreated(string id, string name, string userCredentials, string locationIPAddress);

	void AuditMarkedDataDelete(string id, string name, string userCredentials, string locationIPAddress);

	void AuditMarkedDataUpdated(string id, string name, string userCredentials, string locationIPAddress);

	void AuditRecordingServerAuthorize(Guid recorderID, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditRecordingServerRemove(Guid recorderID, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditRecordingServerRename(string oldRecordingServerName, string newRecordingServerName, Guid recorderID, string userCredentials, string locationIPAddress);

	void AuditRetentionOptionRoleRelationUpdated(string roleId, string roleName, string retentionName, string userCredentials, string remoteIPAddress);

	void AuditLoginProviderCreated(Guid loginProviderId, string loginProviderName, string userCredentials, string locationIPAddress);

	void AuditLoginProviderUpdated(Guid loginProviderId, string loginProviderName, string userCredentials, string locationIPAddress);

	void AuditLoginProviderDeleted(Guid loginProviderId, string loginProviderName, string userCredentials, string locationIPAddress);

	void AuditRoleCreated(Guid roleID, string roleName, string userCredentials, string locationIPAddress);

	void AuditRoleDelete(Guid roleID, string roleName, string userCredentials, string locationIPAddress);

	void AuditRoleEdited(Guid roleID, string roleName, string userCredentials, string locationIPAddress);

	void AuditRoleGroupAdded(string userGroupName, Guid roleID, string roleName, string userCredentials, string locationIPAddress);

	void AuditRoleGroupRemoved(string userGroupName, Guid roleID, string roleName, string userCredentials, string locationIPAddress);

	void AuditRoleRenamed(string oldroleName, string newroleName, Guid roleID, string userCredentials, string locationIPAddress);

	void AuditRoleUserAdded(string userName, Guid roleID, string roleName, string userCredentials, string locationIPAddress);

	void AuditRoleUserRemoved(string userName, Guid roleID, string roleName, string userCredentials, string locationIPAddress);

	void AuditRuleCreated(Guid ruleID, string ruleName, string userCredentials, string locationIPAddress);

	void AuditRuleDelete(Guid ruleID, string ruleName, string userCredentials, string locationIPAddress);

	void AuditRuleDisabled(Guid ruleID, string ruleName, string userCredentials, string locationIPAddress);

	void AuditRuleEdited(Guid ruleID, string ruleName, string userCredentials, string locationIPAddress);

	void AuditRuleEnabled(Guid ruleID, string ruleName, string userCredentials, string locationIPAddress);

	void AuditRuleRenamed(string oldruleName, string newruleName, Guid ruleID, string userCredentials, string locationIPAddress);

	void AuditSitesMasterSiteAuthorized(Guid masterSiteId, string masterSiteName, string masterSiteAddress, string userCredentials, string locationIPAddress);

	void AuditSitesMasterSiteAuthorizeDenied(Guid masterSiteId, string masterSiteName, string masterSiteAddress, string userCredentials, string locationIPAddress);

	void AuditSitesMasterSiteDeleted(Guid masterSiteId, string masterSiteName, string userCredentials, string locationIPAddress);

	void AuditSitesRenamed(Guid siteId, string newSiteName, string oldSiteName, string userCredentials, string locationIPAddress);

	void AuditSitesSlaveSiteCreated(Guid slaveSiteId, string slaveSiteName, string slaveSiteAddress, string userCredentials, string locationIPAddress);

	void AuditSitesSlaveSiteDeleted(Guid slaveSiteId, string slaveSiteName, string slaveSiteAddress, string userCredentials, string locationIPAddress);

	void AuditStorageArchiveCreated(Guid storageId, string storageName, string archiveName, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditStorageArchiveDeleted(Guid storageId, string storageName, string archiveName, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditStorageArchiveEdited(Guid storageId, string storageName, string archiveName, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditStorageArchiveMoved(Guid storageId, string storageName, string archiveName, string oldPath, string newPath, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditStorageCreated(Guid storageId, string storageName, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditStorageDeleted(Guid storageId, string storageName, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditStorageEdited(Guid storageId, string storageName, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditStorageMoved(Guid storageId, string storageName, string oldPath, string newPath, string recordingServerName, string userCredentials, string locationIPAddress);

	void AuditTimeProfileCreated(Guid timeProfileID, string timeProfileName, string userCredentials, string locationIPAddress);

	void AuditTimeProfileDelete(Guid timeProfileID, string timeProfileName, string userCredentials, string locationIPAddress);

	void AuditTimeProfileEdited(Guid timeProfileID, string timeProfileName, string userCredentials, string locationIPAddress);

	void AuditTimeProfileRenamed(string oldtimeProfileName, string newtimeProfileName, Guid timeProfileID, string userCredentials, string locationIPAddress);

	void AuditUserDefinedEventActivated(Guid eventId, string userDefinedEventName, string userCredentials, string locationIPAddress);

	void AuditVideoWallCreated(Guid videoWallId, string videoWallName, string userCredentials, string locationIPAddress);

	void AuditVideoWallDeleted(Guid videoWallId, string videoWallName, string userCredentials, string locationIPAddress);

	void AuditVideoWallMonitorAdded(Guid videoWallMonitorId, string videoWallMonitorName, Guid videoWallId, string videoWallName, string userCredentials, string locationIPAddress);

	void AuditVideoWallMonitorDeleted(Guid videoWallMonitorId, string videoWallMonitorName, Guid videoWallId, string videoWallName, string userCredentials, string locationIPAddress);

	void AuditVideoWallMonitorEdited(Guid videoWallMonitorId, string videoWallMonitorName, Guid videoWallId, string videoWallName, string userCredentials, string locationIPAddress);

	void AuditVideoWallMonitorLayoutCameraPlaced(string cameraName, string position, string videoWallMonitorName, Guid videoWallMonitorId, string videoWallName, string userCredentials, string locationIPAddress);

	void AuditVideoWallMonitorLayoutChanged(string layoutName, string videoWallMonitorName, Guid videoWallMonitorId, string videoWallName, string userCredentials, string locationIPAddress);

	void AuditVideoWallMonitorLayoutCleared(Guid videoWallMonitorId, string videoWallMonitorName, Guid videoWallId, string videoWallName, string userCredentials, string locationIPAddress);

	void AuditVideoWallMonitorLayoutMonitorApplied(Guid videoWallMonitorId, string videoWallMonitorName, Guid videoWallId, string videoWallName, string sourceVideoWallMonitorName, string userCredentials, string locationIPAddress);

	void AuditVideoWallMonitorLayoutPositionCleared(string position, string videoWallMonitorName, Guid videoWallMonitorId, string videoWallName, string userCredentials, string locationIPAddress);

	void AuditVideoWallMonitorLayoutPresetSelected(string presetName, string videoWallMonitorName, Guid videoWallMonitorId, string videoWallName, string userCredentials, string locationIPAddress);

	void AuditVideoWallMonitorLayoutViewApplied(Guid videoWallMonitorId, string videoWallMonitorName, Guid videoWallId, string videoWallName, string sourceVideoWallViewName, string userCredentials, string locationIPAddress);

	void AuditVideoWallMonitorPresetEdited(Guid videoWallPresetId, string videoWallPresetName, Guid videoWallMonitorId, string videoWallMonitorName, string userCredentials, string locationIPAddress);

	void AuditVideoWallMonitorRenamed(string newVideoWallMonitorName, string oldVideoWallMonitorName, Guid videoWallMonitorId, string videoWallName, string userCredentials, string locationIPAddress);

	void AuditVideoWallPresetActivated(Guid videoWallPresetId, string videoWallPresetName, Guid videoWallId, string videoWallName, string userCredentials, string locationIPAddress);

	void AuditVideoWallPresetAdded(Guid videoWallPresetId, string videoWallPresetName, Guid videoWallId, string videoWallName, string userCredentials, string locationIPAddress);

	void AuditVideoWallPresetDeleted(Guid videoWallPresetId, string videoWallPresetName, Guid videoWallId, string videoWallName, string userCredentials, string locationIPAddress);

	void AuditVideoWallPresetRenamed(Guid videoWallPresetId, string oldVideoWallPresetName, string newVideoWallPresetName, Guid videoWallId, string videoWallName, string userCredentials, string locationIPAddress);

	void AuditVideoWallRenamed(string newVideoWallName, string oldVideoWallName, Guid videoWallId, string userCredentials, string locationIPAddress);

	void AuditVideoWallEdited(Guid videoWallId, string videoWallName, string userCredentials, string locationIPAddress);

	void LogEntryAuditLog(LogEntryAudit log);
}
