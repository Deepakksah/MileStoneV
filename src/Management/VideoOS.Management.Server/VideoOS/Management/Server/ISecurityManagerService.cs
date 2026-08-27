using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public interface ISecurityManagerService
{
	void CheckApplicationPermission(IGeminiRequestContext requestContext, string actionId);

	void CheckOverallPermission(IGeminiRequestContext requestContext, Guid namespaceId, string actionId);

	bool HasApplicationPermission(IGeminiRequestContext requestContext, string userSid, string actionId);

	bool HasApplicationPermission(IGeminiRequestContext requestContext, string actionId);

	void CheckObjectPermission(IGeminiRequestContext requestContext, Guid namespaceId, string actionId, string objectId);

	bool HasObjectPermission(IGeminiRequestContext requestContext, Guid namespaceId, string userSid, string actionId, string objectId);

	bool HasObjectPermission(IGeminiRequestContext requestContext, Guid namespaceId, string actionId, string objectId);

	bool HasObjectPermission(IGeminiRequestContext requestContext, Guid namespaceId, string actionId, string objectId, bool checkAdministrator);

	void CheckTimeBasedPermission(IGeminiRequestContext requestContext, Guid namespaceId, string actionId, string objectId);

	bool HasApplicationLoginPermission(IGeminiRequestContext requestContext, out DateTime evaluationTime, out string[] timeProfileIds);

	AccessControlEntry[] QueryPermissions(IGeminiRequestContext requestContext, Guid namespaceId, string securityToken);

	AccessControlEntry[] QueryPermissions(IGeminiRequestContext requestContext, Guid namespaceId, string securityToken, string identity);

	AccessControlEntry[] QueryPermissions(IGeminiRequestContext requestContext, Guid namespaceId, string objectId, string[] identities);

	bool AddPermissions(IGeminiRequestContext requestContext, Guid namespaceId, string securityToken, string objectName, AccessControlEntry[] entries);

	bool ReplacePermissions(IGeminiRequestContext requestContext, Guid namespaceId, string securityToken, string objectName, AccessControlEntry[] entries);

	bool RemovePermissions(IGeminiRequestContext requestContext, Guid namespaceId, string securityToken, string objectName, AccessControlEntry[] entries);

	bool AddRemovePermissions(IGeminiRequestContext requestContext, Guid namespaceId, string securityToken, string objectName, AccessControlEntry[] addEntries, AccessControlEntry[] removeEntries);

	void CreateSecurityNamespace(IGeminiRequestContext requestContext, SecurityNamespaceInfo description, string orderAfterNamespaceId);

	void UpdateSecurityNamespace(IGeminiRequestContext requestContext, SecurityNamespaceInfo description);

	void DeleteSecurityNamespace(IGeminiRequestContext requestContext, Guid namespaceId);

	void RegisterSecurityObject(IGeminiRequestContext requestContext, Guid namespaceId, string objectId, string parentObjectId);

	void UnregisterSecurityObject(IGeminiRequestContext requestContext, Guid namespaceId, string objectId);

	bool IsAdministrator(IGeminiRequestContext requestContext);

	void ForceReloadOfAllNamespaces(IGeminiRequestContext requestContext);

	IGeminiSecurityNamespace GetSecurityNamespace(IGeminiRequestContext requestContext, Guid namespaceId);

	IGeminiSecurityNamespace GetSecurityNamespaceFromToken(IGeminiRequestContext requestContext, string securityToken);

	List<AccessControlDeviceTimeProfileEntry> QueryDeviceTimeProfile(IGeminiRequestContext requestContext, string userId, string deviceId);

	void UpdateDeviceTimeProfilePermissions(IGeminiRequestContext requestContext, Guid namespaceId, List<(AccessControlDeviceTimeProfileEntry, string)> entries);

	List<Tuple<AccessControlEntry, AccessControlTimeProfile>> QueryAccessEntryAndTimeProfile(IGeminiRequestContext requestContext, Guid namespaceId, string objectId, string roleId);

	void RemoveIdentity(IGeminiRequestContext requestContext, string userSid);
}
