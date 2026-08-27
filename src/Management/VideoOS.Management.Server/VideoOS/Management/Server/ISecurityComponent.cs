using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public interface ISecurityComponent : IDisposable
{
	bool RegisterObject(Guid namespaceId, string objectId, string parentObjectId);

	bool RegisterObjectWithName(Guid namespaceId, string objectId, string parentObjectId, string objectName);

	bool UnregisterObject(Guid namespaceId, string objectId);

	ResultCollection QueryPrivateViewGroupTokens();

	string[] QuerySecurityObjects(Guid namespaceId, bool userDefined);

	List<Tuple<string, string>> QuerySecurityObjectMaps(Guid namespaceId);

	ResultCollection QuerySecurityNamespaces(string namespaceId);

	ResultCollection QuerySecurityAces(string namespaceId, bool getInheritanceInformation);

	List<AccessControlDeviceTimeProfileEntry> QueryDeviceTimeProfile(string roleId, string objectId);

	void UpdateDeviceTimeProfile(AccessControlDeviceTimeProfileEntry entry, string deviceName, Guid sessionId);

	void CreateSecurityNamespace(SecurityNamespaceInfo description, string orderAfterNamespaceId);

	void UpdateSecurityNamespace(SecurityNamespaceInfo description);

	void DeleteSecurityNamespace(Guid namespaceId);

	ResultCollection QueryPermissions(Guid namespaceId, string securityToken);

	bool AddPermission(Guid namespaceId, string securityToken, string objectName, AccessControlEntry[] entries, bool replace, Guid sessionId);

	bool RemovePermissions(Guid namespaceId, string securityToken, string objectName, AccessControlEntry[] entries, Guid sessionId);

	bool AddMatrixPermission(string securityToken, string objectName, AccessControlEntry[] entries, bool replace, Guid sessionId);

	bool RemoveMatrixPermissions(string securityToken, string objectName, AccessControlEntry[] entries, Guid sessionId);

	bool QueryMatrixPermission(string securityToken, string objectName);

	void SetInheritance(Guid namespaceId, string securityToken, string parentObjectId);

	bool HasPermission(Guid namespaceId, string userSid, string actionId, string securityToken, string legacySecurityArea, string legacySecurityAttributeId);

	string GetSecurityNamespaceIdFromToken(string securityToken);

	bool TimeBasedSecurityIsPermitted(Guid namespaceId, string objectId, string actionId, AccessControlEntry[] accessControlEntry);

	bool TimeBasedSecurityIsPermitted(Guid namespaceId, string objectId, string roleId, string actionId, DateTime evaluationTime, out string timeProfileId);

	AccessControlTimeProfileEntry QueryAccessTimeProfileEntry(Guid namespaceId, string objectId, string identity, string actionId);

	void CreateUpdateAccessTimeProfileEntry(Guid namespaceId, string objectId, string objectName, string identitySid, string actionId, string timeProfileId, bool isDefaultTimeProfile, Guid sessionId);

	void DeleteAccessTimeProfileEntry(Guid namespaceId, string objectId, string objectName, string identitySid, string actionId, Guid sessionId);

	AccessControlTimeProfile QueryAccessTimeProfileEntryIsPermitted(Guid namespaceId, string objectId, string userSid, string actionId);

	void RemoveUser(string sid);

	RolePermissionObjectEntry[] QueryRolePermissionsForObjects(Guid namespaceId, string roleId, HashSet<Guid> objectIds);

	RolePermissionObjectEntry[] QueryPermissionsForObject(Guid namespaceId, Guid objectId);

	List<AccessControlDeviceTimeProfileEntry> QueryTimeProfiles(string roleId, HashSet<Guid> objectIds);

	List<AccessControlDeviceTimeProfileEntry> QueryTimeProfilesAllRoles(Guid objectId);

	List<AccessControlTimeProfileObjectEntry> QueryNonDeviceTimeProfiles(Guid namespaceId, string roleId, HashSet<Guid> objectIds);

	List<AccessControlTimeProfileObjectEntry> QueryNonDeviceTimeProfilesAllRoles(Guid namespaceId, Guid objectId);

	List<string> VerifySecurityObjects(Guid namespaceId, HashSet<Guid> objectIds);
}
