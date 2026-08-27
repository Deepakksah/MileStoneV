using System;
using System.Collections.Generic;
using System.Security.Principal;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient.Proxy;

namespace VideoOS.Management.VmoClient;

public abstract class ServerCommunicationBase : IServer, IServerObjectHandling, IServerFunctions, ISecurity
{
	protected List<VMOWebserviceObject> _objectCache = new List<VMOWebserviceObject>();

	protected readonly IVmoClient _vmoClient;

	protected ServerCommunicationBase(IVmoClient vmoClient)
	{
		_vmoClient = vmoClient;
	}

	protected string GetUserName()
	{
		string result = null;
		if (_vmoClient.ServerConnection.ConnectAsUser)
		{
			result = _vmoClient.ServerConnection.ConnectAsUserName;
		}
		else
		{
			using WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
			result = windowsIdentity.Name;
		}
		return result;
	}

	public virtual VMOWebserviceObject CreateObject(Type type, VmoPropertyCollection properties, CustomPropertyCollection customProperties)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject UpdateObject(Type type, string id, VmoPropertyCollection properties, CustomPropertyCollection customProperties)
	{
		throw new NotSupportedException();
	}

	public virtual void DeleteObject(Type type, string id)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject GetObjectById(Type type, string id)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject[] GetObjectsByIds(Type type, string[] ids)
	{
		throw new NotSupportedException();
	}

	public virtual string[] GetObjectRelations(Type parentType, string parentId, Type type)
	{
		throw new NotSupportedException();
	}

	public virtual void AddGroupMembership(string parentServerGroupId, Type type, string id)
	{
		throw new NotSupportedException();
	}

	public virtual void RemoveGroupMembership(string parentServerGroupId, Type type, string id)
	{
		throw new NotSupportedException();
	}

	public virtual CustomProperty[] GetCustomProperties(string objectId)
	{
		throw new NotSupportedException();
	}

	public virtual CustomProperty GetCustomProperty(string objectId, string key)
	{
		throw new NotSupportedException();
	}

	public virtual void DeleteCustomProperty(string objectId, string key)
	{
		throw new NotSupportedException();
	}

	public virtual void SaveCustomProperties(string objectId, CustomProperty[] customProperties)
	{
		throw new NotSupportedException();
	}

	public virtual void ActivateVideoWallPreset(string videoWallPresetId)
	{
		throw new NotSupportedException();
	}

	public virtual void BasicUserChangePassword(string basicUserId, string newPassword)
	{
		throw new NotSupportedException();
	}

	public virtual void TriggerUserDefinedEvent(string userDefinedEventId)
	{
		throw new NotSupportedException();
	}

	public virtual void TriggerPluginEvent(Guid pluginEventId, Guid deviceId, Guid[] metadataDeviceIds, Guid[] metadataDeviceGroupIds, string customData)
	{
		throw new NotSupportedException();
	}

	public virtual void StopEdgeStorageRetrieval(string taskId)
	{
		throw new NotSupportedException();
	}

	public virtual void CleanupTask(string taskId)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject AuthorizeMasterSite()
	{
		throw new NotSupportedException();
	}

	public virtual void MarkChildSiteInconsistent(string childSiteId)
	{
		throw new NotSupportedException();
	}

	public virtual void SynchronizeSite(string siteId)
	{
		throw new NotSupportedException();
	}

	public virtual void TriggerFullSiteSynchronization()
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject AttachChildSite(string childSiteId, AddressCollection addresses, string childSiteVersion)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject AttachMasterSite(string masterSiteId, AddressCollection addresses, string masterSiteVersion)
	{
		throw new NotSupportedException();
	}

	public virtual void DetachSite(string siteId)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject ApplyLayout(string monitorId, string layoutId)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject ApplyCamerasToMonitorView(string monitorId, string[] cameras, string[] cameraGroups, int fromPosition)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject ApplyCamerasAndLayoutToMonitorView(string monitorId, string[] cameras, string[] cameraGroups, int fromPosition, string layoutId)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject ApplyViewItemXmlsToMonitorView(string monitorId, string[] viewItemXmls, int fromPosition)
	{
		throw new NotSupportedException();
	}

	public virtual void ApplyMonitorStateToMonitor(string monitorId, string xmlState)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject ApplyTextAndLayoutToMonitorView(string monitorId, string messageToDisplay, int fromPosition, string layoutId)
	{
		throw new NotSupportedException();
	}

	public virtual void RemoveCamerasFromMonitorView(string monitorId, string[] cameras, string[] cameraGroups)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject CreateView(string viewGroupId, VmoPropertyCollection properties, CustomPropertyCollection customProperties, VmoPropertyCollection[] viewItems, CustomPropertyCollection[] viewItemsCustomProperties)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject ClearViewItem(string viewItemId)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject ClearView(string viewId)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject ApplyViewToView(string sourceViewId, string viewId)
	{
		throw new NotSupportedException();
	}

	public virtual void ChangeViewItemId(string oldViewItemId, string newViewItemId, string viewId)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject GetRecordingServerInformation(string recordingServerId)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject GetRecordingStorageInformation(string recordingStorageId)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject GetArchiveStorageInformation(string archiveStorageId)
	{
		throw new NotSupportedException();
	}

	public virtual bool ValidateEncryptionPasswordRecordingStorage(string recordingStorageId, string password)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject[] EnableRecordingStorageEncryption(string recordingStorageId, StorageEncryptionMethod method, string password)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject[] DisableRecordingStorageEncryption(string recordingStorageId, string password)
	{
		throw new NotSupportedException();
	}

	public virtual void SetRecordingStorageAsDefault(string recordingStorageId)
	{
		throw new NotSupportedException();
	}

	public virtual void EnableFramerateReductionArchiveStorage(string archiveStorageId, bool enabled)
	{
		throw new NotSupportedException();
	}

	public virtual MarkedDataReference QueryMarkedDataReference(string[] deviceIds, bool realTime, MarkedDataDefinitions.MarkedDataTypes markedDataTypes)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject[] QuerySystemMonitorStateSourcesByIds(string[] ids)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject[] QuerySystemMonitorStateSourcesByTypes(string[] types)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject[] QueryAllRuleSources()
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject QueryMarkedData(string markedDataId)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject[] QueryBookmarksSearch(DateTime centerTime, DateTime timeLimit, int countLimit, DeviceType[] deviceTypes, string[] optDeviceIds, string[] optUsers, string optSearchStr, string optFromBookmarkId)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject[] QueryBookmarksSearch2(DateTime centerTime, DateTime timeLimit, int countLimit, DeviceType[] deviceTypes, string[] optDeviceIds, string[] optUsers, string optSearchStr, bool optionalSearchHeader, bool optionalSearchDescription, bool optionalSearchReference, bool optionalSearchUser, string optionalHeaderFilter, string optionalDescriptionFilter, string optFromBookmarkId)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject[] QueryMarkedDataSearch(string[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, string sortOrder, bool sortAscending)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject[] QueryMarkedDataSearch2(string[] deviceIds, string searchText, bool optionalSearchHeader, bool optionalSearchDescription, bool optionalSearchReference, bool optionalSearchUser, string optionalHeaderFilter, string optionalDescriptionFilter, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, string sortOrder, bool sortAscending)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject CreateMarkedData(VmoPropertyCollection properties)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject UpdateMarkedData(VMOWebserviceObject markedDataVMOWebserviceObject)
	{
		throw new NotSupportedException();
	}

	public virtual void DeleteMarkedData(string markedDataId, MarkedDataDefinitions.MarkedDataTypes markedDataType)
	{
		throw new NotSupportedException();
	}

	public virtual void DeleteBookmarksByDeviceIdBeforeTime(MediaStatusEntry mediaStatus)
	{
		throw new NotSupportedException();
	}

	public virtual Identity ReadIdentity(SearchFactor searchFactor, string factoryValue, QueryMembership queryMembership)
	{
		throw new NotSupportedException();
	}

	public virtual Identity[] ReadIdentities(SearchFactor searchFactor, string[] factoryValues, QueryMembership queryMembership)
	{
		throw new NotSupportedException();
	}

	public virtual Identity ReadIdentityFromSource(SearchFactor searchFactor, string factorValue)
	{
		throw new NotSupportedException();
	}

	public virtual void RemoveIdentity(string memberSid)
	{
		throw new NotSupportedException();
	}

	public virtual Identity[] QueryRoleMembers(string roleId)
	{
		throw new NotSupportedException();
	}

	public virtual Identity[] QueryRoleMembersExtended(string roleId, QueryMembership queryMembership)
	{
		throw new NotSupportedException();
	}

	public virtual Identity[] QueryRoleMembersExtended(string roleId, QueryMembership queryMembership, bool includeMappedExternalUsers)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject CopyRole(string sourceRoleId, string name, string description)
	{
		throw new NotSupportedException();
	}

	public virtual Identity AddMemberToRole(string roleId, string memberSid)
	{
		throw new NotSupportedException();
	}

	public virtual void RemoveMemberFromRole(string roleId, string memberSid)
	{
		throw new NotSupportedException();
	}

	public virtual SecurityAction[] QueryObjectClassActions(string classId)
	{
		throw new NotSupportedException();
	}

	public virtual void RegisterObject(string objectId, string classId)
	{
		throw new NotSupportedException();
	}

	public virtual void UnregisterObject(string objectId)
	{
		throw new NotSupportedException();
	}

	public virtual void CheckPermission(string objectId, string actionId)
	{
		throw new NotSupportedException();
	}

	public virtual AccessControlEntry[] QueryAccessControlList(string objectId)
	{
		throw new NotSupportedException();
	}

	public virtual AccessControlEntry[] QueryAccessControlList(string identity, string objectId)
	{
		throw new NotSupportedException();
	}

	public virtual void AddAccessControlEntries(string objectId, AccessControlEntry[] entries)
	{
		throw new NotSupportedException();
	}

	public virtual void RemoveAccessControlEntries(string objectId, AccessControlEntry[] entries)
	{
		throw new NotSupportedException();
	}

	public virtual void ReplaceAccessControlEntries(string objectId, AccessControlEntry[] entries)
	{
		throw new NotSupportedException();
	}

	public virtual void SetInheritance(string objectId, string parentObjectId)
	{
		throw new NotSupportedException();
	}

	public virtual bool[] IsPermittedByIdentityList(string[] identityList, string actionId, string objectId)
	{
		throw new NotSupportedException();
	}

	public virtual bool[] IsPermittedByActionList(string identity, string[] actionIdList, string objectId)
	{
		throw new NotSupportedException();
	}

	public virtual bool[] IsPermittedByObjectList(string identity, string actionId, string[] objectIdList)
	{
		throw new NotSupportedException();
	}

	public virtual AccessControlTimeProfileEntry QueryTimeBasedAccessEntry(string identity, string actionId, string objectId)
	{
		throw new NotSupportedException();
	}

	public virtual AccessControlTimeProfileEntry CreateUpdateTimeBasedAccessEntry(string identity, string actionId, string objectId, string timeProfileId, bool isDefaultTimeProfile)
	{
		throw new NotSupportedException();
	}

	public virtual void DeleteTimeBasedAccessEntry(string identity, string actionId, string objectId)
	{
		throw new NotSupportedException();
	}

	public virtual void EnableFailoverServer(string failoverServerId)
	{
		throw new NotSupportedException();
	}

	public virtual void DisableFailoverServer(string failoverServerId)
	{
		throw new NotSupportedException();
	}

	public virtual void StopFailoverService(string failoverServerId)
	{
		throw new NotSupportedException();
	}

	public virtual void MoveFailoverServer(string failoverServerId, string newParentGroupId, int insertAtPosition)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject[] QueryRecordingServerHardwareDrivers(string recordingServerId, bool used)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject[] QueryFailoverServerHardwareDrivers(string failoverServerId)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject[] QueryActiveHardwareDrivers(string recordingServerId)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject QueryActiveHardwareDriver(string recordingServerId, string driverId)
	{
		throw new NotSupportedException();
	}

	public virtual bool IsFeatureLicensed(string featureName)
	{
		throw new NotSupportedException();
	}

	public virtual string GetFeatureParameterValue(string featureName, string parameterName)
	{
		throw new NotSupportedException();
	}

	public virtual void UpdateLicense(byte[] license, string ownerXml, string sitesInformation)
	{
		throw new NotSupportedException();
	}

	public virtual void UpdateMipPluginsLicenseRequestPart(string license)
	{
		throw new NotSupportedException();
	}

	public virtual void QueueAutomaticActivation()
	{
		throw new NotSupportedException();
	}

	public virtual void RefreshLicenseCache()
	{
		throw new NotSupportedException();
	}

	public virtual VMOExceptionDetail GetLastAutomaticActivationError()
	{
		throw new NotSupportedException();
	}

	public virtual void SetSystemSetting(string name, string password)
	{
		throw new NotSupportedException();
	}

	public virtual string GetSystemSetting(string name)
	{
		throw new NotSupportedException();
	}

	public virtual void DeleteServerTasksByDeviceBeforeTime(string deviceId, DateTime time)
	{
		throw new NotSupportedException();
	}

	public virtual bool[] HasPermissionByIdentityList(string namespaceId, string[] identityList, string actionId, string objectId)
	{
		throw new NotSupportedException();
	}

	public virtual bool[] HasPermissionByActionList(string namespaceId, string identity, string[] actionList, string objectId)
	{
		throw new NotSupportedException();
	}

	public virtual bool[] HasPermissionByActionListCurrentUser(string namespaceId, string[] actionList, string objectId)
	{
		throw new NotSupportedException();
	}

	public virtual bool[] HasPermissionByObjectList(string namespaceId, string identity, string actionId, string[] objectList)
	{
		throw new NotSupportedException();
	}

	public virtual bool IsAdministrator()
	{
		throw new NotSupportedException();
	}

	public virtual string GetMIPObjectGroupKind(string mipObjectGroupId)
	{
		throw new NotSupportedException();
	}

	public virtual string GetMIPObjectGroupSubKind(string mipObjectGroupId)
	{
		throw new NotSupportedException();
	}

	public virtual void ClientProfileMoveUpPriority(string profileId)
	{
		throw new NotSupportedException();
	}

	public virtual void ClientProfileMoveDownPriority(string profileId)
	{
		throw new NotSupportedException();
	}

	public virtual string QueryActiveManagementClientProfileId()
	{
		throw new NotSupportedException();
	}

	public virtual string QueryActiveSmartClientProfileId()
	{
		throw new NotSupportedException();
	}

	public virtual string QueryActiveSmartClientProfileId(string sid)
	{
		throw new NotSupportedException();
	}

	public virtual void AttachClientProfileToRole(string profileId, string roleId)
	{
		throw new NotSupportedException();
	}

	public virtual int GetServerStatus()
	{
		throw new NotSupportedException();
	}

	public virtual string GetCreatePrivateViewRoot()
	{
		throw new NotSupportedException();
	}

	public virtual bool IsPermittedTimeBasedAction(string actionId, string objectId)
	{
		throw new NotSupportedException();
	}

	public virtual VMOWebserviceObject[] Execute(string functionName, VmoPropertyCollection parameters)
	{
		VMOWebserviceObject vMOWebserviceObject = new VMOWebserviceObject(parameters, (VmoRelationCollection)null, (string)null, (string)null);
		return Execute(functionName, new VMOWebserviceObject[1] { vMOWebserviceObject });
	}

	public virtual VMOWebserviceObject[] Execute(string functionName, VMOWebserviceObject[] parameters)
	{
		throw new NotSupportedException();
	}
}
