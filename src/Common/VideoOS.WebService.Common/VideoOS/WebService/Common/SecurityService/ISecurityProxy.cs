using System;
using VideoOS.WebService.Common.Framework;

namespace VideoOS.WebService.Common.SecurityService;

public interface ISecurityProxy : ITimeoutSettings, IDisposable
{
	void AddAccessControlEntries(string objectId, AccessControlEntry[] entries);

	Identity AddMemberToRole(string roleId, string memberSid);

	void AddPermissions(string namespaceId, string securityToken, AccessControlEntry[] entries);

	void AddRemovePermissions(string namespaceId, string securityToken, AccessControlEntry[] addEntries, AccessControlEntry[] removeEntries);

	void CheckPermission(string objectId, string actionId);

	RoleInfo CopyRole(string sourceRoleId, string name, string description);

	RoleInfo CreateRole(string name, string description);

	void CreateSecurityNamespace(SecurityNamespaceInfo info, string orderAfterNamespaceId);

	AccessControlTimeProfileEntry CreateUpdateTimeBasedAccessEntry(string identity, string actionId, string objectId, string timeProfileId, bool isDefaultTimeProfile);

	void DeleteRole(string roleId);

	void DeleteSecurityNamespace(string namespaceId);

	void DeleteTimeBasedAccessEntry(string identity, string actionId, string objectId);

	bool[] HasPermissionByActionList(string namespaceId, string identity, string[] actionList, string objectId);

	bool[] HasPermissionByActionListCurrentUser(string namespaceId, string[] actionList, string objectId);

	bool[] HasPermissionByIdentityList(string namespaceId, string[] identityList, string actionId, string objectId);

	bool[] HasPermissionByObjectList(string namespaceId, string identity, string actionId, string[] objectList);

	bool[] IsPermittedByActionList(string identity, string[] actionIdList, string objectId);

	bool[] IsPermittedByIdentityList(string[] identityList, string actionId, string objectId);

	bool[] IsPermittedByObjectList(string identity, string actionId, string[] objectIdList);

	bool IsPermittedTimeBasedAction(string actionId, string objectId);

	AccessControlEntry[] QueryAccessControlList(string objectId);

	AccessControlEntry[] QueryAccessControlList(string identity, string objectId);

	SecurityAction[] QueryObjectClassActions(string classId);

	AccessControlEntry[] QueryPermissions(string namespaceId, string securityToken, string identity);

	RoleInfo QueryRoleById(string id);

	RoleInfo QueryRoleByName(string name);

	Identity[] QueryRoleMembers(string roleId);

	Identity[] QueryRoleMembersExtended(string roleId, QueryMembership queryMembership);

	RoleInfo[] QueryRoles();

	SecurityNamespaceInfo[] QuerySecurityNamespaces(string namespaceId);

	AccessControlTimeProfileEntry QueryTimeBasedAccessEntry(string identity, string actionId, string objectId);

	Identity[] ReadIdentities(SearchFactor searchFactor, string[] factorValues, QueryMembership queryMembership);

	Identity[] ReadIdentity(SearchFactor searchFactor, string factorValue, QueryMembership queryMembership);

	Identity ReadIdentityFromSource(SearchFactor searchFactor, string factorValue);

	void RemoveIdentity(string sid);

	void RegisterObject(string objectId, string classId);

	void RemoveAccessControlEntries(string objectId, AccessControlEntry[] entries);

	void RemoveMemberFromRole(string roleId, string memberSid);

	void RemovePermissions(string namespaceId, string securityToken, AccessControlEntry[] entries);

	void ReplaceAccessControlEntries(string objectId, AccessControlEntry[] entries);

	void SetInheritance(string objectId, string parentObjectId);

	void UnregisterObject(string objectId);

	RoleInfo UpdateRole(string id, string name, string description);

	void UpdateSecurityNamespace(SecurityNamespaceInfo info);
}
