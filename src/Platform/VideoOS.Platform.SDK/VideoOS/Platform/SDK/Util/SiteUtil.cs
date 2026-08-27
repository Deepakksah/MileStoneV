using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using VideoOS.Platform.Login;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Platform;
using VideoOS.Platform.SDK.Server;

namespace VideoOS.Platform.SDK.Util;

public class SiteUtil : ISiteUtil
{
	public Item LoadSiteItem(bool secureOnly, Uri uri)
	{
		if (EnvironmentManager.UsingUserContext)
		{
			EnvironmentManager.Instance.Log(error: false, "LoadSiteItem(bool, Uri)", $"Use LoasSiteItem(bool, Uri, UserContext) for an environment which is using multi-user context");
			return null;
		}
		LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(uri.DnsSafeHost);
		if (loginSettings != null)
		{
			return GetServerSiteItem(loginSettings);
		}
		EnvironmentManager.Instance.Log(error: false, "LoadSiteItem(bool, Uri)", $"Failed to retrieve login settings from the cache for URI: {uri}");
		return null;
	}

	public Item LoadSiteItem(bool secureOnly, Uri uri, UserContext userContext)
	{
		if (!EnvironmentManager.UsingUserContext)
		{
			EnvironmentManager.Instance.Log(error: false, "LoadSiteItem(bool, Uri, UserContext)", $"Use LoasSiteItem(bool, Uri) for an environment which is not using multi-user context");
			return null;
		}
		LoginSettings loginSettings = userContext.GetLoginSettings(uri.DnsSafeHost);
		if (loginSettings != null)
		{
			return GetServerSiteItem(loginSettings);
		}
		EnvironmentManager.Instance.Log(error: false, "LoadSiteItem(bool, Uri, UserContext)", $"Failed to retrieve login settings from UserContext for URI: {uri}");
		return null;
	}

	private static SiteServerItem GetServerSiteItem(LoginSettings loginSettings)
	{
		return GetServerSiteItem(loginSettings.Uri, loginSettings.ServerType, loginSettings);
	}

	public Item LoadSiteItem(bool secureOnly, Uri uri, CredentialCache credentialCache)
	{
		if (!ServerTypeCache.Contains(uri))
		{
			ParallelHandler.FindBestAddress(secureOnly, new List<string> { uri.ToString() }, credentialCache);
			if (!ServerTypeCache.Contains(uri))
			{
				return null;
			}
		}
		string serverType = ServerTypeCache.GetServerType(uri);
		return LoadSiteItem(secureOnly, uri, credentialCache, serverType);
	}

	public Item LoadSiteItem(bool secureOnly, Uri uri, CredentialCache credentialCache, string serverType)
	{
		ServerTypeCache.ServerInfo serverInfo = GetServerInfo(secureOnly, uri, credentialCache, serverType);
		if (serverInfo == null)
		{
			EnvironmentManager.Instance.Log(error: false, "LoadSiteItem", $"ServerInfo not found for URI: {uri}");
			return null;
		}
		Uri uri2 = new Uri(serverInfo.ServerAddress);
		UpdateCredentialCacheUri(uri, uri2, credentialCache);
		if (!IsSecureOnlyConfigurationValid(secureOnly, serverInfo, uri2))
		{
			EnvironmentManager.Instance.Log(error: false, "LoadSiteItem", "Secure only requires OAuth server and HTTPS scheme");
			return null;
		}
		bool disposeTokenCache;
		LoginSettings loginSettings = GetLoginSettings(secureOnly, uri2, credentialCache, serverInfo, out disposeTokenCache);
		if (loginSettings == null)
		{
			EnvironmentManager.Instance.Log(error: true, "LoadSiteItem", $"Failed to retrieve loginSettings for URI: {uri2}");
			return null;
		}
		SiteServerItem serverSiteItem = GetServerSiteItem(uri2, serverType, loginSettings);
		if (disposeTokenCache)
		{
			EnvironmentManager.Instance.Log(error: false, "SiteUtil", $"Setting IdentityToken = null for instance={loginSettings.InstanceGuid}");
			loginSettings.IdentityTokenCache?.Dispose();
			loginSettings.IdentityTokenCache = null;
		}
		return serverSiteItem;
	}

	private static bool IsSecureOnlyConfigurationValid(bool secureOnly, ServerTypeCache.ServerInfo serverInfo, Uri serverInfoUri)
	{
		if (!secureOnly)
		{
			return true;
		}
		if (serverInfoUri.Scheme == Uri.UriSchemeHttps)
		{
			return serverInfo.IsOAuthServer;
		}
		return false;
	}

	private static void UpdateCredentialCacheUri(Uri inputUri, Uri serverInfoUri, CredentialCache credentialCache)
	{
		if (credentialCache != null && inputUri != serverInfoUri)
		{
			CredentialUtil.UpdateCredentialCacheUri(inputUri, serverInfoUri, credentialCache);
		}
	}

	private static SiteServerItem GetServerSiteItem(Uri uri, string serverType, LoginSettings loginSettings)
	{
		SiteInfoManager siteInfoManager = new SiteInfoManager();
		UserContext userContext = CreateUserContext(loginSettings);
		Guid guid = siteInfoManager.BuildSiteInfo(uri, serverType, loginSettings, userContext);
		if (guid != Guid.Empty)
		{
			SiteInfo site = siteInfoManager.GetSite(guid, userContext);
			if (site != null)
			{
				return new SiteServerItem(new ServerId(site.ServerType, site.ServerUri.DnsSafeHost, site.ServerUri.Port, site.Id), site, siteInfoManager);
			}
		}
		return null;
	}

	public static bool GetSecureOnlyFlagForSubSite(bool secureOnlyMaster, string serverVersionSlave)
	{
		int result = 0;
		int result2 = 0;
		if (!string.IsNullOrEmpty(serverVersionSlave))
		{
			string[] array = serverVersionSlave.Split('.');
			int.TryParse(array[0], NumberStyles.Number, CultureInfo.InvariantCulture, out result);
			int.TryParse(array[1], NumberStyles.Number, CultureInfo.InvariantCulture, out result2);
		}
		return (result > 21 || (result == 21 && result2 >= 1)) && secureOnlyMaster;
	}

	private static LoginSettings GetLoginSettings(bool secureOnly, Uri uri, CredentialCache credentialCache, ServerTypeCache.ServerInfo serverInfo, out bool disposeTokenCache)
	{
		disposeTokenCache = false;
		LoginSettings loginSettingsFromCache = GetLoginSettingsFromCache(uri, credentialCache);
		if (loginSettingsFromCache != null)
		{
			return loginSettingsFromCache;
		}
		if (serverInfo.IsOAuthServer)
		{
			loginSettingsFromCache = LoginSettingsUtil.LoginOAuth(uri, credentialCache, secureOnly);
			if (loginSettingsFromCache == null)
			{
				EnvironmentManager.Instance.Log(error: false, "LoadSiteItem", $"Login to OAuth server failed. Server URI: {uri}");
			}
			disposeTokenCache = true;
			return loginSettingsFromCache;
		}
		return new LoginSettings(uri, credentialCache, Guid.Empty, string.Empty, serverInfo.ServerType, secureOnly);
	}

	private static LoginSettings GetLoginSettingsFromCache(Uri uri, CredentialCache credentialCache)
	{
		if (EnvironmentManager.UsingUserContext)
		{
			return null;
		}
		LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(uri.DnsSafeHost);
		if (loginSettings == null)
		{
			return null;
		}
		NetworkCredential networkCredential = credentialCache.GetCredential(uri, "Basic") ?? credentialCache.GetCredential(uri, "Negotiate");
		if (networkCredential != null && CredentialUtil.AreNetworkCredentialsEqual(networkCredential, loginSettings.NetworkCredential))
		{
			return loginSettings;
		}
		return null;
	}

	private static ServerTypeCache.ServerInfo GetServerInfo(bool secureOnly, Uri uri, CredentialCache credentialCache, string serverType)
	{
		ServerTypeCache.ServerInfo serverInfo = ServerTypeCache.TryGet(uri);
		if (serverInfo != null && (string.IsNullOrEmpty(serverInfo.ServerType) || string.Equals(serverInfo.ServerType, serverType, StringComparison.OrdinalIgnoreCase)))
		{
			return serverInfo;
		}
		if (!string.IsNullOrEmpty(ParallelHandler.FindBestAddress(secureOnly, new List<string> { uri.ToString() }, credentialCache, out var _, serverType)))
		{
			return ServerTypeCache.TryGet(uri);
		}
		return null;
	}

	private static UserContext CreateUserContext(LoginSettings loginSettings)
	{
		if (!EnvironmentManager.UsingUserContext)
		{
			return null;
		}
		UserContext obj = new UserContext
		{
			CredentialCache = loginSettings.CredentialCache
		};
		obj.Configuration = new SDKConfiguration(obj);
		obj.AddLoginSettings(loginSettings);
		return obj;
	}
}
