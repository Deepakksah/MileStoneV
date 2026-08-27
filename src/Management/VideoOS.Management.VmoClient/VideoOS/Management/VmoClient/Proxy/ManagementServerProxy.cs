using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using VideoOS.Management.Client.Proxy;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient.Proxy;

public sealed class ManagementServerProxy : ProxyClientServiceBase<ManagementServerServiceClient>
{
	private const string MANAGEMENT_SERVER_NAMESPACE = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01";

	private static Dictionary<string, string> _taskDbTypeToEntityName = new Dictionary<string, string>
	{
		{ "Database.Archiving", "TaskArchiving" },
		{ "Database.Repairing2", "TaskRepairing" },
		{ "Database.DeleteArchive", "TaskDeleteArchive" },
		{ "Database.DeleteStorage", "TaskDeleteStorage" },
		{ "Database.MoveArchive", "TaskMoveArchiveStorage" },
		{ "Database.MoveDeviceDb", "TaskMoveDevice" },
		{ "Database.MoveStorage", "TaskMoveRecordingStorage" },
		{ "Database.SetEncryptionMethod", "TaskSetEncryptionMethod" },
		{ "Database.SetEncryptionPassword", "TaskSetEncryptionPassword" },
		{ "Database.TruncateDeviceStorage", "TaskTruncateDeviceStorage" },
		{ "Database.MountArchive", "TaskMountArchiveStorage" },
		{ "Database.UnmountArchive", "TaskUnmountArchiveStorage" },
		{ "Edge.StorageRetrieval", "TaskRequestEdgeStorage" },
		{ "System.ActivateLicense", "TaskActivateLicense" },
		{ "System.AddHardware", "TaskAddHardware" },
		{ "System.ChangeHardwarePassword", "TaskChangeHardwarePassword" },
		{ "System.FirmwareUpgrade", "TaskFirmwareUpgrade" },
		{ "System.HardwareScan", "TaskHardwareScan" },
		{ "System.MoveHardware", "TaskMoveHardware" }
	};

	public override string ServiceName => ResolveServiceName("ManagementServerService");

	public override Uri ServiceAddress => new Uri("urn:videoos:management:server:managementserverservice");

	public override ICommunicationObject CommunicationObject
	{
		get
		{
			if (base.ServiceClient != null)
			{
				return base.ServiceClient.ChannelFactory;
			}
			return null;
		}
	}

	public ManagementServerProxy(ServerConnection serverConnection)
		: base((ConnectionSettings)serverConnection)
	{
	}

	public override void Initialize()
	{
		if (base.ServiceClient != null)
		{
			try
			{
				base.ServiceClient.Close();
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
				base.ServiceClient.Abort();
			}
		}
		base.ServiceClient = new ManagementServerServiceClient(GetBinding(), GetEndpointAddress());
		ConfigureEndpoint(base.ServiceClient.Endpoint);
		ConfigureCredentials(base.ServiceClient.ClientCredentials);
		AddMessageHeader("http://schemas.videoos/ManagementServerService/2008/06/Vmo/01", "VMO_Client_Version", 1);
		base.IsInitialized = true;
	}

	public VMOWebserviceObject QueryObject2(string entityName, string id, string[] properties)
	{
		GenericObjectInfo2 genericObjectInfo;
		try
		{
			genericObjectInfo = CallWithRetry(() => base.ServiceClient.QueryObject2(entityName, id, properties));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObject(genericObjectInfo);
	}

	public VMOWebserviceObject CreateObject2(VMOWebserviceObject clientObj, CustomPropertyCollection customProperties)
	{
		GenericObjectInfo2 genericObjectInfo;
		try
		{
			GenericObjectInfo2 info = ConvertToGenericObjectInfo2(clientObj);
			genericObjectInfo = CallWithRetry(delegate
			{
				string entityName = EntityNameAttributeHandlerWrapper.GetEntityName(clientObj.Type);
				ObjectPropertyInfo2[] properties = info.Properties;
				CustomPropertyInfo[] customProperties2 = ConvertToCustomPropertyInfos(clientObj.Id, customProperties.DirtyCustomPropertyArray());
				return base.ServiceClient.CreateObject2(entityName, properties, customProperties2);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObject(genericObjectInfo);
	}

	public VMOWebserviceObject UpdateObject2(VMOWebserviceObject clientObj, CustomProperty[] customProperties)
	{
		GenericObjectInfo2 genericObject = ConvertToGenericObjectInfo2(clientObj);
		try
		{
			genericObject = CallWithRetry(delegate
			{
				CustomPropertyInfo[] customProperties2 = ConvertToCustomPropertyInfos(clientObj.Id, customProperties);
				return base.ServiceClient.UpdateObject2(genericObject, customProperties2);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObject(genericObject);
	}

	public void DeleteObject2(string entityName, string id)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.DeleteObject2(entityName, id);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public string[] QueryRelationIds2(string entityName, string parentEntityName, string parentId, string[] properties)
	{
		try
		{
			return CallWithRetry(() => base.ServiceClient.QueryObjectCollectionIds(entityName, parentEntityName, parentId, properties));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public VMOWebserviceObject[] QueryObjects2(string entityName, string[] ids, string[] properties)
	{
		GenericObjectInfo2[] genericObjectInfo;
		try
		{
			genericObjectInfo = CallWithRetry(() => base.ServiceClient.QueryObjects(entityName, ids, properties));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObjects(genericObjectInfo);
	}

	public ServerGroupInfo[] QueryPrivateServerGroups()
	{
		try
		{
			return CallWithRetry(() => base.ServiceClient.QueryServerGroups("VIEW_GROUP", "4BF8A8B0-54AB-4E82-994F-98D8E9584176"));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public ServerGroupInfo CreatePrivateUserServerGroup(string name)
	{
		ServerGroupInfo serverGroupInfo = null;
		try
		{
			ServerGroupOptions serverGroupOptions = new ServerGroupOptions();
			serverGroupOptions.UserDefinedId = null;
			serverGroupOptions.CustomProperties = new CustomPropertyInfo[0];
			return CallWithRetry(() => base.ServiceClient.CreateServerGroup(name, string.Empty, "VIEW_GROUP", "4BF8A8B0-54AB-4E82-994F-98D8E9584176", serverGroupOptions));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public CustomProperty[] GetCustomProperties(string objectId)
	{
		CustomPropertyInfo[] customPropertyInfo = new CustomPropertyInfo[0];
		try
		{
			customPropertyInfo = CallWithRetry(() => base.ServiceClient.GetCustomProperties(objectId));
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
		}
		return ConvertToCustomProperties(customPropertyInfo);
	}

	public CustomProperty GetCustomProperty(string objectId, string key)
	{
		CustomPropertyInfo customPropertyInfo;
		try
		{
			customPropertyInfo = CallWithRetry(() => base.ServiceClient.GetCustomProperty(objectId, key));
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			try
			{
				ThrowConvertedExceptionIfNeeded(ex2);
			}
			catch (CustomPropertyNotFoundException)
			{
				return null;
			}
			throw;
		}
		return ConvertToCustomProperty(customPropertyInfo);
	}

	public void UpdateCustomProperty(string objectId, CustomProperty[] customPropertyArray)
	{
		try
		{
			CustomPropertyInfo[] customProperty = ConvertToCustomPropertyInfos(objectId, customPropertyArray);
			CallWithRetry(delegate
			{
				base.ServiceClient.UpdateCustomProperties(objectId, customProperty);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void DeleteCustomProperty(string objectId, string key)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.DeleteCustomProperty(objectId, key);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void ChangePasswordBasicUser(string id, string newPassword)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.ChangePasswordBasicUser(id, newPassword);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void DeleteServerTasksByDeviceIdBeforeTime(string deviceId, DateTime time)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.DeleteServerTasksByDeviceIdBeforeTime(deviceId, time);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public VMOWebserviceObject ApplyLayoutGeneric(string monitorId, string layoutId)
	{
		GenericObjectInfo2 genericObjectInfo = null;
		try
		{
			genericObjectInfo = CallWithRetry(() => base.ServiceClient.ApplyLayoutGeneric(monitorId, layoutId));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObject(genericObjectInfo);
	}

	public VMOWebserviceObject ApplyCamerasToMonitorViewGeneric(string monitorId, string[] cameras, string[] cameraGroups, int fromPosition)
	{
		GenericObjectInfo2 genericObjectInfo = null;
		try
		{
			genericObjectInfo = CallWithRetry(() => base.ServiceClient.ApplyCamerasToMonitorViewGeneric(monitorId, cameras, cameraGroups, fromPosition));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObject(genericObjectInfo);
	}

	public VMOWebserviceObject ApplyCamerasAndLayoutToMonitorViewGeneric(string monitorId, string[] cameras, string[] cameraGroups, int fromPosition, string layoutId)
	{
		GenericObjectInfo2 genericObjectInfo = null;
		try
		{
			genericObjectInfo = CallWithRetry(() => base.ServiceClient.ApplyCamerasAndLayoutToMonitorViewGeneric(monitorId, cameras, cameraGroups, fromPosition, layoutId));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObject(genericObjectInfo);
	}

	public VMOWebserviceObject ApplyViewItemXmlsToMonitorViewGeneric(string monitorId, string[] viewItemXmls, int fromPosition)
	{
		GenericObjectInfo2 genericObjectInfo = null;
		try
		{
			genericObjectInfo = CallWithRetry(() => base.ServiceClient.ApplyViewItemXmlsToMonitorViewGeneric(monitorId, viewItemXmls, fromPosition));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObject(genericObjectInfo);
	}

	public VMOWebserviceObject ApplyTextAndLayoutToMonitorView(string monitorId, string messageToDisplay, int fromPosition, string layoutId)
	{
		GenericObjectInfo2 genericObjectInfo = null;
		try
		{
			genericObjectInfo = CallWithRetry(() => base.ServiceClient.ApplyTextAndLayoutToMonitorViewGeneric(monitorId, messageToDisplay, fromPosition, layoutId));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObject(genericObjectInfo);
	}

	public void ActivateVideoWallPreset(string videoWallPresetId)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.ActivateVideoWallPreset(videoWallPresetId);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public VMOWebserviceObject AttachSlaveSite2(string slaveSiteId, string addressesXml, string slaveSiteVersion)
	{
		GenericObjectInfo2 genericObjectInfo = null;
		try
		{
			genericObjectInfo = CallWithRetry(() => base.ServiceClient.AttachSlaveSite2(slaveSiteId, addressesXml, slaveSiteVersion));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObject(genericObjectInfo);
	}

	public VMOWebserviceObject AttachMasterSite2(string masterSiteId, string addressesXml, string masterSiteVersion)
	{
		GenericObjectInfo2 genericObjectInfo = null;
		try
		{
			genericObjectInfo = CallWithRetry(() => base.ServiceClient.AttachMasterSite2(masterSiteId, addressesXml, masterSiteVersion));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObject(genericObjectInfo);
	}

	public VMOWebserviceObject AuthorizeMaster2()
	{
		GenericObjectInfo2 genericObjectInfo = null;
		try
		{
			genericObjectInfo = CallWithRetry(() => base.ServiceClient.AuthorizeMaster2());
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObject(genericObjectInfo);
	}

	public VMOWebserviceObject CreateView(string viewGroupId, VmoPropertyCollection viewProperties, CustomPropertyCollection customProperties, VmoPropertyCollection[] viewItemProperties, CustomPropertyCollection[] viewItemsCustomProperties)
	{
		GenericObjectInfo2 genericObjectInfo = null;
		try
		{
			VMOWebserviceObject vMOWebserviceObject = new VMOWebserviceObject(viewProperties, null, "", "View");
			GenericObjectInfo2 info = ConvertToGenericObjectInfo2(vMOWebserviceObject);
			CustomPropertyInfo[] cp = ConvertToCustomPropertyInfos(vMOWebserviceObject.Id, customProperties.DirtyCustomPropertyArray());
			List<ObjectPropertyInfo2[]> viewItems = new List<ObjectPropertyInfo2[]>();
			List<CustomPropertyInfo[]> viewItemCustomProperties = new List<CustomPropertyInfo[]>();
			for (int i = 0; i < viewItemProperties.GetLength(0); i++)
			{
				GenericObjectInfo2 genericObjectInfo2 = ConvertToGenericObjectInfo2(new VMOWebserviceObject(viewItemProperties[i], null, "", "View"));
				viewItems.Add(genericObjectInfo2.Properties);
				viewItemCustomProperties.Add(ConvertToCustomPropertyInfos(vMOWebserviceObject.Id, viewItemsCustomProperties[i].DirtyCustomPropertyArray()));
			}
			genericObjectInfo = CallWithRetry(() => base.ServiceClient.CreateViewGeneric(viewGroupId, info.Properties, cp, viewItems.ToArray(), viewItemCustomProperties.ToArray()));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObject(genericObjectInfo);
	}

	public VMOWebserviceObject ClearView(string viewId)
	{
		GenericObjectInfo2 genericObjectInfo = null;
		try
		{
			genericObjectInfo = CallWithRetry(() => base.ServiceClient.ClearViewGeneric(viewId));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObject(genericObjectInfo);
	}

	public VMOWebserviceObject ApplyViewToView(string sourceViewId, string viewId)
	{
		GenericObjectInfo2 genericObjectInfo = null;
		try
		{
			genericObjectInfo = CallWithRetry(() => base.ServiceClient.ApplyViewToViewGeneric(sourceViewId, viewId));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObject(genericObjectInfo);
	}

	public VMOWebserviceObject ClearViewItem(string viewItemId)
	{
		GenericObjectInfo2 genericObjectInfo = null;
		try
		{
			genericObjectInfo = CallWithRetry(() => base.ServiceClient.ClearViewItemGeneric(viewItemId));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObject(genericObjectInfo);
	}

	public void ChangeViewItemId(string existingId, string newId)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.ChangeViewItemId(existingId, newId);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public VMOWebserviceObject GetRecordingServerInformation(string recordingServerId)
	{
		VMOWebserviceObject vMOWebserviceObject = null;
		try
		{
			vMOWebserviceObject = ConvertToVMOWebserviceObject(CallWithRetry(() => base.ServiceClient.GetRecordingServerInformation(recordingServerId)));
			string propertyValue = vMOWebserviceObject.Properties.GetPropertyValue<string>("DiskUsageXml");
			vMOWebserviceObject.Properties.Add("DiskUsage", RecordingServerDiskUsages.GetFromXml(propertyValue));
			return vMOWebserviceObject;
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public VMOWebserviceObject GetRecordingStorageInformation(string recordingStorageId)
	{
		GenericObjectInfo2 genericObjectInfo = null;
		try
		{
			genericObjectInfo = CallWithRetry(() => base.ServiceClient.GetRecordingStorageInformation(recordingStorageId));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObject(genericObjectInfo);
	}

	public VMOWebserviceObject GetArchiveStorageInformation(string archiveStorageId)
	{
		GenericObjectInfo2 genericObjectInfo = null;
		try
		{
			genericObjectInfo = CallWithRetry(() => base.ServiceClient.GetArchiveStorageInformation(archiveStorageId));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObject(genericObjectInfo);
	}

	public VMOWebserviceObject[] QuerySystemMonitorStateSourcesByIds(string[] ids)
	{
		GenericObjectInfo2[] array = null;
		try
		{
			array = CallWithRetry(() => base.ServiceClient.QuerySystemMonitorStateSourcesByIds(ids));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObjects(array);
	}

	public VMOWebserviceObject[] QuerySystemMonitorStateSourcesByTypes(string[] types)
	{
		GenericObjectInfo2[] array = null;
		try
		{
			array = CallWithRetry(() => base.ServiceClient.QuerySystemMonitorStateSourcesByTypes(types));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObjects(array);
	}

	public VMOWebserviceObject[] QueryAllRuleSources()
	{
		GenericObjectInfo2[] array = null;
		try
		{
			array = CallWithRetry(() => base.ServiceClient.QueryAllRuleSources());
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObjects(array);
	}

	public VMOWebserviceObject QueryMarkedData(string markedDataId)
	{
		GenericObjectInfo2 genericObjectInfo = null;
		try
		{
			genericObjectInfo = CallWithRetry(() => base.ServiceClient.QueryMarkedData(markedDataId));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObject(genericObjectInfo);
	}

	public VMOWebserviceObject[] QueryBookmarksSearch(DateTime centerTime, DateTime timeLimit, int countLimit, DeviceType[] deviceTypes, string[] optDeviceIds, string[] optUsers, string optSearchStr, string optFromBookmarkId)
	{
		GenericObjectInfo2[] array = null;
		try
		{
			string[] deviceTypeStrings = deviceTypes.Select((DeviceType f) => f.ToString()).ToArray();
			array = CallWithRetry(() => base.ServiceClient.QueryBookmarksSearch(centerTime, timeLimit, countLimit, deviceTypeStrings, optDeviceIds, optUsers, optSearchStr, optFromBookmarkId));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObjects(array);
	}

	public VMOWebserviceObject[] QueryBookmarksSearch2(DateTime centerTime, DateTime timeLimit, int countLimit, DeviceType[] deviceTypes, string[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, string optFromBookmarkId)
	{
		GenericObjectInfo2[] array = null;
		try
		{
			string[] deviceTypeStrings = deviceTypes.Select((DeviceType f) => f.ToString()).ToArray();
			array = CallWithRetry(() => base.ServiceClient.QueryBookmarksSearch2(centerTime, timeLimit, countLimit, deviceTypeStrings, optDeviceIds, optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter, optFromBookmarkId));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObjects(array);
	}

	public VMOWebserviceObject[] QueryMarkedDataSearch(string[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, string sortOrder, bool sortAscending)
	{
		GenericObjectInfo2[] array = null;
		try
		{
			array = CallWithRetry(() => base.ServiceClient.QueryMarkedDataSearch(deviceIds, searchText, users, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime, retentionExpireFrom, retentionExpireTo, pageIndex, pageSize, sortOrder, sortAscending));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObjects(array);
	}

	public VMOWebserviceObject[] QueryMarkedDataSearch2(string[] deviceIds, string searchText, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFromTime, DateTime retentionExpireToTime, int pageIndex, int pageSize, string sortOrder, bool sortAscending)
	{
		GenericObjectInfo2[] array = null;
		try
		{
			array = CallWithRetry(() => base.ServiceClient.QueryMarkedDataSearch2(deviceIds, searchText, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter, users, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime, retentionExpireFromTime, retentionExpireToTime, pageIndex, pageSize, sortOrder, sortAscending));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObjects(array);
	}

	public VMOWebserviceObject CreateMarkedData(VmoPropertyCollection properties)
	{
		GenericObjectInfo2 genericObjectInfo = null;
		try
		{
			ObjectPropertyInfo2[] genericProperties = ConvertToGenericObjectPropertyInfo2(properties);
			genericObjectInfo = CallWithRetry(() => base.ServiceClient.CreateMarkedData(genericProperties));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObject(genericObjectInfo);
	}

	public VMOWebserviceObject UpdateMarkedData(VMOWebserviceObject markedDataVMOWebserviceObject)
	{
		GenericObjectInfo2 genericMarkedData = null;
		try
		{
			genericMarkedData = ConvertToGenericObjectInfo2(markedDataVMOWebserviceObject);
			genericMarkedData = CallWithRetry(() => base.ServiceClient.UpdateMarkedData(genericMarkedData.Properties));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObject(genericMarkedData);
	}

	public bool ValidateEncryptionPassword(string recordingStorageId, string password)
	{
		bool flag = false;
		try
		{
			return CallWithRetry(() => base.ServiceClient.ValidateEncryptionPassword(recordingStorageId, password));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void SetRecordingStorageAsDefault(string recordingStorageId)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.SetRecordingStorageAsDefault(recordingStorageId);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void EnableFramerateReduction(string archiveStorageId, bool enable)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.EnableFramerateReduction(archiveStorageId, enable);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public MarkedDataReference QueryMarkedDataReference(IVmoRepositories repositories, string[] deviceIds, bool realTime, MarkedDataDefinitions.MarkedDataTypes markedDataTypes)
	{
		MarkedDataReferenceInfo2 markedDataReferenceInfo;
		try
		{
			markedDataReferenceInfo = CallWithRetry(() => base.ServiceClient.QueryMarkedDataReference(deviceIds, realTime, markedDataTypes));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return new MarkedDataReference(repositories, markedDataReferenceInfo.Reference, markedDataReferenceInfo.Time, markedDataReferenceInfo.Devices);
	}

	public bool IsFeatureLicensed(string featureName)
	{
		try
		{
			return CallWithRetry(() => base.ServiceClient.IsFeatureLicensed(featureName));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public string GetFeatureParameterValue(string featureName, string parameterName)
	{
		try
		{
			return CallWithRetry(() => base.ServiceClient.GetFeatureParameterValue(featureName, parameterName));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void UpdateLicense(byte[] license, string ownerXml, string sitesInformation)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.UpdateLicense(license, ownerXml, sitesInformation);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void QueueAutomaticActivation()
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.QueueAutomaticActivation();
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void RefreshLicenseCache()
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.RefreshLicenseCache();
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public VMOExceptionDetail GetLastAutomaticActivationError()
	{
		try
		{
			return CallWithRetry(() => base.ServiceClient.GetLastAutomaticActivationError());
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void UpdateMipPluginsLicenseRequestPart(string license)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.UpdateMipPluginsLicenseRequestPart(license);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void StopEdgeStorageRetrieval(string taskId)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.StopEdgeStorageRetrieval(taskId);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void CleanupTask(string taskId)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.CleanupTask(taskId);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void ApplyMonitorStateToMonitor(string monitorId, string xmlState)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.ApplyMonitorStateToMonitorGeneric(monitorId, xmlState);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void RemoveCamerasFromMonitorViewGeneric(string monitorId, string[] cameras, string[] cameraGroups)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.RemoveCamerasFromMonitorViewGeneric(monitorId, cameras, cameraGroups);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void SetSystemSetting(string name, string value)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.SetSystemSetting(name, value);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public string GetSystemSetting(string name)
	{
		try
		{
			return CallWithRetry(() => base.ServiceClient.GetSystemSetting(name));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void TriggerUserDefinedEvent2(string userDefinedEventId)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.TriggerUserDefinedEvent2(userDefinedEventId);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void TriggerPluginEvent(Guid pluginEventId, Guid sourceId, Guid[] metadataDeviceIds, Guid[] metadataDeviceGroupIds, string customData)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.TriggerPluginEvent(pluginEventId, sourceId, metadataDeviceIds, metadataDeviceGroupIds, customData);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void TriggerPluginEventBulk(PluginEventInfo[] pluginEvents)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.TriggerPluginEventBulk(pluginEvents);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void MarkInconsistent(string siteId)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.MarkInconsistent(siteId);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void SynchronizeSite(string siteId)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.SyncronizeSite(siteId.ToString().ToUpper(CultureInfo.InvariantCulture));
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void TriggerFullSiteSynchronization()
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.TriggerFullSiteSynchronization();
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void DeleteSite(string siteId)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.DeleteSite(siteId);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public int GetServerStatus()
	{
		try
		{
			return CallWithRetry(() => base.ServiceClient.GetServerStatus());
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public VMOWebserviceObject[] EnableRecordingStorageEncryption(string recordingStorageId, EncryptionMethod encryptionMethod, string password)
	{
		TaskInfo[] taskInfo;
		try
		{
			taskInfo = CallWithRetry(() => base.ServiceClient.EnableRecordingStorageEncryption(recordingStorageId, encryptionMethod, password));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObjects(taskInfo);
	}

	public VMOWebserviceObject[] DisableRecordingStorageEncryption(string recordingStorageId, string password)
	{
		TaskInfo[] taskInfo;
		try
		{
			taskInfo = CallWithRetry(() => base.ServiceClient.DisableRecordingStorageEncryption(recordingStorageId, password));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObjects(taskInfo);
	}

	public void DeleteMarkedData(string markedDataId, MarkedDataDefinitions.MarkedDataTypes markedDataType)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.DeleteMarkedData(markedDataId, markedDataType);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void DeleteBookmarksByDeviceIdBeforeTime(MediaStatusEntry mediaStatus)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.DeleteBookmarksByDeviceIdBeforeTime(mediaStatus);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void EnableFailoverServer(string failoverServerId)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.EnableFailoverServer(failoverServerId);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void DisableFailoverServer(string failoverServerId)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.DisableFailoverServer(failoverServerId);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void StopFailoverService(string failverServerId)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.StopFailoverService(failverServerId);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void MoveFailoverServer(string failoverServerId, string newParentId, int insertAtPosition)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.MoveFailoverServer(failoverServerId, newParentId, insertAtPosition);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public VMOWebserviceObject[] QueryRecordingServerHardwareDrivers(string recordingServerId, bool used)
	{
		GenericObjectInfo2[] genericObjectInfo;
		try
		{
			genericObjectInfo = CallWithRetry(() => base.ServiceClient.QueryRecordingServerHardwareDrivers(recordingServerId, used));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObjects(genericObjectInfo);
	}

	public VMOWebserviceObject[] QueryFailoverServerHardwareDrivers(string failoverServerId)
	{
		GenericObjectInfo2[] genericObjectInfo;
		try
		{
			genericObjectInfo = CallWithRetry(() => base.ServiceClient.QueryFailoverServerHardwareDrivers(failoverServerId));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObjects(genericObjectInfo);
	}

	public VMOWebserviceObject[] QueryActiveRecordingServerHardwareDrivers(string recordingServerId)
	{
		GenericObjectInfo2[] genericObjectInfo;
		try
		{
			genericObjectInfo = CallWithRetry(() => base.ServiceClient.QueryActiveRecordingServerHardwareDrivers(recordingServerId));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObjects(genericObjectInfo);
	}

	public VMOWebserviceObject QueryActiveHardwareDriver(string recordingServerId, string driverId)
	{
		GenericObjectInfo2 genericObjectInfo;
		try
		{
			genericObjectInfo = CallWithRetry(() => base.ServiceClient.QueryActiveHardwareDriver(recordingServerId, driverId));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObject(genericObjectInfo);
	}

	public void ClientProfileMovePriorityUp(string profileId)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.ClientProfileMovePriorityUp(profileId);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void ClientProfileMovePriorityDown(string profileId)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.ClientProfileMovePriorityDown(profileId);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public string QueryActiveManagementClientProfileId()
	{
		string text = null;
		try
		{
			return CallWithRetry(() => base.ServiceClient.QueryActiveManagementClientProfileId());
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public string QueryActiveSmartClientProfileId()
	{
		string text = null;
		try
		{
			return CallWithRetry(() => base.ServiceClient.QueryActiveSmartClientProfileId());
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public string QueryActiveSmartClientProfileId(string sid)
	{
		string text = null;
		try
		{
			return CallWithRetry(() => base.ServiceClient.QueryActiveSmartClientProfileIdBySid(sid));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void AttachClientProfileToRole(string id, string roleId)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.AttachClientProfileToRole(id, roleId);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void ResolveDataCollectorIdOnRegisteredServices(string[] serviceIds)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.ResolveDataCollectorIdOnRegisteredServices(serviceIds);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public VMOWebserviceObject[] Execute(string functionName, VMOWebserviceObject[] parameters)
	{
		GenericObjectInfo2[] genericObjectInfo;
		try
		{
			GenericObjectInfo2[] inputParameters = ConvertToGenericObjectInfo2(parameters);
			genericObjectInfo = CallWithRetry(() => base.ServiceClient.Execute(functionName, inputParameters));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObjects(genericObjectInfo);
	}

	private static VMOWebserviceObject ConvertToVMOWebserviceObject(GenericObjectInfo2 genericObjectInfo)
	{
		VMOWebserviceObject result = null;
		if (genericObjectInfo != null)
		{
			VmoPropertyCollection vmoPropertyCollection = new VmoPropertyCollection();
			ObjectPropertyInfo2[] properties = genericObjectInfo.Properties;
			foreach (ObjectPropertyInfo2 objectPropertyInfo in properties)
			{
				vmoPropertyCollection.Add(value: (!NestedPropertyHelper.IsNestedPropertyName(objectPropertyInfo.Name)) ? objectPropertyInfo.Value : VmoPropertyCollection.FromKeyAndValueArray(KeyAndValue.FromXml((string)objectPropertyInfo.Value)), key: objectPropertyInfo.Name);
			}
			VmoRelationCollection vmoRelationCollection = new VmoRelationCollection();
			if (genericObjectInfo.Relations != null && genericObjectInfo.Relations.Length != 0)
			{
				RelationPropertyInfo[] relations = genericObjectInfo.Relations;
				foreach (RelationPropertyInfo relationPropertyInfo in relations)
				{
					vmoRelationCollection.Add(new VmoRelation(relationPropertyInfo.EntityName, relationPropertyInfo.Id, relationPropertyInfo.IsWeakRelation));
				}
			}
			result = new VMOWebserviceObject(vmoPropertyCollection, vmoRelationCollection, genericObjectInfo.Id, genericObjectInfo.EntityName);
		}
		return result;
	}

	private static VMOWebserviceObject[] ConvertToVMOWebserviceObjects(GenericObjectInfo2[] genericObjectInfo)
	{
		if (genericObjectInfo == null)
		{
			return null;
		}
		List<VMOWebserviceObject> list = new List<VMOWebserviceObject>();
		foreach (GenericObjectInfo2 genericObjectInfo2 in genericObjectInfo)
		{
			if (genericObjectInfo2 != null)
			{
				list.Add(ConvertToVMOWebserviceObject(genericObjectInfo2));
			}
		}
		return list.ToArray();
	}

	private static GenericObjectInfo2[] ConvertToGenericObjectInfo2(VMOWebserviceObject[] items)
	{
		List<GenericObjectInfo2> list = new List<GenericObjectInfo2>();
		foreach (VMOWebserviceObject vmoWebserviceObject in items)
		{
			GenericObjectInfo2 genericObjectInfo = null;
			genericObjectInfo = ConvertToGenericObjectInfo2(vmoWebserviceObject);
			if (genericObjectInfo != null)
			{
				list.Add(genericObjectInfo);
			}
		}
		return list.ToArray();
	}

	private static GenericObjectInfo2 ConvertToGenericObjectInfo2(VMOWebserviceObject vmoWebserviceObject)
	{
		if (vmoWebserviceObject == null)
		{
			return null;
		}
		return new GenericObjectInfo2
		{
			Id = vmoWebserviceObject.Id,
			EntityName = EntityNameAttributeHandlerWrapper.GetEntityName(vmoWebserviceObject.Type),
			Properties = ConvertToGenericObjectPropertyInfo2(vmoWebserviceObject.Properties)
		};
	}

	private static ObjectPropertyInfo2[] ConvertToGenericObjectPropertyInfo2(VmoPropertyCollection properties)
	{
		if (properties != null)
		{
			List<ObjectPropertyInfo2> list = new List<ObjectPropertyInfo2>();
			foreach (KeyValuePair<string, object> property in properties)
			{
				if (NestedPropertyHelper.IsNestedPropertyName(property.Key))
				{
					string value = KeyAndValue.ToXml(((VmoPropertyCollection)property.Value).ToKeyAndValueArray());
					list.Add(new ObjectPropertyInfo2
					{
						Name = property.Key,
						Value = value
					});
				}
				else
				{
					list.Add(new ObjectPropertyInfo2
					{
						Name = property.Key,
						Value = property.Value
					});
				}
			}
			return list.ToArray();
		}
		return null;
	}

	private static CustomProperty ConvertToCustomProperty(CustomPropertyInfo customPropertyInfo)
	{
		if (customPropertyInfo == null)
		{
			return null;
		}
		return new CustomProperty(customPropertyInfo.Name, customPropertyInfo.Value);
	}

	private static CustomProperty[] ConvertToCustomProperties(CustomPropertyInfo[] customPropertyInfo)
	{
		if (customPropertyInfo == null)
		{
			return null;
		}
		List<CustomProperty> list = new List<CustomProperty>();
		foreach (CustomPropertyInfo customPropertyInfo2 in customPropertyInfo)
		{
			list.Add(ConvertToCustomProperty(customPropertyInfo2));
		}
		return list.ToArray();
	}

	private static CustomPropertyInfo ConvertToCustomPropertyInfo(string objectId, CustomProperty customProperty)
	{
		if (customProperty == null)
		{
			return null;
		}
		return new CustomPropertyInfo
		{
			Name = customProperty.Key,
			Value = customProperty.Value,
			ObjectId = objectId
		};
	}

	public static CustomPropertyInfo[] ConvertToCustomPropertyInfos(string objectId, CustomProperty[] customProperties)
	{
		if (customProperties == null)
		{
			return null;
		}
		List<CustomPropertyInfo> list = new List<CustomPropertyInfo>();
		foreach (CustomProperty customProperty in customProperties)
		{
			list.Add(ConvertToCustomPropertyInfo(objectId, customProperty));
		}
		return list.ToArray();
	}

	private static VMOWebserviceObject ConvertToVMOWebserviceObject(BasicUserInfo basicUserInfo)
	{
		return new VMOWebserviceObject(new VmoPropertyCollection
		{
			{ "Id", basicUserInfo.Id },
			{ "Name", basicUserInfo.Name },
			{ "Description", basicUserInfo.Description },
			{ "Sid", basicUserInfo.Sid },
			{ "CanChangePassword", basicUserInfo.CanChangePassword },
			{ "CreatedDate", basicUserInfo.CreatedDate },
			{ "LastModified", basicUserInfo.LastModified }
		}, new VmoRelationCollection(), basicUserInfo.Id, "BasicUser");
	}

	private static VMOWebserviceObject[] ConvertToVMOWebserviceObjects(BasicUserInfo[] basicUserInfo)
	{
		List<VMOWebserviceObject> list = new List<VMOWebserviceObject>();
		foreach (BasicUserInfo basicUserInfo2 in basicUserInfo)
		{
			list.Add(ConvertToVMOWebserviceObject(basicUserInfo2));
		}
		return list.ToArray();
	}

	private static BasicUserInfo ConvertToBasicUserInfo(VMOWebserviceObject vmoWebserviceObject)
	{
		BasicUserInfo basicUserInfo = new BasicUserInfo();
		basicUserInfo.Id = vmoWebserviceObject.Id;
		if (vmoWebserviceObject.Properties.ContainsKey("Id"))
		{
			basicUserInfo.Id = vmoWebserviceObject.Properties.GetPropertyValue<string>("Id");
		}
		if (vmoWebserviceObject.Properties.ContainsKey("Name"))
		{
			basicUserInfo.Name = vmoWebserviceObject.Properties.GetPropertyValue<string>("Name");
		}
		if (vmoWebserviceObject.Properties.ContainsKey("Description"))
		{
			basicUserInfo.Description = vmoWebserviceObject.Properties.GetPropertyValue<string>("Description");
		}
		if (vmoWebserviceObject.Properties.ContainsKey("Sid"))
		{
			basicUserInfo.Sid = vmoWebserviceObject.Properties.GetPropertyValue<string>("Sid");
		}
		if (vmoWebserviceObject.Properties.ContainsKey("CanChangePassword"))
		{
			basicUserInfo.CanChangePassword = vmoWebserviceObject.Properties.GetPropertyValue<bool>("CanChangePassword");
		}
		if (vmoWebserviceObject.Properties.ContainsKey("CreatedDate"))
		{
			basicUserInfo.CreatedDate = vmoWebserviceObject.Properties.GetPropertyValue<DateTime>("CreatedDate");
		}
		if (vmoWebserviceObject.Properties.ContainsKey("LastModified"))
		{
			basicUserInfo.LastModified = vmoWebserviceObject.Properties.GetPropertyValue<DateTime>("LastModified");
		}
		return basicUserInfo;
	}

	public static string TaskDbTypeToEntityName(string taskDbType)
	{
		return _taskDbTypeToEntityName[taskDbType];
	}

	private static VMOWebserviceObject ConvertToVMOWebserviceObject(TaskInfo taskInfo)
	{
		VmoPropertyCollection vmoPropertyCollection = new VmoPropertyCollection();
		vmoPropertyCollection.Add("Id", taskInfo.Id);
		vmoPropertyCollection.Add("Target", taskInfo.Target);
		vmoPropertyCollection.Add("Information", taskInfo.Information);
		vmoPropertyCollection.Add("PercentCompleted", taskInfo.PercentCompleted);
		vmoPropertyCollection.Add("Status", taskInfo.AsyncStatus.ExceptionStatus.ToString());
		vmoPropertyCollection.Add("StatusDetails", taskInfo.AsyncStatus.LastException);
		vmoPropertyCollection.Add("StartTime", taskInfo.StartTime);
		vmoPropertyCollection.Add("EndTime", taskInfo.EndTime);
		VmoPropertyCollection vmoPropertyCollection2 = VmoPropertyCollection.FromKeyValueInfoArray(taskInfo.TaskProperties);
		vmoPropertyCollection.Add("InputParameters", vmoPropertyCollection2);
		string propertyValue = vmoPropertyCollection2.GetPropertyValue<string>("idRecorder");
		if (propertyValue == null)
		{
			propertyValue = vmoPropertyCollection2.GetPropertyValue<string>("target");
			vmoPropertyCollection.SetNestedPropertyOrDefault("InputParameters", "idRecorder", propertyValue);
		}
		VmoRelationCollection vmoRelationCollection = new VmoRelationCollection();
		vmoRelationCollection.Add(new VmoRelation("RecordingServer", propertyValue));
		return new VMOWebserviceObject(vmoPropertyCollection, vmoRelationCollection, taskInfo.Id, TaskDbTypeToEntityName(taskInfo.Type));
	}

	private static VMOWebserviceObject[] ConvertToVMOWebserviceObjects(TaskInfo[] taskInfo)
	{
		List<VMOWebserviceObject> list = new List<VMOWebserviceObject>();
		foreach (TaskInfo taskInfo2 in taskInfo)
		{
			list.Add(ConvertToVMOWebserviceObject(taskInfo2));
		}
		return list.ToArray();
	}
}
