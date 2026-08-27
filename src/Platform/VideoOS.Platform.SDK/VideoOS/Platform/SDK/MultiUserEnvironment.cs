using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security;
using System.Threading.Tasks;
using VideoOS.IdentityServer.Common.Utilities.Tokens;
using VideoOS.Platform.Common.OAuth;
using VideoOS.Platform.Data;
using VideoOS.Platform.Login;
using VideoOS.Platform.OAuth;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Platform;
using VideoOS.Platform.SDK.Server;
using VideoOS.Platform.SDK.Util;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.SDK;

public class MultiUserEnvironment
{
	private struct Server(bool secureOnly, Uri uri)
	{
		public Uri Uri { get; } = uri;

		public bool SecureOnly { get; } = secureOnly;
	}

	private class SiteTaskInfo
	{
		public bool SecureOnly { get; set; }

		public string ServerType { get; set; }

		public Task<string> FindBestAddressTask { get; set; }
	}

	private static List<Server> _allServers = new List<Server>();

	private static List<UserContext> _allUserContexts = new List<UserContext>();

	private static QueryConfigurationChanges _queryChangesInstance = new QueryConfigurationChanges();

	private static readonly ISiteUtil _siteUtil = new SiteUtil();

	private static bool _masterOnly = false;

	private static Uri _masterUri = null;

	private static bool _masterSecureOnly = false;

	private static object _syncObject = new object();

	private static EnvironmentProperties _properties = new EnvironmentProperties(_queryChangesInstance);

	public static Uri MasterUri => _masterUri;

	public static ILogClientFactory LogClientFactory { get; set; }

	public static EnvironmentProperties Properties => _properties;

	public static bool InitializeLoggedIn { get; set; }

	[Obsolete("This method is deprecated, please use InitializeUsingUserContext(bool, Uri, string, string, bool, bool) instead.")]
	public static void InitializeUsingUserContext(Uri uri, string username, string password, bool usingAd, bool masterOnly = false)
	{
		InitializeUsingUserContext(secureOnly: false, uri, username, password, usingAd, masterOnly);
	}

	[Obsolete("This method is deprecated, please use InitializeUsingUserContext(bool, Uri, string, string, bool, string, bool) instead.")]
	public static void InitializeUsingUserContext(Uri uri, string username, string password, bool usingAd, string companyFolderName, bool masterOnly = false)
	{
		InitializeUsingUserContext(secureOnly: false, uri, username, password, usingAd, companyFolderName, masterOnly);
	}

	[Obsolete("This method is deprecated, please use InitializeUsingUserContext(bool, Uri, string, SecureString, bool, string, bool) instead.")]
	public static void InitializeUsingUserContext(Uri uri, string username, SecureString password, bool usingAd, string companyFolderName, bool masterOnly = false)
	{
		InitializeUsingUserContext(secureOnly: false, uri, username, password, usingAd, companyFolderName, masterOnly);
	}

	public static void InitializeUsingUserContext(bool secureOnly, Uri uri, string username, string password, bool usingAd, bool masterOnly = false)
	{
		InitializeUsingUserContext(secureOnly, uri, username, password, usingAd, "Milestone", masterOnly);
	}

	public static void InitializeUsingUserContext(bool secureOnly, Uri uri, string username, string password, bool usingAd, string companyFolderName, bool masterOnly = false)
	{
		InitializeUsingUserContext(secureOnly, uri, username, password.ToSecureString(), usingAd, companyFolderName, masterOnly);
	}

	public static void InitializeUsingUserContext(bool secureOnly, Uri uri, string username, SecureString password, bool usingAd, string companyFolderName, bool masterOnly = false)
	{
		lock (_syncObject)
		{
			EnsureUriSchemeValid(uri);
			CredentialCache credentialCache = ((!usingAd) ? VideoOS.Platform.Login.Util.BuildCredentialCache(uri, username, password, "Basic") : VideoOS.Platform.Login.Util.BuildCredentialCache(uri, username, password, "Negotiate"));
			LoginSettings loginSettings = new LoginSettings(uri, credentialCache, Guid.Empty, uri.DnsSafeHost, secureOnly);
			InitializeUserContextCommon(secureOnly, companyFolderName, uri, loginSettings, masterOnly).CredentialCache = credentialCache;
		}
	}

	public static void InitializeUsingUserContext(bool secureOnly, Uri uri, IMipTokenCache mipTokenCache, string companyFolderName, bool masterOnly = false)
	{
		lock (_syncObject)
		{
			EnsureUriSchemeValid(uri);
			LoginSettings loginSettings = new LoginSettings(uri, mipTokenCache, Guid.Empty, uri.DnsSafeHost, secureOnly);
			InitializeUserContextCommon(secureOnly, companyFolderName, uri, loginSettings, masterOnly);
		}
	}

	private static UserContext InitializeUserContextCommon(bool secureOnly, string companyFolderName, Uri uri, LoginSettings loginSettings, bool masterOnly)
	{
		UserContext userContext;
		if (EnvironmentManager.Instance != null && Configuration.Instance != null)
		{
			userContext = ((SDKConfiguration)Configuration.Instance).UserContext;
			((SDKConfiguration)userContext.Configuration).RemoveAllServers();
			EnvironmentManager.UsingUserContext = true;
		}
		else
		{
			EnvironmentManager.Instance = new SDKEnvironmentManager();
			BookmarkService.Instance = new SDKBookmarkService();
			RestrictedMediaService.Instance = new SDKRestrictedMediaService();
			userContext = new UserContext
			{
				CredentialCache = new CredentialCache()
			};
			userContext.Configuration = new SDKConfiguration(userContext, subscribeToServerOfflineCacheChanged: true);
			Configuration.Instance = userContext.Configuration;
			((SDKEnvironmentManager)EnvironmentManager.Instance).SDKEnvironmentManagerInitialize(multiuser: true);
		}
		EnvironmentManager.Instance.EnvironmentOptions["CompanyNameFolder"] = companyFolderName;
		uri = EnsureUriSchemeValid(uri);
		EnvironmentManager.Instance.Log(error: false, "MultiUserEnvironment:InitializeUsingUserContext", "Provided URI=" + uri.ToString());
		((IEnvironmentProperties)Properties).QueryConfigurationChanges.AddUserContext(userContext);
		_masterOnly = masterOnly;
		userContext.AddLoginSettings(loginSettings);
		LoginSettingsCache.AddLoginSettings(loginSettings);
		((SDKConfiguration)userContext.Configuration).AddConfiguration(userContext.LoginSettings, masterOnly);
		InitializeLoggedIn = false;
		try
		{
			InitializeLoggedIn = ((ISDKConfiguration)userContext.Configuration).Login(uri);
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: true, "InitializeUsingUserContext", ex.Message);
		}
		if (InitializeLoggedIn)
		{
			LoginSettings loginSettings2 = LoginSettingsCache.GetLoginSettings(loginSettings.Guid);
			_masterUri = ((loginSettings2 != null) ? loginSettings2.Uri : uri);
		}
		else
		{
			_masterUri = uri;
		}
		_masterSecureOnly = secureOnly;
		((ISDKEnvironmentManager)EnvironmentManager.Instance).ApploggedState = true;
		return userContext;
	}

	public static void UnInitialize()
	{
		if (EnvironmentManager.Instance == null)
		{
			return;
		}
		EnvironmentManager.Instance.Log(error: false, "MultiUserEnvironment:UnInitialize", "UserContext count: " + _allUserContexts.Count);
		lock (_syncObject)
		{
			RemoveAllServers();
			EnvironmentManager.Instance.EnableConfigurationChangedService = false;
			if (_queryChangesInstance != null)
			{
				_queryChangesInstance.StopQuery(updateLoginState: true);
			}
			_queryChangesInstance = new QueryConfigurationChanges();
			_properties = new EnvironmentProperties(_queryChangesInstance);
			(EnvironmentManager.Instance as SDKEnvironmentManager)?.TelemetryFlush();
			EnvironmentManager.Instance = null;
			BookmarkService.Instance = null;
			Configuration.Instance = null;
		}
	}

	[Obsolete("This method is deprecated, please use AddServer(bool, Uri) instead.")]
	public static void AddServer(Uri uri)
	{
		AddServer(secureOnly: false, uri);
	}

	[Obsolete("This method is deprecated, please use AddServer(bool, Collection<string>) instead.")]
	public static bool AddServer(Collection<string> addressAlternatives)
	{
		return AddServer(secureOnly: false, addressAlternatives);
	}

	public static void AddServer(bool secureOnly, Uri uri)
	{
		uri = EnsureUriSchemeValid(uri);
		foreach (Server allServer in _allServers)
		{
			if (uri.Equals(allServer.Uri) || (uri.DnsSafeHost == allServer.Uri.DnsSafeHost && uri.Port == allServer.Uri.Port))
			{
				return;
			}
		}
		_allServers.Add(new Server(secureOnly, uri));
		if (_allUserContexts.Count > 0)
		{
			EnvironmentManager.Instance.Log(error: true, "MultiUserEnvironment.AddServer", "Servers added after users will not be added to those users", null);
		}
		((ISDKEnvironmentManager)EnvironmentManager.Instance).ApploggedState = true;
	}

	public static bool AddServer(bool secureOnly, Collection<string> addressAlternatives)
	{
		if (addressAlternatives.Count == 0)
		{
			return false;
		}
		List<string> list = new List<string>();
		foreach (string addressAlternative in addressAlternatives)
		{
			if (!list.Contains(addressAlternative))
			{
				list.Add(addressAlternative);
			}
		}
		string text = ParallelHandler.FindBestAddress(secureOnly, list, ((SDKConfiguration)Configuration.Instance).UserContext.CredentialCache);
		if (text == null)
		{
			text = addressAlternatives[0];
			AddServer(secureOnly, UrlUtil.BuildUri(text));
			((ISDKEnvironmentManager)EnvironmentManager.Instance).ApploggedState = true;
			return false;
		}
		AddServer(secureOnly, UrlUtil.BuildUri(text));
		((ISDKEnvironmentManager)EnvironmentManager.Instance).ApploggedState = true;
		return true;
	}

	public static void RemoveAllServers()
	{
		if (EnvironmentManager.Instance == null)
		{
			return;
		}
		foreach (UserContext allUserContext in _allUserContexts)
		{
			((SDKConfiguration)allUserContext.Configuration).RemoveAllServers();
			((SDKConfiguration)allUserContext.Configuration).UnsubscribeToServerOfflineCacheChanged();
			allUserContext.Clear();
			_queryChangesInstance.InvalidateData(allUserContext);
		}
		((ISDKEnvironmentManager)EnvironmentManager.Instance).ApploggedState = false;
		_allServers.Clear();
		_queryChangesInstance?.StopQuery(updateLoginState: false);
		ServerTypeCache.RemoveAllServers();
		LoginSettingsCache.Clear();
		ServerOfflineCache.Clear();
	}

	public static UserContext CreateUserContext(string username, string password, bool usingAd)
	{
		if (!usingAd && username.Contains('\\'))
		{
			throw new InvalidParameterMIPException("Cannot use username with a domain as a basic user.");
		}
		UserContext userContext = new UserContext
		{
			CredentialCache = new CredentialCache()
		};
		userContext.Configuration = new SDKConfiguration(userContext, subscribeToServerOfflineCacheChanged: true);
		userContext.LogClient = LogClientFactory?.GetLogByContext(userContext);
		if (usingAd)
		{
			NetworkCredential networkCredential = VideoOS.Platform.Login.Util.BuildNetworkCredential(_masterUri, username, password, "Negotiate");
			List<SiteInfo> list = new SiteInfoManager().FindSiteUriForUser(_masterUri, !usingAd, networkCredential, userContext, _masterSecureOnly, out var unauthorized);
			if (unauthorized)
			{
				userContext.Clear();
				throw new InvalidCredentialsMIPException(_masterUri.DnsSafeHost);
			}
			RemoveDuplicateServers(list);
			list = (_masterOnly ? new List<SiteInfo> { list[0] } : list);
			TryAddLoginSettingsForEachSite(list, userContext, networkCredential);
		}
		else
		{
			if (_allServers.Count == 0)
			{
				_allServers.Add(new Server(_masterSecureOnly, _masterUri));
			}
			foreach (Server allServer in _allServers)
			{
				NetworkCredential cred = new NetworkCredential(username, password);
				userContext.CredentialCache.Add(allServer.Uri, "Basic", cred);
				LoginSettings loginSettings = new LoginSettings(allServer.Uri, userContext.CredentialCache, Guid.Empty, allServer.Uri.DnsSafeHost, allServer.SecureOnly);
				ServerTypeCache.Fill(loginSettings);
				userContext.AddLoginSettings(loginSettings);
			}
		}
		((SDKConfiguration)userContext.Configuration).AddConfiguration(userContext.LoginSettings, masterOnly: true);
		_allUserContexts.Add(userContext);
		_queryChangesInstance.AddUserContext(userContext);
		return userContext;
	}

	public static UserContext CreateUserContext(IMipTokenCache mipTokenCache, string authenticationType)
	{
		switch (authenticationType)
		{
		case "Basic":
			return CreateUserContext(mipTokenCache, ignoreMasterOnly: true);
		case "Negotiate":
			return CreateUserContext(mipTokenCache, ignoreMasterOnly: true);
		case "External":
			return CreateUserContext(mipTokenCache, ignoreMasterOnly: false);
		default:
		{
			string text = string.Join(", ", Enum.GetNames(typeof(VideoOS.Platform.Login.AuthenticationType)));
			throw new NotSupportedException("Authentication type " + authenticationType + " is not supported. Supported authentication types: " + text + ".");
		}
		}
	}

	public static UserContext CreateUserContext(IMipTokenCache mipTokenCache, bool ignoreMasterOnly)
	{
		if (mipTokenCache == null)
		{
			throw new ArgumentNullException("mipTokenCache");
		}
		if (EnvironmentManager.Instance == null)
		{
			throw new MIPException("MultiUserEnvironment is not initialized");
		}
		UserContext userContext = new UserContext();
		userContext.CredentialCache = null;
		userContext.Configuration = new SDKConfiguration(userContext, subscribeToServerOfflineCacheChanged: true);
		userContext.LogClient = LogClientFactory?.GetLogByContext(userContext);
		Server masterServer = GetMasterServer();
		LoginSettings loginSettings = new LoginSettings(masterServer.Uri, mipTokenCache, Guid.Empty, masterServer.Uri.DnsSafeHost, masterServer.SecureOnly);
		loginSettings.IdentityTokenCache = mipTokenCache;
		EnvironmentManager.Instance.Log(error: false, "CreateUserContext", $"Setting IdentityToken = value for instance={loginSettings.InstanceGuid}");
		ServerTypeCache.Fill(loginSettings);
		userContext.AddLoginSettings(loginSettings);
		if (!ignoreMasterOnly)
		{
			List<SiteInfo> list = new SiteInfoManager().FindSiteUriWithOAuthToken(loginSettings, userContext, _masterSecureOnly, out var unauthorized);
			if (unauthorized)
			{
				userContext.Clear();
				throw new InvalidCredentialsMIPException(_masterUri.DnsSafeHost);
			}
			RemoveDuplicateServers(list);
			list = (_masterOnly ? new List<SiteInfo> { list[0] } : list);
			TryAddLoginSettingsForEachSiteOAuth(list, userContext, mipTokenCache);
		}
		((SDKConfiguration)userContext.Configuration).AddConfiguration(userContext.LoginSettings, ignoreMasterOnly || _masterOnly);
		_allUserContexts.Add(userContext);
		_queryChangesInstance.AddUserContext(userContext);
		return userContext;
	}

	[Obsolete("This method is deprecated, please use CreateUserContext(IMipTokenCache, string) instead.")]
	public static UserContext CreateUserContext(string clientId, IIdpTokens idpTokens)
	{
		return CreateUserContext(new MipTokenCacheInternal(new Uri(GetMasterServer().Uri, "idp"), clientId, idpTokens));
	}

	[Obsolete("This method is deprecated, please use CreateUserContext(IMipTokenCache, string) instead.")]
	public static UserContext CreateUserContext(IMipTokenCache mipTokenCache)
	{
		return CreateUserContext(mipTokenCache, ignoreMasterOnly: true);
	}

	private static Server GetMasterServer()
	{
		Server server = new Server(_masterSecureOnly, _masterUri);
		if (_allServers.Count == 0)
		{
			_allServers.Add(server);
		}
		return server;
	}

	public static UserContext CreateUserContext()
	{
		UserContext userContext = new UserContext
		{
			CredentialCache = new CredentialCache()
		};
		userContext.Configuration = new SDKConfiguration(userContext, subscribeToServerOfflineCacheChanged: true);
		userContext.LogClient = LogClientFactory?.GetLogByContext(userContext);
		foreach (Server allServer in _allServers)
		{
			NetworkCredential cred = VideoOS.Platform.Login.Util.BuildNetworkCredential(allServer.Uri, "", "", "Negotiate");
			userContext.CredentialCache.Add(allServer.Uri, "Negotiate", cred);
			userContext.AddLoginSettings(new LoginSettings(allServer.Uri, userContext.CredentialCache, Guid.Empty, allServer.Uri.DnsSafeHost, allServer.SecureOnly));
		}
		((SDKConfiguration)userContext.Configuration).AddConfiguration(userContext.LoginSettings, _masterOnly);
		_allUserContexts.Add(userContext);
		_queryChangesInstance.AddUserContext(userContext);
		return userContext;
	}

	[Obsolete("This method is deprecated, please use CreateSingleServerUserContext(bool, string, string, bool, Uri) instead.")]
	public static UserContext CreateSingleServerUserContext(string username, string password, bool usingAd, Uri uri)
	{
		return CreateSingleServerUserContext(secureOnly: false, username, password, usingAd, uri);
	}

	[Obsolete("This method is deprecated, please use CreateSingleServerUserContext(bool, string, string, bool, Uri, bool) instead.")]
	public static UserContext CreateSingleServerUserContext(string username, string password, bool usingAd, Uri uri, bool useQueryChanges)
	{
		return CreateSingleServerUserContext(secureOnly: false, username, password, usingAd, uri, useQueryChanges);
	}

	public static UserContext CreateSingleServerUserContext(bool secureOnly, string username, string password, bool usingAd, Uri uri)
	{
		return CreateSingleServerUserContext(secureOnly, username, password, usingAd, uri, useQueryChanges: true);
	}

	public static UserContext CreateSingleServerUserContext(bool secureOnly, string username, string password, bool usingAd, Uri uri, bool useQueryChanges)
	{
		UserContext userContext = new UserContext
		{
			CredentialCache = new CredentialCache()
		};
		userContext.Configuration = new SDKConfiguration(userContext, subscribeToServerOfflineCacheChanged: true);
		userContext.LogClient = LogClientFactory?.GetLogByContext(userContext);
		uri = EnsureUriSchemeValid(uri);
		if (usingAd)
		{
			NetworkCredential cred = VideoOS.Platform.Login.Util.BuildNetworkCredential(uri, username, password, "Negotiate");
			userContext.CredentialCache.Add(uri, "Negotiate", cred);
		}
		else
		{
			NetworkCredential cred = new NetworkCredential(username, password);
			userContext.CredentialCache.Add(uri, "Basic", cred);
		}
		userContext.AddLoginSettings(new LoginSettings(uri, userContext.CredentialCache, Guid.Empty, uri.DnsSafeHost, secureOnly));
		((SDKConfiguration)userContext.Configuration).AddConfiguration(userContext.LoginSettings, _masterOnly);
		((ISDKEnvironmentManager)EnvironmentManager.Instance).ApploggedState = true;
		_allUserContexts.Add(userContext);
		if (useQueryChanges)
		{
			_queryChangesInstance.AddUserContext(userContext);
		}
		return userContext;
	}

	public static void LoginUserContext(UserContext userContext)
	{
		LoginUserContext(userContext, "");
	}

	public static void LoginUserContext(UserContext userContext, string clientType)
	{
		userContext.ClientType = clientType ?? "";
		((SDKConfiguration)userContext.Configuration).SubscribeToServerOfflineCacheChanged();
		if (ParallelHandler.PopulateUserContext(userContext, out var lastException))
		{
			_queryChangesInstance.StartQuery(updateLoginState: true);
			return;
		}
		if (lastException is MIPException)
		{
			throw lastException;
		}
		throw new MIPException("Not logged in to any servers", lastException);
	}

	public static bool IsLoggedIn(UserContext userContext, Uri serverUri)
	{
		Uri uri = EnsureUriSchemeValid(serverUri);
		return ((SDKConfiguration)userContext.Configuration).IsLoggedIn(uri);
	}

	public static void ReloadConfiguration(UserContext userContext, FQID serverFQID)
	{
		((ISDKConfiguration)userContext.Configuration).RefreshServer(serverFQID);
	}

	public static void Logout(UserContext userContext, Uri serverUri)
	{
		Uri serverUri2 = EnsureUriSchemeValid(serverUri);
		((ISDKConfiguration)userContext.Configuration).Logout(serverUri2);
		((SDKConfiguration)userContext.Configuration).UnsubscribeToServerOfflineCacheChanged_IfNotLoggedInToAnyServer();
	}

	public static void Logout(UserContext userContext)
	{
		((ISDKConfiguration)userContext.Configuration).LogoutAllServers();
		((SDKConfiguration)userContext.Configuration).UnsubscribeToServerOfflineCacheChanged();
	}

	public static bool IsServerConnected(UserContext userContext, Uri uri)
	{
		ISDKConfiguration iSDKConfiguration = (ISDKConfiguration)userContext.Configuration;
		if (!iSDKConfiguration.CheckServerConnection(EnsureUriSchemeValid(uri)))
		{
			return iSDKConfiguration.CheckServerConnection(uri);
		}
		return true;
	}

	[Obsolete("Deprecated, please use LoadSiteItem(bool, Uri, CredentialCache) instead")]
	public static Item LoadSiteItem(Uri uri, CredentialCache credentialCache)
	{
		return LoadSiteItem(secureOnly: false, uri, credentialCache);
	}

	public static Item LoadSiteItem(bool secureOnly, Uri uri, CredentialCache credentialCache)
	{
		uri = EnsureUriSchemeValid(uri);
		return _siteUtil.LoadSiteItem(secureOnly, uri, credentialCache);
	}

	public static Item LoadSiteItem(bool secureOnly, Uri uri, UserContext userContext)
	{
		if (userContext == null)
		{
			EnvironmentManager.Instance.Log(error: false, "MultiEnvironment.LoadSiteItem(bool, Uri, UserContext)", "UserContext must not be null");
			return null;
		}
		return _siteUtil.LoadSiteItem(secureOnly, uri, userContext);
	}

	[Obsolete("Deprecated, please use LoadSiteItem(bool, Uri, CredentialCache, String) instead")]
	public static Item LoadSiteItem(Uri uri, CredentialCache credentialCache, string serverType)
	{
		return LoadSiteItem(secureOnly: false, uri, credentialCache, serverType);
	}

	public static Item LoadSiteItem(bool secureOnly, Uri uri, CredentialCache credentialCache, string serverType)
	{
		uri = EnsureUriSchemeValid(uri);
		return _siteUtil.LoadSiteItem(secureOnly, uri, credentialCache, serverType);
	}

	public static void RemoveUserContext(UserContext userContext)
	{
		foreach (LoginSettings loginSetting in userContext.LoginSettings)
		{
			((SDKConfiguration)userContext.Configuration).Logout(loginSetting.Uri);
			((SDKConfiguration)userContext.Configuration).UserContext = null;
		}
		((SDKConfiguration)userContext.Configuration).UnsubscribeToServerOfflineCacheChanged();
		userContext.Clear();
		if (userContext.Configuration != null)
		{
			userContext.Configuration.Logoff();
		}
		userContext.Configuration = null;
		userContext.LogClient?.Close();
		userContext.LogClient = null;
		if (_allUserContexts.Contains(userContext))
		{
			_allUserContexts.Remove(userContext);
		}
		_queryChangesInstance.RemoveUserContext(userContext);
		if (_queryChangesInstance.UserContextCount == 0)
		{
			_queryChangesInstance.StopQuery(updateLoginState: true);
		}
	}

	private static Uri EnsureUriSchemeValid(Uri uri)
	{
		if (uri.Scheme != Uri.UriSchemeFile && uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
		{
			uri = new UriBuilder("http://" + uri.OriginalString).Uri;
			EnvironmentManager.Instance.Log(error: false, "MultiUserEnvironment", "Unsupported scheme found: " + uri.Scheme + " Uri changed to: " + uri.ToString());
		}
		return uri;
	}

	private static bool AreUrisSameAddress(Uri first, Uri second)
	{
		if (first.Scheme != second.Scheme)
		{
			return false;
		}
		if (first.IsDefaultPort != second.IsDefaultPort && first.Port != second.Port)
		{
			return false;
		}
		IPAddress[] hostAddresses = Dns.GetHostAddresses(first.Host);
		IPAddress[] hostAddresses2 = Dns.GetHostAddresses(second.Host);
		IPAddress[] array = hostAddresses;
		for (int i = 0; i < array.Length; i++)
		{
			if (IPAddress.IsLoopback(array[i]))
			{
				string machineName = System.Environment.MachineName;
				if (!first.IsDefaultPort)
				{
					machineName = machineName + ":" + first.Port;
				}
				return AreUrisSameAddress(first.IsDefaultPort ? new UriBuilder(first.Scheme, System.Environment.MachineName).Uri : new UriBuilder(first.Scheme, System.Environment.MachineName, first.Port).Uri, second);
			}
		}
		array = hostAddresses2;
		for (int i = 0; i < array.Length; i++)
		{
			if (IPAddress.IsLoopback(array[i]))
			{
				string machineName2 = System.Environment.MachineName;
				if (!second.IsDefaultPort)
				{
					machineName2 = machineName2 + ":" + second.Port;
				}
				return AreUrisSameAddress(second.IsDefaultPort ? new UriBuilder(second.Scheme, System.Environment.MachineName).Uri : new UriBuilder(second.Scheme, System.Environment.MachineName, second.Port).Uri, first);
			}
		}
		array = hostAddresses;
		foreach (IPAddress iPAddress in array)
		{
			IPAddress[] array2 = hostAddresses2;
			foreach (IPAddress obj in array2)
			{
				if (iPAddress.Equals(obj))
				{
					return true;
				}
			}
		}
		return false;
	}

	private static void RemoveDuplicateServers(List<SiteInfo> sites)
	{
		List<Server> list = new List<Server>(_allServers);
		foreach (SiteInfo site in sites)
		{
			List<string> list2 = site.AddressCollection.Select((Address x) => x.Url).ToList();
			try
			{
				foreach (string item in list2)
				{
					Uri uri = new UriBuilder(item).Uri;
					foreach (Server item2 in list)
					{
						if (item2.Uri.Equals(uri))
						{
							list.Remove(item2);
							break;
						}
						if (item2.Uri.Host.IndexOf('.') < 0 && uri.Host.StartsWith(item2.Uri.Host, StringComparison.OrdinalIgnoreCase))
						{
							list.Remove(item2);
							break;
						}
						if (AreUrisSameAddress(item2.Uri, uri))
						{
							list.Remove(item2);
							break;
						}
					}
				}
			}
			catch (Exception)
			{
			}
			if (_masterOnly)
			{
				break;
			}
		}
		foreach (Server item3 in list)
		{
			SiteInfo siteInfo = new SiteInfo();
			siteInfo.AddressCollection.Add(new Address(item3.Uri.ToString()));
			sites.Add(siteInfo);
		}
	}

	private static void TryAddLoginSettingsForEachSite(List<SiteInfo> mfaSites, UserContext userContext, NetworkCredential nc)
	{
		Dictionary<Guid, SiteTaskInfo> dictionary = new Dictionary<Guid, SiteTaskInfo>();
		new Dictionary<Guid, string>();
		foreach (SiteInfo mfaSite in mfaSites)
		{
			HashSet<string> hashSet = mfaSite.AddressCollection.Select((Address x) => x.Url).ToHashSet();
			try
			{
				foreach (string item in hashSet)
				{
					userContext.CredentialCache.Add(new UriBuilder(item).Uri, "Negotiate", nc);
				}
				bool secureOnlyFlagForSubSite = SiteUtil.GetSecureOnlyFlagForSubSite(_masterSecureOnly, mfaSite.ServerVersion);
				string serverType = ServerTypeCache.GetServerType(_masterUri);
				Task<string> findBestAddressTask = FindBestAddressForSiteAsync(hashSet.ToList(), secureOnlyFlagForSubSite, userContext.CredentialCache, serverType);
				dictionary.Add(mfaSite.Id, new SiteTaskInfo
				{
					SecureOnly = secureOnlyFlagForSubSite,
					ServerType = serverType,
					FindBestAddressTask = findBestAddressTask
				});
			}
			catch (Exception ex)
			{
				EnvironmentManager.Instance.Log(error: true, "FindBestAddressForEachSite", $"Unable to use a site: {mfaSite.Name}. Error message: {ex.Message}");
			}
		}
		Task.WhenAll(dictionary.Values.Select((SiteTaskInfo x) => x.FindBestAddressTask)).Wait();
		foreach (SiteInfo mfaSite2 in mfaSites)
		{
			try
			{
				Uri uri = UrlUtil.BuildUri(dictionary[mfaSite2.Id].FindBestAddressTask.Result);
				LoginSettings loginSettings = new LoginSettings(uri, userContext.CredentialCache, Guid.Empty, uri.DnsSafeHost, dictionary[mfaSite2.Id].SecureOnly)
				{
					ServerType = dictionary[mfaSite2.Id].ServerType
				};
				ServerTypeCache.Fill(loginSettings);
				userContext.AddLoginSettings(loginSettings);
			}
			catch (Exception ex2)
			{
				EnvironmentManager.Instance.Log(error: true, "FindBestAddressForEachSite", $"Unable to add login settings for a site: {mfaSite2.Name}. Error message: {ex2.Message}");
			}
		}
	}

	private static async Task<string> FindBestAddressForSiteAsync(List<string> addressAlternatives, bool secureOnly, CredentialCache credentialCache, string serverType)
	{
		string foundAddress = null;
		try
		{
			if (addressAlternatives.Count == 1)
			{
				return addressAlternatives[0];
			}
			bool unauthorized = false;
			foundAddress = await Task.Run(() => ParallelHandler.FindBestAddress(secureOnly, addressAlternatives, credentialCache, out unauthorized, serverType)).ConfigureAwait(continueOnCapturedContext: false);
			if (foundAddress == null)
			{
				foundAddress = addressAlternatives[0];
			}
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: true, "FindBestAddressForSiteAsync", string.Format("Unable to find best address from addresses: {0}. Error: {1}", string.Join(", ", addressAlternatives), ex.Message));
		}
		return foundAddress;
	}

	private static void TryAddLoginSettingsForEachSiteOAuth(List<SiteInfo> mfaSites, UserContext userContext, IMipTokenCache mipTokenCache)
	{
		new Dictionary<Guid, string>();
		Dictionary<Guid, SiteTaskInfo> dictionary = new Dictionary<Guid, SiteTaskInfo>();
		foreach (SiteInfo mfaSite in mfaSites)
		{
			HashSet<string> source = mfaSite.AddressCollection.Select((Address x) => x.Url).ToHashSet();
			try
			{
				bool secureOnlyFlagForSubSite = SiteUtil.GetSecureOnlyFlagForSubSite(_masterSecureOnly, mfaSite.ServerVersion);
				string serverType = ServerTypeCache.GetServerType(_masterUri);
				Task<string> findBestAddressTask = FindBestAddressForSiteAsyncOAuth(source.ToList(), secureOnlyFlagForSubSite);
				dictionary.Add(mfaSite.Id, new SiteTaskInfo
				{
					SecureOnly = secureOnlyFlagForSubSite,
					ServerType = serverType,
					FindBestAddressTask = findBestAddressTask
				});
			}
			catch (Exception ex)
			{
				EnvironmentManager.Instance.Log(error: true, "TryAddLoginSettingsForEachSiteOAuth", string.Format(CultureInfo.InvariantCulture, "Unable to use a site: {0}. Error message: {1}", mfaSite.Name, ex.Message));
			}
		}
		Task.WhenAll(dictionary.Values.Select((SiteTaskInfo x) => x.FindBestAddressTask)).Wait();
		foreach (SiteInfo mfaSite2 in mfaSites)
		{
			try
			{
				Uri uri = UrlUtil.BuildUri(dictionary[mfaSite2.Id].FindBestAddressTask.Result);
				MipTokenCacheInternal identityTokenCache = new MipTokenCacheInternal(new Uri(uri, "idp"), mipTokenCache);
				LoginSettings loginSettings = new LoginSettings(uri, identityTokenCache, Guid.Empty, uri.DnsSafeHost, dictionary[mfaSite2.Id].SecureOnly)
				{
					ServerType = dictionary[mfaSite2.Id].ServerType
				};
				ServerTypeCache.Fill(loginSettings);
				userContext.AddLoginSettings(loginSettings);
			}
			catch (Exception ex2)
			{
				EnvironmentManager.Instance.Log(error: true, "TryAddLoginSettingsForEachSiteOAuth", string.Format(CultureInfo.InvariantCulture, "Unable to add login settings for a site: {0}. Error message: {1}", mfaSite2.Name, ex2.Message));
			}
		}
	}

	private static async Task<string> FindBestAddressForSiteAsyncOAuth(List<string> addressAlternatives, bool secureOnly)
	{
		try
		{
			if (addressAlternatives.Count == 1)
			{
				return addressAlternatives[0];
			}
			List<Uri> httpsAddresses = (from a in addressAlternatives
				select new Uri(a) into uri
				where uri.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase)
				select uri).ToList();
			string text = await Task.Run(() => ParallelHandler.FindBestAddressOAuth(httpsAddresses, secureOnly)).ConfigureAwait(continueOnCapturedContext: false);
			if (text != null)
			{
				return text;
			}
			if (!secureOnly)
			{
				List<Uri> httpAddresses = (from a in addressAlternatives
					select new Uri(a) into uri
					where uri.Scheme.Equals(Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase)
					select uri).ToList();
				string text2 = await Task.Run(() => ParallelHandler.FindBestAddressOAuth(httpAddresses, secureOnly)).ConfigureAwait(continueOnCapturedContext: false);
				if (text2 != null)
				{
					return text2;
				}
			}
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: true, "FindBestAddressForSiteAsyncOAuth", string.Format(CultureInfo.InvariantCulture, "Unable to find best address from addresses: {0}. Error: {1}", string.Join(", ", addressAlternatives), ex.Message));
		}
		if (addressAlternatives.Count == 0)
		{
			return null;
		}
		return addressAlternatives[0];
	}
}
