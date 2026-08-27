using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

public interface IServerFunctions
{
	VMOWebserviceObject[] Execute(string functionName, VmoPropertyCollection parameters);

	VMOWebserviceObject[] Execute(string functionName, VMOWebserviceObject[] parameters);

	void ActivateVideoWallPreset(string videoWallPresetId);

	void BasicUserChangePassword(string basicUserId, string newPassword);

	void TriggerUserDefinedEvent(string userDefinedEventId);

	void TriggerPluginEvent(Guid pluginEventId, Guid sourceId, Guid[] metadataDeviceIds, Guid[] metadataDeviceGroupIds, string customData);

	VMOWebserviceObject AuthorizeMasterSite();

	void MarkChildSiteInconsistent(string childSiteId);

	void SynchronizeSite(string siteId);

	void TriggerFullSiteSynchronization();

	VMOWebserviceObject AttachChildSite(string childSiteId, AddressCollection addresses, string childSiteVersion);

	VMOWebserviceObject AttachMasterSite(string masterSiteId, AddressCollection addresses, string masterSiteVersion);

	void DetachSite(string siteId);

	VMOWebserviceObject ApplyLayout(string monitorId, string layoutId);

	VMOWebserviceObject ApplyCamerasToMonitorView(string monitorId, string[] cameras, string[] cameraGroups, int fromPosition);

	VMOWebserviceObject ApplyCamerasAndLayoutToMonitorView(string monitorId, string[] cameras, string[] cameraGroups, int fromPosition, string layoutId);

	VMOWebserviceObject ApplyViewItemXmlsToMonitorView(string monitorId, string[] viewItemXmls, int fromPosition);

	void ApplyMonitorStateToMonitor(string monitorId, string xmlState);

	VMOWebserviceObject ApplyTextAndLayoutToMonitorView(string monitorId, string messageToDisplay, int fromPosition, string layoutId);

	void RemoveCamerasFromMonitorView(string monitorId, string[] cameras, string[] cameraGroups);

	VMOWebserviceObject CreateView(string viewGroupId, VmoPropertyCollection properties, CustomPropertyCollection customProperties, VmoPropertyCollection[] viewItems, CustomPropertyCollection[] viewItemsCustomProperties);

	VMOWebserviceObject ClearViewItem(string viewItemId);

	VMOWebserviceObject ClearView(string viewId);

	VMOWebserviceObject ApplyViewToView(string sourceViewId, string viewId);

	void ChangeViewItemId(string oldViewItemId, string newViewItemId, string viewId);

	VMOWebserviceObject GetRecordingServerInformation(string recordingServerId);

	VMOWebserviceObject GetRecordingStorageInformation(string recordingStorageId);

	VMOWebserviceObject GetArchiveStorageInformation(string archiveStorageId);

	bool ValidateEncryptionPasswordRecordingStorage(string recordingStorageId, string password);

	VMOWebserviceObject[] EnableRecordingStorageEncryption(string recordingStorageId, StorageEncryptionMethod method, string password);

	VMOWebserviceObject[] DisableRecordingStorageEncryption(string recordingStorageId, string password);

	void SetRecordingStorageAsDefault(string recordingStorageId);

	void EnableFramerateReductionArchiveStorage(string archiveStorageId, bool enabled);

	VMOWebserviceObject[] QuerySystemMonitorStateSourcesByIds(string[] ids);

	VMOWebserviceObject[] QuerySystemMonitorStateSourcesByTypes(string[] types);

	VMOWebserviceObject[] QueryAllRuleSources();

	MarkedDataReference QueryMarkedDataReference(string[] deviceIds, bool realTime, MarkedDataDefinitions.MarkedDataTypes markedDataTypes);

	VMOWebserviceObject QueryMarkedData(string markedDataId);

	VMOWebserviceObject[] QueryBookmarksSearch(DateTime centerTime, DateTime timeLimit, int countLimit, DeviceType[] deviceTypes, string[] optDeviceIds, string[] optUsers, string optSearchStr, string optFromBookmarkId);

	VMOWebserviceObject[] QueryBookmarksSearch2(DateTime centerTime, DateTime timeLimit, int countLimit, DeviceType[] deviceTypes, string[] optDeviceIds, string[] optUsers, string optSearchStr, bool optionalSearchHeader, bool optionalSearchDescription, bool optionalSearchReference, bool optionalSearchUser, string optionalHeaderFilter, string optionalDescriptionFilter, string optFromBookmarkId);

	VMOWebserviceObject[] QueryMarkedDataSearch(string[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, string sortOrder, bool sortAscending);

	VMOWebserviceObject[] QueryMarkedDataSearch2(string[] deviceIds, string searchText, bool optionalSearchHeader, bool optionalSearchDescription, bool optionalSearchReference, bool optionalSearchUser, string optionalHeaderFilter, string optionalDescriptionFilter, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, string sortOrder, bool sortAscending);

	VMOWebserviceObject CreateMarkedData(VmoPropertyCollection properties);

	VMOWebserviceObject UpdateMarkedData(VMOWebserviceObject markedDataVMOWebserviceObject);

	void DeleteMarkedData(string markedDataId, MarkedDataDefinitions.MarkedDataTypes markedDataType);

	void DeleteBookmarksByDeviceIdBeforeTime(MediaStatusEntry mediaStatus);

	void EnableFailoverServer(string failoverServerId);

	void DisableFailoverServer(string failoverServerId);

	void StopFailoverService(string failoverServerId);

	void MoveFailoverServer(string failoverServerId, string newParentGroupId, int insertAtPosition);

	VMOWebserviceObject[] QueryRecordingServerHardwareDrivers(string recordingServerId, bool used);

	VMOWebserviceObject[] QueryFailoverServerHardwareDrivers(string failoverServerId);

	VMOWebserviceObject[] QueryActiveHardwareDrivers(string recordingServerId);

	VMOWebserviceObject QueryActiveHardwareDriver(string recordingServerId, string driverId);

	void StopEdgeStorageRetrieval(string taskId);

	void CleanupTask(string taskId);

	bool IsFeatureLicensed(string featureName);

	string GetFeatureParameterValue(string featureName, string parameterName);

	void UpdateLicense(byte[] license, string ownerXml, string sitesInformation);

	void UpdateMipPluginsLicenseRequestPart(string license);

	void QueueAutomaticActivation();

	void RefreshLicenseCache();

	void SetSystemSetting(string name, string value);

	string GetSystemSetting(string name);

	VMOExceptionDetail GetLastAutomaticActivationError();

	void DeleteServerTasksByDeviceBeforeTime(string deviceId, DateTime time);

	void ClientProfileMoveUpPriority(string profileId);

	void ClientProfileMoveDownPriority(string profileId);

	string QueryActiveManagementClientProfileId();

	string QueryActiveSmartClientProfileId();

	string QueryActiveSmartClientProfileId(string sid);

	void AttachClientProfileToRole(string profileId, string roleId);

	int GetServerStatus();

	string GetCreatePrivateViewRoot();
}
