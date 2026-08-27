using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using VideoOS.Management.Client.Proxy;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient.Proxy;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class ManagementServerServiceClient : ClientBase<IManagementServerService>, IManagementServerService
{
	public ManagementServerServiceClient()
	{
	}

	public ManagementServerServiceClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public ManagementServerServiceClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ManagementServerServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ManagementServerServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public GenericObjectInfo2[] Execute(string functionName, GenericObjectInfo2[] parameters)
	{
		return base.Channel.Execute(functionName, parameters);
	}

	public GenericObjectInfo2 QueryObject2(string entityName, string id, string[] propertiesToLoad)
	{
		return base.Channel.QueryObject2(entityName, id, propertiesToLoad);
	}

	public GenericObjectInfo2[] QueryObjects(string entityName, string[] ids, string[] propertiesToLoad)
	{
		return base.Channel.QueryObjects(entityName, ids, propertiesToLoad);
	}

	public string[] QueryObjectCollectionIds(string entityName, string parentEntityName, string parentId, string[] propertiesToLoad)
	{
		return base.Channel.QueryObjectCollectionIds(entityName, parentEntityName, parentId, propertiesToLoad);
	}

	public GenericObjectInfo2 CreateObject2(string entityName, ObjectPropertyInfo2[] properties, CustomPropertyInfo[] customProperties)
	{
		return base.Channel.CreateObject2(entityName, properties, customProperties);
	}

	public GenericObjectInfo2 UpdateObject2(GenericObjectInfo2 genericObject, CustomPropertyInfo[] customProperties)
	{
		return base.Channel.UpdateObject2(genericObject, customProperties);
	}

	public void DeleteObject2(string entityName, string id)
	{
		base.Channel.DeleteObject2(entityName, id);
	}

	public ServerGroupInfo[] QueryServerGroups(string groupStructureType, string parentGroupId)
	{
		return base.Channel.QueryServerGroups(groupStructureType, parentGroupId);
	}

	public ServerGroupInfo QueryServerGroupById(string parentGroupId, string serverGroupId)
	{
		return base.Channel.QueryServerGroupById(parentGroupId, serverGroupId);
	}

	public ServerGroupInfo QueryServerGroupByName(string parentGroupId, string name)
	{
		return base.Channel.QueryServerGroupByName(parentGroupId, name);
	}

	public ServerGroupInfo CreateServerGroup(string name, string description, string groupStructureType, string parentGroupId, ServerGroupOptions serverGroupOptions)
	{
		return base.Channel.CreateServerGroup(name, description, groupStructureType, parentGroupId, serverGroupOptions);
	}

	public ServerGroupInfo UpdateServerGroup(string serverGroupId, string name, string description, ServerGroupOptions serverGroupOptions)
	{
		return base.Channel.UpdateServerGroup(serverGroupId, name, description, serverGroupOptions);
	}

	public void DeleteServerGroup(string serverGroupId)
	{
		base.Channel.DeleteServerGroup(serverGroupId);
	}

	public void MoveServerGroup(string serverGroupId, string newParentGroupId)
	{
		base.Channel.MoveServerGroup(serverGroupId, newParentGroupId);
	}

	public CustomPropertyInfo[] GetCustomProperties(string objectId)
	{
		return base.Channel.GetCustomProperties(objectId);
	}

	public CustomPropertyInfo GetCustomProperty(string objectId, string name)
	{
		return base.Channel.GetCustomProperty(objectId, name);
	}

	public void UpdateCustomProperties(string objectId, CustomPropertyInfo[] customPropertyArray)
	{
		base.Channel.UpdateCustomProperties(objectId, customPropertyArray);
	}

	public void DeleteCustomProperty(string objectId, string name)
	{
		base.Channel.DeleteCustomProperty(objectId, name);
	}

	public void TriggerUserDefinedEvent2(string userDefinedEventId)
	{
		base.Channel.TriggerUserDefinedEvent2(userDefinedEventId);
	}

	public void TriggerPluginEvent(Guid pluginEventId, Guid sourceId, Guid[] metadataDeviceIds, Guid[] metadataDeviceGroupIds, string customData)
	{
		base.Channel.TriggerPluginEvent(pluginEventId, sourceId, metadataDeviceIds, metadataDeviceGroupIds, customData);
	}

	public void TriggerPluginEventBulk(PluginEventInfo[] pluginEvents)
	{
		base.Channel.TriggerPluginEventBulk(pluginEvents);
	}

	public void DeleteSite(string siteId)
	{
		base.Channel.DeleteSite(siteId);
	}

	public void SyncronizeSite(string siteId)
	{
		base.Channel.SyncronizeSite(siteId);
	}

	public void MarkInconsistent(string siteId)
	{
		base.Channel.MarkInconsistent(siteId);
	}

	public void TriggerFullSiteSynchronization()
	{
		base.Channel.TriggerFullSiteSynchronization();
	}

	public TaskInfo[] QueryTasks()
	{
		return base.Channel.QueryTasks();
	}

	public TaskInfo[] QueryTasksByRecordingServer(string recordingServerId)
	{
		return base.Channel.QueryTasksByRecordingServer(recordingServerId);
	}

	public TaskInfo QueryTask(string taskId)
	{
		return base.Channel.QueryTask(taskId);
	}

	public TaskInfo MoveRecordingStorage(string recordingStorageId, string newPath)
	{
		return base.Channel.MoveRecordingStorage(recordingStorageId, newPath);
	}

	public TaskInfo DeleteRecordingStorage(string recordingStorageId, bool deleteIfOffline)
	{
		return base.Channel.DeleteRecordingStorage(recordingStorageId, deleteIfOffline);
	}

	public TaskInfo MoveArchiveStorage(string archiveStorageId, string newPath)
	{
		return base.Channel.MoveArchiveStorage(archiveStorageId, newPath);
	}

	public TaskInfo DeleteArchiveStorage(string archiveStorageId, bool deleteIfOffline)
	{
		return base.Channel.DeleteArchiveStorage(archiveStorageId, deleteIfOffline);
	}

	public TaskInfo TruncateDeviceStorage(string deviceId)
	{
		return base.Channel.TruncateDeviceStorage(deviceId);
	}

	public TaskInfo ChangeDeviceRecordingStorage(string deviceId, string newStorageId, bool moveData)
	{
		return base.Channel.ChangeDeviceRecordingStorage(deviceId, newStorageId, moveData);
	}

	public TaskInfo[] EnableRecordingStorageEncryption(string recordingStorageId, EncryptionMethod encryptionMethod, string password)
	{
		return base.Channel.EnableRecordingStorageEncryption(recordingStorageId, encryptionMethod, password);
	}

	public TaskInfo[] DisableRecordingStorageEncryption(string recordingStorageId, string password)
	{
		return base.Channel.DisableRecordingStorageEncryption(recordingStorageId, password);
	}

	public TaskInfo ChangeRecordingStorageEncryptionMethod(string recordingStorageId, EncryptionMethod encryptionMethod, string password)
	{
		return base.Channel.ChangeRecordingStorageEncryptionMethod(recordingStorageId, encryptionMethod, password);
	}

	public TaskInfo ChangeRecordingStorageEncryptionPassword(string recordingStorageId, string oldPassword, string newPassword)
	{
		return base.Channel.ChangeRecordingStorageEncryptionPassword(recordingStorageId, oldPassword, newPassword);
	}

	public TaskInfo RequestEdgeStorageRetrieval(string deviceId, DateTime startTime, DateTime endTime, string metadata)
	{
		return base.Channel.RequestEdgeStorageRetrieval(deviceId, startTime, endTime, metadata);
	}

	public void StopEdgeStorageRetrieval(string taskId)
	{
		base.Channel.StopEdgeStorageRetrieval(taskId);
	}

	public void CleanupTask(string taskId)
	{
		base.Channel.CleanupTask(taskId);
	}

	public BasicUserInfo[] QueryBasicUsers()
	{
		return base.Channel.QueryBasicUsers();
	}

	public BasicUserInfo QueryBasicUserById(string id)
	{
		return base.Channel.QueryBasicUserById(id);
	}

	public BasicUserInfo QueryBasicUserBySid(string sid)
	{
		return base.Channel.QueryBasicUserBySid(sid);
	}

	public BasicUserInfo[] QueryBasicUsersByIds(string[] ids)
	{
		return base.Channel.QueryBasicUsersByIds(ids);
	}

	public BasicUserInfo CreateBasicUser(string id, string userName, string password, string description, bool canChangePassword)
	{
		return base.Channel.CreateBasicUser(id, userName, password, description, canChangePassword);
	}

	public BasicUserInfo UpdateBasicUser(string id, string userName, string description, bool canChangePassword)
	{
		return base.Channel.UpdateBasicUser(id, userName, description, canChangePassword);
	}

	public void DeleteBasicUser(string id)
	{
		base.Channel.DeleteBasicUser(id);
	}

	public void ChangePasswordBasicUser(string id, string newPassword)
	{
		base.Channel.ChangePasswordBasicUser(id, newPassword);
	}

	public GenericObjectInfo2 ApplyLayoutGeneric(string monitorId, string layoutId)
	{
		return base.Channel.ApplyLayoutGeneric(monitorId, layoutId);
	}

	public GenericObjectInfo2 ApplyCamerasToMonitorViewGeneric(string monitorId, string[] cameras, string[] cameraGroups, int fromPosition)
	{
		return base.Channel.ApplyCamerasToMonitorViewGeneric(monitorId, cameras, cameraGroups, fromPosition);
	}

	public GenericObjectInfo2 ApplyCamerasAndLayoutToMonitorViewGeneric(string monitorId, string[] cameras, string[] cameraGroups, int fromPosition, string layoutId)
	{
		return base.Channel.ApplyCamerasAndLayoutToMonitorViewGeneric(monitorId, cameras, cameraGroups, fromPosition, layoutId);
	}

	public GenericObjectInfo2 ApplyViewItemXmlsToMonitorViewGeneric(string monitorId, string[] viewItemXmls, int fromPosition)
	{
		return base.Channel.ApplyViewItemXmlsToMonitorViewGeneric(monitorId, viewItemXmls, fromPosition);
	}

	public void ApplyMonitorStateToMonitorGeneric(string monitorId, string xmlState)
	{
		base.Channel.ApplyMonitorStateToMonitorGeneric(monitorId, xmlState);
	}

	public GenericObjectInfo2 ApplyTextAndLayoutToMonitorViewGeneric(string monitorId, string messageToDisplay, int fromPosition, string layoutId)
	{
		return base.Channel.ApplyTextAndLayoutToMonitorViewGeneric(monitorId, messageToDisplay, fromPosition, layoutId);
	}

	public void RemoveCamerasFromMonitorViewGeneric(string monitorId, string[] cameras, string[] cameraGroups)
	{
		base.Channel.RemoveCamerasFromMonitorViewGeneric(monitorId, cameras, cameraGroups);
	}

	public void ActivateVideoWallPreset(string videoWallPresetId)
	{
		base.Channel.ActivateVideoWallPreset(videoWallPresetId);
	}

	public GenericObjectInfo2 AttachSlaveSite2(string slaveSiteId, string slaveSiteAddressesXml, string slaveSiteVersion)
	{
		return base.Channel.AttachSlaveSite2(slaveSiteId, slaveSiteAddressesXml, slaveSiteVersion);
	}

	public GenericObjectInfo2 AttachMasterSite2(string masterSiteId, string masterSiteAddressesXml, string masterSiteVersion)
	{
		return base.Channel.AttachMasterSite2(masterSiteId, masterSiteAddressesXml, masterSiteVersion);
	}

	public GenericObjectInfo2 AuthorizeMaster2()
	{
		return base.Channel.AuthorizeMaster2();
	}

	public int GetServerStatus()
	{
		return base.Channel.GetServerStatus();
	}

	public GenericObjectInfo2 CreateViewGeneric(string viewGroupId, ObjectPropertyInfo2[] viewProperties, CustomPropertyInfo[] viewCustomProperties, ObjectPropertyInfo2[][] viewItemsProperties, CustomPropertyInfo[][] viewItemsCustomProperties)
	{
		return base.Channel.CreateViewGeneric(viewGroupId, viewProperties, viewCustomProperties, viewItemsProperties, viewItemsCustomProperties);
	}

	public GenericObjectInfo2 ClearViewGeneric(string viewId)
	{
		return base.Channel.ClearViewGeneric(viewId);
	}

	public GenericObjectInfo2 ApplyViewToViewGeneric(string sourceViewId, string destinationViewId)
	{
		return base.Channel.ApplyViewToViewGeneric(sourceViewId, destinationViewId);
	}

	public GenericObjectInfo2 ClearViewItemGeneric(string viewItemId)
	{
		return base.Channel.ClearViewItemGeneric(viewItemId);
	}

	public void ChangeViewItemId(string existingId, string newId)
	{
		base.Channel.ChangeViewItemId(existingId, newId);
	}

	public GenericObjectInfo2 GetRecordingServerInformation(string recordingServerId)
	{
		return base.Channel.GetRecordingServerInformation(recordingServerId);
	}

	public GenericObjectInfo2 GetRecordingStorageInformation(string recordingStorageId)
	{
		return base.Channel.GetRecordingStorageInformation(recordingStorageId);
	}

	public GenericObjectInfo2 GetArchiveStorageInformation(string archiveStorageId)
	{
		return base.Channel.GetArchiveStorageInformation(archiveStorageId);
	}

	public void SetRecordingStorageAsDefault(string recordingStorageId)
	{
		base.Channel.SetRecordingStorageAsDefault(recordingStorageId);
	}

	public void EnableFramerateReduction(string archiveStorageId, bool enable)
	{
		base.Channel.EnableFramerateReduction(archiveStorageId, enable);
	}

	public bool ValidateEncryptionPassword(string recordingStorageId, string password)
	{
		return base.Channel.ValidateEncryptionPassword(recordingStorageId, password);
	}

	public GenericObjectInfo2[] QuerySystemMonitorStateSourcesByIds(string[] ids)
	{
		return base.Channel.QuerySystemMonitorStateSourcesByIds(ids);
	}

	public GenericObjectInfo2[] QuerySystemMonitorStateSourcesByTypes(string[] types)
	{
		return base.Channel.QuerySystemMonitorStateSourcesByTypes(types);
	}

	public GenericObjectInfo2[] QueryAllRuleSources()
	{
		return base.Channel.QueryAllRuleSources();
	}

	public MarkedDataReferenceInfo2 QueryMarkedDataReference(string[] deviceIds, bool realTime, MarkedDataDefinitions.MarkedDataTypes markedDataTypes)
	{
		return base.Channel.QueryMarkedDataReference(deviceIds, realTime, markedDataTypes);
	}

	public GenericObjectInfo2 QueryMarkedData(string markedDataId)
	{
		return base.Channel.QueryMarkedData(markedDataId);
	}

	public GenericObjectInfo2[] QueryBookmarksSearch(DateTime centerTime, DateTime timeLimit, int countLimit, string[] deviceTypes, string[] optDeviceIds, string[] optUsers, string optSearchStr, string optFromBookmarkId)
	{
		return base.Channel.QueryBookmarksSearch(centerTime, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, optFromBookmarkId);
	}

	public GenericObjectInfo2[] QueryBookmarksSearch2(DateTime centerTime, DateTime timeLimit, int countLimit, string[] deviceTypes, string[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, string optFromBookmarkId)
	{
		return base.Channel.QueryBookmarksSearch2(centerTime, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter, optFromBookmarkId);
	}

	public GenericObjectInfo2[] QueryMarkedDataSearch(string[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFromTime, DateTime retentionExpireToTime, int pageIndex, int pageSize, string sortOrder, bool sortAscending)
	{
		return base.Channel.QueryMarkedDataSearch(deviceIds, searchText, users, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime, retentionExpireFromTime, retentionExpireToTime, pageIndex, pageSize, sortOrder, sortAscending);
	}

	public GenericObjectInfo2[] QueryMarkedDataSearch2(string[] deviceIds, string searchText, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFromTime, DateTime retentionExpireToTime, int pageIndex, int pageSize, string sortOrder, bool sortAscending)
	{
		return base.Channel.QueryMarkedDataSearch2(deviceIds, searchText, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter, users, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime, retentionExpireFromTime, retentionExpireToTime, pageIndex, pageSize, sortOrder, sortAscending);
	}

	public GenericObjectInfo2 CreateMarkedData(ObjectPropertyInfo2[] properties)
	{
		return base.Channel.CreateMarkedData(properties);
	}

	public GenericObjectInfo2 UpdateMarkedData(ObjectPropertyInfo2[] properties)
	{
		return base.Channel.UpdateMarkedData(properties);
	}

	public void DeleteMarkedData(string markedDataId, MarkedDataDefinitions.MarkedDataTypes markedDataType)
	{
		base.Channel.DeleteMarkedData(markedDataId, markedDataType);
	}

	public void DeleteBookmarksByDeviceIdBeforeTime(MediaStatusEntry mediaStatus)
	{
		base.Channel.DeleteBookmarksByDeviceIdBeforeTime(mediaStatus);
	}

	public void EnableFailoverServer(string failoverServerId)
	{
		base.Channel.EnableFailoverServer(failoverServerId);
	}

	public void DisableFailoverServer(string failoverServerId)
	{
		base.Channel.DisableFailoverServer(failoverServerId);
	}

	public void StopFailoverService(string failoverServerId)
	{
		base.Channel.StopFailoverService(failoverServerId);
	}

	public void MoveFailoverServer(string failoverServerId, string newParentId, int insertAtPosition)
	{
		base.Channel.MoveFailoverServer(failoverServerId, newParentId, insertAtPosition);
	}

	public GenericObjectInfo2[] QueryRecordingServerHardwareDrivers(string recordingServerId, bool used)
	{
		return base.Channel.QueryRecordingServerHardwareDrivers(recordingServerId, used);
	}

	public GenericObjectInfo2[] QueryFailoverServerHardwareDrivers(string failoverServerId)
	{
		return base.Channel.QueryFailoverServerHardwareDrivers(failoverServerId);
	}

	public GenericObjectInfo2[] QueryActiveRecordingServerHardwareDrivers(string recordingServerId)
	{
		return base.Channel.QueryActiveRecordingServerHardwareDrivers(recordingServerId);
	}

	public GenericObjectInfo2 QueryActiveHardwareDriver(string recordingServerId, string driverId)
	{
		return base.Channel.QueryActiveHardwareDriver(recordingServerId, driverId);
	}

	public void DeleteServerTasksByDeviceIdBeforeTime(string deviceId, DateTime time)
	{
		base.Channel.DeleteServerTasksByDeviceIdBeforeTime(deviceId, time);
	}

	public void ClientProfileMovePriorityUp(string id)
	{
		base.Channel.ClientProfileMovePriorityUp(id);
	}

	public void ClientProfileMovePriorityDown(string id)
	{
		base.Channel.ClientProfileMovePriorityDown(id);
	}

	public string QueryActiveManagementClientProfileId()
	{
		return base.Channel.QueryActiveManagementClientProfileId();
	}

	public string QueryActiveSmartClientProfileId()
	{
		return base.Channel.QueryActiveSmartClientProfileId();
	}

	public string QueryActiveSmartClientProfileIdBySid(string sid)
	{
		return base.Channel.QueryActiveSmartClientProfileIdBySid(sid);
	}

	public void AttachClientProfileToRole(string id, string roleId)
	{
		base.Channel.AttachClientProfileToRole(id, roleId);
	}

	public void SetSystemSetting(string name, string value)
	{
		base.Channel.SetSystemSetting(name, value);
	}

	public string GetSystemSetting(string name)
	{
		return base.Channel.GetSystemSetting(name);
	}

	public bool IsFeatureLicensed(string featureName)
	{
		return base.Channel.IsFeatureLicensed(featureName);
	}

	public string GetFeatureParameterValue(string featureName, string parameterName)
	{
		return base.Channel.GetFeatureParameterValue(featureName, parameterName);
	}

	public void UpdateLicense(byte[] license, string ownerXml, string sitesInformation)
	{
		base.Channel.UpdateLicense(license, ownerXml, sitesInformation);
	}

	public VMOExceptionDetail GetLastAutomaticActivationError()
	{
		return base.Channel.GetLastAutomaticActivationError();
	}

	public void UpdateMipPluginsLicenseRequestPart(string license)
	{
		base.Channel.UpdateMipPluginsLicenseRequestPart(license);
	}

	public void QueueAutomaticActivation()
	{
		base.Channel.QueueAutomaticActivation();
	}

	public void RefreshLicenseCache()
	{
		base.Channel.RefreshLicenseCache();
	}

	public void ResolveDataCollectorIdOnRegisteredServices(string[] serviceIds)
	{
		base.Channel.ResolveDataCollectorIdOnRegisteredServices(serviceIds);
	}
}
