#define TRACE
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security;
using System.Windows.Forms;
using System.Xml;
using VideoOS.Platform.Client;
using VideoOS.Platform.ConfigurationItems;
using VideoOS.Platform.Log;
using VideoOS.Platform.Login;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.Proxy.ConfigApi;
using VideoOS.Platform.Proxy.RestApi;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Interface;
using VideoOS.Platform.SDK.Util;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.SDK.Platform;

public class SDKConfiguration : Configuration, ISDKConfiguration
{
	private ISystemInfo _masterSystemInfo;

	private Hashtable _pluginCommonConfiguration = new Hashtable();

	private Hashtable _pluginCommonConfigurationDataVersion = new Hashtable();

	private ConfigurationAsync _configurationAsync;

	private static object _syncRoot = new object();

	private Dictionary<string, ISystemInfo> _systemInfoList = new Dictionary<string, ISystemInfo>();

	private Dictionary<string, ISystemInfo> _onlineSystemInfos = new Dictionary<string, ISystemInfo>();

	private bool _isSubscribedToServerOfflineCacheChanged;

	private static Guid _licenseKindId = new Guid("E145597A-ECAF-40A1-99E6-14BB9D45A09E");

	private static Guid _registrationsObjectId = new Guid("AD5EFDCE-3B21-47D5-A6A8-657E43A3E638");

	private static Guid _legacyRegistrationsObjectId = new Guid("E125597A-ECAF-40A1-99E6-24BB9D45A09E");

	private static Guid _configServiceId = new Guid("7BA35BDA-87DA-4D9B-9403-A1EDAE42DAA8");

	private static Guid _configServiceKind = new Guid("E05DDF5B-EB39-41F3-A0EF-420277AB6C39");

	private INameConvert _nameConvert;

	private List<LoginSettings> _loginSettingsList;

	private UserContext _userContext;

	private List<object> _registeredReceivers = new List<object>();

	private Hashtable _kindToPluginId = new Hashtable();

	public UserContext UserContext
	{
		get
		{
			return _userContext;
		}
		set
		{
			_userContext = value;
		}
	}

	public bool AutoReconnect { get; set; }

	public bool AutoLoadConfiguration { get; set; }

	public ISystemInfo MasterServerSystemInfo => _masterSystemInfo;

	public override FQID ServerFQID
	{
		get
		{
			if (_masterSystemInfo != null)
			{
				return new FQID(_masterSystemInfo.ServerId, FolderType.SystemDefined, Kind.Server);
			}
			return new FQID(new ServerId("XP", "localhost", 80, Guid.Empty)
			{
				UserContext = _userContext
			}, FolderType.SystemDefined, Kind.Server);
		}
	}

	public override string ServerName
	{
		get
		{
			if (_masterSystemInfo != null)
			{
				return _masterSystemInfo.ServerId.ServerHostname;
			}
			return "localhost";
		}
	}

	public List<LoginSettings> LoginSettingsList => _loginSettingsList;

	public Dictionary<string, ISystemInfo> SystemInfoList => _systemInfoList;

	public Dictionary<string, ISystemInfo> OnlineSystemInfos => _onlineSystemInfos;

	public SDKConfiguration(UserContext userContext)
	{
		_configurationAsync = new ConfigurationAsync(userContext);
		_loginSettingsList = new List<LoginSettings>();
		_userContext = userContext;
		_nameConvert = new NameConvert();
	}

	public SDKConfiguration(UserContext userContext, bool subscribeToServerOfflineCacheChanged)
		: this(userContext)
	{
		if (subscribeToServerOfflineCacheChanged)
		{
			SubscribeToServerOfflineCacheChanged();
		}
	}

	public void SubscribeToServerOfflineCacheChanged()
	{
		if (!_isSubscribedToServerOfflineCacheChanged)
		{
			ServerOfflineCache.ServerOfflineCacheChanged += ServerOfflineCache_ServerOfflineCacheChanged;
			_isSubscribedToServerOfflineCacheChanged = true;
		}
	}

	public void UnsubscribeToServerOfflineCacheChanged()
	{
		ServerOfflineCache.ServerOfflineCacheChanged -= ServerOfflineCache_ServerOfflineCacheChanged;
		_isSubscribedToServerOfflineCacheChanged = false;
	}

	public void UnsubscribeToServerOfflineCacheChanged_IfNotLoggedInToAnyServer()
	{
		if (!_systemInfoList.Values.Any((ISystemInfo sysInfo) => sysInfo.LoginServer != null && sysInfo.LoginServer.IsLoggedIn()))
		{
			UnsubscribeToServerOfflineCacheChanged();
		}
	}

	private void ServerOfflineCache_ServerOfflineCacheChanged(object sender, EventArgs e)
	{
		lock (this)
		{
			_onlineSystemInfos = _systemInfoList.Where((KeyValuePair<string, ISystemInfo> x) => ServerOfflineCache.IsServerOnline(x.Value.Uri)).ToDictionary((KeyValuePair<string, ISystemInfo> x) => x.Key, (KeyValuePair<string, ISystemInfo> x) => x.Value);
		}
	}

	private object ClearRestApiCache(VideoOS.Platform.Messaging.Message message, FQID destination, FQID sender)
	{
		if (Factory.KindToItemType.TryGetValue(message.RelatedFQID.Kind, out var value))
		{
			string resourceType = _nameConvert.ItemTypeToResourcePlural(value);
			base.ConfigurationApiManager.ClearCache(resourceType);
			return null;
		}
		base.ConfigurationApiManager.ClearCache();
		return null;
	}

	public void AddConfiguration(List<LoginSettings> servers, bool masterOnly = false)
	{
		if (servers == null || servers.Count <= 0)
		{
			return;
		}
		try
		{
			foreach (LoginSettings server in servers)
			{
				string host = server.Uri.Host;
				if (server.Uri.Scheme != Uri.UriSchemeFile && LookupSystemInfo(host, server.Uri.Port) != null)
				{
					EnvironmentManager.Instance.Log(error: true, "Configuration Initialization", "Duplicate serverid:" + host, null);
				}
				else if (server.Uri.Scheme == Uri.UriSchemeFile)
				{
					DBSystemInfo dBSystemInfo = new DBSystemInfo(server, _userContext);
					AddConfiguration(dBSystemInfo);
					if (_masterSystemInfo == null)
					{
						_masterSystemInfo = dBSystemInfo;
					}
				}
				else
				{
					SystemInfo systemInfo = CreateSystemInfo(server, _userContext, masterOnly);
					AddConfiguration(systemInfo);
					if (_masterSystemInfo == null)
					{
						_masterSystemInfo = systemInfo;
					}
				}
			}
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("Configuration Initialization", exception);
		}
	}

	public void AddConfiguration(LoginSettings loginSettings, bool masterOnly)
	{
		if (loginSettings == null)
		{
			return;
		}
		try
		{
			string text = loginSettings.Uri.Host;
			if (loginSettings.Uri.Scheme == Uri.UriSchemeFile)
			{
				text = loginSettings.Uri.AbsolutePath;
			}
			if (LookupSystemInfo(text, loginSettings.Uri.Port) != null)
			{
				EnvironmentManager.Instance.Log(error: true, "Configuration Initialization", "Duplicate serverid:" + text, null);
			}
			else if (loginSettings.Uri.Scheme == Uri.UriSchemeFile)
			{
				DBSystemInfo dBSystemInfo = new DBSystemInfo(loginSettings, _userContext);
				AddConfiguration(dBSystemInfo);
				if (_masterSystemInfo == null)
				{
					_masterSystemInfo = dBSystemInfo;
				}
			}
			else
			{
				SystemInfo systemInfo = CreateSystemInfo(loginSettings, _userContext, masterOnly);
				AddConfiguration(systemInfo);
				if (_masterSystemInfo == null)
				{
					_masterSystemInfo = systemInfo;
				}
			}
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("Configuration Initialization", exception);
		}
	}

	public static SystemInfo CreateSystemInfo(LoginSettings loginSettings, UserContext userContext, bool masterOnly)
	{
		SystemInfo obj = new SystemInfo(loginSettings, userContext)
		{
			MasterOnly = masterOnly,
			Name = loginSettings.Name
		};
		UpdateAuthenticationType(obj, loginSettings);
		return obj;
	}

	public static SystemInfo CreateSystemInfo(LoginSettings loginSettings)
	{
		SystemInfo systemInfo = new SystemInfo(loginSettings, null);
		UpdateAuthenticationType(systemInfo, loginSettings);
		return systemInfo;
	}

	private static void UpdateAuthenticationType(SystemInfo sysinfo, LoginSettings loginSettings)
	{
		try
		{
			if (loginSettings.IsOAuthIdentity)
			{
				sysinfo.AuthenticationType = VideoOS.Platform.SDK.Config.AuthenticationType.OAuthToken;
				return;
			}
			NetworkCredential credential = loginSettings.CredentialCache.GetCredential(loginSettings.Uri, "Negotiate");
			if (credential != null)
			{
				if (credential.UserName == "")
				{
					sysinfo.AuthenticationType = VideoOS.Platform.SDK.Config.AuthenticationType.WindowsDefault;
				}
				else
				{
					sysinfo.AuthenticationType = VideoOS.Platform.SDK.Config.AuthenticationType.Windows;
				}
			}
			else if (loginSettings.CredentialCache.GetCredential(loginSettings.Uri, "Basic") != null)
			{
				sysinfo.AuthenticationType = VideoOS.Platform.SDK.Config.AuthenticationType.Basic;
			}
		}
		catch
		{
		}
	}

	public void AddConfiguration(ISystemInfo sysinfo)
	{
		lock (this)
		{
			string text = sysinfo.LoginSettings.Uri.Host;
			if (sysinfo.LoginSettings.Uri.Scheme == Uri.UriSchemeFile)
			{
				text = sysinfo.LoginSettings.Uri.AbsolutePath;
			}
			if (LookupSystemInfo(text, sysinfo.LoginSettings.Uri.Port) != null)
			{
				EnvironmentManager.Instance.Log(error: true, "Configuration Initialization", "Duplicate serverid:" + text, null);
				return;
			}
			_systemInfoList.Add(text + ":" + sysinfo.LoginSettings.Uri.Port, sysinfo);
			_loginSettingsList.Add(sysinfo.LoginSettings);
			if (ServerOfflineCache.IsServerOnline(sysinfo.LoginSettings.Uri))
			{
				_onlineSystemInfos.Add(text + ":" + sysinfo.LoginSettings.Uri.Port, sysinfo);
			}
			if (!_userContext.LoginSettings.Contains(sysinfo.LoginSettings))
			{
				_userContext.AddLoginSettings(sysinfo.LoginSettings);
			}
			if (!EnvironmentManager.UsingUserContext)
			{
				LoginSettingsCache.AddLoginSettings(sysinfo.LoginSettings);
			}
		}
	}

	public static CredentialCache GetNetworkCredentialCache(Uri serverAddress, string userName, SecureString password, bool basicUser)
	{
		string text = ((!basicUser) ? "Negotiate" : "Basic");
		NetworkCredential cred = VideoOS.Platform.Login.Util.BuildNetworkCredential(userName, password, text);
		return new CredentialCache { { serverAddress, text, cred } };
	}

	public ISystemInfo GetSystemInfo(Guid systemId)
	{
		foreach (ISystemInfo value in _systemInfoList.Values)
		{
			if (value.Id == systemId)
			{
				return value;
			}
		}
		return null;
	}

	ISystemInfo ISDKConfiguration.GetSystemInfo(Uri uri)
	{
		foreach (ISystemInfo value in _systemInfoList.Values)
		{
			if (value.LoginSettings.Uri.Equals(uri) || UriUtil.UriEqual(uri, value.LoginSettings.Uri, ignoreHttpHttpsDifference: true))
			{
				return value;
			}
		}
		return null;
	}

	public bool TryLogin(Uri uri)
	{
		return TryConnect(uri);
	}

	public bool TryConnect(Uri uri)
	{
		foreach (ISystemInfo value in _systemInfoList.Values)
		{
			if (value.LoginSettings.Uri.Equals(uri) || uri.DnsSafeHost == value.LoginSettings.Uri.DnsSafeHost)
			{
				value.Populate();
				return value.IsConnected;
			}
		}
		EnvironmentManager.Instance.Log(error: true, "Configuration.TryConnect(Uri)", "Server URI not found:" + uri, null);
		return false;
	}

	public Exception LastException(Uri uri)
	{
		foreach (ISystemInfo value in _systemInfoList.Values)
		{
			if (value.LoginSettings.Uri.Equals(uri) || uri.DnsSafeHost == value.LoginSettings.Uri.DnsSafeHost)
			{
				Exception lastException = value.LastException;
				value.LastException = null;
				return lastException;
			}
		}
		return null;
	}

	bool ISDKConfiguration.TryLoadConfiguration(Uri uri)
	{
		foreach (ISystemInfo value in _systemInfoList.Values)
		{
			if (value.LoginSettings.Uri.Equals(uri) || (uri.DnsSafeHost == value.LoginSettings.Uri.DnsSafeHost && uri.DnsSafeHost != ""))
			{
				value.Populate();
				return value.IsConnected;
			}
		}
		EnvironmentManager.Instance.Log(error: true, "SDKConfiguration.TryLoadConfiguration(Uri)", "Invalid server URI", null);
		return false;
	}

	bool ISDKConfiguration.Login(Uri uri, bool masterOnly)
	{
		if (TryLogin(uri, masterOnly))
		{
			return true;
		}
		if (EnvironmentManager.Instance.EnvironmentOptions["UseRestApiCaching"] == "Yes")
		{
			ConfigurationApiManager.EnableRestApiCaching = true;
			_registeredReceivers.Add(EnvironmentManager.Instance.RegisterReceiver(ClearRestApiCache, new MessageIdFilter(MessageId.System.LocalConfigurationChangedIndication)));
		}
		else
		{
			ConfigurationApiManager.EnableRestApiCaching = false;
		}
		EnvironmentManager.Instance.Log(error: true, "SDKConfiguration.Login(Uri)", "Unable to login on server URI=" + uri, null);
		return false;
	}

	public bool IsLoggedIn(Uri uri)
	{
		if (_systemInfoList != null)
		{
			foreach (ISystemInfo value in _systemInfoList.Values)
			{
				if (value.LoginSettings.Uri.Equals(uri) || uri.DnsSafeHost == value.LoginSettings.Uri.DnsSafeHost)
				{
					return value.IsLoggedIn();
				}
			}
		}
		return false;
	}

	public bool IsLoggedInAny()
	{
		foreach (ISystemInfo value in _systemInfoList.Values)
		{
			if (value.IsLoggedIn())
			{
				return true;
			}
		}
		return false;
	}

	public bool Logout(Uri uri)
	{
		ClearConfigCache();
		foreach (object registeredReceiver in _registeredReceivers)
		{
			EnvironmentManager.Instance.UnRegisterReceiver(registeredReceiver);
		}
		_registeredReceivers.Clear();
		foreach (ISystemInfo value in _systemInfoList.Values)
		{
			if (value.LoginSettings.Uri.Equals(uri) || uri.DnsSafeHost == value.LoginSettings.Uri.DnsSafeHost)
			{
				SecurityAccess.Logout(value.LoginSettings);
				MessageCommunicationManager.Stop(value.ServerId);
				LogClient.Instance.Close(value.ServerId.Id);
				value.Logout();
				return true;
			}
		}
		EnvironmentManager.Instance.Log(error: true, "SDKConfiguration.Logout(Uri)", "Unable to find URI=" + uri, null);
		return false;
	}

	void ISDKConfiguration.LogoutAllServers()
	{
		ClearConfigCache();
		SecurityAccess.Logoff();
		foreach (ISystemInfo value in _systemInfoList.Values)
		{
			MessageCommunicationManager.Stop(value.ServerId);
			LogClient.Instance.Close(value.ServerId.Id);
			value.Logout();
		}
	}

	public void RemoveAllServers()
	{
		ClearConfigCache();
		SecurityAccess.Logoff();
		lock (this)
		{
			foreach (ISystemInfo value in _systemInfoList.Values)
			{
				MessageCommunicationManager.Stop(value.ServerId);
				LogClient.Instance.Close(value.ServerId.Id);
				value.Logout();
				if (!EnvironmentManager.UsingUserContext)
				{
					LoginSettingsCache.RemoveLoginSettings(value.LoginSettings);
				}
			}
			_systemInfoList = new Dictionary<string, ISystemInfo>();
			_onlineSystemInfos = new Dictionary<string, ISystemInfo>();
		}
		_loginSettingsList = new List<LoginSettings>();
		_userContext?.Clear();
		base.ConfigurationApiManager.Close();
		_masterSystemInfo = null;
	}

	private List<string> GetSlaves(ISystemInfo systemInfo)
	{
		List<string> list = new List<string>();
		foreach (Guid slaveId in systemInfo.SlaveIds)
		{
			foreach (string key in _systemInfoList.Keys)
			{
				if (_systemInfoList[key].Id == slaveId)
				{
					list.Add(key);
				}
			}
		}
		return list;
	}

	public void RemoveServer(Guid id)
	{
		ClearConfigCache();
		List<string> list = new List<string>();
		foreach (string key in _systemInfoList.Keys)
		{
			ISystemInfo systemInfo = _systemInfoList[key];
			if (systemInfo.Id == id)
			{
				MessageCommunicationManager.Stop(systemInfo.ServerId);
				systemInfo.Disconnect();
				list.Add(key);
				list.AddRange(GetSlaves(systemInfo));
				if (_loginSettingsList.Contains(systemInfo.LoginSettings))
				{
					_loginSettingsList.Remove(systemInfo.LoginSettings);
				}
				if (_userContext.LoginSettings.Contains(systemInfo.LoginSettings))
				{
					_userContext.LoginSettings.Remove(systemInfo.LoginSettings);
				}
				if (_masterSystemInfo == systemInfo)
				{
					_masterSystemInfo = null;
				}
				break;
			}
		}
		if (list.Count == 0)
		{
			return;
		}
		lock (this)
		{
			foreach (string item in list)
			{
				_systemInfoList.Remove(item);
				_onlineSystemInfos.Remove(item);
			}
		}
	}

	public LoginSettings RemoveServer(Uri uri)
	{
		LoginSettings result = null;
		ClearConfigCache();
		List<string> list = new List<string>();
		foreach (string key in _systemInfoList.Keys)
		{
			ISystemInfo systemInfo = _systemInfoList[key];
			if (systemInfo.LoginSettings.Uri.Equals(uri) || uri.DnsSafeHost == systemInfo.LoginSettings.Uri.DnsSafeHost)
			{
				MessageCommunicationManager.Stop(systemInfo.ServerId);
				systemInfo.Disconnect();
				list.Add(key);
				list.AddRange(GetSlaves(systemInfo));
				result = systemInfo.LoginSettings;
				if (_loginSettingsList.Contains(systemInfo.LoginSettings))
				{
					_loginSettingsList.Remove(systemInfo.LoginSettings);
				}
				if (_userContext.LoginSettings.Contains(systemInfo.LoginSettings))
				{
					_userContext.LoginSettings.Remove(systemInfo.LoginSettings);
				}
				if (_masterSystemInfo == systemInfo)
				{
					_masterSystemInfo = null;
				}
				break;
			}
		}
		if (list.Count != 0)
		{
			lock (this)
			{
				foreach (string item in list)
				{
					_systemInfoList.Remove(item);
					_onlineSystemInfos.Remove(item);
				}
			}
		}
		return result;
	}

	private void ClearConfigCache()
	{
		RefreshConfiguration(Guid.Empty);
		_pluginCommonConfiguration.Clear();
		_pluginCommonConfigurationDataVersion.Clear();
		EnvironmentManager.Instance.LicenseManager.ReloadActivatedLicense();
	}

	bool ISDKConfiguration.CheckServerConnection(Uri uri)
	{
		if (_systemInfoList != null)
		{
			foreach (ISystemInfo value in _systemInfoList.Values)
			{
				if (value.LoginSettings.Uri.Equals(uri) || UriUtil.UriEqual(uri, value.LoginSettings.Uri, ignoreHttpHttpsDifference: true))
				{
					return value.ServerConnectionCheck();
				}
			}
		}
		return false;
	}

	public void Close()
	{
		ClearConfigCache();
		foreach (ISystemInfo value in _systemInfoList.Values)
		{
			if (value.IsConnected)
			{
				value.Disconnect();
			}
		}
		_systemInfoList = new Dictionary<string, ISystemInfo>();
		ServerOfflineCache.ServerOfflineCacheChanged -= ServerOfflineCache_ServerOfflineCacheChanged;
		_onlineSystemInfos = new Dictionary<string, ISystemInfo>();
	}

	public void UpdateServerId(string safeDnsName, string name2, Guid id, ServerTypes serverType)
	{
		if (_masterSystemInfo != null && (_masterSystemInfo.Name == safeDnsName || string.Compare(_masterSystemInfo.Name, name2, StringComparison.InvariantCultureIgnoreCase) == 0))
		{
			_masterSystemInfo.UpdateServerId(id);
		}
	}

	public override List<Item> GetItems()
	{
		return _configurationAsync.GetItems(Guid.Empty, ItemHierarchy.UserDefined);
	}

	public override List<Item> GetItems(ItemHierarchy itemHierarchy)
	{
		return _configurationAsync.GetItems(Guid.Empty, itemHierarchy);
	}

	public override void GetItemsAsync(AsyncItemsHandler handler, Control control, object callerReference)
	{
		_configurationAsync.RequestAsyncItems(handler, control, callerReference);
	}

	public override Item GetItem(FQID fqid)
	{
		return _configurationAsync.LookupItem(fqid);
	}

	public override void GetItemAsync(FQID fqid, AsyncItemHandler handler, Control control, object callerReference)
	{
		_configurationAsync.RequestAsyncLookupItem(fqid, handler, control, callerReference);
	}

	public override Item GetItem(ServerId serverId, Guid objectId, Guid kind)
	{
		return _configurationAsync.LookupItem(serverId, objectId, kind);
	}

	public override Item GetItem(Guid objectId, Guid kind)
	{
		return _configurationAsync.LookupItem(objectId, kind);
	}

	public override List<Item> GetItemsByKind(Guid kind)
	{
		return _configurationAsync.GetAllOfKind(kind, ItemHierarchy.UserDefined);
	}

	public override List<Item> GetItemsByKind(Guid kind, ItemHierarchy itemHierarchy)
	{
		return _configurationAsync.GetAllOfKind(kind, itemHierarchy);
	}

	public XmlNode GetPluginConfiguration(Guid platformDefinitionId, out bool wasEmpty)
	{
		try
		{
			wasEmpty = true;
			if (_masterSystemInfo == null)
			{
				return null;
			}
			if (_pluginCommonConfiguration[platformDefinitionId] == null)
			{
				int dataVersion = 0;
				XmlNode configuration = _configurationAsync.GetConfiguration(_masterSystemInfo.ServerId, platformDefinitionId, out dataVersion);
				if (configuration != null)
				{
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.LoadXml(configuration.OuterXml);
					_pluginCommonConfiguration[platformDefinitionId] = xmlDocument;
					_pluginCommonConfigurationDataVersion[platformDefinitionId] = (long)dataVersion;
				}
			}
			XmlDocument xmlDocument2 = (XmlDocument)_pluginCommonConfiguration[platformDefinitionId];
			if (xmlDocument2 == null)
			{
				wasEmpty = true;
				xmlDocument2 = new XmlDocument();
				xmlDocument2.LoadXml(CreateInitialXml(platformDefinitionId));
				_pluginCommonConfiguration[platformDefinitionId] = xmlDocument2;
				_pluginCommonConfigurationDataVersion[platformDefinitionId] = 0L;
			}
			else
			{
				wasEmpty = false;
			}
			return xmlDocument2.SelectSingleNode("/PlatformConfiguration");
		}
		catch (Exception ex)
		{
			EnvironmentManager instance = EnvironmentManager.Instance;
			string message = ex.Message;
			object[] args = new Exception[1] { ex };
			instance.Log(error: true, "Configuration.GetPluginConfiguration", message, args);
			throw;
		}
	}

	private static string CreateInitialXml(Guid platformDefinitionId)
	{
		return "<PlatformConfiguration ID=\"" + platformDefinitionId.ToString() + "\"><Configurations /></PlatformConfiguration>";
	}

	public override List<Item> GetItemConfigurations(ServerId serverId, Guid pluginDefinitionId, Item parentItem, Guid kind)
	{
		if (base.IsMIPItemSupported)
		{
			return base.GetItemConfigurations(serverId, pluginDefinitionId, parentItem, kind);
		}
		int dataVersion;
		XmlNode configuration = _configurationAsync.GetConfiguration(serverId, pluginDefinitionId, out dataVersion);
		List<Item> list = new List<Item>();
		if (kind == Guid.Empty || configuration == null)
		{
			return list;
		}
		XmlNodeList xmlNodeList = configuration.SelectNodes("Configurations/GUID" + kind.ToString());
		if (xmlNodeList != null)
		{
			foreach (XmlNode item2 in xmlNodeList)
			{
				Item item = ConfigurationUtil.DeserializeItem(item2);
				item.FQID.ServerId.UserContext = UserContext;
				if (parentItem == null)
				{
					list.Add(item);
				}
				else if (item.FQID.ParentId == parentItem.FQID.ObjectId || item.FQID.ParentId == parentItem.FQID.ServerId.Id)
				{
					list.Add(item);
				}
			}
		}
		return list;
	}

	public override Item GetItemConfiguration(ServerId serverId, Guid pluginDefinitionId, Guid kind, Guid objectId)
	{
		if (kind == Guid.Empty)
		{
			return null;
		}
		if (base.IsMIPItemSupported)
		{
			return base.GetItemConfiguration(serverId, pluginDefinitionId, kind, objectId);
		}
		int dataVersion;
		XmlNode configuration = _configurationAsync.GetConfiguration(serverId, pluginDefinitionId, out dataVersion);
		if (configuration == null)
		{
			return null;
		}
		XmlNode xmlNode = configuration.SelectSingleNode("Configurations/GUID" + kind.ToString() + "[@itemID=\"" + objectId.ToString() + "\"]");
		if (xmlNode == null)
		{
			return null;
		}
		Item item = ConfigurationUtil.DeserializeItem(xmlNode);
		item.FQID.ServerId.UserContext = UserContext;
		return item;
	}

	private XmlNode GetItemConfigurationAsXml(ServerId serverId, Guid pluginDefinitionId, Guid kind, Guid objectId, out XmlNode rootNode, out int dataVersion)
	{
		rootNode = null;
		dataVersion = 0;
		if (kind == Guid.Empty)
		{
			return null;
		}
		rootNode = _configurationAsync.GetConfiguration(serverId, pluginDefinitionId, out dataVersion);
		if (rootNode == null)
		{
			return null;
		}
		return rootNode.SelectSingleNode("Configurations/GUID" + kind.ToString() + "[@itemID=\"" + objectId.ToString() + "\"]");
	}

	public override List<Item> GetItemConfigurations(Guid platformDefinitionId, Item parentItem, Guid kind)
	{
		if (base.IsMIPItemSupported)
		{
			return base.GetItemConfigurations(platformDefinitionId, parentItem, kind);
		}
		try
		{
			_kindToPluginId[kind] = platformDefinitionId;
			List<Item> list = new List<Item>();
			bool wasEmpty;
			XmlNode pluginConfiguration = GetPluginConfiguration(platformDefinitionId, out wasEmpty);
			if (kind == Guid.Empty || pluginConfiguration == null)
			{
				return list;
			}
			XmlNodeList xmlNodeList = pluginConfiguration.SelectNodes("Configurations/GUID" + kind.ToString());
			if (xmlNodeList != null)
			{
				foreach (XmlNode item2 in xmlNodeList)
				{
					Item item = ConfigurationUtil.DeserializeItem(item2);
					item.FQID.ServerId.UserContext = UserContext;
					if (parentItem == null)
					{
						list.Add(item);
					}
					else if (item.FQID.ParentId == parentItem.FQID.ObjectId || item.FQID.ParentId == parentItem.FQID.ServerId.Id)
					{
						list.Add(item);
					}
				}
			}
			return list;
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("GetConfigurationForItemType", exception);
			return null;
		}
	}

	public override Item GetItemConfiguration(Guid platformDefinitionId, Guid itemKind, Guid itemId)
	{
		if (base.IsMIPItemSupported)
		{
			return base.GetItemConfiguration(platformDefinitionId, itemKind, itemId);
		}
		_kindToPluginId[itemKind] = platformDefinitionId;
		bool wasEmpty;
		XmlNode pluginConfiguration = GetPluginConfiguration(platformDefinitionId, out wasEmpty);
		if (pluginConfiguration == null || wasEmpty || string.IsNullOrEmpty(pluginConfiguration.InnerText))
		{
			ServerId serverId = EnvironmentManager.Instance.MasterSite.ServerId;
			if (itemKind == _licenseKindId && (itemId == _legacyRegistrationsObjectId || itemId == _registrationsObjectId))
			{
				ISystemInfo systemInfo = FindSystemInfo(serverId);
				if (systemInfo != null)
				{
					ISystemLicenseInfo systemLicenseInfo = systemInfo.LoginServer.GetSystemLicenseInfo();
					if (systemLicenseInfo != null && systemLicenseInfo.HasLicense)
					{
						Item item = new Item(new FQID(serverId, FolderType.No, itemKind), "Registrations");
						string text = "";
						string[] features = systemLicenseInfo.Features;
						foreach (string text2 in features)
						{
							text = text + text2 + ",";
						}
						item.Properties["Features"] = text;
						item.Properties["SLC"] = systemLicenseInfo.SLC;
						if (systemLicenseInfo.Expires)
						{
							item.Properties["Expire"] = systemLicenseInfo.ExpiryDate.ToString();
						}
						return item;
					}
				}
			}
			return null;
		}
		XmlNode xmlNode = pluginConfiguration.SelectSingleNode("Configurations/GUID" + itemKind.ToString() + "[@itemID=\"" + itemId.ToString() + "\"]");
		if (xmlNode == null)
		{
			return null;
		}
		Item item2 = ConfigurationUtil.DeserializeItem(xmlNode);
		item2.FQID.ServerId.UserContext = UserContext;
		return item2;
	}

	public override long GetItemConfigurationDataVersion(Guid itemKind)
	{
		if (base.IsMIPItemSupported)
		{
			return base.GetItemConfigurationDataVersion(itemKind);
		}
		PluginDefinition pluginDefinitionFromKind = PluginManager.GetPluginDefinitionFromKind(itemKind);
		if (pluginDefinitionFromKind != null)
		{
			if (_pluginCommonConfigurationDataVersion[pluginDefinitionFromKind.Id] != null)
			{
				return (long)_pluginCommonConfigurationDataVersion[pluginDefinitionFromKind.Id];
			}
		}
		else if (itemKind == _licenseKindId && _pluginCommonConfigurationDataVersion[itemKind] != null)
		{
			return (long)_pluginCommonConfigurationDataVersion[itemKind];
		}
		return 0L;
	}

	public override XmlNode GetOptionsConfiguration(Guid optionsDialogId, bool userPrivate)
	{
		if (base.IsMIPItemSupported)
		{
			try
			{
				MIPOptionProperties mipOptionProperties = new MIPKindFolder(EnvironmentManager.Instance.MasterSite.ServerId).GetMipOptionProperties(optionsDialogId, userPrivate);
				if (mipOptionProperties == null)
				{
					return null;
				}
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(mipOptionProperties.PropertiesXml);
				return xmlDocument.FirstChild;
			}
			catch (Exception)
			{
			}
		}
		int dataVersion = 0;
		return _configurationAsync.GetConfiguration(_masterSystemInfo.ServerId, optionsDialogId, out dataVersion, userPrivate);
	}

	public override void SaveOptionsConfiguration(Guid optionsDialogId, bool userPrivate, XmlNode xmlNode)
	{
		if (xmlNode == null)
		{
			throw new ArgumentNullException("xmlNode");
		}
		if (base.IsMIPItemSupported)
		{
			try
			{
				if (xmlNode.ChildNodes[0] is XmlDeclaration)
				{
					xmlNode = xmlNode.ChildNodes[1];
				}
				MIPKindFolder mIPKindFolder = new MIPKindFolder(EnvironmentManager.Instance.MasterSite.ServerId);
				MIPOptionProperties mIPOptionProperties = mIPKindFolder.GetMipOptionProperties(optionsDialogId, userPrivate);
				if (mIPOptionProperties == null)
				{
					mIPOptionProperties = new MIPOptionProperties
					{
						DataVersion = 0,
						OptionId = optionsDialogId,
						PropertiesXml = ""
					};
				}
				if (mIPOptionProperties.PropertiesXml != xmlNode.OuterXml)
				{
					mIPOptionProperties.PropertiesXml = xmlNode.OuterXml;
					mIPOptionProperties.DataVersion++;
					mIPKindFolder.SetMipOptionProperties(mIPOptionProperties, userPrivate);
				}
				return;
			}
			catch (Exception)
			{
			}
		}
		try
		{
			if (xmlNode.ChildNodes[0] is XmlDeclaration)
			{
				xmlNode = xmlNode.ChildNodes[1];
			}
			int dataVersion = 0;
			_configurationAsync.GetConfiguration(_masterSystemInfo.ServerId, optionsDialogId, out dataVersion, userPrivate);
			dataVersion++;
			_configurationAsync.SaveCustomSetting(_masterSystemInfo.ServerId, optionsDialogId, dataVersion, xmlNode, !userPrivate);
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("SaveOptionsConfiguration:", exception);
		}
	}

	public override void DeleteItemConfiguration(ServerId serverId, Guid platformDefinitionId, Item item)
	{
		if (base.IsMIPItemSupported)
		{
			base.DeleteItemConfiguration(serverId, platformDefinitionId, item);
			return;
		}
		throw new NotImplementedException();
	}

	public override void DeleteItemConfiguration(Guid platformDefinitionId, Item item)
	{
		if (base.IsMIPItemSupported)
		{
			base.DeleteItemConfiguration(platformDefinitionId, item);
			return;
		}
		throw new NotImplementedException();
	}

	public override void SaveItemConfiguration(ServerId serverId, Guid pluginDefinitionId, Item item)
	{
		if (base.IsMIPItemSupported)
		{
			base.SaveItemConfiguration(serverId, pluginDefinitionId, item);
			return;
		}
		Guid objectId = ((item.FQID.ObjectId == Guid.Empty) ? item.FQID.ServerId.Id : item.FQID.ObjectId);
		try
		{
			XmlNode rootNode;
			int dataVersion;
			XmlNode itemConfigurationAsXml = GetItemConfigurationAsXml(serverId, pluginDefinitionId, item.FQID.Kind, objectId, out rootNode, out dataVersion);
			if (rootNode == null)
			{
				rootNode = CreateInitialXmlNode(pluginDefinitionId);
			}
			if (ConfigurationUtil.SaveItemConfiguration(rootNode, itemConfigurationAsXml, item))
			{
				dataVersion++;
				_configurationAsync.SaveCustomSetting(serverId, pluginDefinitionId, dataVersion, rootNode, global: true);
				StoreUpdatedDataVersion(serverId, pluginDefinitionId, item.FQID.Kind, dataVersion);
			}
			RefreshConfiguration(pluginDefinitionId);
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("SaveItemConfiguration", exception);
		}
	}

	public override void SaveItemConfiguration(Guid pluginDefinitionId, Item item)
	{
		if (base.IsMIPItemSupported)
		{
			base.SaveItemConfiguration(pluginDefinitionId, item);
		}
		else
		{
			SaveItemConfiguration(_masterSystemInfo.ServerId, pluginDefinitionId, item);
		}
	}

	private static XmlNode CreateInitialXmlNode(Guid platformDefinitionId)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(CreateInitialXml(platformDefinitionId));
		return xmlDocument.SelectSingleNode("/PlatformConfiguration");
	}

	private void StoreUpdatedDataVersion(ServerId serverId, Guid pluginId, Guid kind, int version)
	{
		try
		{
			int dataVersion;
			XmlNode xmlNode = _configurationAsync.GetConfiguration(serverId, _configServiceId, out dataVersion);
			if (xmlNode == null)
			{
				xmlNode = CreateInitialXmlNode(_configServiceId);
			}
			dataVersion++;
			string text = "";
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("Configurations/GUID" + _configServiceKind.ToString() + "[@itemID=\"" + kind.ToString() + "\"]");
			Item item;
			if (xmlNode2 != null)
			{
				item = ConfigurationUtil.DeserializeItem(xmlNode2);
				item.Properties["DataVersion"] = version.ToString() ?? "";
				Dictionary<string, string> properties = item.Properties;
				Guid guid = pluginId;
				properties["PluginId"] = guid.ToString() ?? "";
				item.Properties["PluginName"] = text ?? "";
			}
			else
			{
				item = new Item(new ServerId("N/A", "N/A", 0, _configServiceId), Guid.Empty, kind, "Update", FolderType.No, _configServiceKind);
				item.Properties["DataVersion"] = version.ToString() ?? "";
				Dictionary<string, string> properties2 = item.Properties;
				Guid guid = pluginId;
				properties2["PluginId"] = guid.ToString() ?? "";
				item.Properties["PluginName"] = text ?? "";
			}
			ConfigurationUtil.SaveItemConfiguration(xmlNode, xmlNode2, item);
			_configurationAsync.SaveCustomSetting(_masterSystemInfo.ServerId, _configServiceId, dataVersion, xmlNode, global: true);
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("MIP:StoreUpdatedDataVersions", exception);
		}
	}

	public override void RefreshConfiguration(Guid platformDefinitionId)
	{
		base.RefreshConfiguration(platformDefinitionId);
		if (_pluginCommonConfiguration.ContainsKey(platformDefinitionId))
		{
			_pluginCommonConfiguration.Remove(platformDefinitionId);
			_pluginCommonConfigurationDataVersion[platformDefinitionId] = 0L;
		}
		else if (_kindToPluginId[platformDefinitionId] != null)
		{
			object obj = _kindToPluginId[platformDefinitionId];
			if (obj != null && _pluginCommonConfiguration.ContainsKey(obj))
			{
				_pluginCommonConfiguration.Remove(obj);
				_pluginCommonConfigurationDataVersion[obj] = 0L;
			}
		}
	}

	public Tuple<DateTime, string> GetliveJpeg(FQID cameraFqid, int width, int height)
	{
		return _configurationAsync.GetLiveJPEG(cameraFqid, width, height);
	}

	public override ReadOnlyCollection<ViewGroupInfo> GetViewGroups()
	{
		try
		{
			Collection<ViewGroupInfo> collection = new Collection<ViewGroupInfo>();
			ISystemInfo systemInfo = FindSystemInfo(ServerFQID.ServerId);
			if (systemInfo != null)
			{
				IViewGroupInfo[] viewGroups = systemInfo.LoginServer.GetViewGroups();
				foreach (IViewGroupInfo viewGroupInfo in viewGroups)
				{
					collection.Add(new ViewGroupInfo
					{
						Id = viewGroupInfo.Id,
						Name = viewGroupInfo.Name,
						LastModifiedTime = viewGroupInfo.LastModifiedTime,
						LastModifiedUser = viewGroupInfo.LastModifiedUser,
						ReadOnly = viewGroupInfo.ReadOnly,
						ViewGroupType = (VideoOS.Platform.Client.ViewGroupType)viewGroupInfo.ViewGroupType
					});
				}
			}
			return new ReadOnlyCollection<ViewGroupInfo>(collection);
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("Configuration.GetViewGroups()", exception);
			throw;
		}
	}

	public override VideoOS.Platform.Client.ViewGroup GetViewGroup(Guid groupId)
	{
		try
		{
			ISystemInfo systemInfo = FindSystemInfo(ServerFQID.ServerId);
			if (systemInfo != null)
			{
				IViewGroup viewGroupData = systemInfo.LoginServer.GetViewGroupData(groupId);
				IViewGroupInfo viewGroupInfo = viewGroupData.ViewGroupInfo;
				return new VideoOS.Platform.Client.ViewGroup
				{
					ViewGroupInfo = new ViewGroupInfo
					{
						Id = viewGroupInfo.Id,
						Name = viewGroupInfo.Name,
						LastModifiedTime = viewGroupInfo.LastModifiedTime,
						LastModifiedUser = viewGroupInfo.LastModifiedUser,
						ReadOnly = viewGroupInfo.ReadOnly,
						ViewGroupType = (VideoOS.Platform.Client.ViewGroupType)viewGroupInfo.ViewGroupType
					},
					ViewGroupData = new ViewGroupData
					{
						DataVersion = viewGroupData.ViewGroupData.DataVersion,
						XmlVersion = viewGroupData.ViewGroupData.XmlVersion,
						ViewGroupDataXml = viewGroupData.ViewGroupData.ViewGroupDataXml
					}
				};
			}
			return null;
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("Configuration.GetViewGroup( id )", exception);
			throw;
		}
	}

	public ISystemInfo FindSystemInfo(ServerId serverId, bool populate = false)
	{
		if (serverId.Id != Guid.Empty)
		{
			foreach (ISystemInfo value in _systemInfoList.Values)
			{
				if (value.Id == serverId.Id)
				{
					if (populate)
					{
						value.Populate();
					}
					return value;
				}
				if (value.Recorders.ContainsKey(serverId.Id) && value.Recorders[serverId.Id] != null)
				{
					if (populate)
					{
						value.Populate();
					}
					return value;
				}
			}
			foreach (ISystemInfo value2 in _systemInfoList.Values)
			{
				if (value2.Id == Guid.Empty && value2.LoginServer == null)
				{
					value2.Login();
				}
				if (value2.Id == serverId.Id)
				{
					if (populate)
					{
						value2.Populate();
					}
					return value2;
				}
			}
			foreach (ISystemInfo value3 in _systemInfoList.Values)
			{
				if (value3.Recorders.ContainsKey(serverId.Id) && value3.Recorders[serverId.Id] != null)
				{
					if (populate)
					{
						value3.Populate();
					}
					return value3;
				}
			}
		}
		ISystemInfo systemInfo = LookupSystemInfo(serverId.ServerHostname, serverId.Serverport);
		if (systemInfo != null)
		{
			if (populate)
			{
				systemInfo.Populate();
			}
			return systemInfo;
		}
		string text = serverId.ServerHostname.ToLower();
		if (text.StartsWith("http://"))
		{
			text = text.Substring("http://".Length);
		}
		if (text.StartsWith("https://"))
		{
			text = text.Substring("https://".Length);
		}
		if (text.EndsWith("/"))
		{
			text = text.Substring(0, text.Length - 1);
		}
		int num = text.IndexOf(':');
		int result = 0;
		if (num > 0)
		{
			if (text.Length > num + 1)
			{
				int.TryParse(text.Substring(num + 1), out result);
			}
			text = text.Substring(0, num);
		}
		systemInfo = LookupSystemInfo(text, result);
		if (systemInfo != null)
		{
			if (populate)
			{
				systemInfo.Populate();
			}
			return systemInfo;
		}
		return null;
	}

	private ISystemInfo LookupSystemInfo(string safename, int port)
	{
		foreach (ISystemInfo value in _systemInfoList.Values)
		{
			if (value.Hostname == safename)
			{
				if (value.Port == port)
				{
					return value;
				}
				if (value.Port == 80 && port == 0)
				{
					return value;
				}
			}
		}
		return null;
	}

	List<Item> ISDKConfiguration.GetDisabledDevices(Guid serverGuid)
	{
		List<Item> list = new List<Item>();
		foreach (ISystemInfo value in _systemInfoList.Values)
		{
			if (!(value.ServerId.Id == serverGuid))
			{
				continue;
			}
			if (!value.IsLoggedIn())
			{
				throw new CommunicationMIPException("Server not connected");
			}
			if (value.ServerType != ServerTypes.CORPORATE)
			{
				return list;
			}
			try
			{
				value.Populate();
				IDeviceDisabledInfo[] disabledDevices = value.LoginServer.GetDisabledDevices();
				foreach (IDeviceDisabledInfo deviceDisabledInfo in disabledDevices)
				{
					if (value.Recorders.ContainsKey(deviceDisabledInfo.RecorderId))
					{
						IRecorder recorder = value.Recorders[deviceDisabledInfo.RecorderId];
						list.Add(new Item(recorder.ServerId, recorder.ServerId.Id, deviceDisabledInfo.DeviceId, deviceDisabledInfo.DeviceName, FolderType.No, ConvertToKind(deviceDisabledInfo.DeviceType))
						{
							Enabled = false
						});
					}
					else
					{
						EnvironmentManager.Instance.Log(error: true, "GetDisabledDevices", "Recorder not found for disabled device:" + deviceDisabledInfo.RecorderId.ToString());
					}
				}
			}
			catch (Exception)
			{
			}
		}
		return list;
	}

	private static Guid ConvertToKind(string deviceType)
	{
		return deviceType switch
		{
			"Camera" => Kind.Camera, 
			"Microphone" => Kind.Microphone, 
			"Speaker" => Kind.Speaker, 
			"Metadata" => Kind.Metadata, 
			"Input" => Kind.InputEvent, 
			"Output" => Kind.Output, 
			_ => Guid.Empty, 
		};
	}

	public IChange[] QueryChanges(DateTime changesFrom, Guid systemId)
	{
		foreach (ISystemInfo value in _systemInfoList.Values)
		{
			if (value.Id == systemId && value.IsLoggedIn())
			{
				return value.QueryChanges(changesFrom);
			}
		}
		return null;
	}

	public Dictionary<Guid, TimeSpan> GetFailoverCheckIntervals(TimeSpan defaultInterval)
	{
		Dictionary<Guid, TimeSpan> dictionary = new Dictionary<Guid, TimeSpan>();
		foreach (ISystemInfo value in _systemInfoList.Values)
		{
			if (value.IsLoggedIn())
			{
				if (dictionary.ContainsKey(value.Id))
				{
					EnvironmentManager.Instance.Log(error: true, "FailoverCheckInterval", "Duplicate ID:" + value.Id.ToString() + " (Ignored)");
				}
				else
				{
					dictionary.Add(value.Id, defaultInterval);
				}
			}
		}
		return dictionary;
	}

	public List<IRecorder> GetRecorders(Guid systemId)
	{
		List<IRecorder> list = new List<IRecorder>();
		foreach (ISystemInfo item in _systemInfoList.Values.Where((ISystemInfo si) => si.Id == systemId))
		{
			list.AddRange(item.Recorders);
		}
		return list;
	}

	public void OnRecorderInfoChange(IChange[] changes)
	{
		HashSet<IRecorder> hashSet = new HashSet<IRecorder>();
		List<FQID> list = new List<FQID>();
		try
		{
			foreach (ISystemInfo value2 in _systemInfoList.Values)
			{
				hashSet.Clear();
				foreach (IChange change in changes)
				{
					Guid guid = new Guid(change.Id);
					if (change.ChangeType.Recorder)
					{
						bool flag = false;
						foreach (IRecorder recorder6 in value2.Recorders)
						{
							if (guid == recorder6.Guid)
							{
								flag = true;
								hashSet.Add(recorder6);
								list.Add(ToFQID(recorder6));
							}
						}
						if (!flag)
						{
							IRecorderInfo[] recorderInfos = value2.LoginServer.GetRecorderInfos(new Guid[1] { guid });
							if (recorderInfos.Any())
							{
								Recorder recorder = AddRecorder(value2, recorderInfos[0]);
								list.AddRange(GetAllFQIDs(recorder));
								hashSet.Add(recorder);
							}
						}
					}
					else
					{
						if (!change.ChangeType.Hardware)
						{
							continue;
						}
						bool flag2 = false;
						foreach (IRecorder recorder7 in value2.Recorders)
						{
							if (!(recorder7 is Recorder) || !recorder7.Hardware.TryGetValue(guid, out var value))
							{
								continue;
							}
							flag2 = true;
							IHardwareInfo[] hardwareInfos = value2.LoginServer.GetHardwareInfos(new Guid[1] { guid });
							if (hardwareInfos.Length != 1)
							{
								EnvironmentManager.Instance.Log(error: true, "SDKConfiguration", "Unable to update hardware change");
								continue;
							}
							IHardwareInfo hardwareInfo = hardwareInfos[0];
							bool num = value.RecorderId != hardwareInfo.RecorderId;
							value.Uri = hardwareInfo.Uri;
							value.Description = hardwareInfo.Description;
							value.Name = hardwareInfo.Name;
							value.LastModified = hardwareInfo.LastModified;
							if (num)
							{
								List<object> list2 = ((Recorder)recorder7).RemoveHardware(guid);
								if (!value2.Recorders.ContainsKey(hardwareInfo.RecorderId))
								{
									IRecorderInfo[] recorderInfos2 = value2.LoginServer.GetRecorderInfos(new Guid[1] { hardwareInfo.RecorderId });
									if (recorderInfos2.Length != 1)
									{
										EnvironmentManager.Instance.Log(error: true, "SDKConfiguration", "Unable to move hardware to new recorder");
										continue;
									}
									Recorder recorder2 = new Recorder(value2, recorderInfos2[0]);
									value2.Recorders.Add(recorder2);
									value2.RebuildDeviceCollections();
									list.AddRange(ToFQIDForAllDevices(recorder2, value, list2));
									break;
								}
								Recorder recorder3 = (Recorder)value2.Recorders[hardwareInfo.RecorderId];
								recorder3.AddHardware(list2);
								list.AddRange(ToFQIDForAllDevices(recorder3, value, list2));
								break;
							}
							list.AddRange(ToFQIDForAllDevices(recorder7, value, null));
							break;
						}
						if (flag2)
						{
							continue;
						}
						IHardwareInfo[] hardwareInfos2 = value2.LoginServer.GetHardwareInfos(new Guid[1] { guid });
						if (!hardwareInfos2.Any())
						{
							continue;
						}
						IHardwareInfo hardwareInfo2 = hardwareInfos2.First();
						IRecorderInfo[] recorderInfos3 = value2.LoginServer.GetRecorderInfos(new Guid[1] { hardwareInfo2.RecorderId });
						if (!recorderInfos3.Any())
						{
							continue;
						}
						if (value2.Recorders.ContainsKey(hardwareInfo2.RecorderId))
						{
							if (value2.Recorders[hardwareInfo2.RecorderId] is Recorder recorder4)
							{
								value2.UpdateLastHardwareUpdateIfNewer(hardwareInfo2.LastModified);
								recorder4.UpdateRecorder(recorderInfos3.First());
								list.AddRange(ToFQIDForAllDevices(recorder4, hardwareInfo2));
								hashSet.Add(recorder4);
							}
						}
						else
						{
							Recorder recorder5 = AddRecorder(value2, recorderInfos3[0]);
							list.AddRange(GetAllFQIDs(recorder5));
							hashSet.Add(recorder5);
						}
					}
				}
				FQID relatedFQID = null;
				foreach (IRecorder item in hashSet)
				{
					IRecorderInfo recorderInfo = value2.QueryRecorderInfo(item.Guid);
					((Recorder)item).LastModified = recorderInfo.LastModified;
					Uri uri = new Uri(recorderInfo.WebServerUri);
					if (uri != item.WebServerUri)
					{
						((Recorder)item).WebServerUri = uri;
						relatedFQID = new FQID(item.ServerId, FolderType.SystemDefined, Kind.Server);
						EnvironmentManager.Instance.PostMessage(new VideoOS.Platform.Messaging.Message("Server.RecorderFQIDChangedIndication")
						{
							RelatedFQID = relatedFQID
						});
					}
				}
				EnvironmentManager.Instance.PostMessage(new VideoOS.Platform.Messaging.Message(MessageId.System.LocalConfigurationChangedIndication)
				{
					Data = list,
					RelatedFQID = relatedFQID
				});
			}
		}
		catch (Exception ex)
		{
			Trace.WriteLine("OnRecorderInfoChange() Exception");
			EnvironmentManager instance = EnvironmentManager.Instance;
			string message = ex.Message;
			object[] args = new Exception[1] { ex };
			instance.Log(error: true, "SDKConfiguration.OnRecorderInfoChange", message, args);
		}
	}

	private static Recorder AddRecorder(ISystemInfo systemInfo, IRecorderInfo recorderInfo)
	{
		Recorder recorder = new Recorder(systemInfo, recorderInfo);
		systemInfo.Recorders.Add(recorder);
		systemInfo.RebuildDeviceCollections();
		return recorder;
	}

	private static FQID ToFQID(object source)
	{
		if (source is IRecorder recorder)
		{
			return new FQID(recorder.ServerId, FolderType.SystemDefined, Kind.Server);
		}
		return null;
	}

	private static IEnumerable<FQID> GetAllFQIDs(IRecorder recorder)
	{
		List<FQID> list = new List<FQID>();
		list.Add(new FQID(recorder.ServerId, FolderType.SystemDefined, Kind.Server));
		foreach (IHardwareInfo value in recorder.Hardware.Values)
		{
			list.AddRange(ToFQIDForAllDevices(recorder, value));
		}
		return list;
	}

	private static List<FQID> ToFQIDForAllDevices(IRecorder recorder, IHardwareInfo hardware)
	{
		List<FQID> list = new List<FQID>();
		list.Add(new FQID(recorder.ServerId, hardware.HardwareId, hardware.HardwareId, FolderType.No, Kind.Hardware));
		Guid[] deviceIds = hardware.DeviceIds;
		foreach (Guid guid in deviceIds)
		{
			Guid guid2 = Guid.Empty;
			if (recorder.Cameras.ContainsKey(guid))
			{
				guid2 = Kind.Camera;
			}
			else if (recorder.Microphones.ContainsKey(guid))
			{
				guid2 = Kind.Microphone;
			}
			else if (recorder.Speakers.ContainsKey(guid))
			{
				guid2 = Kind.Speaker;
			}
			else if (recorder.MetadataDevices.ContainsKey(guid))
			{
				guid2 = Kind.Metadata;
			}
			else if (recorder.Events.ContainsKey(guid))
			{
				guid2 = Kind.InputEvent;
			}
			else if (recorder.OutputDevices.ContainsKey(guid))
			{
				guid2 = Kind.Output;
			}
			if (guid2 != Guid.Empty)
			{
				list.Add(new FQID(recorder.ServerId, recorder.ServerId.Id, guid, FolderType.No, guid2));
			}
		}
		return list;
	}

	private static List<FQID> ToFQIDForAllDevices(IRecorder recorder, IHardwareInfo hardware, List<object> devices)
	{
		List<FQID> list = new List<FQID>();
		list.Add(new FQID(recorder.ServerId, hardware.HardwareId, hardware.HardwareId, FolderType.No, Kind.Hardware));
		if (devices != null)
		{
			foreach (object device in devices)
			{
				ICamera camera = device as ICamera;
				IMicrophone microphone = device as IMicrophone;
				VideoOS.Platform.SDK.Config.Speaker speaker = device as VideoOS.Platform.SDK.Config.Speaker;
				OutputDevice outputDevice = device as OutputDevice;
				VideoOS.Platform.SDK.Config.UserDefinedEvent userDefinedEvent = device as VideoOS.Platform.SDK.Config.UserDefinedEvent;
				IMetadataDevice metadataDevice = device as IMetadataDevice;
				Guid objectId = Guid.Empty;
				Guid guid = Guid.Empty;
				if (camera != null)
				{
					objectId = camera.Guid;
					guid = Kind.Camera;
					camera.Recorder = recorder;
				}
				else if (microphone != null)
				{
					objectId = microphone.Guid;
					guid = Kind.Microphone;
					microphone.Recorder = recorder;
				}
				else if (speaker != null)
				{
					objectId = speaker.Guid;
					guid = Kind.Speaker;
					speaker.Recorder = recorder;
				}
				else if (outputDevice != null)
				{
					objectId = outputDevice.Guid;
					guid = Kind.Output;
					outputDevice.Recorder = recorder;
				}
				else if (userDefinedEvent != null)
				{
					objectId = userDefinedEvent.Guid;
					guid = Kind.Output;
					userDefinedEvent.Recorder = recorder;
				}
				else if (metadataDevice != null)
				{
					objectId = metadataDevice.Guid;
					guid = Kind.Metadata;
					metadataDevice.Recorder = recorder;
				}
				if (guid != Guid.Empty)
				{
					list.Add(new FQID(recorder.ServerId, recorder.ServerId.Id, objectId, FolderType.No, guid));
				}
			}
		}
		return list;
	}

	void ISDKConfiguration.RefreshServer(FQID serverFqid)
	{
		if (serverFqid.ServerId.ServerType == "XPCORS")
		{
			RefreshRecorder(serverFqid);
			return;
		}
		try
		{
			ISystemInfo systemInfo = GetSystemInfo(serverFqid.ServerId.Id);
			if (systemInfo == null)
			{
				EnvironmentManager.Instance.Log(error: true, "RefreshServer", "Server identified by FQID not found - ignored");
			}
			else if (systemInfo is SystemInfo systemInfo2 && systemInfo2.LoginServer.GetSystemInfoAndLogin())
			{
				systemInfo2.RebuildConfiguration();
				EnvironmentManager.Instance.Log(error: false, "SDKConfiguration", "System configuration reloaded: " + systemInfo2.Name);
			}
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("SDKConfiguration", "RefreshServer", exception);
		}
	}

	private bool TryLogin(Uri uri, bool masterOnly)
	{
		ISystemInfo systemInfo = ((ISDKConfiguration)this).GetSystemInfo(uri);
		if (systemInfo == null)
		{
			return false;
		}
		if (!systemInfo.IsLoggedIn())
		{
			systemInfo.LastException = null;
			systemInfo.MasterOnly = masterOnly;
			systemInfo.Login();
		}
		return systemInfo.IsLoggedIn();
	}

	private void RefreshRecorder(FQID recorderFqid)
	{
		try
		{
			ISystemInfo systemInfo = GetSystemInfo(recorderFqid.ParentId);
			if (systemInfo == null)
			{
				EnvironmentManager.Instance.Log(error: true, "RefreshRecorder", "Recorder identified by FQID not found - ignored");
			}
			else
			{
				systemInfo.RefreshRecorder(recorderFqid);
			}
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("SDKConfiguration", "RefreshRecorder", exception);
		}
	}

	public DateTime GetLastHardwareUpdate(Guid systemId)
	{
		return _systemInfoList.Values.FirstOrDefault((ISystemInfo si) => si.Id == systemId)?.LastHardwareUpdate ?? DateTime.MinValue;
	}

	private static string GetMachineInstanceId()
	{
		return System.Environment.MachineName.GetHashCode().ToString();
	}

	public void RegisterIntegration(Uri serverUri, Guid integrationId, string integrationName, string version, string manufacturerName)
	{
		if (integrationId != Guid.Empty && !EnvironmentManager.Instance.EnvironmentOptions.ContainsKey("RestApiCallerId"))
		{
			EnvironmentManager.Instance.EnvironmentOptions["RestApiCallerId"] = integrationId.ToString();
		}
		ISystemInfo systemInfo = ((ISDKConfiguration)this).GetSystemInfo(serverUri);
		if (systemInfo != null && systemInfo.ServerType == ServerTypes.CORPORATE)
		{
			systemInfo.RegisterIntegration(GetMachineInstanceId(), integrationId, integrationName, version, manufacturerName);
		}
	}
}
