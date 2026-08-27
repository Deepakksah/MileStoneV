using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

public class SecuritySection : VmoObjectSecurity
{
	private readonly ManagementServer _managementServer;

	public LoginProviderCollection LoginProviders { get; }

	public RoleCollection Roles { get; }

	public BasicUserCollection BasicUsers { get; }

	public SecuritySection(ManagementServer managementServer)
		: base(managementServer.Repositories, managementServer.InternalId)
	{
		_managementServer = managementServer;
		LoginProviders = new LoginProviderCollection(LoadLoginProviders, managementServer, isInitialized: false);
		Roles = new RoleCollection(LoadRoles, managementServer, isInitialized: false);
		BasicUsers = new BasicUserCollection(LoadBasicUsers, managementServer, isInitialized: false);
	}

	public IdpClient GetVmsWebClient()
	{
		_managementServer.ThrowIfCheckVersionFail(ServerVersions.v22_3);
		VMOWebserviceObject[] array = base.Repositories.ServerCommunication.Execute("GetVmsWebClient", new VmoPropertyCollection());
		if (array == null || !array.Any())
		{
			return null;
		}
		return new IdpClient(array[0].Properties.GetPropertyValue<string>("Id"), array[0].Properties, null, null, isInitialized: true);
	}

	public void UpdateVmsWebClient(IEnumerable<string> uris)
	{
		_managementServer.ThrowIfCheckVersionFail(ServerVersions.v22_3);
		if (uris == null)
		{
			throw new InvalidArgumentException("uris");
		}
		VmoPropertyCollection vmoPropertyCollection = new VmoPropertyCollection();
		VmoPropertyCollection vmoPropertyCollection2 = VmoPropertyCollection.FromKeyAndValueArray(uris.Select((string el, int ix) => new KeyAndValue(ix.ToString(CultureInfo.InvariantCulture), el)).ToArray());
		vmoPropertyCollection.SetNestedPropertyCollection("ClientRedirectUris", vmoPropertyCollection2);
		base.Repositories.ServerCommunication.Execute("UpdateVmsWebClient", vmoPropertyCollection2);
	}

	private LoginProvider[] LoadLoginProviders(bool refreshCachedObjects)
	{
		_managementServer.ThrowIfCheckVersionFail(ServerVersions.v22_1);
		return base.Repositories.GetObjectsByParentId<ManagementServer, LoginProvider>(_managementServer.Id, refreshCachedObjects);
	}

	private Role[] LoadRoles(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<ManagementServer, Role>(_managementServer.Id, refreshCachedObjects);
	}

	private BasicUser[] LoadBasicUsers(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<ManagementServer, BasicUser>(_managementServer.Id, refreshCachedObjects);
	}

	public ManagementServerPermissionSet EffectivePermissions(string identity)
	{
		return EffectivePermissions(identity, ManagementServerPermissionSet.SupportedPermissions(base.Repositories.ServerVersion));
	}

	public ManagementServerPermissionSet EffectivePermissions(string identity, ManagementServerPermissionSet permissions)
	{
		return base.EffectivePermissions(identity, permissions);
	}

	public ManagementServerPermissionInfo[] EnumPermissions()
	{
		return EnumPermissions<ManagementServerPermissionInfo>();
	}

	public ManagementServerPermissionInfo[] EnumPermissions(ManagementServerPermissionSet permissions)
	{
		return EnumPermissions<ManagementServerPermissionInfo>(permissions);
	}

	public ManagementServerPermissionInfo[] EnumPermissions(string identity)
	{
		return EnumPermissions<ManagementServerPermissionInfo>(identity, ManagementServerPermissionSet.SupportedPermissions(base.Repositories.ServerVersion));
	}

	public ManagementServerPermissionInfo[] EnumPermissions(string identity, ManagementServerPermissionSet permissions)
	{
		return EnumPermissions<ManagementServerPermissionInfo>(identity, permissions);
	}

	public void Allow(ManagementServerPermissionSet permission, string identity)
	{
		Allow((PermissionSetBase)permission, new string[1] { identity });
	}

	public void Allow(ManagementServerPermissionSet permission, string[] identities)
	{
		Allow((PermissionSetBase)permission, identities);
	}

	public void Deny(ManagementServerPermissionSet permission, string identity)
	{
		Deny(permission, new string[1] { identity });
	}

	public void Revoke(ManagementServerPermissionSet permission, string identity)
	{
		Revoke((PermissionSetBase)permission, new string[1] { identity });
	}

	public void Revoke(ManagementServerPermissionSet permission, string[] identities)
	{
		Revoke((PermissionSetBase)permission, identities);
	}

	protected override PermissionSetBase PermissionSetFromActionId(string actionId)
	{
		return (ManagementServerPermissionSet)ManagementServerPermissionSet.CreateFromActionId(actionId);
	}

	public bool IsPermittedTimeBasedAction(string action, string objectId)
	{
		return base.Repositories.ServerCommunication.IsPermittedTimeBasedAction(action, objectId);
	}

	public bool ClientLogOnSupported(ClientType clientType)
	{
		if (base.Repositories.CheckVersion(ServerVersions.v21_1))
		{
			VmoPropertyCollection vmoPropertyCollection = new VmoPropertyCollection();
			vmoPropertyCollection.Add("ClientApplicationType", clientType.ToString());
			return base.Repositories.ServerCommunication.Execute("ClientLogOnAllowedCurrentUser", vmoPropertyCollection)[0].Properties.GetPropertyValue<bool>("ClientLogOnAllowed");
		}
		return ClientLogOnSupported(clientType, base.Repositories.ConnectedUser);
	}

	public bool ClientLogOnSupported(ClientType clientType, BasicUser basicUser)
	{
		if (basicUser == null)
		{
			throw new ArgumentNullException("basicUser");
		}
		return ClientLogOnSupported(clientType, basicUser.DomainAccountName);
	}

	public bool ClientLogOnSupported(ClientType clientType, string userAccountName)
	{
		Identity identity = ReadIdentity(SearchFactor.AccountName, userAccountName, QueryMembership.None);
		if (identity == null)
		{
			identity = ReadIdentityFromSource(SearchFactor.AccountName, userAccountName);
		}
		if (identity == null)
		{
			throw new UserNotExistsException();
		}
		VmoPropertyCollection vmoPropertyCollection = new VmoPropertyCollection();
		vmoPropertyCollection.Add("ClientApplicationType", clientType.ToString());
		vmoPropertyCollection.Add("DomainAccountName", userAccountName);
		return base.Repositories.ServerCommunication.Execute("ClientLogOnAllowed", vmoPropertyCollection)[0].Properties.GetPropertyValue<bool>("ClientLogOnAllowed");
	}

	public Identity ReadIdentityFromSource(SearchFactor searchFactor, string userName)
	{
		return base.Repositories.ServerCommunication.ReadIdentityFromSource(searchFactor, userName);
	}

	public Identity[] ReadIdentities(SearchFactor searchFactor, string[] userNames, QueryMembership queryMembership)
	{
		return base.Repositories.ServerCommunication.ReadIdentities(searchFactor, userNames, queryMembership);
	}

	public Identity ReadIdentity(SearchFactor searchFactor, string factorValue, QueryMembership queryMembership)
	{
		return base.Repositories.ServerCommunication.ReadIdentity(searchFactor, factorValue, queryMembership);
	}

	public void RemoveIdentity(string userSid)
	{
		base.Repositories.ServerCommunication.RemoveIdentity(userSid);
	}

	public bool HasOverallPermission(Guid namespaceId, string actionId)
	{
		if (actionId == null)
		{
			throw new ArgumentNullException("actionId");
		}
		return HasPermissionByActionListCurrentUser(namespaceId.ToString(), new string[1] { actionId }, "/").First();
	}

	public void RegisterObject(string objectId, string classId)
	{
		base.Repositories.ServerCommunication.RegisterObject(objectId, classId);
	}

	public void UnregisterObject(string objectId)
	{
		base.Repositories.ServerCommunication.UnregisterObject(objectId);
	}

	public void CheckPermission(string objectId, string actionId)
	{
		base.Repositories.ServerCommunication.CheckPermission(objectId, actionId);
	}

	public bool IsPermitted(string identitySid, string action, string objectId)
	{
		return base.Repositories.ServerCommunication.IsPermittedByActionList(identitySid, new string[1] { action.ToString() }, objectId)[0];
	}

	public bool[] HasPermissionByIdentityList(string namespaceId, string[] identityList, string actionId, string objectId)
	{
		return base.Repositories.ServerCommunication.HasPermissionByIdentityList(namespaceId, identityList, actionId, objectId);
	}

	public bool[] HasPermissionByActionList(string namespaceId, string identity, string[] actionList, string objectId)
	{
		return base.Repositories.ServerCommunication.HasPermissionByActionList(namespaceId, identity, actionList, objectId);
	}

	public bool[] HasPermissionByActionListCurrentUser(string namespaceId, string[] actionList, string objectId)
	{
		return base.Repositories.ServerCommunication.HasPermissionByActionListCurrentUser(namespaceId, actionList, objectId);
	}

	public bool[] HasPermissionByObjectList(string namespaceId, string identity, string actionId, string[] objectList)
	{
		return base.Repositories.ServerCommunication.HasPermissionByObjectList(namespaceId, identity, actionId, objectList);
	}
}
