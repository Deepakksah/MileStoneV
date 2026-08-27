using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient.Proxy;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class SecurityServiceClient : ClientBase<ISecurityService>, ISecurityService
{
	public SecurityServiceClient()
	{
	}

	public SecurityServiceClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public SecurityServiceClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public SecurityServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public SecurityServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public RoleInfo[] QueryRoles()
	{
		return base.Channel.QueryRoles();
	}

	public RoleInfo QueryRoleById(string roleId)
	{
		return base.Channel.QueryRoleById(roleId);
	}

	public RoleInfo QueryRoleByName(string name)
	{
		return base.Channel.QueryRoleByName(name);
	}

	public Identity[] QueryRoleMembers(string roleId)
	{
		return base.Channel.QueryRoleMembers(roleId);
	}

	public Identity[] QueryRoleMembersExtended(string roleId, QueryMembership queryMembership, bool includeMappedExternalUsers)
	{
		return base.Channel.QueryRoleMembersExtended(roleId, queryMembership, includeMappedExternalUsers);
	}

	public RoleInfo CreateRole(string name, string description)
	{
		return base.Channel.CreateRole(name, description);
	}

	public RoleInfo UpdateRole(string id, string name, string description)
	{
		return base.Channel.UpdateRole(id, name, description);
	}

	public RoleInfo CopyRole(string sourceRoleId, string name, string description)
	{
		return base.Channel.CopyRole(sourceRoleId, name, description);
	}

	public void DeleteRole(string roleId)
	{
		base.Channel.DeleteRole(roleId);
	}

	public Identity AddMemberToRole(string roleId, string memberSid)
	{
		return base.Channel.AddMemberToRole(roleId, memberSid);
	}

	public void RemoveMemberFromRole(string roleId, string memberSid)
	{
		base.Channel.RemoveMemberFromRole(roleId, memberSid);
	}

	public Identity[] ReadIdentities(SearchFactor factor, string[] factorValue, QueryMembership queryMembership)
	{
		return base.Channel.ReadIdentities(factor, factorValue, queryMembership);
	}

	public Identity ReadIdentityFromSource(SearchFactor factor, string factorValue)
	{
		return base.Channel.ReadIdentityFromSource(factor, factorValue);
	}

	public void RemoveIdentity(string userSid)
	{
		base.Channel.RemoveIdentity(userSid);
	}

	public string[] QueryObjectClasses()
	{
		return base.Channel.QueryObjectClasses();
	}

	public string QueryObjectClass(string objectId)
	{
		return base.Channel.QueryObjectClass(objectId);
	}

	public SecurityAction[] QueryObjectClassActions(string classId)
	{
		return base.Channel.QueryObjectClassActions(classId);
	}

	public void CreateObjectClass(string objectClassId, string[] classActions)
	{
		base.Channel.CreateObjectClass(objectClassId, classActions);
	}

	public AccessControlEntry[] QueryAccessControlList(string objectId)
	{
		return base.Channel.QueryAccessControlList(objectId);
	}

	public AccessControlEntry[] QueryAccessControlListByIdentity(string identity, string objectId)
	{
		return base.Channel.QueryAccessControlListByIdentity(identity, objectId);
	}

	public void RegisterSecurityObject(string objectId, string objectClassId, string parentObjectId)
	{
		base.Channel.RegisterSecurityObject(objectId, objectClassId, parentObjectId);
	}

	public void UnregisterSecurityObject(string objectId)
	{
		base.Channel.UnregisterSecurityObject(objectId);
	}

	public void AddAccessControlEntries(string objectId, AccessControlEntry[] entries)
	{
		base.Channel.AddAccessControlEntries(objectId, entries);
	}

	public void RemoveAccessControlEntries(string objectId, AccessControlEntry[] entries)
	{
		base.Channel.RemoveAccessControlEntries(objectId, entries);
	}

	public void ReplaceAccessControlEntries(string objectId, AccessControlEntry[] entries)
	{
		base.Channel.ReplaceAccessControlEntries(objectId, entries);
	}

	public AccessControlTimeProfileEntry QueryAccessControlTimeProfileEntry(string objectId, string identity, string actionId)
	{
		return base.Channel.QueryAccessControlTimeProfileEntry(objectId, identity, actionId);
	}

	public AccessControlTimeProfileEntry AddUpdateAccessControlTimeProfileEntry(string objectId, string identity, string actionId, string timeProfileId, bool isDefaultTimeProfile)
	{
		return base.Channel.AddUpdateAccessControlTimeProfileEntry(objectId, identity, actionId, timeProfileId, isDefaultTimeProfile);
	}

	public void DeleteAccessControlTimeProfile(string objectId, string identity, string actionId)
	{
		base.Channel.DeleteAccessControlTimeProfile(objectId, identity, actionId);
	}

	public void SetInheritance(string objectId, string parentObjectId)
	{
		base.Channel.SetInheritance(objectId, parentObjectId);
	}

	public void CheckPermission(string objectId, string actionId)
	{
		base.Channel.CheckPermission(objectId, actionId);
	}

	public bool IsMember(string identitySid, string groupSid)
	{
		return base.Channel.IsMember(identitySid, groupSid);
	}

	public bool[] IsPermittedByIdentityList(string[] identityList, string actionId, string objectId)
	{
		return base.Channel.IsPermittedByIdentityList(identityList, actionId, objectId);
	}

	public bool[] IsPermittedByActionList(string identity, string[] actionIdList, string objectId)
	{
		return base.Channel.IsPermittedByActionList(identity, actionIdList, objectId);
	}

	public bool[] IsPermittedByObjectList(string identity, string actionId, string[] objectIdList)
	{
		return base.Channel.IsPermittedByObjectList(identity, actionId, objectIdList);
	}

	public bool IsPermittedTimeBasedAction(string actionId, string objectId)
	{
		return base.Channel.IsPermittedTimeBasedAction(actionId, objectId);
	}

	public void RegisterObject(string namespaceId, string objectId, string parentObjectId)
	{
		base.Channel.RegisterObject(namespaceId, objectId, parentObjectId);
	}

	public void UnregisterObject(string namespaceId, string objectId)
	{
		base.Channel.UnregisterObject(namespaceId, objectId);
	}

	public SecurityNamespaceInfo[] QuerySecurityNamespaces(string namespaceId)
	{
		return base.Channel.QuerySecurityNamespaces(namespaceId);
	}

	public void CreateSecurityNamespace(SecurityNamespaceInfo description, string orderAfterNamespaceId)
	{
		base.Channel.CreateSecurityNamespace(description, orderAfterNamespaceId);
	}

	public void UpdateSecurityNamespace(SecurityNamespaceInfo description)
	{
		base.Channel.UpdateSecurityNamespace(description);
	}

	public void DeleteSecurityNamespace(string namespaceId)
	{
		base.Channel.DeleteSecurityNamespace(namespaceId);
	}

	public void CreateSecurityGroup(string namespaceId, string path, string name)
	{
		base.Channel.CreateSecurityGroup(namespaceId, path, name);
	}

	public void RenameSecurityGroup(string namespaceId, string path, string newName)
	{
		base.Channel.RenameSecurityGroup(namespaceId, path, newName);
	}

	public void DeleteSecurityGroup(string namespaceId, string path, string moveItemsToPath)
	{
		base.Channel.DeleteSecurityGroup(namespaceId, path, moveItemsToPath);
	}

	public void MoveSecurityGroup(string namespaceId, string sourcePath, string targetPath)
	{
		base.Channel.MoveSecurityGroup(namespaceId, sourcePath, targetPath);
	}

	public void MoveItems(string namespaceId, string[] sourcePath, string[] targetPath)
	{
		base.Channel.MoveItems(namespaceId, sourcePath, targetPath);
	}

	public AccessControlEntry[] QueryPermissions(string namespaceId, string securityToken, string identity)
	{
		return base.Channel.QueryPermissions(namespaceId, securityToken, identity);
	}

	public void AddPermissions(string namespaceId, string objectId, AccessControlEntry[] entries)
	{
		base.Channel.AddPermissions(namespaceId, objectId, entries);
	}

	public void RemovePermissions(string namespaceId, string objectId, AccessControlEntry[] entries)
	{
		base.Channel.RemovePermissions(namespaceId, objectId, entries);
	}

	public void AddRemovePermissions(string namespaceId, string objectId, AccessControlEntry[] addEntries, AccessControlEntry[] removeEntries)
	{
		base.Channel.AddRemovePermissions(namespaceId, objectId, addEntries, removeEntries);
	}

	public bool[] HasPermissionByIdentityList(string namespaceId, string[] identityList, string actionId, string objectId)
	{
		return base.Channel.HasPermissionByIdentityList(namespaceId, identityList, actionId, objectId);
	}

	public bool[] HasPermissionByActionList(string namespaceId, string identity, string[] actionList, string objectId)
	{
		return base.Channel.HasPermissionByActionList(namespaceId, identity, actionList, objectId);
	}

	public bool[] HasPermissionByActionListCurrentUser(string namespaceId, string[] actionList, string objectId)
	{
		return base.Channel.HasPermissionByActionListCurrentUser(namespaceId, actionList, objectId);
	}

	public bool[] HasPermissionByObjectList(string namespaceId, string identity, string actionId, string[] objectList)
	{
		return base.Channel.HasPermissionByObjectList(namespaceId, identity, actionId, objectList);
	}
}
