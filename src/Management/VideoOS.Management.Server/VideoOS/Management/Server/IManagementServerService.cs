using System;
using System.ServiceModel;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[ServiceContract(Namespace = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01")]
public interface IManagementServerService
{
	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2[] Execute(string functionName, GenericObject2[] parameters);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2 QueryObject2(string entityName, string id, string[] propertiesToLoad);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2[] QueryObjects(string entityName, string[] ids, string[] propertiesToLoad);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	string[] QueryObjectCollectionIds(string entityName, string parentEntityName, string parentId, string[] propertiesToLoad);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2 CreateObject2(string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2 UpdateObject2(GenericObject2 genericObject, CustomProperty[] customProperties);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void DeleteObject2(string entityName, string id);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	ServerGroupOld[] QueryServerGroups(string groupStructureType, string parentGroupId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	ServerGroupOld CreateServerGroup(string name, string description, string groupStructureType, string parentGroupId, ServerGroupOptions serverGroupOptions);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	CustomProperty[] GetCustomProperties(string objectId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	CustomProperty GetCustomProperty(string objectId, string name);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void UpdateCustomProperties(string objectId, CustomProperty[] customPropertyArray);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void DeleteCustomProperty(string objectId, string name);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void TriggerUserDefinedEvent2(string userDefinedEventId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void TriggerPluginEvent(Guid pluginEventId, Guid sourceId, Guid[] metadataDeviceIds, Guid[] metadataDeviceGroupIds, string customData);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void TriggerPluginEventBulk(PluginEventInfo[] pluginEvents);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void DeleteSite(string siteId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void SyncronizeSite(string siteId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void MarkInconsistent(string siteId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void TriggerFullSiteSynchronization();

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	TaskOld[] EnableRecordingStorageEncryption(string recordingStorageId, EncryptionMethod encryptionMethod, string password);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	TaskOld[] DisableRecordingStorageEncryption(string recordingStorageId, string password);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void StopEdgeStorageRetrieval(string taskId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void CleanupTask(string taskId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void ChangePasswordBasicUser(string id, string newPassword);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2 ApplyLayoutGeneric(string monitorId, string layoutId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2 ApplyCamerasToMonitorViewGeneric(string monitorId, string[] cameras, string[] cameraGroups, int fromPosition);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2 ApplyCamerasAndLayoutToMonitorViewGeneric(string monitorId, string[] cameras, string[] cameraGroups, int fromPosition, string layoutId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2 ApplyViewItemXmlsToMonitorViewGeneric(string monitorId, string[] viewItemXmls, int fromPosition);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void ApplyMonitorStateToMonitorGeneric(string monitorId, string xmlState);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2 ApplyTextAndLayoutToMonitorViewGeneric(string monitorId, string messageToDisplay, int fromPosition, string layoutId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void RemoveCamerasFromMonitorViewGeneric(string monitorId, string[] cameras, string[] cameraGroups);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void ActivateVideoWallPreset(string videoWallPresetId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2 AttachSlaveSite2(string slaveSiteId, string slaveSiteAddressesXml, string slaveSiteVersion);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2 AttachMasterSite2(string masterSiteId, string masterSiteAddressesXml, string masterSiteVersion);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2 AuthorizeMaster2();

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	int GetServerStatus();

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2 CreateViewGeneric(string viewGroupId, ObjectProperty2[] viewProperties, CustomProperty[] viewCustomProperties, ObjectProperty2[][] viewItemsProperties, CustomProperty[][] viewItemsCustomProperties);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2 ClearViewGeneric(string viewId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2 ApplyViewToViewGeneric(string sourceViewId, string destinationViewId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2 ClearViewItemGeneric(string viewItemId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void ChangeViewItemId(string existingId, string newId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2 GetRecordingServerInformation(string recordingServerId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2 GetRecordingStorageInformation(string recordingStorageId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2 GetArchiveStorageInformation(string archiveStorageId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void SetRecordingStorageAsDefault(string recordingStorageId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void EnableFramerateReduction(string archiveStorageId, bool enable);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	bool ValidateEncryptionPassword(string recordingStorageId, string password);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2[] QuerySystemMonitorStateSourcesByIds(string[] ids);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2[] QuerySystemMonitorStateSourcesByTypes(string[] types);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2[] QueryAllRuleSources();

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	MarkedDataReference2 QueryMarkedDataReference(string[] deviceIds, bool realTime, MarkedDataDefinitions.MarkedDataTypes markedDataTypes);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2 QueryMarkedData(string markedDataId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2[] QueryBookmarksSearch(DateTime centerTime, DateTime timeLimit, int countLimit, string[] deviceTypes, string[] optDeviceIds, string[] optUsers, string optSearchStr, string optFromBookmarkId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2[] QueryBookmarksSearch2(DateTime centerTime, DateTime timeLimit, int countLimit, string[] deviceTypes, string[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, string optFromBookmarkId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2[] QueryMarkedDataSearch(string[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFromTime, DateTime retentionExpireToTime, int pageIndex, int pageSize, string sortOrder, bool sortAscending);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2[] QueryMarkedDataSearch2(string[] deviceIds, string searchText, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFromTime, DateTime retentionExpireToTime, int pageIndex, int pageSize, string sortOrder, bool sortAscending);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2 CreateMarkedData(ObjectProperty2[] properties);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2 UpdateMarkedData(ObjectProperty2[] properties);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void DeleteMarkedData(string markedDataId, MarkedDataDefinitions.MarkedDataTypes markedDataType);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void DeleteBookmarksByDeviceIdBeforeTime(MediaStatusEntry mediaStatus);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void EnableFailoverServer(string failoverServerId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void DisableFailoverServer(string failoverServerId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void StopFailoverService(string failoverServerId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void MoveFailoverServer(string failoverServerId, string newParentId, int insertAtPosition);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2[] QueryRecordingServerHardwareDrivers(string recordingServerId, bool used);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2[] QueryFailoverServerHardwareDrivers(string failoverServerId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2[] QueryActiveRecordingServerHardwareDrivers(string recordingServerId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	GenericObject2 QueryActiveHardwareDriver(string recordingServerId, string driverId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void DeleteServerTasksByDeviceIdBeforeTime(string deviceId, DateTime time);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void ClientProfileMovePriorityUp(string id);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void ClientProfileMovePriorityDown(string id);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	string QueryActiveManagementClientProfileId();

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	string QueryActiveSmartClientProfileId();

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	string QueryActiveSmartClientProfileIdBySid(string sid);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void AttachClientProfileToRole(string id, string roleId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void SetSystemSetting(string name, string value);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	string GetSystemSetting(string name);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	bool IsFeatureLicensed(string featureName);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	string GetFeatureParameterValue(string featureName, string parameterName);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void UpdateLicense(byte[] license, string ownerXml, string sitesInformation);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	VMOExceptionDetail GetLastAutomaticActivationError();

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void UpdateMipPluginsLicenseRequestPart(string license);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void QueueAutomaticActivation();

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void RefreshLicenseCache();

	[OperationContract]
	void ResolveDataCollectorIdOnRegisteredServices(string[] serviceIds);
}
