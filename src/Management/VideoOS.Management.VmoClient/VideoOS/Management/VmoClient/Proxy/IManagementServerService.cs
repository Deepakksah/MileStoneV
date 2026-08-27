using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using VideoOS.Management.Client.Proxy;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient.Proxy;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01", ConfigurationName = "VideoOS.Management.VmoClient.Proxy.IManagementServerService")]
public interface IManagementServerService
{
	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/Execute", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ExecuteResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ExecuteVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2[] Execute(string functionName, GenericObjectInfo2[] parameters);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryObject2", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryObject2Response")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryObject2VMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2 QueryObject2(string entityName, string id, string[] propertiesToLoad);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryObjects", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryObjectsResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryObjectsVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2[] QueryObjects(string entityName, string[] ids, string[] propertiesToLoad);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryObjectCollectionIds", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryObjectCollectionIdsResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryObjectCollectionIdsVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	string[] QueryObjectCollectionIds(string entityName, string parentEntityName, string parentId, string[] propertiesToLoad);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/CreateObject2", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/CreateObject2Response")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/CreateObject2VMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2 CreateObject2(string entityName, ObjectPropertyInfo2[] properties, CustomPropertyInfo[] customProperties);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/UpdateObject2", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/UpdateObject2Response")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/UpdateObject2VMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2 UpdateObject2(GenericObjectInfo2 genericObject, CustomPropertyInfo[] customProperties);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DeleteObject2", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DeleteObject2Response")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DeleteObject2VMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void DeleteObject2(string entityName, string id);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryServerGroups", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryServerGroupsResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryServerGroupsVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	ServerGroupInfo[] QueryServerGroups(string groupStructureType, string parentGroupId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryServerGroupById", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryServerGroupByIdResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryServerGroupByIdVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	ServerGroupInfo QueryServerGroupById(string parentGroupId, string serverGroupId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryServerGroupByName", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryServerGroupByNameResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryServerGroupByNameVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	ServerGroupInfo QueryServerGroupByName(string parentGroupId, string name);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/CreateServerGroup", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/CreateServerGroupResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/CreateServerGroupVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	ServerGroupInfo CreateServerGroup(string name, string description, string groupStructureType, string parentGroupId, ServerGroupOptions serverGroupOptions);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/UpdateServerGroup", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/UpdateServerGroupResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/UpdateServerGroupVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	ServerGroupInfo UpdateServerGroup(string serverGroupId, string name, string description, ServerGroupOptions serverGroupOptions);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DeleteServerGroup", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DeleteServerGroupResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DeleteServerGroupVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void DeleteServerGroup(string serverGroupId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/MoveServerGroup", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/MoveServerGroupResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/MoveServerGroupVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void MoveServerGroup(string serverGroupId, string newParentGroupId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/GetCustomProperties", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/GetCustomPropertiesResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/GetCustomPropertiesVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	CustomPropertyInfo[] GetCustomProperties(string objectId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/GetCustomProperty", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/GetCustomPropertyResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/GetCustomPropertyVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	CustomPropertyInfo GetCustomProperty(string objectId, string name);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/UpdateCustomProperties", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/UpdateCustomPropertiesResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/UpdateCustomPropertiesVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void UpdateCustomProperties(string objectId, CustomPropertyInfo[] customPropertyArray);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DeleteCustomProperty", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DeleteCustomPropertyResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DeleteCustomPropertyVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void DeleteCustomProperty(string objectId, string name);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/TriggerUserDefinedEvent2", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/TriggerUserDefinedEvent2Response")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/TriggerUserDefinedEvent2VMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void TriggerUserDefinedEvent2(string userDefinedEventId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/TriggerPluginEvent", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/TriggerPluginEventResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/TriggerPluginEventVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void TriggerPluginEvent(Guid pluginEventId, Guid sourceId, Guid[] metadataDeviceIds, Guid[] metadataDeviceGroupIds, string customData);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/TriggerPluginEventBulk", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/TriggerPluginEventBulkResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/TriggerPluginEventBulkVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void TriggerPluginEventBulk(PluginEventInfo[] pluginEvents);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DeleteSite", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DeleteSiteResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DeleteSiteVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void DeleteSite(string siteId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/SyncronizeSite", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/SyncronizeSiteResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/SyncronizeSiteVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void SyncronizeSite(string siteId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/MarkInconsistent", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/MarkInconsistentResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/MarkInconsistentVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void MarkInconsistent(string siteId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/TriggerFullSiteSynchronization", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/TriggerFullSiteSynchronizationResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/TriggerFullSiteSynchronizationVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void TriggerFullSiteSynchronization();

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryTasks", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryTasksResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryTasksVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	TaskInfo[] QueryTasks();

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryTasksByRecordingServer", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryTasksByRecordingServerResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryTasksByRecordingServerVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	TaskInfo[] QueryTasksByRecordingServer(string recordingServerId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryTask", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryTaskResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryTaskVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	TaskInfo QueryTask(string taskId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/MoveRecordingStorage", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/MoveRecordingStorageResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/MoveRecordingStorageVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	TaskInfo MoveRecordingStorage(string recordingStorageId, string newPath);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DeleteRecordingStorage", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DeleteRecordingStorageResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DeleteRecordingStorageVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	TaskInfo DeleteRecordingStorage(string recordingStorageId, bool deleteIfOffline);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/MoveArchiveStorage", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/MoveArchiveStorageResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/MoveArchiveStorageVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	TaskInfo MoveArchiveStorage(string archiveStorageId, string newPath);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DeleteArchiveStorage", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DeleteArchiveStorageResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DeleteArchiveStorageVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	TaskInfo DeleteArchiveStorage(string archiveStorageId, bool deleteIfOffline);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/TruncateDeviceStorage", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/TruncateDeviceStorageResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/TruncateDeviceStorageVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	TaskInfo TruncateDeviceStorage(string deviceId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ChangeDeviceRecordingStorage", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ChangeDeviceRecordingStorageResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ChangeDeviceRecordingStorageVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	TaskInfo ChangeDeviceRecordingStorage(string deviceId, string newStorageId, bool moveData);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/EnableRecordingStorageEncryption", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/EnableRecordingStorageEncryptionResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/EnableRecordingStorageEncryptionVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	TaskInfo[] EnableRecordingStorageEncryption(string recordingStorageId, EncryptionMethod encryptionMethod, string password);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DisableRecordingStorageEncryption", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DisableRecordingStorageEncryptionResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DisableRecordingStorageEncryptionVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	TaskInfo[] DisableRecordingStorageEncryption(string recordingStorageId, string password);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ChangeRecordingStorageEncryptionMethod", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ChangeRecordingStorageEncryptionMethodResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ChangeRecordingStorageEncryptionMethodVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	TaskInfo ChangeRecordingStorageEncryptionMethod(string recordingStorageId, EncryptionMethod encryptionMethod, string password);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ChangeRecordingStorageEncryptionPassword", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ChangeRecordingStorageEncryptionPasswordResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ChangeRecordingStorageEncryptionPasswordVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	TaskInfo ChangeRecordingStorageEncryptionPassword(string recordingStorageId, string oldPassword, string newPassword);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/RequestEdgeStorageRetrieval", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/RequestEdgeStorageRetrievalResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/RequestEdgeStorageRetrievalVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	TaskInfo RequestEdgeStorageRetrieval(string deviceId, DateTime startTime, DateTime endTime, string metadata);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/StopEdgeStorageRetrieval", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/StopEdgeStorageRetrievalResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/StopEdgeStorageRetrievalVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void StopEdgeStorageRetrieval(string taskId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/CleanupTask", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/CleanupTaskResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/CleanupTaskVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void CleanupTask(string taskId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryBasicUsers", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryBasicUsersResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryBasicUsersVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	BasicUserInfo[] QueryBasicUsers();

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryBasicUserById", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryBasicUserByIdResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryBasicUserByIdVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	BasicUserInfo QueryBasicUserById(string id);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryBasicUserBySid", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryBasicUserBySidResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryBasicUserBySidVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	BasicUserInfo QueryBasicUserBySid(string sid);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryBasicUsersByIds", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryBasicUsersByIdsResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryBasicUsersByIdsVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	BasicUserInfo[] QueryBasicUsersByIds(string[] ids);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/CreateBasicUser", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/CreateBasicUserResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/CreateBasicUserVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	BasicUserInfo CreateBasicUser(string id, string userName, string password, string description, bool canChangePassword);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/UpdateBasicUser", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/UpdateBasicUserResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/UpdateBasicUserVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	BasicUserInfo UpdateBasicUser(string id, string userName, string description, bool canChangePassword);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DeleteBasicUser", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DeleteBasicUserResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DeleteBasicUserVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void DeleteBasicUser(string id);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ChangePasswordBasicUser", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ChangePasswordBasicUserResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ChangePasswordBasicUserVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void ChangePasswordBasicUser(string id, string newPassword);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ApplyLayoutGeneric", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ApplyLayoutGenericResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ApplyLayoutGenericVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2 ApplyLayoutGeneric(string monitorId, string layoutId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ApplyCamerasToMonitorViewGeneric", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ApplyCamerasToMonitorViewGenericResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ApplyCamerasToMonitorViewGenericVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2 ApplyCamerasToMonitorViewGeneric(string monitorId, string[] cameras, string[] cameraGroups, int fromPosition);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ApplyCamerasAndLayoutToMonitorViewGeneric", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ApplyCamerasAndLayoutToMonitorViewGenericResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ApplyCamerasAndLayoutToMonitorViewGenericVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2 ApplyCamerasAndLayoutToMonitorViewGeneric(string monitorId, string[] cameras, string[] cameraGroups, int fromPosition, string layoutId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ApplyViewItemXmlsToMonitorViewGeneric", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ApplyViewItemXmlsToMonitorViewGenericResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ApplyViewItemXmlsToMonitorViewGenericVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2 ApplyViewItemXmlsToMonitorViewGeneric(string monitorId, string[] viewItemXmls, int fromPosition);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ApplyMonitorStateToMonitorGeneric", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ApplyMonitorStateToMonitorGenericResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ApplyMonitorStateToMonitorGenericVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void ApplyMonitorStateToMonitorGeneric(string monitorId, string xmlState);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ApplyTextAndLayoutToMonitorViewGeneric", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ApplyTextAndLayoutToMonitorViewGenericResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ApplyTextAndLayoutToMonitorViewGenericVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2 ApplyTextAndLayoutToMonitorViewGeneric(string monitorId, string messageToDisplay, int fromPosition, string layoutId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/RemoveCamerasFromMonitorViewGeneric", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/RemoveCamerasFromMonitorViewGenericResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/RemoveCamerasFromMonitorViewGenericVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void RemoveCamerasFromMonitorViewGeneric(string monitorId, string[] cameras, string[] cameraGroups);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ActivateVideoWallPreset", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ActivateVideoWallPresetResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ActivateVideoWallPresetVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void ActivateVideoWallPreset(string videoWallPresetId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/AttachSlaveSite2", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/AttachSlaveSite2Response")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/AttachSlaveSite2VMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2 AttachSlaveSite2(string slaveSiteId, string slaveSiteAddressesXml, string slaveSiteVersion);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/AttachMasterSite2", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/AttachMasterSite2Response")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/AttachMasterSite2VMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2 AttachMasterSite2(string masterSiteId, string masterSiteAddressesXml, string masterSiteVersion);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/AuthorizeMaster2", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/AuthorizeMaster2Response")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/AuthorizeMaster2VMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2 AuthorizeMaster2();

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/GetServerStatus", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/GetServerStatusResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/GetServerStatusVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	int GetServerStatus();

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/CreateViewGeneric", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/CreateViewGenericResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/CreateViewGenericVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2 CreateViewGeneric(string viewGroupId, ObjectPropertyInfo2[] viewProperties, CustomPropertyInfo[] viewCustomProperties, ObjectPropertyInfo2[][] viewItemsProperties, CustomPropertyInfo[][] viewItemsCustomProperties);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ClearViewGeneric", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ClearViewGenericResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ClearViewGenericVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2 ClearViewGeneric(string viewId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ApplyViewToViewGeneric", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ApplyViewToViewGenericResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ApplyViewToViewGenericVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2 ApplyViewToViewGeneric(string sourceViewId, string destinationViewId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ClearViewItemGeneric", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ClearViewItemGenericResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ClearViewItemGenericVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2 ClearViewItemGeneric(string viewItemId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ChangeViewItemId", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ChangeViewItemIdResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ChangeViewItemIdVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void ChangeViewItemId(string existingId, string newId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/GetRecordingServerInformation", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/GetRecordingServerInformationResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/GetRecordingServerInformationVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2 GetRecordingServerInformation(string recordingServerId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/GetRecordingStorageInformation", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/GetRecordingStorageInformationResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/GetRecordingStorageInformationVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2 GetRecordingStorageInformation(string recordingStorageId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/GetArchiveStorageInformation", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/GetArchiveStorageInformationResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/GetArchiveStorageInformationVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2 GetArchiveStorageInformation(string archiveStorageId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/SetRecordingStorageAsDefault", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/SetRecordingStorageAsDefaultResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/SetRecordingStorageAsDefaultVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void SetRecordingStorageAsDefault(string recordingStorageId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/EnableFramerateReduction", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/EnableFramerateReductionResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/EnableFramerateReductionVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void EnableFramerateReduction(string archiveStorageId, bool enable);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ValidateEncryptionPassword", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ValidateEncryptionPasswordResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ValidateEncryptionPasswordVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	bool ValidateEncryptionPassword(string recordingStorageId, string password);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QuerySystemMonitorStateSourcesByIds", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QuerySystemMonitorStateSourcesByIdsResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QuerySystemMonitorStateSourcesByIdsVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2[] QuerySystemMonitorStateSourcesByIds(string[] ids);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QuerySystemMonitorStateSourcesByTypes", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QuerySystemMonitorStateSourcesByTypesResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QuerySystemMonitorStateSourcesByTypesVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2[] QuerySystemMonitorStateSourcesByTypes(string[] types);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryAllRuleSources", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryAllRuleSourcesResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryAllRuleSourcesVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2[] QueryAllRuleSources();

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryMarkedDataReference", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryMarkedDataReferenceResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryMarkedDataReferenceVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	MarkedDataReferenceInfo2 QueryMarkedDataReference(string[] deviceIds, bool realTime, MarkedDataDefinitions.MarkedDataTypes markedDataTypes);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryMarkedData", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryMarkedDataResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryMarkedDataVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2 QueryMarkedData(string markedDataId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryBookmarksSearch", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryBookmarksSearchResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryBookmarksSearchVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2[] QueryBookmarksSearch(DateTime centerTime, DateTime timeLimit, int countLimit, string[] deviceTypes, string[] optDeviceIds, string[] optUsers, string optSearchStr, string optFromBookmarkId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryBookmarksSearch2", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryBookmarksSearch2Response")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryBookmarksSearch2VMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2[] QueryBookmarksSearch2(DateTime centerTime, DateTime timeLimit, int countLimit, string[] deviceTypes, string[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, string optFromBookmarkId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryMarkedDataSearch", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryMarkedDataSearchResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryMarkedDataSearchVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2[] QueryMarkedDataSearch(string[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFromTime, DateTime retentionExpireToTime, int pageIndex, int pageSize, string sortOrder, bool sortAscending);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryMarkedDataSearch2", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryMarkedDataSearch2Response")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryMarkedDataSearch2VMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2[] QueryMarkedDataSearch2(string[] deviceIds, string searchText, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFromTime, DateTime retentionExpireToTime, int pageIndex, int pageSize, string sortOrder, bool sortAscending);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/CreateMarkedData", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/CreateMarkedDataResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/CreateMarkedDataVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2 CreateMarkedData(ObjectPropertyInfo2[] properties);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/UpdateMarkedData", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/UpdateMarkedDataResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/UpdateMarkedDataVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2 UpdateMarkedData(ObjectPropertyInfo2[] properties);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DeleteMarkedData", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DeleteMarkedDataResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DeleteMarkedDataVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void DeleteMarkedData(string markedDataId, MarkedDataDefinitions.MarkedDataTypes markedDataType);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DeleteBookmarksByDeviceIdBeforeTime", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DeleteBookmarksByDeviceIdBeforeTimeResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DeleteBookmarksByDeviceIdBeforeTimeVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void DeleteBookmarksByDeviceIdBeforeTime(MediaStatusEntry mediaStatus);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/EnableFailoverServer", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/EnableFailoverServerResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/EnableFailoverServerVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void EnableFailoverServer(string failoverServerId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DisableFailoverServer", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DisableFailoverServerResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DisableFailoverServerVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void DisableFailoverServer(string failoverServerId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/StopFailoverService", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/StopFailoverServiceResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/StopFailoverServiceVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void StopFailoverService(string failoverServerId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/MoveFailoverServer", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/MoveFailoverServerResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/MoveFailoverServerVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void MoveFailoverServer(string failoverServerId, string newParentId, int insertAtPosition);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryRecordingServerHardwareDrivers", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryRecordingServerHardwareDriversResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryRecordingServerHardwareDriversVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2[] QueryRecordingServerHardwareDrivers(string recordingServerId, bool used);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryFailoverServerHardwareDrivers", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryFailoverServerHardwareDriversResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryFailoverServerHardwareDriversVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2[] QueryFailoverServerHardwareDrivers(string failoverServerId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryActiveRecordingServerHardwareDrivers", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryActiveRecordingServerHardwareDriversResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryActiveRecordingServerHardwareDriversVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2[] QueryActiveRecordingServerHardwareDrivers(string recordingServerId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryActiveHardwareDriver", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryActiveHardwareDriverResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryActiveHardwareDriverVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	GenericObjectInfo2 QueryActiveHardwareDriver(string recordingServerId, string driverId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DeleteServerTasksByDeviceIdBeforeTime", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DeleteServerTasksByDeviceIdBeforeTimeResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/DeleteServerTasksByDeviceIdBeforeTimeVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void DeleteServerTasksByDeviceIdBeforeTime(string deviceId, DateTime time);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ClientProfileMovePriorityUp", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ClientProfileMovePriorityUpResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ClientProfileMovePriorityUpVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void ClientProfileMovePriorityUp(string id);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ClientProfileMovePriorityDown", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ClientProfileMovePriorityDownResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ClientProfileMovePriorityDownVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void ClientProfileMovePriorityDown(string id);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryActiveManagementClientProfileId", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryActiveManagementClientProfileIdResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryActiveManagementClientProfileIdVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	string QueryActiveManagementClientProfileId();

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryActiveSmartClientProfileId", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryActiveSmartClientProfileIdResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryActiveSmartClientProfileIdVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	string QueryActiveSmartClientProfileId();

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryActiveSmartClientProfileIdBySid", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryActiveSmartClientProfileIdBySidResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueryActiveSmartClientProfileIdBySidVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	string QueryActiveSmartClientProfileIdBySid(string sid);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/AttachClientProfileToRole", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/AttachClientProfileToRoleResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/AttachClientProfileToRoleVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void AttachClientProfileToRole(string id, string roleId);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/SetSystemSetting", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/SetSystemSettingResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/SetSystemSettingVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void SetSystemSetting(string name, string value);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/GetSystemSetting", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/GetSystemSettingResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/GetSystemSettingVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	string GetSystemSetting(string name);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/IsFeatureLicensed", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/IsFeatureLicensedResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/IsFeatureLicensedVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	bool IsFeatureLicensed(string featureName);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/GetFeatureParameterValue", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/GetFeatureParameterValueResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/GetFeatureParameterValueVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	string GetFeatureParameterValue(string featureName, string parameterName);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/UpdateLicense", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/UpdateLicenseResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/UpdateLicenseVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void UpdateLicense(byte[] license, string ownerXml, string sitesInformation);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/GetLastAutomaticActivationError", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/GetLastAutomaticActivationErrorResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/GetLastAutomaticActivationErrorVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	VMOExceptionDetail GetLastAutomaticActivationError();

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/UpdateMipPluginsLicenseRequestPart", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/UpdateMipPluginsLicenseRequestPartResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/UpdateMipPluginsLicenseRequestPartVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void UpdateMipPluginsLicenseRequestPart(string license);

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueueAutomaticActivation", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueueAutomaticActivationResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/QueueAutomaticActivationVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void QueueAutomaticActivation();

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/RefreshLicenseCache", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/RefreshLicenseCacheResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/RefreshLicenseCacheVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void RefreshLicenseCache();

	[OperationContract(Action = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ResolveDataCollectorIdOnRegisteredServices", ReplyAction = "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01/IManagementServerService/ResolveDataCollectorIdOnRegisteredServicesResponse")]
	void ResolveDataCollectorIdOnRegisteredServices(string[] serviceIds);
}
