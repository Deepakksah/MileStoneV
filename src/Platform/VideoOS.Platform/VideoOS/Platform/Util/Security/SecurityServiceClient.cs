using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VideoOS.Platform.Util.Security;

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

	public SecurityNamespaceInfo[] QuerySecurityNamespaces(string namespaceId)
	{
		return base.Channel.QuerySecurityNamespaces(namespaceId);
	}

	public void UpdateSecurityNamespace(SecurityNamespaceInfo description)
	{
		base.Channel.UpdateSecurityNamespace(description);
	}

	public Identity[] ReadIdentities(SearchFactor factor, string[] factorValue, QueryMembership queryMembership)
	{
		return base.Channel.ReadIdentities(factor, factorValue, queryMembership);
	}

	public Identity ReadIdentityFromSource(SearchFactor factor, string factorValue)
	{
		return base.Channel.ReadIdentityFromSource(factor, factorValue);
	}

	[Obsolete]
	public string[] QueryObjectClasses()
	{
		return base.Channel.QueryObjectClasses();
	}

	[Obsolete]
	public string QueryObjectClass(string objectId)
	{
		return base.Channel.QueryObjectClass(objectId);
	}

	[Obsolete]
	public SecurityAction[] QueryObjectClassActions(string classId)
	{
		return base.Channel.QueryObjectClassActions(classId);
	}

	public void CreateObjectClass(string objectClassId, string[] classActions)
	{
		base.Channel.CreateObjectClass(objectClassId, classActions);
	}

	[Obsolete]
	public AccessControlEntry[] QueryAccessControlList(string objectId)
	{
		return base.Channel.QueryAccessControlList(objectId);
	}

	[Obsolete]
	public AccessControlEntry[] QueryAccessControlListByIdentity(string identity, string objectId)
	{
		return base.Channel.QueryAccessControlListByIdentity(identity, objectId);
	}

	[Obsolete]
	public void RegisterSecurityObject(string objectId, string objectClassId, string parentObjectId)
	{
		base.Channel.RegisterSecurityObject(objectId, objectClassId, parentObjectId);
	}

	[Obsolete]
	public void UnregisterSecurityObject(string objectId)
	{
		base.Channel.UnregisterSecurityObject(objectId);
	}

	public void RegisterObject(string namespaceId, string objectId, string parentObjectId)
	{
		base.Channel.RegisterObject(namespaceId, objectId, parentObjectId);
	}

	public void UnregisterObject(string namespaceId, string objectId)
	{
		base.Channel.UnregisterObject(namespaceId, objectId);
	}

	[Obsolete]
	public void AddAccessControlEntries(string objectId, AccessControlEntry[] entries)
	{
		base.Channel.AddAccessControlEntries(objectId, entries);
	}

	[Obsolete]
	public void RemoveAccessControlEntries(string objectId, AccessControlEntry[] entries)
	{
		base.Channel.RemoveAccessControlEntries(objectId, entries);
	}

	[Obsolete]
	public void ReplaceAccessControlEntries(string objectId, AccessControlEntry[] entries)
	{
		base.Channel.ReplaceAccessControlEntries(objectId, entries);
	}

	[Obsolete]
	public void SetInheritance(string objectId, string parentObjectId)
	{
		base.Channel.SetInheritance(objectId, parentObjectId);
	}

	[Obsolete("CheckPermission is deprecated, please use one of the HasPermission methods instead.", false)]
	public void CheckPermission(string objectId, string actionId)
	{
		base.Channel.CheckPermission(objectId, actionId);
	}

	public bool IsMember(string identitySid, string groupSid)
	{
		return base.Channel.IsMember(identitySid, groupSid);
	}

	[Obsolete]
	public bool[] IsPermittedByIdentityList(string[] identityList, string actionId, string objectId)
	{
		return base.Channel.IsPermittedByIdentityList(identityList, actionId, objectId);
	}

	public void AddPermissions(string namespaceId, string objectId, AccessControlEntry[] entries)
	{
		base.Channel.AddPermissions(namespaceId, objectId, entries);
	}

	public void RemovePermissions(string namespaceId, string objectId, AccessControlEntry[] entries)
	{
		base.Channel.RemovePermissions(namespaceId, objectId, entries);
	}

	public bool[] HasPermissionByActionList(string namespaceId, string identity, string[] actionIdList, string objectId)
	{
		return base.Channel.HasPermissionByActionList(namespaceId, identity, actionIdList, objectId);
	}

	[Obsolete]
	public bool[] IsPermittedByObjectList(string identity, string actionId, string[] objectIdList)
	{
		return base.Channel.IsPermittedByObjectList(identity, actionId, objectIdList);
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
