using System;
using System.Net;
using System.Security;
using VideoOS.Platform.Data;
using VideoOS.Platform.Login;
using VideoOS.Platform.OAuth;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Platform;
using VideoOS.Platform.SDK.Server;
using VideoOS.Platform.SDK.Util;

namespace VideoOS.Platform.SDK;

public class MultiEnvironment
{
	private static QueryConfigurationChanges _queryChangesInstance = new QueryConfigurationChanges();

	private static bool _initialized = false;

	private static object _syncObject = new object();

	private static EnvironmentProperties _properties = new EnvironmentProperties(_queryChangesInstance);

	private static readonly ISiteUtil _siteUtil = new SiteUtil();

	public static ILogClientFactory LogClientFactory { get; set; }

	public static EnvironmentProperties Properties => _properties;

	public static void InitializeUsingUserContext()
	{
		lock (_syncObject)
		{
			if (!_initialized)
			{
				_initialized = true;
				EnvironmentManager.Instance = new SDKEnvironmentManager();
				BookmarkService.Instance = new SDKBookmarkService();
				RestrictedMediaService.Instance = new SDKRestrictedMediaService();
				UserContext userContext = new UserContext();
				userContext.Configuration = new SDKConfiguration(userContext);
				Configuration.Instance = userContext.Configuration;
				((SDKEnvironmentManager)EnvironmentManager.Instance).SDKEnvironmentManagerInitialize(multiuser: true);
			}
		}
	}

	public static void UnInitialize()
	{
		lock (_syncObject)
		{
			EnvironmentManager.Instance.EnableConfigurationChangedService = false;
			(EnvironmentManager.Instance as SDKEnvironmentManager)?.TelemetryFlush();
			EnvironmentManager.Instance = null;
			BookmarkService.Instance = null;
			Configuration.Instance = null;
			_queryChangesInstance?.StopQuery(updateLoginState: true);
			_queryChangesInstance = new QueryConfigurationChanges();
			_properties = new EnvironmentProperties(_queryChangesInstance);
			ServerTypeCache.RemoveAllServers();
			LoginSettingsCache.Clear();
			ServerOfflineCache.Clear();
			_initialized = false;
		}
	}

	[Obsolete("This method is deprecated, please use CreateSingleServerUserContext(bool, string, string, bool, Uri, string) instead.")]
	public static UserContext CreateSingleServerUserContext(string username, string password, bool usingAD, Uri uri, string serverType = "XP")
	{
		return CreateSingleServerUserContext(secureOnly: false, username, password.ToSecureString(), usingAD, uri, serverType);
	}

	[Obsolete("This method is deprecated, please use CreateSingleServerUserContext(bool, string, string, bool, Uri, bool, string) instead.")]
	public static UserContext CreateSingleServerUserContext(string username, string password, bool usingAD, Uri uri, bool useQueryChanges, string serverType = "XP")
	{
		return CreateSingleServerUserContext(secureOnly: false, username, password.ToSecureString(), usingAD, uri, useQueryChanges, serverType);
	}

	[Obsolete("This method is deprecated, please use CreateSingleServerUserContext(bool, string, SecureString, bool, Uri, string) instead.")]
	public static UserContext CreateSingleServerUserContext(string username, SecureString password, bool usingAD, Uri uri, string serverType = "XP")
	{
		return CreateSingleServerUserContext(secureOnly: false, username, password, usingAD, uri, useQueryChanges: true, serverType);
	}

	[Obsolete("This method is deprecated, please use CreateSingleServerUserContext(bool, string, SecureString, bool, Uri, bool, string) instead.")]
	public static UserContext CreateSingleServerUserContext(string username, SecureString password, bool usingAD, Uri uri, bool useQueryChanges, string serverType = "XP")
	{
		return CreateSingleServerUserContext(secureOnly: false, username, password, usingAD, uri, useQueryChanges, serverType);
	}

	public static UserContext CreateSingleServerUserContext(bool secureOnly, string username, string password, bool usingAD, Uri uri, string serverType = "XP")
	{
		return CreateSingleServerUserContext(secureOnly, username, password.ToSecureString(), usingAD, uri, serverType);
	}

	public static UserContext CreateSingleServerUserContext(bool secureOnly, string username, string password, bool usingAD, Uri uri, bool useQueryChanges, string serverType = "XP")
	{
		return CreateSingleServerUserContext(secureOnly, username, password.ToSecureString(), usingAD, uri, useQueryChanges, serverType);
	}

	public static UserContext CreateSingleServerUserContext(bool secureOnly, string username, SecureString password, bool usingAD, Uri uri, string serverType = "XP")
	{
		return CreateSingleServerUserContext(secureOnly, username, password, usingAD, uri, useQueryChanges: true, serverType);
	}

	public static UserContext CreateSingleServerUserContext(bool secureOnly, string username, SecureString password, bool usingAD, Uri uri, bool useQueryChanges, string serverType = "XP")
	{
		UserContext userContext = new UserContext
		{
			CredentialCache = new CredentialCache()
		};
		userContext.Configuration = new SDKConfiguration(userContext);
		userContext.LogClient = LogClientFactory?.GetLogByContext(userContext);
		if (usingAD)
		{
			NetworkCredential cred = VideoOS.Platform.Login.Util.BuildNetworkCredential(username, password, "Negotiate");
			userContext.CredentialCache.Add(uri, "Negotiate", cred);
		}
		else
		{
			NetworkCredential cred = new NetworkCredential(username, password);
			userContext.CredentialCache.Add(uri, "Basic", cred);
		}
		userContext.AddLoginSettings(new LoginSettings(uri, userContext.CredentialCache, Guid.Empty, uri.DnsSafeHost, serverType, secureOnly));
		return SetupUserContextConfiguration(userContext, useQueryChanges);
	}

	public static UserContext CreateSingleServerUserContext(bool secureOnly, IMipTokenCache mipToken, Uri uri, bool useQueryChanges)
	{
		UserContext userContext = new UserContext
		{
			CredentialCache = new CredentialCache()
		};
		userContext.Configuration = new SDKConfiguration(userContext);
		userContext.LogClient = LogClientFactory?.GetLogByContext(userContext);
		userContext.AddLoginSettings(new LoginSettings(uri, mipToken, Guid.Empty, uri.DnsSafeHost, secureOnly));
		return SetupUserContextConfiguration(userContext, useQueryChanges);
	}

	private static UserContext SetupUserContextConfiguration(UserContext userContext, bool useQueryChanges)
	{
		((SDKConfiguration)userContext.Configuration).AddConfiguration(userContext.LoginSettings, masterOnly: true);
		((ISDKEnvironmentManager)EnvironmentManager.Instance).ApploggedState = true;
		if (useQueryChanges)
		{
			_queryChangesInstance.AddUserContext(userContext);
		}
		if (EnvironmentManager.Instance.TraceFunctionCalls)
		{
			EnvironmentManager instance = EnvironmentManager.Instance;
			Guid instanceGuid = userContext.InstanceGuid;
			instance.Log(error: false, "MultiEnvironment.CreateSingleServerUserContext", "UserContext " + instanceGuid.ToString());
		}
		return userContext;
	}

	public static bool LoginUserContext(UserContext userContext, bool autoReconnect = false, bool autoLoadConfiguration = false)
	{
		if (EnvironmentManager.Instance.TraceFunctionCalls)
		{
			EnvironmentManager instance = EnvironmentManager.Instance;
			Guid instanceGuid = userContext.InstanceGuid;
			instance.Log(error: false, "MultiEnvironment.LoginUserContext", "UserContext " + instanceGuid.ToString());
		}
		Exception lastException;
		bool num = ParallelHandler.TryLoginUserContext(userContext, out lastException, autoReconnect, autoLoadConfiguration);
		if (lastException != null)
		{
			EnvironmentManager.Instance.Log(error: false, "LoginUserContext", lastException.Message, null);
		}
		if (num)
		{
			_queryChangesInstance.StartQuery(updateLoginState: true);
			return true;
		}
		return false;
	}

	public static bool IsLoggedIn(UserContext userContext)
	{
		LoginSettings loginSettings = userContext.LoginSettings[0];
		return ((SDKConfiguration)userContext.Configuration).IsLoggedIn(loginSettings.Uri);
	}

	public static Exception GetLastException(UserContext userContext)
	{
		Exception ex = null;
		foreach (LoginSettings loginSetting in userContext.LoginSettings)
		{
			try
			{
				ex = ((SDKConfiguration)userContext.Configuration).LastException(loginSetting.Uri);
				if (ex != null)
				{
					return ex;
				}
			}
			catch (Exception)
			{
			}
		}
		return null;
	}

	public static bool LoadConfiguration(UserContext userContext)
	{
		LoginSettings loginSettings = userContext.LoginSettings[0];
		return ((ISDKConfiguration)userContext.Configuration).TryLoadConfiguration(loginSettings.Uri);
	}

	public static void ReloadConfiguration(UserContext userContext, FQID serverFQID)
	{
		((ISDKConfiguration)userContext.Configuration).RefreshServer(serverFQID);
	}

	[Obsolete("Deprecated, please use LoadSiteItem(bool, Uri, CredentialCache) instead")]
	public static Item LoadSiteItem(Uri uri, CredentialCache credentialCache)
	{
		return LoadSiteItem(secureOnly: false, uri, credentialCache);
	}

	public static Item LoadSiteItem(bool secureOnly, Uri uri, CredentialCache credentialCache)
	{
		return _siteUtil.LoadSiteItem(secureOnly, uri, credentialCache);
	}

	[Obsolete("Deprecated, please use LoadSiteItem(bool, Uri, CredentialCache, String) instead")]
	public static Item LoadSiteItem(Uri uri, CredentialCache credentialCache, string serverType)
	{
		return LoadSiteItem(secureOnly: false, uri, credentialCache, serverType);
	}

	public static Item LoadSiteItem(bool secureOnly, Uri uri, CredentialCache credentialCache, string serverType)
	{
		return _siteUtil.LoadSiteItem(secureOnly, uri, credentialCache, serverType);
	}

	public static Item LoadSiteItem(bool secureOnly, Uri uri, UserContext userContext)
	{
		if (userContext == null)
		{
			EnvironmentManager.Instance.Log(error: false, "MultiEnvironment.LoadSiteItem(bool, Uri, UserContext)", "UserContext must not be null");
			return null;
		}
		if (!IsLoggedIn(userContext))
		{
			EnvironmentManager instance = EnvironmentManager.Instance;
			Guid instanceGuid = userContext.InstanceGuid;
			instance.Log(error: false, "MultiEnvironment.LoadSiteItem(bool, Uri, UserContext)", "UserContext must be logged in. UserContext.InstanceGuid:  " + instanceGuid.ToString());
			return null;
		}
		return _siteUtil.LoadSiteItem(secureOnly, uri, userContext);
	}

	public static void Logout(UserContext userContext)
	{
		if (EnvironmentManager.Instance.TraceFunctionCalls)
		{
			EnvironmentManager instance = EnvironmentManager.Instance;
			Guid instanceGuid = userContext.InstanceGuid;
			instance.Log(error: false, "MultiEnvironment.Logout", "UserContext " + instanceGuid.ToString());
		}
		foreach (LoginSettings loginSetting in userContext.LoginSettings)
		{
			((SDKConfiguration)userContext.Configuration).Logout(loginSetting.Uri);
		}
		_queryChangesInstance.RemoveUserContext(userContext);
		if (_queryChangesInstance.UserContextCount == 0)
		{
			_queryChangesInstance.StopQuery(updateLoginState: true);
		}
	}

	public static void SetQueryChangeEnabled(UserContext userContext, bool enabled)
	{
		if (EnvironmentManager.Instance.TraceFunctionCalls)
		{
			EnvironmentManager instance = EnvironmentManager.Instance;
			Guid instanceGuid = userContext.InstanceGuid;
			instance.Log(error: false, "MultiEnvironment.SetQueryChangeEnabled", "UserContext " + instanceGuid.ToString() + ", set to " + enabled);
		}
		if (enabled)
		{
			_queryChangesInstance.AddUserContext(userContext);
		}
		else
		{
			_queryChangesInstance.RemoveUserContext(userContext);
		}
	}
}
