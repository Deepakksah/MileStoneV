using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using VideoOS.Platform.Admin;
using VideoOS.Platform.ConfigurationItems;
using VideoOS.Platform.Login;
using VideoOS.Platform.UI;
using VideoOS.Platform.Util.ImageServer;
using VideoOS.Platform.Util.Security;

namespace VideoOS.Platform.Util;

public static class SecurityAccess
{
	private static SecurityClientBase _securityClient;

	private static readonly Lazy<Dictionary<Guid, ItemNode>> _itemNodeByKind = new Lazy<Dictionary<Guid, ItemNode>>(() => BuildItemNodeByKind());

	private static readonly Dictionary<Guid, SecurityClientBase> _securityClients = new Dictionary<Guid, SecurityClientBase>();

	private static LoginSettings _loginSettings = null;

	private static readonly Dictionary<string, List<string>> _savedMemberShips = new Dictionary<string, List<string>>();

	public static readonly Guid AdministratorRoleId = new Guid("1f30d14b-ba91-4e8a-8838-eda17931f8fc");

	private static bool _dirty = false;

	private static DifferentiatedAdministrationRights _differentiatedAdministrationRights = new DifferentiatedAdministrationRights();

	private static SecurityClientBase SecurityClient
	{
		get
		{
			lock (_securityClients)
			{
				if (_securityClient == null)
				{
					try
					{
						if (_securityClients.TryGetValue(Configuration.Instance.ServerFQID.ServerId.Id, out _securityClient))
						{
							return _securityClient;
						}
						_loginSettings = LoginSettingsCache.GetLoginSettings(Configuration.Instance.ServerFQID.ServerId.ServerHostname, Configuration.Instance.ServerFQID.ServerId.Serverport);
						string serverType = Configuration.Instance.ServerFQID.ServerId.ServerType;
						if (serverType == "XP" && _loginSettings != null)
						{
							serverType = _loginSettings.ServerType;
						}
						_securityClient = new SecurityClientXPCOWrapper(_loginSettings);
						_securityClient.ServerType = serverType;
						_securityClient.Initialize();
						if (EnvironmentManager.Instance.Mode == Mode.ServerSetup && _dirty)
						{
							Save();
							_dirty = false;
						}
						_securityClients.Add(Configuration.Instance.ServerFQID.ServerId.Id, _securityClient);
					}
					catch (Exception ex)
					{
						throw new NotAuthorizedMIPException("Unable to verify:" + ex.Message, ex);
					}
				}
			}
			return _securityClient;
		}
	}

	public static bool Dirty
	{
		get
		{
			return _dirty;
		}
		set
		{
			_dirty = value;
		}
	}

	public static DifferentiatedAdministrationRights DifferentiatedAdministrationRights
	{
		get
		{
			return _differentiatedAdministrationRights;
		}
		set
		{
			_differentiatedAdministrationRights = value;
		}
	}

	private static SecurityClientBase GetSecurityClient(ServerId serverIdParm)
	{
		lock (_securityClients)
		{
			ServerId serverId = serverIdParm;
			if (serverId.ServerType == "XPCORS")
			{
				serverId = ((serverId.UserContext != null) ? serverId.UserContext.Configuration : Configuration.Instance).GetItem(serverId.Id, Kind.Server).GetParent().FQID.ServerId;
			}
			LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(serverId);
			if (_securityClients.TryGetValue(loginSettings.InstanceGuid, out var value))
			{
				return value;
			}
			try
			{
				value = new SecurityClientXPCOWrapper(loginSettings);
				value.ServerType = serverId.ServerType;
				value.Initialize();
				if (EnvironmentManager.Instance.Mode == Mode.ServerSetup && _dirty)
				{
					Save();
					_dirty = false;
				}
				_securityClients.Add(loginSettings.InstanceGuid, value);
				return value;
			}
			catch (Exception ex)
			{
				throw new NotAuthorizedMIPException("Unable to verify:" + ex.Message, ex);
			}
		}
	}

	private static SecurityClientBase GetSecurityClient(LoginSettings loginSettings)
	{
		lock (_securityClients)
		{
			if (_securityClients.TryGetValue(loginSettings.InstanceGuid, out var value))
			{
				return value;
			}
			try
			{
				value = new SecurityClientXPCOWrapper(loginSettings);
				value.ServerType = loginSettings.ServerType;
				value.Initialize();
				if (EnvironmentManager.Instance.Mode == Mode.ServerSetup && _dirty)
				{
					Save();
					_dirty = false;
				}
				_securityClients.Add(loginSettings.InstanceGuid, value);
				return value;
			}
			catch (Exception ex)
			{
				throw new NotAuthorizedMIPException("Unable to verify:" + ex.Message, ex);
			}
		}
	}

	public static void Save()
	{
		if (_securityClient != null)
		{
			_securityClient.Flush();
		}
	}

	public static void Flush()
	{
		if (_securityClient != null)
		{
			_securityClient.Flush();
		}
	}

	public static void Reload()
	{
		if (_securityClient != null)
		{
			_securityClient.Reload();
		}
	}

	public static void Logout(LoginSettings loginSettings)
	{
		if (loginSettings == null)
		{
			throw new ArgumentNullException("loginSettings");
		}
		lock (_securityClients)
		{
			if (_securityClients.TryGetValue(loginSettings.InstanceGuid, out var value))
			{
				value.Logoff();
				_securityClients.Remove(loginSettings.InstanceGuid);
			}
		}
	}

	public static void Logoff()
	{
		lock (_securityClients)
		{
			foreach (SecurityClientBase value in _securityClients.Values)
			{
				value.Logoff();
			}
			_securityClients.Clear();
		}
		if (_securityClient != null)
		{
			_securityClient.Logoff();
			_securityClient = null;
		}
	}

	[Obsolete("Do not use this method, as it is handled automatically in the Management Client")]
	public static void RegisterItem(Item item)
	{
	}

	[Obsolete("Do not use this method, as it is handled automatically in the Management Client")]
	public static void UnregisterItem(Item item)
	{
	}

	public static void CheckPermission(Item item, string actionId)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (item.FQID == null)
		{
			throw new ArgumentException("The value of item.FQID cannot be null.", "item");
		}
		ItemNode itemNode = PluginManager.GetItemNode(item.FQID.Kind);
		if (itemNode != null && (itemNode.SecurityActions == null || itemNode.SecurityActions.Count == 0))
		{
			return;
		}
		if (VideoOS.Platform.UI.Util.IsBuiltInKind(item.FQID.Kind))
		{
			if (!Kind.KindToNamespace.ContainsKey(item.FQID.Kind))
			{
				throw new NotAuthorizedMIPException("Unable to validate permission via this method");
			}
			Guid kind = Kind.KindToNamespace[item.FQID.Kind];
			SecurityClient.CheckPermission(kind, item.FQID.ObjectId.ToString(), actionId);
		}
		else
		{
			string objectId = ConvertToFullPath(item, itemNode);
			SecurityClient.CheckPermission(item.FQID.Kind, objectId, actionId);
		}
	}

	public static void CheckPermission(Item item, string actionId, string SID)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (item.FQID == null)
		{
			throw new ArgumentException("The value of item.FQID cannot be null.", "item");
		}
		ItemNode itemNode = PluginManager.GetItemNode(item.FQID.Kind);
		if (itemNode != null && (itemNode.SecurityActions == null || itemNode.SecurityActions.Count == 0))
		{
			return;
		}
		string objectId = ConvertToFullPath(item, itemNode);
		AccessControlEntry[] array = SecurityClient.QueryAccessControlList(ConvertToNamespaceId(item), SID, objectId, new List<string> { actionId });
		foreach (AccessControlEntry accessControlEntry in array)
		{
			if (string.Compare(actionId, accessControlEntry.ActionId, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return;
			}
		}
		throw new NotAuthorizedMIPException("Access not permitted");
	}

	public static void CheckPermission(Item item, string actionId, string SID, List<string> groups)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (item.FQID == null)
		{
			throw new ArgumentException("The value of item.FQID cannot be null.", "item");
		}
		if (groups == null)
		{
			groups = new List<string>();
		}
		ItemNode itemNode = PluginManager.GetItemNode(item.FQID.Kind);
		if (itemNode != null && (itemNode.SecurityActions == null || itemNode.SecurityActions.Count == 0))
		{
			return;
		}
		_savedMemberShips[SID] = groups;
		string objectId = ConvertToFullPath(item, itemNode);
		AccessControlEntry[] array = SecurityClient.QueryAccessControlList(ConvertToNamespaceId(item), SID, objectId, new List<string> { actionId });
		foreach (AccessControlEntry accessControlEntry in array)
		{
			if (string.Compare(actionId, accessControlEntry.ActionId, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return;
			}
		}
		throw new NotAuthorizedMIPException("Access not permitted");
	}

	public static void CheckPermission(Item item, string actionId, ServerId serverId)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (item.FQID == null)
		{
			throw new ArgumentException("The value of item.FQID cannot be null.", "item");
		}
		ItemNode itemNode = PluginManager.GetItemNode(item.FQID.Kind);
		if (itemNode != null && (itemNode.SecurityActions == null || itemNode.SecurityActions.Count == 0))
		{
			return;
		}
		if (VideoOS.Platform.UI.Util.IsBuiltInKind(item.FQID.Kind))
		{
			if (!Kind.KindToNamespace.ContainsKey(item.FQID.Kind))
			{
				throw new NotAuthorizedMIPException("Unable to validate permission via this method");
			}
			Guid kind = Kind.KindToNamespace[item.FQID.Kind];
			GetSecurityClient(serverId).CheckPermission(kind, item.FQID.ObjectId.ToString(), actionId);
		}
		else
		{
			string objectId = ConvertToFullPath(item, itemNode);
			GetSecurityClient(serverId).CheckPermission(item.FQID.Kind, objectId, actionId);
		}
	}

	public static void CheckPermission(PluginDefinition pluginDefinition, string actionId)
	{
		if (pluginDefinition == null)
		{
			throw new NotAuthorizedMIPException("Access not permitted");
		}
		if (pluginDefinition.SecurityActions != null && pluginDefinition.SecurityActions.Count != 0)
		{
			string text = pluginDefinition.Id.ToString();
			SecurityClient.CheckPermission(pluginDefinition.Id, text.ToUpperInvariant(), actionId);
		}
	}

	public static void CheckPermission(Guid pluginDefinitionId, string actionId)
	{
		PluginDefinition pluginDefinition = PluginManager.GetPluginDefinition(pluginDefinitionId);
		if (pluginDefinition == null || (pluginDefinition.SecurityActions != null && pluginDefinition.SecurityActions.Count != 0))
		{
			SecurityClient.CheckPermission(pluginDefinitionId, pluginDefinitionId.ToString().ToUpperInvariant(), actionId);
		}
	}

	public static void CheckPermission(Guid pluginDefinitionId, string actionId, string SID)
	{
		PluginDefinition pluginDefinition = PluginManager.GetPluginDefinition(pluginDefinitionId);
		if (pluginDefinition != null && (pluginDefinition.SecurityActions == null || pluginDefinition.SecurityActions.Count == 0))
		{
			return;
		}
		AccessControlEntry[] array = SecurityClient.QueryAccessControlList(pluginDefinitionId, SID, pluginDefinitionId.ToString().ToUpperInvariant(), new List<string> { actionId });
		foreach (AccessControlEntry accessControlEntry in array)
		{
			if (string.Compare(actionId, accessControlEntry.ActionId, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return;
			}
		}
		throw new NotAuthorizedMIPException("Access not permitted");
	}

	public static void CheckPermission(Guid pluginDefinitionId, string actionId, string SID, List<string> groups)
	{
		CheckPermission(pluginDefinitionId, actionId, SID);
	}

	public static void CheckPermission(PluginDefinition pluginDefinition, string actionId, ServerId serverId)
	{
		if (pluginDefinition == null)
		{
			throw new NotAuthorizedMIPException("Access not permitted");
		}
		if (pluginDefinition.SecurityActions != null && pluginDefinition.SecurityActions.Count != 0)
		{
			string text = pluginDefinition.Id.ToString();
			GetSecurityClient(serverId).CheckPermission(pluginDefinition.Id, text.ToUpperInvariant(), actionId);
		}
	}

	public static void CheckPermission(Guid pluginDefinitionId, string actionId, ServerId serverId)
	{
		string text = pluginDefinitionId.ToString();
		GetSecurityClient(serverId).CheckPermission(pluginDefinitionId, text.ToUpperInvariant(), actionId);
	}

	public static Collection<string> GetPermittedActionList(Item item, string SID)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (item.FQID == null)
		{
			throw new ArgumentException("The value of item.FQID cannot be null.", "item");
		}
		Collection<string> collection = new Collection<string>();
		ItemNode itemNode = null;
		itemNode = PluginManager.GetItemNode(item.FQID.Kind);
		if (itemNode != null && itemNode.SecurityActions != null)
		{
			string text = ConvertToFullPath(item, itemNode);
			List<string> actionIdList = itemNode.SecurityActions.Select((VideoOS.Platform.Admin.SecurityAction a) => a.ActionId).ToList();
			AccessControlEntry[] array = SecurityClient.QueryAccessControlList(item.FQID.Kind, SID, text.ToUpperInvariant(), actionIdList);
			foreach (AccessControlEntry accessControlEntry in array)
			{
				if (!accessControlEntry.AccessDeny)
				{
					collection.Add(accessControlEntry.ActionId);
				}
			}
		}
		else if (VideoOS.Platform.UI.Util.IsBuiltInKind(item.FQID.Kind))
		{
			Guid kind = ConvertToNamespaceId(item);
			List<string> actionIdList2 = (from a in SecurityClient.QueryObjectClassActions(kind.ToString())
				select a.ActionId).ToList();
			AccessControlEntry[] array = SecurityClient.QueryAccessControlList(kind, SID, item.FQID.ObjectId.ToString().ToUpperInvariant(), actionIdList2);
			foreach (AccessControlEntry accessControlEntry2 in array)
			{
				if (!accessControlEntry2.AccessDeny)
				{
					collection.Add(accessControlEntry2.ActionId);
				}
			}
		}
		return collection;
	}

	public static Collection<string> GetPermittedActionList(Item item, string SID, ServerId serverId)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (item.FQID == null)
		{
			throw new ArgumentException("The value of item.FQID cannot be null.", "item");
		}
		Collection<string> collection = new Collection<string>();
		ItemNode itemNode = null;
		itemNode = PluginManager.GetItemNode(item.FQID.Kind);
		if (itemNode != null && itemNode.SecurityActions != null)
		{
			string text = ConvertToFullPath(item, itemNode);
			List<string> actionIdList = itemNode.SecurityActions.Select((VideoOS.Platform.Admin.SecurityAction a) => a.ActionId).ToList();
			AccessControlEntry[] array = GetSecurityClient(serverId).QueryAccessControlList(item.FQID.Kind, SID, text.ToUpperInvariant(), actionIdList);
			foreach (AccessControlEntry accessControlEntry in array)
			{
				if (!accessControlEntry.AccessDeny)
				{
					collection.Add(accessControlEntry.ActionId);
				}
			}
		}
		else if (VideoOS.Platform.UI.Util.IsBuiltInKind(item.FQID.Kind))
		{
			Guid kind = ConvertToNamespaceId(item);
			List<string> actionIdList2 = (from a in GetSecurityClient(serverId).QueryObjectClassActions(kind.ToString())
				select a.ActionId).ToList();
			AccessControlEntry[] array = GetSecurityClient(serverId).QueryAccessControlList(kind, SID, item.FQID.ObjectId.ToString().ToUpperInvariant(), actionIdList2);
			foreach (AccessControlEntry accessControlEntry2 in array)
			{
				if (!accessControlEntry2.AccessDeny)
				{
					collection.Add(accessControlEntry2.ActionId);
				}
			}
		}
		return collection;
	}

	public static List<Item> BuildPermissionList(List<Item> items, string actionId)
	{
		List<Item> list = new List<Item>();
		foreach (Item item in items)
		{
			try
			{
				CheckPermission(item, actionId);
				list.Add(item);
			}
			catch
			{
			}
		}
		return list;
	}

	public static List<Item> BuildPermissionList(List<Item> items, string actionId, ServerId serverId)
	{
		List<Item> list = new List<Item>();
		foreach (Item item in items)
		{
			try
			{
				CheckPermission(item, actionId, serverId);
				list.Add(item);
			}
			catch
			{
			}
		}
		return list;
	}

	public static List<Item> BuildPermissionList(List<Item> items, string actionId, string SID, List<string> groups)
	{
		if (items == null)
		{
			throw new ArgumentNullException("items");
		}
		if (!items.Any())
		{
			return new List<Item>();
		}
		Guid kind = items[0].FQID.Kind;
		ItemNode itemNodeFromKind = GetItemNodeFromKind(kind);
		bool flag = VideoOS.Platform.UI.Util.IsBuiltInKind(kind);
		Tuple<Item, bool>[] array = new Tuple<Item, bool>[items.Count];
		List<Item> list = new List<Item>();
		List<int> list2 = new List<int>();
		List<string> list3 = new List<string>();
		for (int i = 0; i < items.Count; i++)
		{
			Item item = items[i];
			if (kind != item.FQID.Kind)
			{
				throw new ArgumentMIPException("Multiple kind of items not supported");
			}
			if (itemNodeFromKind != null)
			{
				if (itemNodeFromKind.SecurityActions != null)
				{
					list2.Add(i);
					list.Add(item);
					list3.Add(ConvertToFullPath(item, itemNodeFromKind));
				}
				else
				{
					array[i] = new Tuple<Item, bool>(item, item2: true);
				}
			}
			else if (flag)
			{
				list2.Add(i);
				list.Add(item);
				list3.Add(ConvertToFullPath(item, null));
			}
			else
			{
				array[i] = new Tuple<Item, bool>(item, item2: false);
			}
		}
		Guid kind2 = ConvertToNamespaceId(items[0]);
		bool[] array2 = SecurityClient.HasPermissionByObjectList(kind2, SID, actionId, list3.ToArray());
		for (int j = 0; j < array2.Length; j++)
		{
			Item item2 = list[j];
			int num = list2[j];
			array[num] = new Tuple<Item, bool>(item2, array2[j]);
		}
		return new List<Item>(new List<Tuple<Item, bool>>(new List<Tuple<Item, bool>>(array).Where((Tuple<Item, bool> x) => x.Item2)).ConvertAll((Tuple<Item, bool> x) => x.Item1));
	}

	public static bool IsMember(ServerId serverId, string sidIdentity, string roleId)
	{
		if (serverId == null || sidIdentity == null || roleId == null)
		{
			throw new ArgumentException("");
		}
		return (GetSecurityClient(serverId) ?? throw new ArgumentException("Unknown ServerId")).IsMember(sidIdentity, roleId);
	}

	public static UserIdentification GetUserIdentificationViaSid(string sid, LoginSettings loginSettings)
	{
		return GetSecurityClient(loginSettings).GetUserIdentificationViaSid(sid);
	}

	public static UserIdentification GetUserIdentificationViaName(string name, LoginSettings loginSettings)
	{
		return GetSecurityClient(loginSettings).GetUserIdentificationViaName(name);
	}

	public static bool IsClientAllowed(ServerId serverId, string clientType)
	{
		if (serverId == null)
		{
			throw new ArgumentException("ServerId must be supplied", "serverId");
		}
		try
		{
			ManagementServer managementServer = new ManagementServer(serverId);
			switch (clientType)
			{
			case "SmartClient":
				return managementServer.IsSmartClientAllowed;
			case "MobileClient":
				return managementServer.IsMobileClientAllowed;
			case "WebClient":
				return managementServer.IsWebClientAllowed;
			}
		}
		catch (Exception)
		{
		}
		return true;
	}

	public static bool HasOverallPermission(ServerId serverId, string securityNamespaceName, string securityAction)
	{
		if (serverId == null)
		{
			throw new ArgumentNullException("serverId");
		}
		SecurityClientBase securityClient = GetSecurityClient(serverId);
		if (securityClient == null)
		{
			throw new ArgumentException("Unknown ServerId");
		}
		try
		{
			return securityClient.HasOverallPermission(securityNamespaceName, securityAction);
		}
		catch
		{
			return false;
		}
	}

	private static string ConvertToFullPath(Item item, ItemNode itemNode)
	{
		string text = "";
		text = ((!(item.FQID.ObjectId == Guid.Empty) || string.IsNullOrEmpty(item.FQID.ObjectIdString)) ? ((item.FQID.ObjectId != Guid.Empty) ? item.FQID.ObjectId.ToString() : item.FQID.ServerId.Id.ToString()) : item.FQID.ObjectIdString);
		if (VideoOS.Platform.UI.Util.IsBuiltInKind(item.FQID.Kind))
		{
			return text;
		}
		if (itemNode == null || itemNode.ParentKind == Guid.Empty || item.FQID.ParentId == Guid.Empty)
		{
			return string.Format(CultureInfo.InvariantCulture, "/{0}/{1}", item.FQID.Kind, text);
		}
		return string.Format(CultureInfo.InvariantCulture, "/{0}/{1}", item.FQID.ParentId, text);
	}

	private static Guid ConvertToNamespaceId(Item item)
	{
		if (VideoOS.Platform.UI.Util.IsBuiltInKind(item.FQID.Kind))
		{
			if (Kind.KindToNamespace.ContainsKey(item.FQID.Kind))
			{
				return Kind.KindToNamespace[item.FQID.Kind];
			}
			throw new NotAuthorizedMIPException("Unable to validate permission for this Built-In item type");
		}
		return item.FQID.Kind;
	}

	public static void InitializationCheck()
	{
		_ = SecurityClient.ServiceName;
	}

	public static void SetPermittedAction(string identity, Item item, string actionId, bool permit)
	{
		try
		{
			string objectId = ((item.FQID.ObjectId != Guid.Empty) ? item.FQID.ObjectId.ToString() : item.FQID.ServerId.Id.ToString());
			AccessControlEntry accessControlEntry = new AccessControlEntry
			{
				ActionId = actionId,
				Sid = identity
			};
			if (permit)
			{
				SecurityClient.AddAccessControlEntries(item.FQID.Kind, objectId, new AccessControlEntry[1] { accessControlEntry });
			}
			else
			{
				SecurityClient.RemoveAccessControlEntries(item.FQID.Kind, objectId, new AccessControlEntry[1] { accessControlEntry });
			}
			_dirty = true;
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: true, "SecurityAccess::SetPermittedAction", ex.Message, null);
			throw;
		}
	}

	public static List<string> GetPermittedActionList(string identity, PluginDefinition pluginDefinition)
	{
		try
		{
			string objectId = pluginDefinition.Id.ToString().ToUpperInvariant();
			List<string> list = new List<string>();
			if (pluginDefinition.SecurityActions != null && pluginDefinition.SecurityActions.Count > 0)
			{
				string[] array = BuildActionList(pluginDefinition.SecurityActions);
				bool[] array2 = SecurityClient.IsPermittedByActionList(pluginDefinition.Id, identity, array, objectId);
				for (int i = 0; i < array.Length; i++)
				{
					if (array2[i])
					{
						list.Add(array[i]);
					}
				}
			}
			return list;
		}
		catch (Exception ex)
		{
			EnvironmentManager instance = EnvironmentManager.Instance;
			string message = ex.Message;
			object[] args = new PluginDefinition[1] { pluginDefinition };
			instance.Log(error: true, "SetPermittedAction", message, args);
			return new List<string>();
		}
	}

	public static List<string> GetPermittedActionList(string identity, ItemNode itemNode, Item parentItem)
	{
		try
		{
			string text = "";
			text = ((parentItem == null) ? itemNode.ItemKind.ToString() : ((parentItem.FQID.ObjectId != Guid.Empty) ? parentItem.FQID.ObjectId : parentItem.FQID.ServerId.Id).ToString());
			List<string> list = new List<string>();
			if (itemNode.SecurityActions != null && itemNode.SecurityActions.Count != 0)
			{
				string[] array = BuildActionList(itemNode.SecurityActions);
				bool[] array2 = SecurityClient.IsPermittedByActionList(itemNode.ItemKind, identity, array, text.ToUpperInvariant());
				for (int i = 0; i < array.Length; i++)
				{
					if (array2[i])
					{
						list.Add(array[i]);
					}
				}
			}
			return list;
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: true, "SecurityAccess::GetPermittedActionList - empty list returned to application", ex.Message, null);
			return new List<string>();
		}
	}

	public static List<string> GetOverallPermittedActionList(string identity, ItemNode itemNode)
	{
		if (itemNode == null)
		{
			throw new ArgumentNullException("itemNode");
		}
		try
		{
			string text = "/";
			List<string> list = new List<string>();
			if (itemNode.SecurityActions != null && itemNode.SecurityActions.Count != 0)
			{
				string[] array = BuildActionList(itemNode.SecurityActions);
				bool[] array2 = SecurityClient.IsPermittedByActionList(itemNode.ItemKind, identity, array, text.ToUpperInvariant());
				for (int i = 0; i < array.Length; i++)
				{
					if (array2[i])
					{
						list.Add(array[i]);
					}
				}
			}
			return list;
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: true, "SecurityAccess::GetPermittedActionList - empty list returned to application", ex.Message, null);
			return new List<string>();
		}
	}

	public static List<string> GetOverallPermittedActionList(string identity, PluginDefinition pluginDefinition)
	{
		if (pluginDefinition == null)
		{
			throw new ArgumentNullException("pluginDefinition");
		}
		try
		{
			string text = "/";
			List<string> list = new List<string>();
			if (pluginDefinition.SecurityActions != null && pluginDefinition.SecurityActions.Count != 0)
			{
				string[] array = BuildActionList(pluginDefinition.SecurityActions);
				bool[] array2 = SecurityClient.IsPermittedByActionList(pluginDefinition.Id, identity, array, text.ToUpperInvariant());
				for (int i = 0; i < array.Length; i++)
				{
					if (array2[i])
					{
						list.Add(array[i]);
					}
				}
			}
			return list;
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: true, "SecurityAccess::GetPermittedActionList - empty list returned to application", ex.Message, null);
			return new List<string>();
		}
	}

	public static List<string> GetPermittedActionList(string identity, Item item)
	{
		try
		{
			ItemNode itemNodeFromKind = GetItemNodeFromKind(item.FQID.Kind);
			string text = ConvertToFullPath(item, itemNodeFromKind);
			List<string> list = new List<string>();
			if (itemNodeFromKind != null && itemNodeFromKind.SecurityActions != null)
			{
				string[] array = BuildActionList(itemNodeFromKind.SecurityActions);
				bool[] array2 = SecurityClient.IsPermittedByActionList(item.FQID.Kind, identity, array, text.ToUpperInvariant());
				for (int i = 0; i < array.Length; i++)
				{
					if (array2[i])
					{
						list.Add(array[i]);
					}
				}
			}
			return list;
		}
		catch (Exception ex)
		{
			EnvironmentManager instance = EnvironmentManager.Instance;
			string message = ex.Message;
			object[] args = new Item[1] { item };
			instance.Log(error: true, "SecurityAccess::PermittedActionList", message, args);
			return new List<string>();
		}
	}

	public static void SetPermittedAction(string identity, PluginDefinition pluginDefinition, string actionId, bool permit)
	{
		try
		{
			string objectId = pluginDefinition.Id.ToString().ToUpperInvariant();
			AccessControlEntry accessControlEntry = new AccessControlEntry
			{
				ActionId = actionId,
				Sid = identity
			};
			if (permit)
			{
				SecurityClient.AddAccessControlEntries(pluginDefinition.Id, objectId, new AccessControlEntry[1] { accessControlEntry });
			}
			else
			{
				SecurityClient.RemoveAccessControlEntries(pluginDefinition.Id, objectId, new AccessControlEntry[1] { accessControlEntry });
			}
			_dirty = true;
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: true, "SecurityAccess::SetPermittedAction", ex.Message, null);
			throw;
		}
	}

	public static void SetPermittedAction(string identity, ItemNode itemNode, Item parentItem, string actionId, bool permit)
	{
		try
		{
			string text = "";
			text = ((parentItem == null) ? itemNode.ItemKind.ToString().ToUpperInvariant() : ((parentItem.FQID.ObjectId != Guid.Empty) ? parentItem.FQID.ObjectId : parentItem.FQID.ServerId.Id).ToString().ToUpperInvariant());
			AccessControlEntry accessControlEntry = new AccessControlEntry
			{
				ActionId = actionId,
				Sid = identity
			};
			if (permit)
			{
				SecurityClient.AddAccessControlEntries(itemNode.ItemKind, text, new AccessControlEntry[1] { accessControlEntry });
			}
			else
			{
				SecurityClient.RemoveAccessControlEntries(itemNode.ItemKind, text, new AccessControlEntry[1] { accessControlEntry });
			}
			_dirty = true;
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: true, "SecurityAccess::SetPermittedAction", ex.Message, null);
			throw;
		}
	}

	public static void RegisterSecurityObject(string securityNamespace, string objectId, string parentObjectId)
	{
		SecurityClient.RegisterSecurityObject(securityNamespace, objectId, parentObjectId);
	}

	public static void UnregisterSecurityObject(string securityNamespace, string objectId)
	{
		SecurityClient.UnregisterSecurityObject(securityNamespace, objectId);
	}

	public static VideoOS.Platform.Util.Security.SecurityAction[] GetCurrentActions(string objectClassId)
	{
		VideoOS.Platform.Util.Security.SecurityAction[] result = null;
		try
		{
			result = SecurityClient.QueryObjectClassActions(objectClassId);
		}
		catch (Exception)
		{
		}
		return result;
	}

	private static string[] BuildActionList(List<VideoOS.Platform.Admin.SecurityAction> securityActions)
	{
		List<string> list = new List<string>();
		if (securityActions != null)
		{
			for (int i = 0; i < securityActions.Count; i++)
			{
				list.Add(securityActions[i].ActionId);
			}
		}
		return list.ToArray();
	}

	private static ItemNode GetItemNodeFromKind(Guid kind)
	{
		if (!_itemNodeByKind.Value.TryGetValue(kind, out var value))
		{
			EnvironmentManager instance = EnvironmentManager.Instance;
			Guid guid = kind;
			instance.Log(error: true, "SecurityAccess::GetItemNodeFromKind", "Failed to look up item kind: " + guid.ToString() + ". Access will be denied.");
		}
		return value;
	}

	private static Dictionary<Guid, ItemNode> BuildItemNodeByKind()
	{
		Dictionary<Guid, ItemNode> dictionary = new Dictionary<Guid, ItemNode>();
		foreach (PluginDefinition allPluginDefinition in EnvironmentManager.Instance.AllPluginDefinitions)
		{
			if (allPluginDefinition.ItemNodes != null && allPluginDefinition.ItemNodes.Count > 0)
			{
				BuildItemNodeTable(allPluginDefinition.ItemNodes, dictionary);
			}
		}
		return dictionary;
	}

	private static void BuildItemNodeTable(List<ItemNode> itemNodes, Dictionary<Guid, ItemNode> itemNodeByKind)
	{
		foreach (ItemNode itemNode in itemNodes)
		{
			if (itemNodeByKind.TryGetValue(itemNode.ItemKind, out var value))
			{
				EnvironmentManager.Instance.Log(error: true, "SecurityAccess::BuildItemNodeTable", string.Format(CultureInfo.InvariantCulture, "Warning: Overwriting duplicate item node for kind: {0}. Previous item node: {1}, new item node: {2}", itemNode.ItemKind, value.Name, itemNode.Name));
			}
			itemNodeByKind[itemNode.ItemKind] = itemNode;
			if (itemNode.ItemNodes != null && itemNode.ItemNodes.Count > 0)
			{
				BuildItemNodeTable(itemNode.ItemNodes, itemNodeByKind);
			}
		}
	}
}
