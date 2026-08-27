#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Platform.Common.OAuth;
using VideoOS.Platform.Login;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Platform;
using VideoOS.Platform.SDK.Platform.Exceptions;
using VideoOS.Platform.SDK.Platform.ServerSlaveInfo;
using VideoOS.Platform.SDK.Util;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.SDK.Server;

public class ParallelHandler
{
	public class ServerConnectParameters
	{
		public volatile bool Success;

		public volatile bool Complete;

		public Thread Thread;

		public ISystemInfo SystemInfo;

		public CredentialCache CredentialCache;

		public ISystemInfo MasterSiteSystemInfo;

		public bool AutoReconnect;

		public bool AutoLoadConfiguration;

		public bool Https;

		public bool Unauthorized;

		public bool ExceptionReceived;

		public string Address;

		public string OrginalAddress;

		public string ServerType;
	}

	public static bool TryLoginUserContext(UserContext userContext, out Exception lastException, bool autoReconnect, bool autoLoadConfiguration)
	{
		bool flag = false;
		lastException = null;
		List<ServerConnectParameters> list = new List<ServerConnectParameters>();
		foreach (LoginSettings item in userContext.LoginSettings.Where((LoginSettings x) => ServerOfflineCache.IsServerOnline(x.Uri)))
		{
			ISystemInfo systemInfo = null;
			lock (userContext.Configuration)
			{
				foreach (ISystemInfo value in ((SDKConfiguration)userContext.Configuration).OnlineSystemInfos.Values)
				{
					ServerTrace.Info("ParallelHandler", "TryLoginUserContext", $"Try to log in user context to the server: {value.LoginSettings.Uri}");
					if (!value.IsLoggedIn())
					{
						if (value.LoginSettings.Uri.Equals(item.Uri) || item.Uri.DnsSafeHost == value.LoginSettings.Uri.DnsSafeHost)
						{
							systemInfo = value;
							break;
						}
					}
					else
					{
						flag = true;
					}
				}
			}
			if (systemInfo != null)
			{
				list.Add(TryLogin(systemInfo, autoReconnect, autoLoadConfiguration));
			}
		}
		bool flag2 = true;
		while (flag2)
		{
			flag2 = false;
			foreach (ServerConnectParameters item2 in list)
			{
				if (item2.Success)
				{
					flag = true;
				}
				else if (item2.SystemInfo.LastException != null)
				{
					lastException = item2.SystemInfo.LastException;
				}
				if (!flag && !item2.Complete)
				{
					flag2 = true;
					break;
				}
			}
			if (flag2)
			{
				Thread.Sleep(50);
			}
		}
		return flag;
	}

	private static ServerConnectParameters TryLogin(ISystemInfo systemInfo, bool autoReconnect, bool autoLoadConfiguration)
	{
		ServerConnectParameters serverConnectParameters = new ServerConnectParameters();
		serverConnectParameters.Thread = new Thread(TryLoginThread);
		serverConnectParameters.Thread.Name = "TryLogin thread";
		serverConnectParameters.Thread.IsBackground = true;
		serverConnectParameters.SystemInfo = systemInfo;
		serverConnectParameters.AutoReconnect = autoReconnect;
		serverConnectParameters.AutoLoadConfiguration = autoLoadConfiguration;
		serverConnectParameters.ExceptionReceived = false;
		serverConnectParameters.Thread.Start(serverConnectParameters);
		return serverConnectParameters;
	}

	private static void TryLoginThread(object state)
	{
		ServerConnectParameters serverConnectParameters = (ServerConnectParameters)state;
		try
		{
			if (!serverConnectParameters.SystemInfo.Login())
			{
				serverConnectParameters.SystemInfo.SetConnectRetry(serverConnectParameters.AutoReconnect);
			}
			else
			{
				if (!string.IsNullOrEmpty(serverConnectParameters.SystemInfo.UserContext.ClientType) && !SecurityAccess.IsClientAllowed(serverConnectParameters.SystemInfo.ServerId, serverConnectParameters.SystemInfo.UserContext.ClientType))
				{
					serverConnectParameters.SystemInfo.Logout();
					throw new ClientTypeNotAllowedMIPException("Client type not allowed");
				}
				serverConnectParameters.SystemInfo.SetConnectRetry(serverConnectParameters.AutoReconnect);
				if (serverConnectParameters.AutoLoadConfiguration)
				{
					serverConnectParameters.SystemInfo.Populate();
				}
				serverConnectParameters.Success = true;
			}
		}
		catch (Exception lastException)
		{
			serverConnectParameters.Success = false;
			serverConnectParameters.SystemInfo.LastException = lastException;
		}
		serverConnectParameters.Complete = true;
	}

	public static bool PopulateUserContext(UserContext userContext, out Exception lastException)
	{
		bool flag = false;
		lastException = null;
		List<ServerConnectParameters> list = new List<ServerConnectParameters>();
		foreach (LoginSettings item in userContext.LoginSettings.Where((LoginSettings x) => ServerOfflineCache.IsServerOnline(x.Uri)).ToList())
		{
			ISystemInfo systemInfo = null;
			ISystemInfo masterSiteSystemInfo = null;
			lock (userContext.Configuration)
			{
				masterSiteSystemInfo = ((SDKConfiguration)userContext.Configuration).MasterServerSystemInfo;
				foreach (ISystemInfo value in ((SDKConfiguration)userContext.Configuration).OnlineSystemInfos.Values)
				{
					ServerTrace.Info("ParallelHandler", "PopulateUserContext", $"Populating user context for server: {value.LoginSettings.Uri}");
					if (value.LoginSettings.Uri.Equals(item.Uri) || item.Uri.DnsSafeHost == value.LoginSettings.Uri.DnsSafeHost)
					{
						if (!value.PopulatedHasBeenTried)
						{
							systemInfo = value;
						}
						else
						{
							flag |= value.IsLoggedIn();
						}
						break;
					}
				}
			}
			if (systemInfo != null)
			{
				list.Add(Populate(systemInfo, masterSiteSystemInfo));
			}
		}
		bool flag2 = true;
		while (flag2)
		{
			flag2 = false;
			foreach (ServerConnectParameters item2 in list)
			{
				lock (item2)
				{
					if (item2.Success)
					{
						flag = true;
					}
					else if (item2.SystemInfo.LastException != null)
					{
						lastException = item2.SystemInfo.LastException;
					}
					if (!flag && !item2.Complete)
					{
						flag2 = true;
						break;
					}
				}
			}
			if (flag2)
			{
				Thread.Sleep(50);
			}
		}
		return flag;
	}

	public static ServerConnectParameters Populate(ISystemInfo systemInfo, ISystemInfo masterSiteSystemInfo)
	{
		ServerConnectParameters serverConnectParameters = new ServerConnectParameters();
		serverConnectParameters.Thread = new Thread(PopulateThread);
		serverConnectParameters.Thread.Name = "Populate thread";
		serverConnectParameters.Thread.IsBackground = true;
		serverConnectParameters.SystemInfo = systemInfo;
		serverConnectParameters.MasterSiteSystemInfo = masterSiteSystemInfo;
		serverConnectParameters.ExceptionReceived = false;
		serverConnectParameters.Thread.Start(serverConnectParameters);
		return serverConnectParameters;
	}

	private static void PopulateThread(object state)
	{
		ServerConnectParameters serverConnectParameters = (ServerConnectParameters)state;
		try
		{
			serverConnectParameters.SystemInfo.Populate();
			if (serverConnectParameters.SystemInfo.IsConnected)
			{
				serverConnectParameters.Success = true;
				AddSlaveConfigurations(serverConnectParameters.SystemInfo, serverConnectParameters.MasterSiteSystemInfo);
			}
		}
		catch (Exception lastException)
		{
			serverConnectParameters.Success = false;
			serverConnectParameters.SystemInfo.LastException = lastException;
		}
		serverConnectParameters.Complete = true;
	}

	public static void AddSlaveConfigurations(ISystemInfo parentSiteSystemInfo, ISystemInfo masterSiteSystemInfo)
	{
		if (parentSiteSystemInfo.MasterOnly || parentSiteSystemInfo.SlavesPopulated)
		{
			return;
		}
		SiteInfoManager siteInfoManager = new SiteInfoManager();
		SiteInfo site = siteInfoManager.GetSite(parentSiteSystemInfo.ServerId.Id, parentSiteSystemInfo.ServerId.UserContext);
		if (site == null)
		{
			EnvironmentManager.Instance.Log(error: false, "ParallelHandler:AddSlaveConfigurations", "SiteInfoManager.GetSite() returned null for " + parentSiteSystemInfo.Name);
			return;
		}
		List<SiteInfo> list = new List<SiteInfo>();
		foreach (Guid child in site.Children)
		{
			ISystemInfo systemInfo = ((SDKConfiguration)parentSiteSystemInfo.UserContext.Configuration).GetSystemInfo(child);
			if (systemInfo != null)
			{
				Trace.WriteLine("ParallelHandler: Finding slaves again?  " + systemInfo.Name);
				continue;
			}
			SiteInfo site2 = siteInfoManager.GetSite(child, systemInfo?.UserContext);
			if (site2 != null)
			{
				list.Add(site2);
			}
		}
		List<IServerSlaveInfo> list2 = new List<IServerSlaveInfo>();
		foreach (SiteInfo item in list)
		{
			list2.Clear();
			if (item.AddressCollection.Count <= 0)
			{
				continue;
			}
			bool secureOnlyFlagForSubSite = SiteUtil.GetSecureOnlyFlagForSubSite(parentSiteSystemInfo.LoginSettings.SecureOnly, item.ServerVersion);
			foreach (Address item2 in item.AddressCollection)
			{
				Uri uri = new Uri(item2.Url);
				ManualResetEvent doneEvent = new ManualResetEvent(initialState: false);
				if (parentSiteSystemInfo.LoginSettings.NetworkCredential != null)
				{
					SlaveServerInfoWithCredentials slaveInfo = new SlaveServerInfoWithCredentials(uri, item.Id, secureOnlyFlagForSubSite, parentSiteSystemInfo.LoginSettings.NetworkCredential.UserName, parentSiteSystemInfo.LoginSettings.NetworkCredential.SecurePassword, parentSiteSystemInfo.LoginSettings.IsBasicUser, doneEvent, parentSiteSystemInfo.UserContext, parentSiteSystemInfo);
					list2.Add(slaveInfo);
					Task.Run(delegate
					{
						AddSlaveIfReachable(slaveInfo);
					});
					continue;
				}
				MipTokenCacheInternal tokenCache = new MipTokenCacheInternal(new Uri(new Uri(item2.Url), "idp"), masterSiteSystemInfo.LoginSettings.IdentityTokenCache);
				SlaveServerInfoWithTokenCache slaveInfo2 = new SlaveServerInfoWithTokenCache(uri, item.Id, secureOnlyFlagForSubSite, tokenCache, doneEvent, parentSiteSystemInfo.UserContext, parentSiteSystemInfo);
				list2.Add(slaveInfo2);
				Task.Run(delegate
				{
					AddSlaveIfReachable(slaveInfo2);
				});
			}
			while (list2.Count > 0)
			{
				WaitHandle[] waitHandles = list2.Select((IServerSlaveInfo si) => si.DoneEvent).ToArray();
				int index = WaitHandle.WaitAny(waitHandles);
				IServerSlaveInfo serverSlaveInfo = list2[index];
				if (serverSlaveInfo.LoggedIn)
				{
					break;
				}
				list2.Remove(serverSlaveInfo);
			}
		}
		((SystemInfo)parentSiteSystemInfo).SlavesPopulated = true;
	}

	private static void AddSlaveIfReachable(IServerSlaveInfo slaveInfo)
	{
		LoginSettings loginSettings = slaveInfo.GetLoginSettings();
		lock (slaveInfo.UserContext)
		{
			slaveInfo.UserContext.AddLoginSettings(loginSettings);
		}
		SystemInfo systemInfo = SDKConfiguration.CreateSystemInfo(loginSettings, slaveInfo.UserContext, slaveInfo.MasterInfo.MasterOnly);
		try
		{
			if (PingUtil.IsReachable(slaveInfo.ServerUri.DnsSafeHost))
			{
				systemInfo.Login();
			}
			lock (slaveInfo.UserContext.Configuration)
			{
				if (systemInfo.IsLoggedIn() && !slaveInfo.MasterInfo.SlaveIds.Contains(systemInfo.Id))
				{
					((SDKConfiguration)slaveInfo.UserContext.Configuration).AddConfiguration(systemInfo);
					slaveInfo.MasterInfo.SlaveIds.Add(systemInfo.Id);
					slaveInfo.LoggedIn = true;
				}
			}
		}
		catch (Exception ex)
		{
			if (slaveInfo is SlaveServerInfoWithTokenCache slaveServerInfoWithTokenCache)
			{
				slaveServerInfoWithTokenCache.TokenCache.Dispose();
			}
			EnvironmentManager.Instance.Log(error: false, "AddSlaveIfReachable", string.Format(CultureInfo.InvariantCulture, "Failed to add slave server {0}. Exception: {1}", slaveInfo.ServerUri, ex.Message));
		}
		slaveInfo.DoneEvent.Set();
	}

	public static string FindBestAddress(bool secureOnly, List<string> addresses, CredentialCache credentialCache)
	{
		bool unauthorized;
		return FindBestAddress(secureOnly, addresses, credentialCache, out unauthorized);
	}

	public static string FindBestAddress(bool secureOnly, List<string> addresses, CredentialCache credentialCache, out bool unauthorized)
	{
		string serverType = null;
		foreach (string address in addresses)
		{
			string serverType2 = ServerTypeCache.GetServerType(new UriBuilder(address).Uri);
			if (!string.IsNullOrEmpty(serverType2))
			{
				serverType = serverType2;
				break;
			}
		}
		return FindBestAddress(secureOnly, addresses, credentialCache, out unauthorized, serverType);
	}

	public static string FindBestAddress(bool secureOnly, List<string> addresses, CredentialCache credentialCache, out bool unauthorized, string serverType)
	{
		List<ServerConnectParameters> list = new List<ServerConnectParameters>();
		Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
		unauthorized = false;
		string text = null;
		bool flag = false;
		foreach (string address in addresses)
		{
			try
			{
				string originalAddress = address;
				Uri uri = new UriBuilder(UriUtil.EnsureHttpPrefix(address, Uri.UriSchemeHttps)).Uri;
				string httpAddress = GetHttpAddress(uri);
				string httpsAddress = GetHttpsAddress(uri);
				if (!dictionary.ContainsKey(httpAddress))
				{
					if (serverType == null || serverType == "XP")
					{
						flag = IsOAuthServerFound(uri, secureOnly, out var foundAddress);
						if (flag)
						{
							text = foundAddress;
						}
						if (!flag && !secureOnly && credentialCache != null)
						{
							list.Add(FindBestAddressCreate(httpsAddress, "XPCO", credentialCache));
							list.Add(FindBestAddressCreate(httpAddress, "XPCO", credentialCache));
						}
					}
					else
					{
						flag = false;
						if (serverType == "XPCO")
						{
							flag = IsOAuthServerFound(uri, secureOnly, out var foundAddress2);
							if (flag)
							{
								text = foundAddress2;
							}
						}
						if (!flag && !secureOnly && credentialCache != null)
						{
							list.Add(FindBestAddressCreate(httpAddress, serverType, credentialCache, originalAddress));
						}
					}
					dictionary[httpAddress] = true;
				}
			}
			catch (ForbiddenMIPException)
			{
				EnvironmentManager.Instance.Log(error: false, "FindBestAddress", "Input server URI scheme is not valid for IDP security settings");
			}
			catch (Exception)
			{
				EnvironmentManager.Instance.Log(error: false, "FindBestAddress", "Unable to use address:" + address);
			}
			if (flag)
			{
				list.Clear();
				break;
			}
		}
		long ticks = DateTime.Now.Ticks;
		TimeSpan timeSpan = default(TimeSpan);
		bool flag2 = true;
		bool flag3 = false;
		bool flag4 = true;
		bool flag5 = false;
		while (flag2 && (!flag3 || flag4))
		{
			flag2 = false;
			foreach (ServerConnectParameters item in list)
			{
				lock (item)
				{
					if (item.Complete)
					{
						if (item.Https)
						{
							flag4 = false;
						}
						if (item.Success)
						{
							text = item.Address;
							if (!flag5)
							{
								unauthorized = item.Unauthorized;
								if (!item.Unauthorized)
								{
									flag5 = true;
								}
							}
							if (flag4)
							{
								if (item.ServerType == "XPCO")
								{
									timeSpan = new TimeSpan((DateTime.Now.Ticks - ticks) * 4) + TimeSpan.FromSeconds(2.0);
								}
								flag4 = false;
							}
							flag3 = true;
						}
						else if (!flag5)
						{
							unauthorized = item.Unauthorized;
						}
					}
					else
					{
						flag2 = true;
					}
				}
			}
			if (flag2)
			{
				Thread.Sleep(50);
				continue;
			}
			if (DateTime.Now.Ticks > ticks + timeSpan.Ticks)
			{
				break;
			}
			Thread.Sleep(50);
		}
		if (text == null)
		{
			if (addresses.Count == 1)
			{
				EnvironmentManager.Instance.Log(error: true, "FindBestAddress", $"No server found on address: {addresses[0]}");
			}
			else
			{
				EnvironmentManager.Instance.Log(error: true, "FindBestAddress", string.Format("No server found on addresses: {0}", string.Join(",", addresses.ToArray())));
			}
		}
		else
		{
			EnvironmentManager.Instance.Log(error: false, "FindBestAddress", $"Address was found: {text}. Is OAuth server: {flag}");
		}
		return text;
	}

	public static async Task<string> FindBestAddressOAuth(List<Uri> addresses, bool secureOnly)
	{
		foreach (Uri address in addresses)
		{
			string foundAddress = null;
			await Task.Run(() => IsOAuthServerFound(address, secureOnly, out foundAddress)).ConfigureAwait(continueOnCapturedContext: false);
			if (foundAddress != null)
			{
				return foundAddress;
			}
		}
		return null;
	}

	private static ServerConnectParameters FindBestAddressCreate(string address, string serverType, CredentialCache credentialCache, string originalAddress = "")
	{
		ServerConnectParameters serverConnectParameters = new ServerConnectParameters();
		serverConnectParameters.Thread = new Thread(FindBestAddress);
		serverConnectParameters.Thread.Name = "FindBestAddress thread " + address;
		serverConnectParameters.Thread.IsBackground = true;
		serverConnectParameters.Address = address;
		serverConnectParameters.OrginalAddress = originalAddress;
		serverConnectParameters.ServerType = serverType;
		serverConnectParameters.CredentialCache = credentialCache;
		serverConnectParameters.Https = false;
		serverConnectParameters.Unauthorized = false;
		serverConnectParameters.ExceptionReceived = false;
		serverConnectParameters.Thread.Start(serverConnectParameters);
		return serverConnectParameters;
	}

	private static bool IsOAuthServerFound(Uri plainUri, bool secureOnly, out string foundAddress)
	{
		foundAddress = null;
		if (ServerOfflineCache.IsServerOffline(plainUri))
		{
			return false;
		}
		foundAddress = GetOAuthServerAddress(plainUri, secureOnly);
		if (string.IsNullOrEmpty(foundAddress))
		{
			return false;
		}
		Uri uri = UrlUtil.BuildUri(foundAddress);
		ServerTypeCache.ServerIdentified(uri, uri.Scheme == Uri.UriSchemeHttps, isOAuth: true, plainUri.ToString());
		ServerOfflineCache.RemoveServer(uri);
		return true;
	}

	private static string GetOAuthServerAddress(Uri plainUri, bool secureOnly)
	{
		string httpsAddress = GetHttpsAddress(plainUri);
		string httpAddress = GetHttpAddress(plainUri);
		if (!Environment.KeepLoginServerUriSchemeAllEnvironments)
		{
			if (IsOAuthServerOnline(httpsAddress))
			{
				return httpsAddress;
			}
			if (!secureOnly && IsOAuthServerOnline(httpAddress))
			{
				return httpAddress;
			}
		}
		else
		{
			if (secureOnly && plainUri.Scheme != Uri.UriSchemeHttps)
			{
				EnvironmentManager.Instance.Log(error: true, "ParallelHandler:GetOAuthServerAddress", "Input server URI scheme is not valid for given security settings. Expected " + Uri.UriSchemeHttps + " but was " + plainUri.Scheme + ".");
				return null;
			}
			if (plainUri.Scheme == Uri.UriSchemeHttps && IsOAuthServerOnline(httpsAddress))
			{
				return httpsAddress;
			}
			if (plainUri.Scheme == Uri.UriSchemeHttp && IsOAuthServerOnline(httpAddress))
			{
				return httpAddress;
			}
		}
		EnvironmentManager.Instance.Log(error: false, "ParallelHandler:GetOAuthServerAddress", $"No OAuth server found at {plainUri}");
		return null;
	}

	private static bool IsOAuthServerOnline(string address)
	{
		if (ServerOfflineCache.IsServerOnline(new Uri(address)))
		{
			return Task.Run(() => new ConnectionCheck().IsOAuthServerOnline(address)).GetAwaiter().GetResult();
		}
		return false;
	}

	private static string GetHttpAddress(Uri plainUri)
	{
		if (!plainUri.IsDefaultPort)
		{
			return new UriBuilder(Uri.UriSchemeHttp, plainUri.Host, plainUri.Port).Uri.ToString();
		}
		return new UriBuilder(Uri.UriSchemeHttp, plainUri.Host).Uri.ToString();
	}

	private static string GetHttpsAddress(Uri plainUri)
	{
		if (!plainUri.IsDefaultPort)
		{
			return new UriBuilder(Uri.UriSchemeHttps, plainUri.Host, plainUri.Port).Uri.ToString();
		}
		return new UriBuilder(Uri.UriSchemeHttps, plainUri.Host).Uri.ToString();
	}

	private static void FindBestAddress(object state)
	{
		ServerConnectParameters serverConnectParameters = (ServerConnectParameters)state;
		Uri serverAddress = new Uri(serverConnectParameters.Address);
		try
		{
			if (ServerOfflineCache.IsServerOnline(serverAddress))
			{
				if (ConnectionCheck.IsLegacyServerOnline(serverConnectParameters.Address, out serverConnectParameters.Https, serverConnectParameters.CredentialCache, out var unauthorized))
				{
					ServerOfflineCache.RemoveServer(serverAddress);
					lock (serverConnectParameters)
					{
						ServerTypeCache.ServerIdentified(UrlUtil.BuildUri(serverConnectParameters.Address), serverConnectParameters.Https, isOAuth: false, serverConnectParameters.OrginalAddress);
						serverConnectParameters.Success = true;
					}
				}
				else
				{
					ServerOfflineCache.AddServer(serverAddress);
				}
				serverConnectParameters.Unauthorized = unauthorized;
			}
		}
		catch (Exception)
		{
			ServerOfflineCache.AddServer(serverAddress);
			serverConnectParameters.ExceptionReceived = true;
			lock (serverConnectParameters)
			{
				serverConnectParameters.Success = false;
			}
		}
		lock (serverConnectParameters)
		{
			serverConnectParameters.Complete = true;
		}
	}

	public static void RecheckServerOnlineState(string address, CredentialCache credentialCache, bool useOAuth)
	{
		Uri serverAddress = new Uri(address);
		try
		{
			if (ServerOfflineCache.IsServerOnline(serverAddress))
			{
				bool https = false;
				bool flag = false;
				bool flag2 = false;
				if (useOAuth)
				{
					flag = CheckOAuthServerOnline(address, out https);
				}
				else
				{
					flag2 = CheckLegacyServerOnline(address, out https, credentialCache);
				}
				if (flag || flag2)
				{
					ServerOfflineCache.RemoveServer(serverAddress);
					ServerTypeCache.ServerIdentified(UrlUtil.BuildUri(address), https, useOAuth, address);
				}
				else
				{
					ServerOfflineCache.AddServer(serverAddress);
				}
			}
		}
		catch (Exception)
		{
			ServerOfflineCache.AddServer(serverAddress);
		}
	}

	private static bool CheckOAuthServerOnline(string address, out bool https)
	{
		bool result = IsOAuthServerOnline(address);
		https = address.StartsWith("https", StringComparison.InvariantCulture);
		return result;
	}

	private static bool CheckLegacyServerOnline(string address, out bool https, CredentialCache credentialCache)
	{
		if (ConnectionCheck.IsLegacyServerOnline(address, out https, credentialCache, out var unauthorized) || unauthorized)
		{
			return true;
		}
		return false;
	}
}
