using System.CodeDom.Compiler;
using System.ServiceModel;

namespace VideoOS.Platform.Util.Security;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://schemas.videoos/SecurityService/2008/05/Vmo/01", ConfigurationName = "VideoOS.Platform.Util.Security.ISecurityService")]
public interface ISecurityService
{
	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryRoles", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryRolesResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryRolesVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	RoleInfo[] QueryRoles();

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QuerySecurityNamespaces", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QuerySecurityNamespacesResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QuerySecurityNamespacesVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	SecurityNamespaceInfo[] QuerySecurityNamespaces(string namespaceId);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/UpdateSecurityNamespace", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/UpdateSecurityNamespaceResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/UpdateSecurityNamespaceVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void UpdateSecurityNamespace(SecurityNamespaceInfo description);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/ReadIdentities", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/ReadIdentitiesResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/ReadIdentitiesVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	Identity[] ReadIdentities(SearchFactor factor, string[] factorValue, QueryMembership queryMembership);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/ReadIdentityFromSource", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/ReadIdentityFromSourceResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/ReadIdentityFromSourceVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	Identity ReadIdentityFromSource(SearchFactor factor, string factorValue);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryObjectClasses", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryObjectClassesResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryObjectClassesVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	string[] QueryObjectClasses();

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryObjectClass", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryObjectClassResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryObjectClassVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	string QueryObjectClass(string objectId);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryObjectClassActions", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryObjectClassActionsResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryObjectClassActionsVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	SecurityAction[] QueryObjectClassActions(string classId);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/CreateObjectClass", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/CreateObjectClassResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/CreateObjectClassVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void CreateObjectClass(string objectClassId, string[] classActions);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryAccessControlList", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryAccessControlListResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryAccessControlListVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	AccessControlEntry[] QueryAccessControlList(string objectId);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryAccessControlListByIdentity", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryAccessControlListByIdentityResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryAccessControlListByIdentityVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	AccessControlEntry[] QueryAccessControlListByIdentity(string identity, string objectId);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/RegisterSecurityObject", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/RegisterSecurityObjectResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/RegisterSecurityObjectVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void RegisterSecurityObject(string objectId, string objectClassId, string parentObjectId);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/UnregisterSecurityObject", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/UnregisterSecurityObjectResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/UnregisterSecurityObjectVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void UnregisterSecurityObject(string objectId);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/RegisterObject", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/RegisterObjectResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/RegisterObjectVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void RegisterObject(string namespaceId, string objectId, string parentObjectId);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/UnregisterObject", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/UnregisterObjectResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/UnregisterObjectVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void UnregisterObject(string namespaceId, string objectId);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/AddAccessControlEntries", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/AddAccessControlEntriesResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/AddAccessControlEntriesVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void AddAccessControlEntries(string objectId, AccessControlEntry[] entries);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/RemoveAccessControlEntries", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/RemoveAccessControlEntriesResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/RemoveAccessControlEntriesVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void RemoveAccessControlEntries(string objectId, AccessControlEntry[] entries);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/ReplaceAccessControlEntries", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/ReplaceAccessControlEntriesResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/ReplaceAccessControlEntriesVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void ReplaceAccessControlEntries(string objectId, AccessControlEntry[] entries);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/AddPermissions", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/AddPermissionsResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/AddPermissionsVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void AddPermissions(string namespaceId, string objectId, AccessControlEntry[] entries);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/RemovePermissions", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/RemovePermissionsResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/RemovePermissionsVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void RemovePermissions(string namespaceId, string objectId, AccessControlEntry[] entries);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/SetInheritance", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/SetInheritanceResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/SetInheritanceVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void SetInheritance(string objectId, string parentObjectId);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/CheckPermission", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/CheckPermissionResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/CheckPermissionVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void CheckPermission(string objectId, string actionId);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/IsMember", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/IsMemberResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/IsMemberVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	bool IsMember(string identitySid, string groupSid);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/IsPermittedByIdentityList", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/IsPermittedByIdentityListResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/IsPermittedByIdentityListVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	bool[] IsPermittedByIdentityList(string[] identityList, string actionId, string objectId);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/HasPermissionByActionList", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/HasPermissionByActionListResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/HasPermissionByActionListVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	bool[] HasPermissionByActionList(string namespaceId, string identity, string[] actionList, string objectId);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/IsPermittedByObjectList", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/IsPermittedByObjectListResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/IsPermittedByObjectListVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	bool[] IsPermittedByObjectList(string identity, string actionId, string[] objectIdList);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/HasPermissionByActionListCurrentUser", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/HasPermissionByActionListCurrentUserResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/HasPermissionByActionListCurrentUserVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	bool[] HasPermissionByActionListCurrentUser(string namespaceId, string[] actionList, string objectId);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/HasPermissionByObjectList", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/HasPermissionByObjectListResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/HasPermissionByObjectListVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	bool[] HasPermissionByObjectList(string namespaceId, string identity, string actionId, string[] objectList);
}
