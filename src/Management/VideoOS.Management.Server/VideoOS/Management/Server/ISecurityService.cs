using System.ServiceModel;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[ServiceContract(Namespace = "http://schemas.videoos/SecurityService/2008/05/Vmo/01")]
public interface ISecurityService
{
	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	RoleOld[] QueryRoles();

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	RoleOld QueryRoleById(string roleId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	RoleOld QueryRoleByName(string name);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	Identity[] QueryRoleMembers(string roleId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	Identity[] QueryRoleMembersExtended(string roleId, QueryMembership queryMembership, bool includeMappedExternalUsers);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	RoleOld CreateRole(string name, string description);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	RoleOld UpdateRole(string id, string name, string description);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	RoleOld CopyRole(string sourceRoleId, string name, string description);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void DeleteRole(string roleId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	Identity AddMemberToRole(string roleId, string memberSid);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void RemoveMemberFromRole(string roleId, string memberSid);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	Identity[] ReadIdentities(SearchFactor factor, string[] factorValue, QueryMembership queryMembership);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	Identity ReadIdentityFromSource(SearchFactor factor, string factorValue);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void RemoveIdentity(string userSid);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	string[] QueryObjectClasses();

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	string QueryObjectClass(string objectId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	SecurityAction[] QueryObjectClassActions(string classId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void CreateObjectClass(string objectClassId, string[] classActions);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	AccessControlEntry[] QueryAccessControlList(string objectId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	AccessControlEntry[] QueryAccessControlListByIdentity(string identity, string objectId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void RegisterSecurityObject(string objectId, string objectClassId, string parentObjectId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void UnregisterSecurityObject(string objectId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void AddAccessControlEntries(string objectId, AccessControlEntry[] entries);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void RemoveAccessControlEntries(string objectId, AccessControlEntry[] entries);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void ReplaceAccessControlEntries(string objectId, AccessControlEntry[] entries);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	AccessControlTimeProfileEntry QueryAccessControlTimeProfileEntry(string objectId, string identity, string actionId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	AccessControlTimeProfileEntry AddUpdateAccessControlTimeProfileEntry(string objectId, string identity, string actionId, string timeProfileId, bool isDefaultTimeProfile);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void DeleteAccessControlTimeProfile(string objectId, string identity, string actionId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void SetInheritance(string objectId, string parentObjectId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void CheckPermission(string objectId, string actionId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	bool IsMember(string identitySid, string groupSid);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	bool[] IsPermittedByIdentityList(string[] identityList, string actionId, string objectId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	bool[] IsPermittedByActionList(string identity, string[] actionIdList, string objectId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	bool[] IsPermittedByObjectList(string identity, string actionId, string[] objectIdList);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	bool IsPermittedTimeBasedAction(string actionId, string objectId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void RegisterObject(string namespaceId, string objectId, string parentObjectId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void UnregisterObject(string namespaceId, string objectId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	SecurityNamespaceInfo[] QuerySecurityNamespaces(string namespaceId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void CreateSecurityNamespace(SecurityNamespaceInfo description, string orderAfterNamespaceId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void UpdateSecurityNamespace(SecurityNamespaceInfo description);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void DeleteSecurityNamespace(string namespaceId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void CreateSecurityGroup(string namespaceId, string path, string name);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void RenameSecurityGroup(string namespaceId, string path, string newName);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void DeleteSecurityGroup(string namespaceId, string path, string moveItemsToPath);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void MoveSecurityGroup(string namespaceId, string sourcePath, string targetPath);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void MoveItems(string namespaceId, string[] sourcePath, string[] targetPath);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	AccessControlEntry[] QueryPermissions(string namespaceId, string securityToken, string identity);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void AddPermissions(string namespaceId, string objectId, AccessControlEntry[] entries);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void RemovePermissions(string namespaceId, string objectId, AccessControlEntry[] entries);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	void AddRemovePermissions(string namespaceId, string objectId, AccessControlEntry[] addEntries, AccessControlEntry[] removeEntries);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	bool[] HasPermissionByIdentityList(string namespaceId, string[] identityList, string actionId, string objectId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	bool[] HasPermissionByActionList(string namespaceId, string identity, string[] actionList, string objectId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	bool[] HasPermissionByActionListCurrentUser(string namespaceId, string[] actionList, string objectId);

	[OperationContract]
	[FaultContract(typeof(VMOExceptionDetail))]
	bool[] HasPermissionByObjectList(string namespaceId, string identity, string actionId, string[] objectList);
}
