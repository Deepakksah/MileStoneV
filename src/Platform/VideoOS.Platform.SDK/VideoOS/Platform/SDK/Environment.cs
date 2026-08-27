using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Media;
using Autofac;
using Microsoft.Extensions.Logging;
using VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client;
using VideoOS.Platform.Common.Logging;
using VideoOS.Platform.Data;
using VideoOS.Platform.EventsAndState;
using VideoOS.Platform.Login;
using VideoOS.Platform.OAuth;
using VideoOS.Platform.SDK.Container;
using VideoOS.Platform.SDK.Platform;
using VideoOS.Platform.SDK.Util;

namespace VideoOS.Platform.SDK;

public class Environment
{
	private static IContainer _container;

	private static IEnvironmentService _service;

	private static readonly object _syncObject;

	private static bool _initialized;

	public static bool KeepLoginServerUriSchemeAllEnvironments
	{
		get
		{
			if (!Properties.KeepLoginServerUriScheme && !MultiEnvironment.Properties.KeepLoginServerUriScheme)
			{
				return MultiUserEnvironment.Properties.KeepLoginServerUriScheme;
			}
			return true;
		}
	}

	public static EnvironmentProperties Properties => (EnvironmentProperties)_service.Properties;

	static Environment()
	{
		_syncObject = new object();
		_initialized = false;
		RecreateContainer();
	}

	private static void RecreateContainer()
	{
		if (_container != null)
		{
			((IDisposable)_container).Dispose();
		}
		_container = VideoOS.Platform.SDK.Container.Configuration.BuildContainer();
		_service = ResolutionExtensions.Resolve<IEnvironmentService>((IComponentContext)(object)_container);
	}

	public static void Initialize()
	{
		lock (_syncObject)
		{
			Initialize(_container);
		}
	}

	public static void Initialize(IContainer container)
	{
		if (!_initialized)
		{
			EnvironmentManager.Instance = ResolutionExtensions.Resolve<EnvironmentManager>((IComponentContext)(object)container);
			BookmarkService.Instance = ResolutionExtensions.Resolve<BookmarkService>((IComponentContext)(object)container);
			Configuration.Instance = ResolutionExtensions.Resolve<Configuration>((IComponentContext)(object)container);
			RestrictedMediaService.Instance = ResolutionExtensions.Resolve<RestrictedMediaService>((IComponentContext)(object)container);
			VideoOS.Platform.Common.Logging.Logger mipLogger = new VideoOS.Platform.Common.Logging.Logger();
			VideoOS.Platform.EventsAndState.EventsAndStateSession.SetEventsAndSessionFactory(delegate(Uri uri, IMipTokenCache mipTokenCache, IEventReceiver eventReceiver)
			{
				VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client.EventsAndStateSession eventsAndStateSession = new VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client.EventsAndStateSession(new GatewayProvider(uri), mipTokenCache, eventReceiver, (ILogger)(object)mipLogger);
				Action item = eventsAndStateSession.Connect;
				return ((IEventsAndStateSession, Action))(eventsAndStateSession, item);
			});
			ResolutionExtensions.Resolve<UserContext>((IComponentContext)(object)container).Configuration = Configuration.Instance;
			_service = ResolutionExtensions.Resolve<IEnvironmentService>((IComponentContext)(object)container);
			_service.Init();
			_initialized = true;
			EnvironmentManager.Instance.Log(error: false, "Environment:Initialize", "");
		}
	}

	public static void UnInitialize()
	{
		lock (_syncObject)
		{
			EnvironmentManager.Instance?.Log(error: false, "Environment:UnInitialize", "");
			(EnvironmentManager.Instance as SDKEnvironmentManager)?.TelemetryFlush();
			_service.Reset();
			EnvironmentManager.Instance = null;
			BookmarkService.Instance = null;
			Configuration.Instance = null;
			RestrictedMediaService.Instance = null;
			RecreateContainer();
			_initialized = false;
		}
	}

	public static void AddServer(bool secureOnly, Uri uri, NetworkCredential networkCredential, bool masterOnly = false)
	{
		_service.AddServer(secureOnly, uri, networkCredential, masterOnly);
	}

	public static void AddServer(LoginSettings loginSettings, bool masterOnly = false)
	{
		_service.AddServer(loginSettings, masterOnly);
	}

	public static void AddServer(bool secureOnly, Uri uri, CredentialCache credentialCache, bool masterOnly = false)
	{
		_service.AddServer(secureOnly, uri, credentialCache, masterOnly);
	}

	public static void AddServer(bool secureOnly, Item siteItem, CredentialCache credentialCache, bool masterOnly = false)
	{
		_service.AddServer(secureOnly, siteItem, credentialCache, masterOnly);
	}

	public static void AddServerOAuth(bool secureOnly, Uri uri, IMipTokenCache mipTokenCache, bool masterOnly = true)
	{
		LoginSettings loginSettings = new LoginSettings(uri, mipTokenCache, Guid.Empty, uri.DnsSafeHost, secureOnly);
		ServerTypeCache.Fill(loginSettings);
		_service.AddServer(loginSettings, masterOnly);
	}

	[Obsolete("This method is deprecated, please use AddServer(bool, Uri, NetworkCredential, bool) instead.")]
	public static void AddServer(Uri uri, NetworkCredential networkCredential, bool masterOnly = false)
	{
		_service.AddServer(secureOnly: false, uri, networkCredential, masterOnly);
	}

	[Obsolete("This method is deprecated, please use AddServer(bool, Uri, CredentialCache, bool) instead.")]
	public static void AddServer(Uri uri, CredentialCache credentialCache, bool masterOnly = false)
	{
		_service.AddServer(secureOnly: false, uri, credentialCache, masterOnly);
	}

	[Obsolete("This method is deprecated, please use AddServer(bool, Item, CredentialCache, bool) instead.")]
	public static void AddServer(Item siteItem, CredentialCache credentialCache, bool masterOnly = false)
	{
		_service.AddServer(secureOnly: false, siteItem, credentialCache, masterOnly);
	}

	[Obsolete("This method is deprecated, please use LoadSiteItem(bool, Uri, CredentialCache) instead.")]
	public static Item LoadSiteItem(Uri uri, CredentialCache credentialCache)
	{
		return LoadSiteItem(secureOnly: false, uri, credentialCache);
	}

	public static Item LoadSiteItem(bool secureOnly, Uri uri, CredentialCache credentialCache)
	{
		return _service.LoadSiteItem(secureOnly, uri, credentialCache);
	}

	public static Item LoadSiteItem(bool secureOnly, Uri uri, CredentialCache credentialCache, string serverType)
	{
		return _service.LoadSiteItem(secureOnly, uri, credentialCache, serverType);
	}

	[Obsolete("This method is deprecated, please use LoadSiteItem(bool, Uri, CredentialCache, string) instead.")]
	public static Item LoadSiteItem(Uri uri, CredentialCache credentialCache, string serverType)
	{
		return LoadSiteItem(secureOnly: false, uri, credentialCache, serverType);
	}

	public static Item LoadSiteItem(bool secureOnly, Uri uri)
	{
		if (_service.IsLoggedIn(uri))
		{
			return _service.LoadSiteItem(secureOnly, uri);
		}
		EnvironmentManager.Instance.Log(error: false, "Environment.LoadSiteItem(bool, Uri)", "The user must be logged in to the server. Server URI:  " + uri);
		return null;
	}

	public static bool IsServerConnected(Uri uri)
	{
		return _service.IsServerConnected(uri);
	}

	[Obsolete("This method is obsolete - please use Login(Uri, Guid, string, string, string, bool) instead.")]
	public static void Login(Uri serverUri, bool masterOnly = false)
	{
		_service.Login(serverUri, masterOnly);
	}

	public static void Login(Uri serverUri, Guid integrationId, string integrationName, string version, string manufacturerName, bool masterOnly = false)
	{
		_service.Login(serverUri, masterOnly, integrationId, integrationName, version, manufacturerName);
	}

	public static bool LoadConfiguration(Uri serverUri)
	{
		return _service.LoadConfiguration(serverUri);
	}

	public static void ReloadConfiguration(FQID serverFQID)
	{
		_service.ReloadConfiguration(serverFQID);
	}

	public static void Logout(Uri serverUri)
	{
		_service.Logout(serverUri);
	}

	public static bool IsLoggedIn(Uri serverUri)
	{
		return _service.IsLoggedIn(serverUri);
	}

	public static bool IsLoggedInToAnyServer()
	{
		return _service.IsLoggedInToAnyServer();
	}

	public static void RemoveAllServers()
	{
		_service.RemoveAllServers();
	}

	public static void RemoveServer(Guid id)
	{
		_service.RemoveServer(id);
	}

	public static void RemoveServer(Uri uri)
	{
		_service.RemoveServer(uri);
	}

	public static void Logout()
	{
		_service.Logout();
	}

	public static List<Item> GetDisabledDevices(Guid serverId)
	{
		return _service.GetDisabledDevices(serverId);
	}

	public static void InitializeUIFramework()
	{
		InitializeUIFramework(null);
	}

	public static void InitializeUIFramework(IDictionary<string, Color> brandColors)
	{
		_service.InitializeUIFramework(brandColors);
	}

	[Obsolete("Please call InitializeUIFramework(IDictionary<string, Color>) instead to apply the branding colors during the initialization phase")]
	public static void ApplyBrandColors(IDictionary<string, Color> brandColors)
	{
		_service.LogError("Environment.ApplyBrandColors", "Obsolete method 'ApplyBrandColors()' called.");
	}
}
