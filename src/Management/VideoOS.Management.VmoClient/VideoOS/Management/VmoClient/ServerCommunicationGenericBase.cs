using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Client.Proxy;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient.Proxy;

namespace VideoOS.Management.VmoClient;

public class ServerCommunicationGenericBase : ServerCommunicationBase
{
	private string _privateUserViewGroupId;

	private string _homeSiteId;

	protected string PrivateUserViewGroupId
	{
		get
		{
			if (string.IsNullOrEmpty(_privateUserViewGroupId))
			{
				_privateUserViewGroupId = GetPrivateUserViewGroupId();
			}
			return _privateUserViewGroupId;
		}
	}

	protected string HomeSiteId
	{
		get
		{
			if (string.IsNullOrEmpty(_homeSiteId))
			{
				_homeSiteId = GetHomeSiteId();
			}
			return _homeSiteId;
		}
	}

	public ServerCommunicationGenericBase(IVmoClient vmoClient)
		: base(vmoClient)
	{
	}

	public override VMOWebserviceObject CreateObject(Type type, VmoPropertyCollection properties, CustomPropertyCollection customProperties)
	{
		VMOWebserviceObject clientObj = new VMOWebserviceObject(properties, null, "", type);
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().CreateObject2(clientObj, customProperties);
	}

	public override VMOWebserviceObject UpdateObject(Type type, string id, VmoPropertyCollection properties, CustomPropertyCollection customProperties)
	{
		VMOWebserviceObject clientObj = new VMOWebserviceObject(properties, null, id, type);
		ManagementServerProxy clientProxy = _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>();
		CustomProperty[] customProperties2 = customProperties.DirtyCustomPropertyArray();
		return clientProxy.UpdateObject2(clientObj, customProperties2);
	}

	public override void DeleteObject(Type type, string id)
	{
		ManagementServerProxy clientProxy = _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>();
		string entityName = EntityNameAttributeHandlerWrapper.GetEntityName(type);
		clientProxy.DeleteObject2(entityName, id);
	}

	public override VMOWebserviceObject GetObjectById(Type type, string id)
	{
		ManagementServerProxy clientProxy = _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>();
		string entityName = EntityNameAttributeHandlerWrapper.GetEntityName(type);
		return clientProxy.QueryObject2(entityName, id, null);
	}

	public override VMOWebserviceObject[] GetObjectsByIds(Type type, string[] ids)
	{
		ManagementServerProxy clientProxy = _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>();
		string entityName = EntityNameAttributeHandlerWrapper.GetEntityName(type);
		return clientProxy.QueryObjects2(entityName, ids, null);
	}

	public override string[] GetObjectRelations(Type parentType, string parentId, Type type)
	{
		ManagementServerProxy clientProxy = _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>();
		string entityName = EntityNameAttributeHandlerWrapper.GetEntityName(type);
		return clientProxy.QueryRelationIds2(entityName, EntityNameAttributeHandlerWrapper.GetEntityName(parentType), parentId, null);
	}

	public override void AddGroupMembership(string parentServerGroupId, Type type, string id)
	{
		throw new NotImplementedException();
	}

	public override void RemoveGroupMembership(string parentServerGroupId, Type type, string id)
	{
		throw new NotImplementedException();
	}

	public override CustomProperty[] GetCustomProperties(string objectId)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().GetCustomProperties(objectId);
	}

	public override CustomProperty GetCustomProperty(string objectId, string key)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().GetCustomProperty(objectId, key);
	}

	public override void DeleteCustomProperty(string objectId, string key)
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().DeleteCustomProperty(objectId, key);
	}

	public override void SaveCustomProperties(string objectId, CustomProperty[] customProperties)
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().UpdateCustomProperty(objectId, customProperties);
	}

	public override void TriggerUserDefinedEvent(string userDefinedEventId)
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().TriggerUserDefinedEvent2(userDefinedEventId);
	}

	public override void TriggerPluginEvent(Guid pluginEventId, Guid sourceId, Guid[] metadataDeviceIds, Guid[] metadataDeviceGroupIds, string customData)
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().TriggerPluginEvent(pluginEventId, sourceId, metadataDeviceIds, metadataDeviceGroupIds, customData);
	}

	public override void BasicUserChangePassword(string basicUserId, string newPassword)
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().ChangePasswordBasicUser(basicUserId, newPassword);
	}

	public override bool IsFeatureLicensed(string featureName)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().IsFeatureLicensed(featureName);
	}

	public override string GetFeatureParameterValue(string featureName, string parameterName)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().GetFeatureParameterValue(featureName, parameterName);
	}

	public override void UpdateLicense(byte[] license, string ownerXml, string sitesInformation)
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().UpdateLicense(license, ownerXml, sitesInformation);
	}

	public override void QueueAutomaticActivation()
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().QueueAutomaticActivation();
	}

	public override VMOExceptionDetail GetLastAutomaticActivationError()
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().GetLastAutomaticActivationError();
	}

	public override void UpdateMipPluginsLicenseRequestPart(string license)
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().UpdateMipPluginsLicenseRequestPart(license);
	}

	public override void SetSystemSetting(string name, string value)
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().SetSystemSetting(name, value);
	}

	public override string GetSystemSetting(string name)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().GetSystemSetting(name);
	}

	public override VMOWebserviceObject[] Execute(string functionName, VMOWebserviceObject[] parameters)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().Execute(functionName, parameters);
	}

	public override int GetServerStatus()
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().GetServerStatus();
	}

	public override VMOWebserviceObject[] QuerySystemMonitorStateSourcesByTypes(string[] types)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().QuerySystemMonitorStateSourcesByTypes(types);
	}

	public override VMOWebserviceObject[] QuerySystemMonitorStateSourcesByIds(string[] ids)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().QuerySystemMonitorStateSourcesByIds(ids);
	}

	public override VMOWebserviceObject[] QueryAllRuleSources()
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().QueryAllRuleSources();
	}

	public override void ActivateVideoWallPreset(string videoWallPresetId)
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().ActivateVideoWallPreset(videoWallPresetId);
	}

	public override VMOWebserviceObject ApplyLayout(string monitorId, string layoutId)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().ApplyLayoutGeneric(monitorId, layoutId);
	}

	public override VMOWebserviceObject ApplyCamerasToMonitorView(string monitorId, string[] cameras, string[] cameraGroups, int fromPosition)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().ApplyCamerasToMonitorViewGeneric(monitorId, cameras, cameraGroups, fromPosition);
	}

	public override VMOWebserviceObject ApplyCamerasAndLayoutToMonitorView(string monitorId, string[] cameras, string[] cameraGroups, int fromPosition, string layoutId)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().ApplyCamerasAndLayoutToMonitorViewGeneric(monitorId, cameras, cameraGroups, fromPosition, layoutId);
	}

	public override VMOWebserviceObject ApplyViewItemXmlsToMonitorView(string monitorId, string[] viewItemXmls, int fromPosition)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().ApplyViewItemXmlsToMonitorViewGeneric(monitorId, viewItemXmls, fromPosition);
	}

	public override VMOWebserviceObject ApplyTextAndLayoutToMonitorView(string monitorId, string messageToDisplay, int fromPosition, string layoutId)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().ApplyTextAndLayoutToMonitorView(monitorId, messageToDisplay, fromPosition, layoutId);
	}

	public override void ApplyMonitorStateToMonitor(string monitorId, string xmlState)
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().ApplyMonitorStateToMonitor(monitorId, xmlState);
	}

	public override void RemoveCamerasFromMonitorView(string monitorId, string[] cameras, string[] cameraGroups)
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().RemoveCamerasFromMonitorViewGeneric(monitorId, cameras, cameraGroups);
	}

	public override VMOWebserviceObject AuthorizeMasterSite()
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().AuthorizeMaster2();
	}

	public override void MarkChildSiteInconsistent(string slaveSiteId)
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().MarkInconsistent(slaveSiteId.ToUpperInvariant());
	}

	public override void SynchronizeSite(string siteId)
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().SynchronizeSite(siteId.ToUpperInvariant());
	}

	public override void TriggerFullSiteSynchronization()
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().TriggerFullSiteSynchronization();
	}

	public override VMOWebserviceObject AttachChildSite(string slaveSiteId, AddressCollection addresses, string childSiteVersion)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().AttachSlaveSite2(slaveSiteId, addresses.ToServerAddressXml(), childSiteVersion);
	}

	public override VMOWebserviceObject AttachMasterSite(string masterSiteId, AddressCollection addresses, string masterSiteVersion)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().AttachMasterSite2(masterSiteId, addresses.ToServerAddressXml(), masterSiteVersion);
	}

	public override void DetachSite(string siteId)
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().DeleteSite(siteId);
	}

	public override VMOWebserviceObject ClearViewItem(string viewItemId)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().ClearViewItem(viewItemId);
	}

	public override VMOWebserviceObject ClearView(string viewId)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().ClearView(viewId);
	}

	public override VMOWebserviceObject ApplyViewToView(string sourceViewId, string viewId)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().ApplyViewToView(sourceViewId, viewId);
	}

	public override void ChangeViewItemId(string oldViewItemId, string newViewItemId, string viewId)
	{
		if (((IVmoRepositories)_vmoClient.Repositories).GetGenericRepository<ViewItem>().GetObjectFromCache(oldViewItemId) == null)
		{
			throw new NotSupportedException();
		}
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().ChangeViewItemId(oldViewItemId, newViewItemId);
	}

	public override VMOWebserviceObject CreateView(string viewGroupId, VmoPropertyCollection properties, CustomPropertyCollection customProperties, VmoPropertyCollection[] viewItemsProperties, CustomPropertyCollection[] viewItemsCustomProperties)
	{
		VMOWebserviceObject vMOWebserviceObject = new VMOWebserviceObject(properties, null, null, "View");
		List<VmoPropertyCollection> list = new List<VmoPropertyCollection>();
		foreach (VmoPropertyCollection item in viewItemsProperties)
		{
			list.Add(item);
		}
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().CreateView(viewGroupId, vMOWebserviceObject.Properties, customProperties, list.ToArray(), viewItemsCustomProperties);
	}

	public override VMOWebserviceObject GetRecordingServerInformation(string recordingServerId)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().GetRecordingServerInformation(recordingServerId);
	}

	public override VMOWebserviceObject GetRecordingStorageInformation(string recordingStorageId)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().GetRecordingStorageInformation(recordingStorageId);
	}

	public override VMOWebserviceObject GetArchiveStorageInformation(string archiveStorageId)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().GetArchiveStorageInformation(archiveStorageId);
	}

	public override bool ValidateEncryptionPasswordRecordingStorage(string recordingStorageId, string password)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().ValidateEncryptionPassword(recordingStorageId, password);
	}

	public override VMOWebserviceObject[] EnableRecordingStorageEncryption(string recordingStorageId, StorageEncryptionMethod method, string password)
	{
		if (!Enum.TryParse<EncryptionMethod>(method.ToString(), out var result))
		{
			throw new ArgumentException("Unsupported encryption methods should be validated in entity class", "method");
		}
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().EnableRecordingStorageEncryption(recordingStorageId, result, password);
	}

	public override VMOWebserviceObject[] DisableRecordingStorageEncryption(string recordingStorageId, string password)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().DisableRecordingStorageEncryption(recordingStorageId, password);
	}

	public override void SetRecordingStorageAsDefault(string recordingStorageId)
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().SetRecordingStorageAsDefault(recordingStorageId);
	}

	public override void EnableFramerateReductionArchiveStorage(string archiveStorageId, bool enabled)
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().EnableFramerateReduction(archiveStorageId, enabled);
	}

	public override MarkedDataReference QueryMarkedDataReference(string[] deviceIds, bool realTime, MarkedDataDefinitions.MarkedDataTypes markedDataTypes)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().QueryMarkedDataReference(_vmoClient.Repositories, deviceIds, realTime, markedDataTypes);
	}

	public override VMOWebserviceObject QueryMarkedData(string markedDataId)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().QueryMarkedData(markedDataId);
	}

	public override VMOWebserviceObject[] QueryBookmarksSearch(DateTime centerTime, DateTime timeLimit, int countLimit, DeviceType[] deviceTypes, string[] optDeviceIds, string[] optUsers, string optSearchStr, string optFromBookmarkId)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().QueryBookmarksSearch(centerTime, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, optFromBookmarkId);
	}

	public override VMOWebserviceObject[] QueryMarkedDataSearch(string[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, string sortOrder, bool sortAscending)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().QueryMarkedDataSearch(deviceIds, searchText, users, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime, retentionExpireFrom, retentionExpireTo, pageIndex, pageSize, sortOrder, sortAscending);
	}

	public override VMOWebserviceObject CreateMarkedData(VmoPropertyCollection properties)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().CreateMarkedData(properties);
	}

	public override VMOWebserviceObject UpdateMarkedData(VMOWebserviceObject markedDataVMOWebserviceObject)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().UpdateMarkedData(markedDataVMOWebserviceObject);
	}

	public override void DeleteMarkedData(string markedDataId, MarkedDataDefinitions.MarkedDataTypes markedDataType)
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().DeleteMarkedData(markedDataId, markedDataType);
	}

	public override void DeleteBookmarksByDeviceIdBeforeTime(MediaStatusEntry mediaStatus)
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().DeleteBookmarksByDeviceIdBeforeTime(mediaStatus);
	}

	public override void EnableFailoverServer(string failoverServerId)
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().EnableFailoverServer(failoverServerId);
	}

	public override void DisableFailoverServer(string failoverServerId)
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().DisableFailoverServer(failoverServerId);
	}

	public override void StopFailoverService(string failoverServerId)
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().StopFailoverService(failoverServerId);
	}

	public override void MoveFailoverServer(string failoverServerId, string newParentGroupId, int insertAtPosition)
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().MoveFailoverServer(failoverServerId, newParentGroupId, insertAtPosition);
	}

	public override VMOWebserviceObject[] QueryRecordingServerHardwareDrivers(string recordingServerId, bool used)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().QueryRecordingServerHardwareDrivers(recordingServerId, used);
	}

	public override VMOWebserviceObject[] QueryFailoverServerHardwareDrivers(string failoverServerId)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().QueryFailoverServerHardwareDrivers(failoverServerId);
	}

	public override VMOWebserviceObject[] QueryActiveHardwareDrivers(string recordingServerId)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().QueryActiveRecordingServerHardwareDrivers(recordingServerId);
	}

	public override VMOWebserviceObject QueryActiveHardwareDriver(string recordingServerId, string driverId)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().QueryActiveHardwareDriver(recordingServerId, driverId);
	}

	public override void DeleteServerTasksByDeviceBeforeTime(string deviceId, DateTime time)
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().DeleteServerTasksByDeviceIdBeforeTime(deviceId, time);
	}

	public override void CleanupTask(string taskId)
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().CleanupTask(taskId);
	}

	public override void StopEdgeStorageRetrieval(string taskId)
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().StopEdgeStorageRetrieval(taskId);
	}

	public override void ClientProfileMoveUpPriority(string profileId)
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().ClientProfileMovePriorityUp(profileId);
	}

	public override void ClientProfileMoveDownPriority(string profileId)
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().ClientProfileMovePriorityDown(profileId);
	}

	public override string QueryActiveManagementClientProfileId()
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().QueryActiveManagementClientProfileId();
	}

	public override string QueryActiveSmartClientProfileId()
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().QueryActiveSmartClientProfileId();
	}

	public override string QueryActiveSmartClientProfileId(string sid)
	{
		return _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().QueryActiveSmartClientProfileId(sid);
	}

	public override void AttachClientProfileToRole(string profileId, string roleId)
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().AttachClientProfileToRole(profileId, roleId);
	}

	public override void RefreshLicenseCache()
	{
		_vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().RefreshLicenseCache();
	}

	public override Identity ReadIdentity(SearchFactor searchFactor, string factoryValue, QueryMembership queryMembership)
	{
		return _vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().ReadIdentity(searchFactor, factoryValue, queryMembership);
	}

	public override Identity[] ReadIdentities(SearchFactor searchFactor, string[] factoryValues, QueryMembership queryMembership)
	{
		return _vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().ReadIdentities(searchFactor, factoryValues, queryMembership);
	}

	public override Identity ReadIdentityFromSource(SearchFactor searchFactor, string factorValue)
	{
		return _vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().ReadIdentityFromSource(searchFactor, factorValue);
	}

	public override void RemoveIdentity(string userSid)
	{
		_vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().RemoveIdentity(userSid);
	}

	public override Identity[] QueryRoleMembers(string roleId)
	{
		return _vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().QueryRoleMembers(roleId);
	}

	public override VMOWebserviceObject CopyRole(string sourceRoleId, string name, string description)
	{
		return _vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().CopyRole(sourceRoleId, name, description);
	}

	public override Identity AddMemberToRole(string roleId, string memberSid)
	{
		return _vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().AddMemberToRole(roleId, memberSid);
	}

	public override void RemoveMemberFromRole(string roleId, string memberSid)
	{
		_vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().RemoveMemberFromRole(roleId, memberSid);
	}

	public override SecurityAction[] QueryObjectClassActions(string classId)
	{
		return _vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().QueryObjectClassActions(classId);
	}

	public override void RegisterObject(string objectId, string classId)
	{
		_vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().RegisterObject(objectId, classId);
	}

	public override void UnregisterObject(string objectId)
	{
		_vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().UnregisterObject(objectId);
	}

	public override void CheckPermission(string objectId, string actionId)
	{
		_vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().CheckPermission(objectId, actionId);
	}

	public override AccessControlEntry[] QueryAccessControlList(string objectId)
	{
		return _vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().QueryAccessControlList(objectId);
	}

	public override AccessControlEntry[] QueryAccessControlList(string identity, string objectId)
	{
		return _vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().QueryAccessControlList(identity, objectId);
	}

	public override void AddAccessControlEntries(string objectId, AccessControlEntry[] entries)
	{
		_vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().AddAccessControlEntries(objectId, entries);
	}

	public override void RemoveAccessControlEntries(string objectId, AccessControlEntry[] entries)
	{
		_vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().RemoveAccessControlEntries(objectId, entries);
	}

	public override void ReplaceAccessControlEntries(string objectId, AccessControlEntry[] entries)
	{
		_vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().ReplaceAccessControlEntries(objectId, entries);
	}

	public override void SetInheritance(string objectId, string parentObjectId)
	{
		_vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().SetInheritance(objectId, parentObjectId);
	}

	public override bool[] IsPermittedByIdentityList(string[] identityList, string actionId, string objectId)
	{
		return _vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().IsPermittedByIdentityList(identityList, actionId, objectId);
	}

	public override bool[] IsPermittedByActionList(string identity, string[] actionIdList, string objectId)
	{
		return _vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().IsPermittedByActionList(identity, actionIdList, objectId);
	}

	public override bool[] IsPermittedByObjectList(string identity, string actionId, string[] objectIdList)
	{
		return _vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().IsPermittedByObjectList(identity, actionId, objectIdList);
	}

	public override bool IsPermittedTimeBasedAction(string actionId, string objectId)
	{
		return _vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().IsPermittedTimeBasedAction(actionId, objectId);
	}

	public override AccessControlTimeProfileEntry QueryTimeBasedAccessEntry(string identity, string actionId, string objectId)
	{
		return _vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().QueryTimeBasedAccessEntry(identity, actionId, objectId);
	}

	public override AccessControlTimeProfileEntry CreateUpdateTimeBasedAccessEntry(string identity, string actionId, string objectId, string timeProfileId, bool isDefaultTimeProfile)
	{
		return _vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().CreateUpdateTimeBasedAccessEntry(identity, actionId, objectId, timeProfileId, isDefaultTimeProfile);
	}

	public override void DeleteTimeBasedAccessEntry(string identity, string actionId, string objectId)
	{
		_vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().DeleteTimeBasedAccessEntry(identity, actionId, objectId);
	}

	public override bool[] HasPermissionByIdentityList(string namespaceId, string[] identityList, string actionId, string objectId)
	{
		return _vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().HasPermissionByIdentityList(namespaceId, identityList, actionId, objectId);
	}

	public override bool[] HasPermissionByActionList(string namespaceId, string identity, string[] actionList, string objectId)
	{
		return _vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().HasPermissionByActionList(namespaceId, identity, actionList, objectId);
	}

	public override bool[] HasPermissionByActionListCurrentUser(string namespaceId, string[] actionList, string objectId)
	{
		return _vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().HasPermissionByActionListCurrentUser(namespaceId, actionList, objectId);
	}

	public override bool[] HasPermissionByObjectList(string namespaceId, string identity, string actionId, string[] objectList)
	{
		return _vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().HasPermissionByObjectList(namespaceId, identity, actionId, objectList);
	}

	public override bool IsAdministrator()
	{
		Guid administratorRoleId = new Guid("1f30d14b-ba91-4e8a-8838-eda17931f8fc");
		string logOnName = _vmoClient.ServerConnection.LogOnName;
		Identity identity = ReadIdentity(SearchFactor.AccountName, logOnName, QueryMembership.Expanded);
		if (identity != null && identity.MemberOfRoles != null)
		{
			return identity.MemberOfRoles.Any((Guid f) => f == administratorRoleId);
		}
		return false;
	}

	public override Identity[] QueryRoleMembersExtended(string roleId, QueryMembership queryMembership)
	{
		return _vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().QueryRoleMembersExtended(roleId, queryMembership, includeMappedExternalUsers: false);
	}

	protected bool IsManagementServerId(string id)
	{
		if (string.Equals(id, HomeSiteId, StringComparison.OrdinalIgnoreCase) || string.Equals(id, "909749AE-88C4-4780-A381-383D691E98FA", StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		return false;
	}

	protected bool IsTopServerGroup(string parentGroupId)
	{
		if (string.Equals(parentGroupId, "430C2FE2-B233-42DA-B508-1A22EF03383D", StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		if (string.Equals(parentGroupId, "909749AE-88C4-4780-A381-383D691E98FA", StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		if (string.Equals(parentGroupId, "4BF8A8B0-54AB-4E82-994F-98D8E9584176", StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		if (string.Equals(parentGroupId, PrivateUserViewGroupId, StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		if (string.Equals(parentGroupId, "6A85FFC3-C134-4338-A072-024817CB5820", StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		return false;
	}

	protected string ConvertManagementServerIdToTopServerId(string entityName, string parentGroupId)
	{
		if (IsManagementServerId(parentGroupId))
		{
			if (string.Equals(entityName, "PublicViewGroup", StringComparison.OrdinalIgnoreCase))
			{
				parentGroupId = "430C2FE2-B233-42DA-B508-1A22EF03383D";
			}
			else if (string.Equals(entityName, "PrivateViewGroup", StringComparison.OrdinalIgnoreCase))
			{
				parentGroupId = PrivateUserViewGroupId;
			}
			else if (string.Equals(entityName, "ViewLayoutGroup", StringComparison.OrdinalIgnoreCase))
			{
				parentGroupId = "430C2FE2-B233-42DA-B508-1A22EF03383D";
			}
			else if (string.Equals(entityName, "FailoverServerGroup", StringComparison.OrdinalIgnoreCase))
			{
				parentGroupId = "6A85FFC3-C134-4338-A072-024817CB5820";
			}
			else if (string.Equals(entityName, "FailoverServer", StringComparison.OrdinalIgnoreCase))
			{
				parentGroupId = "6A85FFC3-C134-4338-A072-024817CB5820";
			}
		}
		return parentGroupId;
	}

	protected string CreatePrivateUserViewGroupIfNeeded()
	{
		string userName = GetUserName();
		ServerGroupInfo[] array = _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().QueryPrivateServerGroups();
		if (array != null)
		{
			ServerGroupInfo serverGroupInfo = array.FirstOrDefault((ServerGroupInfo i) => string.Compare(i.Name, userName, StringComparison.OrdinalIgnoreCase) == 0);
			if (serverGroupInfo != null)
			{
				return serverGroupInfo.Id;
			}
		}
		ServerGroupInfo serverGroupInfo2;
		try
		{
			serverGroupInfo2 = _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().CreatePrivateUserServerGroup(userName);
		}
		catch
		{
			serverGroupInfo2 = null;
		}
		return serverGroupInfo2?.Id;
	}

	protected string GetPrivateUserViewGroupId()
	{
		string userName = GetUserName();
		ServerGroupInfo[] array = _vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().QueryPrivateServerGroups();
		foreach (ServerGroupInfo serverGroupInfo in array)
		{
			if (string.Equals(serverGroupInfo.Name, userName, StringComparison.OrdinalIgnoreCase))
			{
				return serverGroupInfo.Id;
			}
		}
		return null;
	}

	protected string GetHomeSiteId()
	{
		return _vmoClient.ManagementServer.Id.ToString();
	}
}
