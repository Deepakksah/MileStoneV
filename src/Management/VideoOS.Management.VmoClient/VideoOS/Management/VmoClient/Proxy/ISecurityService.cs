using System.CodeDom.Compiler;
using System.ServiceModel;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient.Proxy;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://schemas.videoos/SecurityService/2008/05/Vmo/01", ConfigurationName = "VideoOS.Management.VmoClient.Proxy.ISecurityService")]
public interface ISecurityService
{
	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryRoles", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryRolesResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryRolesVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	RoleInfo[] QueryRoles();

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryRoleById", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryRoleByIdResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryRoleByIdVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	RoleInfo QueryRoleById(string roleId);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryRoleByName", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryRoleByNameResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryRoleByNameVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	RoleInfo QueryRoleByName(string name);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryRoleMembers", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryRoleMembersResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryRoleMembersVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	Identity[] QueryRoleMembers(string roleId);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryRoleMembersExtended", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryRoleMembersExtendedResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryRoleMembersExtendedVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	Identity[] QueryRoleMembersExtended(string roleId, QueryMembership queryMembership, bool includeMappedExternalUsers);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/CreateRole", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/CreateRoleResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/CreateRoleVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	RoleInfo CreateRole(string name, string description);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/UpdateRole", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/UpdateRoleResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/UpdateRoleVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	RoleInfo UpdateRole(string id, string name, string description);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/CopyRole", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/CopyRoleResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/CopyRoleVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	RoleInfo CopyRole(string sourceRoleId, string name, string description);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/DeleteRole", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/DeleteRoleResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/DeleteRoleVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void DeleteRole(string roleId);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/AddMemberToRole", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/AddMemberToRoleResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/AddMemberToRoleVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	Identity AddMemberToRole(string roleId, string memberSid);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/RemoveMemberFromRole", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/RemoveMemberFromRoleResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/RemoveMemberFromRoleVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void RemoveMemberFromRole(string roleId, string memberSid);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/ReadIdentities", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/ReadIdentitiesResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/ReadIdentitiesVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	Identity[] ReadIdentities(SearchFactor factor, string[] factorValue, QueryMembership queryMembership);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/ReadIdentityFromSource", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/ReadIdentityFromSourceResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/ReadIdentityFromSourceVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	Identity ReadIdentityFromSource(SearchFactor factor, string factorValue);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/RemoveIdentity", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/RemoveIdentityResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/RemoveIdentityVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void RemoveIdentity(string userSid);

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

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/AddAccessControlEntries", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/AddAccessControlEntriesResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/AddAccessControlEntriesVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void AddAccessControlEntries(string objectId, AccessControlEntry[] entries);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/RemoveAccessControlEntries", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/RemoveAccessControlEntriesResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/RemoveAccessControlEntriesVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void RemoveAccessControlEntries(string objectId, AccessControlEntry[] entries);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/ReplaceAccessControlEntries", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/ReplaceAccessControlEntriesResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/ReplaceAccessControlEntriesVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void ReplaceAccessControlEntries(string objectId, AccessControlEntry[] entries);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryAccessControlTimeProfileEntry", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryAccessControlTimeProfileEntryResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryAccessControlTimeProfileEntryVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	AccessControlTimeProfileEntry QueryAccessControlTimeProfileEntry(string objectId, string identity, string actionId);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/AddUpdateAccessControlTimeProfileEntry", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/AddUpdateAccessControlTimeProfileEntryResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/AddUpdateAccessControlTimeProfileEntryVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	AccessControlTimeProfileEntry AddUpdateAccessControlTimeProfileEntry(string objectId, string identity, string actionId, string timeProfileId, bool isDefaultTimeProfile);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/DeleteAccessControlTimeProfile", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/DeleteAccessControlTimeProfileResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/DeleteAccessControlTimeProfileVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void DeleteAccessControlTimeProfile(string objectId, string identity, string actionId);

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

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/IsPermittedByActionList", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/IsPermittedByActionListResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/IsPermittedByActionListVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	bool[] IsPermittedByActionList(string identity, string[] actionIdList, string objectId);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/IsPermittedByObjectList", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/IsPermittedByObjectListResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/IsPermittedByObjectListVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	bool[] IsPermittedByObjectList(string identity, string actionId, string[] objectIdList);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/IsPermittedTimeBasedAction", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/IsPermittedTimeBasedActionResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/IsPermittedTimeBasedActionVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	bool IsPermittedTimeBasedAction(string actionId, string objectId);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/RegisterObject", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/RegisterObjectResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/RegisterObjectVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void RegisterObject(string namespaceId, string objectId, string parentObjectId);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/UnregisterObject", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/UnregisterObjectResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/UnregisterObjectVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void UnregisterObject(string namespaceId, string objectId);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QuerySecurityNamespaces", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QuerySecurityNamespacesResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QuerySecurityNamespacesVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	SecurityNamespaceInfo[] QuerySecurityNamespaces(string namespaceId);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/CreateSecurityNamespace", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/CreateSecurityNamespaceResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/CreateSecurityNamespaceVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void CreateSecurityNamespace(SecurityNamespaceInfo description, string orderAfterNamespaceId);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/UpdateSecurityNamespace", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/UpdateSecurityNamespaceResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/UpdateSecurityNamespaceVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void UpdateSecurityNamespace(SecurityNamespaceInfo description);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/DeleteSecurityNamespace", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/DeleteSecurityNamespaceResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/DeleteSecurityNamespaceVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void DeleteSecurityNamespace(string namespaceId);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/CreateSecurityGroup", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/CreateSecurityGroupResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/CreateSecurityGroupVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void CreateSecurityGroup(string namespaceId, string path, string name);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/RenameSecurityGroup", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/RenameSecurityGroupResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/RenameSecurityGroupVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void RenameSecurityGroup(string namespaceId, string path, string newName);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/DeleteSecurityGroup", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/DeleteSecurityGroupResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/DeleteSecurityGroupVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void DeleteSecurityGroup(string namespaceId, string path, string moveItemsToPath);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/MoveSecurityGroup", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/MoveSecurityGroupResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/MoveSecurityGroupVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void MoveSecurityGroup(string namespaceId, string sourcePath, string targetPath);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/MoveItems", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/MoveItemsResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/MoveItemsVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void MoveItems(string namespaceId, string[] sourcePath, string[] targetPath);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryPermissions", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryPermissionsResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/QueryPermissionsVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	AccessControlEntry[] QueryPermissions(string namespaceId, string securityToken, string identity);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/AddPermissions", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/AddPermissionsResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/AddPermissionsVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void AddPermissions(string namespaceId, string objectId, AccessControlEntry[] entries);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/RemovePermissions", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/RemovePermissionsResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/RemovePermissionsVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void RemovePermissions(string namespaceId, string objectId, AccessControlEntry[] entries);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/AddRemovePermissions", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/AddRemovePermissionsResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/AddRemovePermissionsVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	void AddRemovePermissions(string namespaceId, string objectId, AccessControlEntry[] addEntries, AccessControlEntry[] removeEntries);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/HasPermissionByIdentityList", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/HasPermissionByIdentityListResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/HasPermissionByIdentityListVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	bool[] HasPermissionByIdentityList(string namespaceId, string[] identityList, string actionId, string objectId);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/HasPermissionByActionList", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/HasPermissionByActionListResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/HasPermissionByActionListVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	bool[] HasPermissionByActionList(string namespaceId, string identity, string[] actionList, string objectId);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/HasPermissionByActionListCurrentUser", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/HasPermissionByActionListCurrentUserResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/HasPermissionByActionListCurrentUserVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	bool[] HasPermissionByActionListCurrentUser(string namespaceId, string[] actionList, string objectId);

	[OperationContract(Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/HasPermissionByObjectList", ReplyAction = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/HasPermissionByObjectListResponse")]
	[FaultContract(typeof(VMOExceptionDetail), Action = "http://schemas.videoos/SecurityService/2008/05/Vmo/01/ISecurityService/HasPermissionByObjectListVMOExceptionDetailFault", Name = "VMOExceptionDetail", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
	bool[] HasPermissionByObjectList(string namespaceId, string identity, string actionId, string[] objectList);
}
