using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.WebService.Common.Framework;

namespace VideoOS.WebService.Common.SecurityService;

public class SecurityProxy : WebServiceManagerBase<SecurityServiceClient, ISecurityService>, ISecurityProxy, ITimeoutSettings, IDisposable
{
	public static ISecurityProxy CreateNew(string hostName, int port, bool runSecure, ITokenCache tokenCache, IWebServiceManagerExceptionHandler exceptionHandler = null)
	{
		return new SecurityProxy(hostName, port, runSecure, tokenCache, exceptionHandler);
	}

	private SecurityProxy(string hostName, int port, bool runSecure, ITokenCache tokenCache, IWebServiceManagerExceptionHandler exceptionHandler)
		: base(new UriBuilder(runSecure ? Uri.UriSchemeHttps : Uri.UriSchemeHttp, hostName, port, "ManagementServer/SecurityServiceOAuth.svc").Uri, "http://schemas.videoos/SecurityService/2008/05/Vmo/01", tokenCache, exceptionHandler)
	{
	}

	public static ISecurityProxy CreateNew(string hostName, int port, IWebServiceManagerExceptionHandler exceptionHandler = null)
	{
		return new SecurityProxy(hostName, port, exceptionHandler);
	}

	public static ISecurityProxy CreateNew(string hostName, int port, string userName, string password, string domain, IWebServiceManagerExceptionHandler exceptionHandler = null)
	{
		return new SecurityProxy(hostName, port, userName, password, domain, exceptionHandler);
	}

	public static ISecurityProxy CreateNew(string hostName, int securePort, string userName, string password, IWebServiceManagerExceptionHandler exceptionHandler = null)
	{
		return new SecurityProxy(hostName, securePort, userName, password, exceptionHandler);
	}

	private SecurityProxy(string hostName, int port, IWebServiceManagerExceptionHandler exceptionHandler)
		: base(new UriBuilder(Uri.UriSchemeHttp, hostName, port, "ManagementServer/SecurityService.svc").Uri, "http://schemas.videoos/SecurityService/2008/05/Vmo/01", exceptionHandler)
	{
	}

	private SecurityProxy(string hostName, int port, string userName, string password, string domain, IWebServiceManagerExceptionHandler exceptionHandler)
		: base(new UriBuilder(Uri.UriSchemeHttp, hostName, port, "ManagementServer/SecurityService.svc").Uri, "http://schemas.videoos/SecurityService/2008/05/Vmo/01", userName, password, domain, exceptionHandler)
	{
	}

	private SecurityProxy(string hostName, int securePort, string userName, string password, IWebServiceManagerExceptionHandler exceptionHandler)
		: base(new UriBuilder(Uri.UriSchemeHttps, hostName, securePort, "ManagementServer/SecurityService.svc").Uri, "http://schemas.videoos/SecurityService/2008/05/Vmo/01", userName, password, exceptionHandler)
	{
	}

	protected override SecurityServiceClient CreateClientObject(Binding binding, EndpointAddress remoteAddress)
	{
		return new SecurityServiceClient(binding, remoteAddress);
	}

	public RoleInfo[] QueryRoles()
	{
		return CallWithRetry((ISecurityService c) => c.QueryRoles());
	}

	public RoleInfo QueryRoleById(string id)
	{
		return CallWithRetry((ISecurityService c) => c.QueryRoleById(id));
	}

	public RoleInfo QueryRoleByName(string name)
	{
		return CallWithRetry((ISecurityService c) => c.QueryRoleByName(name));
	}

	public Identity[] QueryRoleMembers(string roleId)
	{
		return CallWithRetry((ISecurityService c) => c.QueryRoleMembers(roleId));
	}

	public Identity[] QueryRoleMembersExtended(string roleId, QueryMembership queryMembership)
	{
		return CallWithRetry((ISecurityService c) => c.QueryRoleMembersExtended(roleId, queryMembership));
	}

	public Identity AddMemberToRole(string roleId, string memberSid)
	{
		return CallWithRetry((ISecurityService c) => c.AddMemberToRole(roleId, memberSid));
	}

	public void RemoveMemberFromRole(string roleId, string memberSid)
	{
		CallWithRetry(delegate(ISecurityService c)
		{
			c.RemoveMemberFromRole(roleId, memberSid);
		});
	}

	public RoleInfo CreateRole(string name, string description)
	{
		return CallWithRetry((ISecurityService c) => c.CreateRole(name, description));
	}

	public RoleInfo UpdateRole(string id, string name, string description)
	{
		return CallWithRetry((ISecurityService c) => c.UpdateRole(id, name, description));
	}

	public RoleInfo CopyRole(string sourceRoleId, string name, string description)
	{
		return CallWithRetry((ISecurityService c) => c.CopyRole(sourceRoleId, name, description));
	}

	public void DeleteRole(string roleId)
	{
		CallWithRetry(delegate(ISecurityService c)
		{
			c.DeleteRole(roleId);
		});
	}

	public SecurityNamespaceInfo[] QuerySecurityNamespaces(string namespaceId)
	{
		return CallWithRetry((ISecurityService c) => c.QuerySecurityNamespaces(namespaceId));
	}

	public AccessControlEntry[] QueryPermissions(string namespaceId, string securityToken, string identity)
	{
		return CallWithRetry((ISecurityService c) => c.QueryPermissions(namespaceId, securityToken, identity));
	}

	public void AddPermissions(string namespaceId, string securityToken, AccessControlEntry[] entries)
	{
		CallWithRetry(delegate(ISecurityService c)
		{
			c.AddPermissions(namespaceId, securityToken, entries);
		});
	}

	public void RemovePermissions(string namespaceId, string securityToken, AccessControlEntry[] entries)
	{
		CallWithRetry(delegate(ISecurityService c)
		{
			c.RemovePermissions(namespaceId, securityToken, entries);
		});
	}

	public void AddRemovePermissions(string namespaceId, string securityToken, AccessControlEntry[] addEntries, AccessControlEntry[] removeEntries)
	{
		CallWithRetry(delegate(ISecurityService c)
		{
			c.AddRemovePermissions(namespaceId, securityToken, addEntries, removeEntries);
		});
	}

	public Identity[] ReadIdentity(SearchFactor searchFactor, string factorValue, QueryMembership queryMembership)
	{
		return CallWithRetry((ISecurityService c) => c.ReadIdentities(searchFactor, new string[1] { factorValue }, queryMembership));
	}

	public Identity[] ReadIdentities(SearchFactor searchFactor, string[] factorValues, QueryMembership queryMembership)
	{
		return CallWithRetry((ISecurityService c) => c.ReadIdentities(searchFactor, factorValues, queryMembership));
	}

	public Identity ReadIdentityFromSource(SearchFactor searchFactor, string factorValue)
	{
		return CallWithRetry((ISecurityService c) => c.ReadIdentityFromSource(searchFactor, factorValue));
	}

	public void RemoveIdentity(string userSid)
	{
		CallWithRetry(delegate(ISecurityService c)
		{
			c.RemoveIdentity(userSid);
		});
	}

	public SecurityAction[] QueryObjectClassActions(string classId)
	{
		return CallWithRetry((ISecurityService c) => c.QueryObjectClassActions(classId));
	}

	public void RegisterObject(string objectId, string classId)
	{
		CallWithRetry(delegate(ISecurityService c)
		{
			c.RegisterSecurityObject(objectId, classId, null);
		});
	}

	public void UnregisterObject(string objectId)
	{
		CallWithRetry(delegate(ISecurityService c)
		{
			c.UnregisterSecurityObject(objectId);
		});
	}

	public void CheckPermission(string objectId, string actionId)
	{
		CallWithRetry(delegate(ISecurityService c)
		{
			c.CheckPermission(objectId, actionId);
		});
	}

	public AccessControlEntry[] QueryAccessControlList(string objectId)
	{
		return CallWithRetry((ISecurityService c) => c.QueryAccessControlList(objectId));
	}

	public AccessControlEntry[] QueryAccessControlList(string identity, string objectId)
	{
		return CallWithRetry((ISecurityService c) => c.QueryAccessControlListByIdentity(identity, objectId));
	}

	public void AddAccessControlEntries(string objectId, AccessControlEntry[] entries)
	{
		CallWithRetry(delegate(ISecurityService c)
		{
			c.AddAccessControlEntries(objectId, entries);
		});
	}

	public void CreateSecurityNamespace(SecurityNamespaceInfo info, string orderAfterNamespaceId)
	{
		CallWithRetry(delegate(ISecurityService c)
		{
			c.CreateSecurityNamespace(info, orderAfterNamespaceId);
		});
	}

	public void UpdateSecurityNamespace(SecurityNamespaceInfo info)
	{
		CallWithRetry(delegate(ISecurityService c)
		{
			c.UpdateSecurityNamespace(info);
		});
	}

	public void DeleteSecurityNamespace(string namespaceId)
	{
		CallWithRetry(delegate(ISecurityService c)
		{
			c.DeleteSecurityNamespace(namespaceId);
		});
	}

	public void RemoveAccessControlEntries(string objectId, AccessControlEntry[] entries)
	{
		CallWithRetry(delegate(ISecurityService c)
		{
			c.RemoveAccessControlEntries(objectId, entries);
		});
	}

	public void ReplaceAccessControlEntries(string objectId, AccessControlEntry[] entries)
	{
		CallWithRetry(delegate(ISecurityService c)
		{
			c.ReplaceAccessControlEntries(objectId, entries);
		});
	}

	public void SetInheritance(string objectId, string parentObjectId)
	{
		CallWithRetry(delegate(ISecurityService c)
		{
			c.SetInheritance(objectId, parentObjectId);
		});
	}

	public bool[] IsPermittedByIdentityList(string[] identityList, string actionId, string objectId)
	{
		return CallWithRetry((ISecurityService c) => c.IsPermittedByIdentityList(identityList, actionId, objectId));
	}

	public bool[] IsPermittedByActionList(string identity, string[] actionIdList, string objectId)
	{
		return CallWithRetry((ISecurityService c) => c.IsPermittedByActionList(identity, actionIdList, objectId));
	}

	public bool[] IsPermittedByObjectList(string identity, string actionId, string[] objectIdList)
	{
		return CallWithRetry((ISecurityService c) => c.IsPermittedByObjectList(identity, actionId, objectIdList));
	}

	public bool IsPermittedTimeBasedAction(string actionId, string objectId)
	{
		return CallWithRetry((ISecurityService c) => c.IsPermittedTimeBasedAction(actionId, objectId));
	}

	public AccessControlTimeProfileEntry QueryTimeBasedAccessEntry(string identity, string actionId, string objectId)
	{
		return CallWithRetry((ISecurityService c) => c.QueryAccessControlTimeProfileEntry(identity, actionId, objectId));
	}

	public AccessControlTimeProfileEntry CreateUpdateTimeBasedAccessEntry(string identity, string actionId, string objectId, string timeProfileId, bool isDefaultTimeProfile)
	{
		return CallWithRetry((ISecurityService c) => c.AddUpdateAccessControlTimeProfileEntry(identity, actionId, objectId, timeProfileId, isDefaultTimeProfile));
	}

	public void DeleteTimeBasedAccessEntry(string identity, string actionId, string objectId)
	{
		CallWithRetry(delegate(ISecurityService c)
		{
			c.DeleteAccessControlTimeProfile(identity, actionId, objectId);
		});
	}

	public bool[] HasPermissionByIdentityList(string namespaceId, string[] identityList, string actionId, string objectId)
	{
		return CallWithRetry((ISecurityService c) => c.HasPermissionByIdentityList(namespaceId, identityList, actionId, objectId));
	}

	public bool[] HasPermissionByActionList(string namespaceId, string identity, string[] actionList, string objectId)
	{
		return CallWithRetry((ISecurityService c) => c.HasPermissionByActionList(namespaceId, identity, actionList, objectId));
	}

	public bool[] HasPermissionByActionListCurrentUser(string namespaceId, string[] actionList, string objectId)
	{
		return CallWithRetry((ISecurityService c) => c.HasPermissionByActionListCurrentUser(namespaceId, actionList, objectId));
	}

	public bool[] HasPermissionByObjectList(string namespaceId, string identity, string actionId, string[] objectList)
	{
		return CallWithRetry((ISecurityService c) => c.HasPermissionByObjectList(namespaceId, identity, actionId, objectList));
	}
}
