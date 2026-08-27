using VideoOS.Management.Common;
using VideoOS.Management.VmoClient.Proxy;

namespace VideoOS.Management.VmoClient;

public interface ISecurity
{
	Identity ReadIdentity(SearchFactor searchFactor, string factoryValue, QueryMembership queryMembership);

	Identity[] ReadIdentities(SearchFactor searchFactor, string[] factoryValues, QueryMembership queryMembership);

	Identity ReadIdentityFromSource(SearchFactor searchFactor, string factorValue);

	void RemoveIdentity(string userSid);

	Identity[] QueryRoleMembers(string roleId);

	Identity[] QueryRoleMembersExtended(string roleId, QueryMembership queryMembership);

	Identity[] QueryRoleMembersExtended(string roleId, QueryMembership queryMembership, bool includeMappedExternalUsers);

	VMOWebserviceObject CopyRole(string sourceRoleId, string name, string description);

	Identity AddMemberToRole(string roleId, string memberSid);

	void RemoveMemberFromRole(string roleId, string memberSid);

	SecurityAction[] QueryObjectClassActions(string classId);

	void RegisterObject(string objectId, string classId);

	void UnregisterObject(string objectId);

	void CheckPermission(string objectId, string actionId);

	AccessControlEntry[] QueryAccessControlList(string objectId);

	AccessControlEntry[] QueryAccessControlList(string identity, string objectId);

	void AddAccessControlEntries(string objectId, AccessControlEntry[] entries);

	void RemoveAccessControlEntries(string objectId, AccessControlEntry[] entries);

	void ReplaceAccessControlEntries(string objectId, AccessControlEntry[] entries);

	void SetInheritance(string objectId, string parentObjectId);

	bool[] IsPermittedByIdentityList(string[] identityList, string actionId, string objectId);

	bool[] IsPermittedByActionList(string identity, string[] actionIdList, string objectId);

	bool[] IsPermittedByObjectList(string identity, string actionId, string[] objectIdList);

	bool IsPermittedTimeBasedAction(string actionId, string objectId);

	AccessControlTimeProfileEntry QueryTimeBasedAccessEntry(string identity, string actionId, string objectId);

	AccessControlTimeProfileEntry CreateUpdateTimeBasedAccessEntry(string identity, string actionId, string objectId, string timeProfileId, bool isDefaultTimeProfile);

	void DeleteTimeBasedAccessEntry(string identity, string actionId, string objectId);

	bool[] HasPermissionByIdentityList(string namespaceId, string[] identityList, string actionId, string objectId);

	bool[] HasPermissionByActionList(string namespaceId, string identity, string[] actionList, string objectId);

	bool[] HasPermissionByActionListCurrentUser(string namespaceId, string[] actionList, string objectId);

	bool[] HasPermissionByObjectList(string namespaceId, string identity, string actionId, string[] objectList);

	bool IsAdministrator();
}
