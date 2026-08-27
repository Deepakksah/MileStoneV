using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient.Proxy;

namespace VideoOS.Management.VmoClient;

public abstract class VmoObjectSecurity
{
	private readonly IVmoRepositories _repositories;

	private string _internalId;

	public IVmoRepositories Repositories => _repositories;

	public virtual string InternalId
	{
		get
		{
			return _internalId;
		}
		set
		{
			_internalId = value;
		}
	}

	public VmoObjectSecurity(IVmoRepositories repositories, string internalId)
	{
		_repositories = repositories;
		_internalId = internalId;
	}

	protected virtual PermissionSetBase PermissionSetFromActionId(string actionId)
	{
		throw new NotImplementedException();
	}

	protected void Allow(PermissionSetBase permissions, string[] identities)
	{
		if (permissions == null)
		{
			throw new ArgumentNullException("permissions");
		}
		if (identities == null)
		{
			throw new ArgumentNullException("identities");
		}
		List<AccessControlEntry> list = new List<AccessControlEntry>();
		foreach (string text in identities)
		{
			Identity identity = Repositories.ServerCommunication.ReadIdentity(SearchFactor.AccountName, text, QueryMembership.None);
			if (identity == null)
			{
				identity = Repositories.ServerCommunication.ReadIdentityFromSource(SearchFactor.AccountName, text);
			}
			if (identity == null)
			{
				throw new IdentityNotFoundException();
			}
			AccessControlEntry[] collection = permissions.CreateAccessControlEntries(new string[1] { identity.Sid }, accessDeny: false);
			list.AddRange(collection);
		}
		Repositories.ServerCommunication.AddAccessControlEntries(InternalId, list.ToArray());
	}

	protected void Deny(PermissionSetBase permissions, string[] identities)
	{
		if (permissions == null)
		{
			throw new ArgumentNullException("permissions");
		}
		if (identities == null)
		{
			throw new ArgumentNullException("identities");
		}
		List<AccessControlEntry> list = new List<AccessControlEntry>();
		foreach (string text in identities)
		{
			Identity identity = Repositories.ServerCommunication.ReadIdentity(SearchFactor.AccountName, text, QueryMembership.None);
			if (identity == null)
			{
				identity = Repositories.ServerCommunication.ReadIdentityFromSource(SearchFactor.AccountName, text);
			}
			if (identity == null)
			{
				throw new IdentityNotFoundException();
			}
			AccessControlEntry[] collection = permissions.CreateAccessControlEntries(new string[1] { identity.Sid }, accessDeny: true);
			list.AddRange(collection);
		}
		Repositories.ServerCommunication.AddAccessControlEntries(InternalId, list.ToArray());
	}

	protected void Revoke(PermissionSetBase permissions, string[] identities)
	{
		if (permissions == null)
		{
			throw new ArgumentNullException("permissions");
		}
		if (identities == null)
		{
			throw new ArgumentNullException("identities");
		}
		List<AccessControlEntry> list = new List<AccessControlEntry>();
		foreach (string factoryValue in identities)
		{
			Identity identity = Repositories.ServerCommunication.ReadIdentity(SearchFactor.AccountName, factoryValue, QueryMembership.None);
			if (identity == null)
			{
				throw new IdentityNotFoundException();
			}
			AccessControlEntry[] collection = permissions.CreateAccessControlEntries(new string[1] { identity.Sid }, accessDeny: false);
			list.AddRange(collection);
		}
		Repositories.ServerCommunication.RemoveAccessControlEntries(InternalId, list.ToArray());
	}

	protected T[] EnumPermissions<T>() where T : PermissionInfo, new()
	{
		AccessControlEntry[] array = Repositories.ServerCommunication.QueryAccessControlList(InternalId);
		List<T> list = new List<T>();
		AccessControlEntry[] array2 = array;
		foreach (AccessControlEntry accessControlEntry in array2)
		{
			PermissionSetBase permissionSet = PermissionSetFromActionId(accessControlEntry.ActionId);
			T val = new T();
			val.SetPermissionInfo(accessControlEntry, permissionSet);
			list.Add(val);
		}
		return list.ToArray();
	}

	protected T[] EnumPermissions<T>(PermissionSetBase permissions) where T : PermissionInfo, new()
	{
		AccessControlEntry[] accessControlEntries = Repositories.ServerCommunication.QueryAccessControlList(InternalId);
		return CreatePermissionInfoArray<T>(accessControlEntries, permissions);
	}

	protected T[] EnumPermissions<T>(string identity, PermissionSetBase permissions) where T : PermissionInfo, new()
	{
		Identity identity2 = Repositories.ServerCommunication.ReadIdentity(SearchFactor.AccountName, identity, QueryMembership.None);
		if (identity2 == null)
		{
			throw new IdentityNotFoundException();
		}
		AccessControlEntry[] accessControlEntries = Repositories.ServerCommunication.QueryAccessControlList(identity2.Sid, InternalId);
		return CreatePermissionInfoArray<T>(accessControlEntries, permissions);
	}

	protected T EffectivePermissions<T>(string identity, T permissions) where T : PermissionSetBase, new()
	{
		string[] array = permissions.ToArray();
		Identity identity2 = Repositories.ServerCommunication.ReadIdentity(SearchFactor.AccountName, identity, QueryMembership.None);
		if (identity2 == null)
		{
			identity2 = Repositories.ServerCommunication.ReadIdentityFromSource(SearchFactor.AccountName, identity);
		}
		if (identity2 == null)
		{
			throw new IdentityNotFoundException();
		}
		bool[] array2 = Repositories.ServerCommunication.IsPermittedByActionList(identity2.Sid, array, InternalId);
		List<string> list = new List<string>();
		for (int i = 0; i < array2.Length; i++)
		{
			if (array2[i])
			{
				list.Add(array[i]);
			}
		}
		return (T)permissions.CreateFromActionId(list.ToArray());
	}

	protected void SetPermissionInheritanceInternal(bool enableInheritance, string parentId)
	{
		if (enableInheritance)
		{
			Repositories.ServerCommunication.SetInheritance(InternalId, parentId);
		}
		else
		{
			Repositories.ServerCommunication.SetInheritance(InternalId, null);
		}
	}

	private T[] CreatePermissionInfoArray<T>(AccessControlEntry[] accessControlEntries, PermissionSetBase permissions) where T : PermissionInfo, new()
	{
		if (accessControlEntries == null)
		{
			throw new ArgumentNullException("accessControlEntries");
		}
		var enumerable = from ace in accessControlEntries
			let permission = PermissionSetFromActionId(ace.ActionId)
			where permissions.HasAnyPermissionSet(permission)
			select new
			{
				AccessControlEntry = ace,
				Permission = permission
			};
		List<T> list = new List<T>();
		foreach (var item in enumerable)
		{
			T val = new T();
			val.SetPermissionInfo(item.AccessControlEntry, item.Permission);
			list.Add(val);
		}
		return list.ToArray();
	}
}
