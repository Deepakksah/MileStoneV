using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Windows.Media;
using VideoOS.Platform.Login;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Facades;
using VideoOS.Platform.SDK.Platform;
using VideoOS.Platform.SDK.Server;
using VideoOS.Platform.SDK.Util;

namespace VideoOS.Platform.SDK;

public class EnvironmentService : IEnvironmentService
{
	private readonly UserContext _userContext;

	private readonly ISDKEnvironmentManager _environmentManager;

	private readonly ISDKConfiguration _configuration;

	private readonly ICredentialCacheService _credentialCacheService;

	private readonly ISiteUtil _siteUtil;

	private readonly IClientControlFacade _clientControlFacade;

	private readonly IPluginManagerFacade _pluginManagerFacade;

	private readonly ILoginSettingsCacheFacade _loginSettingsCacheFacade;

	private readonly IServerTypeCacheFacade _serverTypeCacheFacade;

	private readonly IServerOfflineCacheFacade _serverOfflineCacheFacade;

	public IEnvironmentProperties Properties { get; private set; }

	protected IQueryConfigurationChanges QueryChangesInstance => Properties.QueryConfigurationChanges;

	public EnvironmentService(UserContext userContext, IEnvironmentProperties environmentProperties, ISDKEnvironmentManager environmentManager, ISDKConfiguration configuration, ICredentialCacheService credentialCacheService, ISiteUtil siteUtil, IClientControlFacade clientControlFacade, IPluginManagerFacade pluginManagerFacade, ILoginSettingsCacheFacade loginSettingsCacheFacade, IServerTypeCacheFacade serverTypeCacheFacade, IServerOfflineCacheFacade serverOfflineCacheFacade)
	{
		_userContext = userContext;
		Properties = environmentProperties;
		_environmentManager = environmentManager;
		_configuration = configuration;
		_credentialCacheService = credentialCacheService;
		_siteUtil = siteUtil;
		_clientControlFacade = clientControlFacade;
		_pluginManagerFacade = pluginManagerFacade;
		_loginSettingsCacheFacade = loginSettingsCacheFacade;
		_serverTypeCacheFacade = serverTypeCacheFacade;
		_serverOfflineCacheFacade = serverOfflineCacheFacade;
	}

	public void Init()
	{
		_environmentManager.SDKEnvironmentManagerInitialize();
		QueryChangesInstance.AddUserContext(_userContext);
	}

	public void Reset()
	{
		_environmentManager.EnableConfigurationChangedService = false;
		QueryChangesInstance.StopQuery(updateLoginState: false);
		_pluginManagerFacade.CloseCheckForStandalone();
		_serverOfflineCacheFacade.Clear();
	}

	public void AddServer(bool secureOnly, Uri uri, NetworkCredential networkCredential, bool masterOnly)
	{
		AddServer(new LoginSettings(uri, networkCredential, Guid.Empty, uri.DnsSafeHost, secureOnly), masterOnly);
	}

	public void AddServer(bool secureOnly, Uri uri, CredentialCache credentialCache, bool masterOnly)
	{
		AddServer(new LoginSettings(uri, credentialCache, Guid.Empty, uri.DnsSafeHost, secureOnly), masterOnly);
	}

	public void AddServer(LoginSettings loginSettings, bool masterOnly)
	{
		_configuration.AddConfiguration(loginSettings, masterOnly);
		_environmentManager.ApploggedState = true;
		QueryChangesInstance.InvalidateData(_userContext);
	}

	private static Uri FindBestAddress(bool secureOnly, Item siteItem, CredentialCache credentialCache)
	{
		List<string> list = new List<string>();
		int num = 0;
		while (siteItem.Properties.ContainsKey("Address" + num.ToString(CultureInfo.InvariantCulture)))
		{
			list.Add(siteItem.Properties["Address" + num.ToString(CultureInfo.InvariantCulture)]);
			num++;
		}
		Uri result = siteItem.FQID.ServerId.Uri;
		if (list.Count > 1)
		{
			string text = ParallelHandler.FindBestAddress(secureOnly, list, credentialCache);
			if (text != null)
			{
				result = new Uri(text);
			}
		}
		return result;
	}

	public void AddServer(bool secureOnly, Item siteItem, CredentialCache credentialCache, bool masterOnly)
	{
		Uri uri = _credentialCacheService.TryCreateParentUri(siteItem, credentialCache);
		Uri uri2 = FindBestAddress(secureOnly, siteItem, credentialCache);
		LoginSettings loginSettings;
		if (uri != null)
		{
			_credentialCacheService.MergeCredentialCache(uri, credentialCache, uri2, _userContext.CredentialCache);
			loginSettings = CreateLoginSettings(secureOnly, siteItem, _userContext.CredentialCache, uri2);
		}
		else
		{
			loginSettings = CreateLoginSettings(secureOnly, siteItem, credentialCache, uri2);
		}
		_configuration.AddConfiguration(loginSettings, masterOnly);
		_environmentManager.ApploggedState = true;
		QueryChangesInstance.InvalidateData(_userContext);
	}

	public Item LoadSiteItem(bool secureOnly, Uri uri, CredentialCache credentialCache)
	{
		return _siteUtil.LoadSiteItem(secureOnly, uri, credentialCache);
	}

	public Item LoadSiteItem(bool secureOnly, Uri uri, CredentialCache credentialCache, string serverType)
	{
		return _siteUtil.LoadSiteItem(secureOnly, uri, credentialCache, serverType);
	}

	public Item LoadSiteItem(bool secureOnly, Uri uri)
	{
		return _siteUtil.LoadSiteItem(secureOnly, uri);
	}

	public bool IsServerConnected(Uri uri)
	{
		return _configuration.CheckServerConnection(uri);
	}

	public bool LoadConfiguration(Uri serverUri)
	{
		return _configuration.TryLoadConfiguration(serverUri);
	}

	public bool IsLoggedIn(Uri serverUri)
	{
		return _configuration.IsLoggedIn(serverUri);
	}

	public bool IsLoggedInToAnyServer()
	{
		return _configuration.IsLoggedInAny();
	}

	public void ReloadConfiguration(FQID serverFQID)
	{
		if (serverFQID != null)
		{
			_configuration.RefreshServer(serverFQID);
		}
	}

	public void Login(Uri serverUri, bool masterOnly)
	{
		if (!_configuration.Login(serverUri, masterOnly))
		{
			Exception ex = _configuration.LastException(serverUri);
			if (ex != null)
			{
				throw ex;
			}
			throw new ServerNotFoundMIPException("");
		}
		QueryChangesInstance.InvalidateData(_userContext);
		QueryChangesInstance.StartQuery(updateLoginState: true);
	}

	public void Login(Uri serverUri, bool masterOnly, Guid integrationId, string integrationName, string version, string manufacturerName)
	{
		Login(serverUri, masterOnly);
		_configuration.RegisterIntegration(serverUri, integrationId, integrationName, version, manufacturerName);
	}

	public void Logout(Uri serverUri)
	{
		ISystemInfo systemInfo = _configuration.GetSystemInfo(serverUri);
		if (systemInfo != null)
		{
			_environmentManager.Logout(systemInfo.ServerId);
		}
		_configuration.Logout(serverUri);
		if (!_configuration.IsLoggedInAny())
		{
			QueryChangesInstance.StopQuery(updateLoginState: true);
		}
	}

	public void Logout()
	{
		_environmentManager.EnableConfigurationChangedService = false;
		_clientControlFacade.Close();
		_environmentManager.LogoutAll();
		_environmentManager.ApploggedState = false;
		_configuration.LogoutAllServers();
		_pluginManagerFacade.CloseCheckForStandalone();
		QueryChangesInstance.StopQuery(updateLoginState: true);
	}

	public void RemoveAllServers()
	{
		_serverTypeCacheFacade.RemoveAllServers();
		_configuration.RemoveAllServers();
		_loginSettingsCacheFacade.Clear();
		_serverOfflineCacheFacade.Clear();
		_userContext.CredentialCache = new CredentialCache();
		_environmentManager.ApploggedState = false;
		QueryChangesInstance.StopQuery(updateLoginState: false);
	}

	public void RemoveServer(Guid id)
	{
		_configuration.RemoveServer(id);
		LoginSettings loginSettings = _loginSettingsCacheFacade.GetLoginSettings(id);
		RemoveServer(loginSettings);
	}

	public void RemoveServer(Uri uri)
	{
		LoginSettings ls = _configuration.RemoveServer(uri);
		RemoveServer(ls);
		if (_configuration.SystemInfoList.Count == 0)
		{
			_environmentManager.ApploggedState = false;
		}
	}

	public List<Item> GetDisabledDevices(Guid serverId)
	{
		return _configuration.GetDisabledDevices(serverId);
	}

	public void InitializeUIFramework(IDictionary<string, Color> brandColors)
	{
		_environmentManager.InitializeUIFramework(brandColors);
	}

	public void LogError(string where, string text)
	{
		_environmentManager.Log(error: true, where, text);
	}

	private void RemoveServer(LoginSettings ls)
	{
		if (ls != null)
		{
			_serverTypeCacheFacade.RemoveServer(ls);
			_loginSettingsCacheFacade.Remove(ls);
			_serverOfflineCacheFacade.Remove(ls.Uri.ToString());
			_userContext.LoginSettings.Remove(ls);
			_credentialCacheService.Remove(_userContext.CredentialCache, ls);
		}
		QueryChangesInstance.InvalidateData(_userContext);
	}

	private static LoginSettings CreateLoginSettings(bool secureOnly, Item siteItem, CredentialCache credentialCache, Uri uri)
	{
		return new LoginSettings(uri, credentialCache, siteItem.FQID.ServerId.Id, uri.DnsSafeHost, secureOnly)
		{
			ServerType = siteItem.FQID.ServerId.ServerType
		};
	}
}
