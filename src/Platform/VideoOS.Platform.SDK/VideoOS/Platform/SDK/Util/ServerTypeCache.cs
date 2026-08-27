using System;
using System.Collections.ObjectModel;
using VideoOS.Platform.Login;

namespace VideoOS.Platform.SDK.Util;

public static class ServerTypeCache
{
	public class ServerInfo
	{
		public string OriginalServerAddress;

		public string ServerAddress;

		public string ServerType;

		public bool IsHttpValid;

		public bool IsHttpsValid;

		public bool IsOAuthServer;

		public int InvalidCount;

		public ServerInfo(string serverAddress, string servertype, bool isHttpValid, bool isHttpsValid, bool isOAuthServer, string originalAddress)
		{
			ServerAddress = serverAddress;
			ServerType = servertype;
			IsHttpValid = isHttpValid;
			IsHttpsValid = isHttpsValid;
			IsOAuthServer = isOAuthServer;
			InvalidCount = 0;
			if (string.IsNullOrEmpty(originalAddress))
			{
				OriginalServerAddress = ServerAddress;
			}
			else
			{
				OriginalServerAddress = originalAddress;
			}
		}
	}

	private static Collection<ServerInfo> _serverInfos = new Collection<ServerInfo>();

	public static ServerInfo TryGet(Uri uri)
	{
		lock (_serverInfos)
		{
			foreach (ServerInfo serverInfo in _serverInfos)
			{
				if (CompareUriWithAddress(uri, serverInfo))
				{
					return serverInfo;
				}
			}
			return null;
		}
	}

	private static bool CompareUriWithAddress(Uri uri, ServerInfo serverInfo)
	{
		int port;
		string dnsSafeHost = GetDnsSafeHost(serverInfo.ServerAddress, serverInfo.IsOAuthServer, out port);
		int port2;
		string dnsSafeHost2 = GetDnsSafeHost(serverInfo.OriginalServerAddress, serverInfo.IsOAuthServer, out port2);
		bool flag = (uri.DnsSafeHost.Equals(dnsSafeHost, StringComparison.InvariantCultureIgnoreCase) && uri.Port == port) || (uri.DnsSafeHost.Equals(dnsSafeHost2, StringComparison.InvariantCultureIgnoreCase) && uri.Port == port2);
		ServerTrace.Info("CompareUriWithAddress", $"CompareUri was: {flag} for {uri.ToString()} ", serverInfo.OriginalServerAddress);
		return flag;
	}

	public static void ServerIdentified(LoginSettings loginSettings, string originalAddress = null)
	{
		lock (_serverInfos)
		{
			ServerInfo serverInfo = TryGet(loginSettings.Uri);
			if (serverInfo != null)
			{
				serverInfo.InvalidCount = 0;
				return;
			}
			serverInfo = new ServerInfo(loginSettings.Uri.ToString(), loginSettings.ServerType, loginSettings.ServerWebType == "ASMX", loginSettings.ServerWebType == "SVC", loginSettings.IsOAuthConnection, originalAddress);
			_serverInfos.Add(serverInfo);
		}
	}

	public static void ServerIdentified(Uri uri, bool https, bool isOAuth, string originalAddress = "")
	{
		lock (_serverInfos)
		{
			ServerInfo serverInfo = TryGet(uri);
			if (serverInfo != null)
			{
				serverInfo.InvalidCount = 0;
				if (https)
				{
					serverInfo.IsHttpsValid = true;
				}
				else
				{
					serverInfo.IsHttpValid = true;
				}
				serverInfo.IsOAuthServer = isOAuth;
			}
			else
			{
				serverInfo = new ServerInfo(uri.ToString(), "XPCO", !https, https, isOAuth, originalAddress);
				_serverInfos.Add(serverInfo);
			}
		}
	}

	public static void ServerNotFound(LoginSettings loginSettings)
	{
		lock (_serverInfos)
		{
			ServerInfo serverInfo = TryGet(loginSettings.Uri);
			if (serverInfo != null)
			{
				serverInfo.InvalidCount++;
				if (serverInfo.InvalidCount > 2)
				{
					_serverInfos.Remove(serverInfo);
				}
			}
		}
	}

	public static void RemoveServer(LoginSettings loginSettings)
	{
		lock (_serverInfos)
		{
			ServerInfo serverInfo = TryGet(loginSettings.Uri);
			if (serverInfo != null)
			{
				_serverInfos.Remove(serverInfo);
			}
		}
	}

	public static void RemoveAllServers()
	{
		lock (_serverInfos)
		{
			_serverInfos.Clear();
		}
	}

	public static void Fill(LoginSettings loginSettings)
	{
		lock (_serverInfos)
		{
			ServerInfo serverInfo = TryGet(loginSettings.Uri);
			if (serverInfo != null)
			{
				if (serverInfo.IsOAuthServer)
				{
					loginSettings.Uri = new Uri(serverInfo.ServerAddress);
				}
				if (!string.IsNullOrEmpty(serverInfo.ServerType))
				{
					loginSettings.ServerType = serverInfo.ServerType;
				}
				if (serverInfo.IsHttpsValid)
				{
					loginSettings.ServerWebType = "SVC";
				}
				else if (serverInfo.IsHttpValid)
				{
					loginSettings.ServerWebType = "ASMX";
				}
			}
		}
	}

	public static bool Contains(Uri uri)
	{
		lock (_serverInfos)
		{
			return TryGet(uri) != null;
		}
	}

	public static string GetServerType(Uri uri)
	{
		lock (_serverInfos)
		{
			ServerInfo serverInfo = TryGet(uri);
			if (serverInfo != null)
			{
				return serverInfo.ServerType;
			}
		}
		return "";
	}

	private static string GetDnsSafeHost(string address, bool isOAuthServer, out int port)
	{
		port = 80;
		try
		{
			string defaultScheme = (isOAuthServer ? Uri.UriSchemeHttps : Uri.UriSchemeHttp);
			Uri uri = new UriBuilder(UriUtil.EnsureHttpPrefix(address, defaultScheme)).Uri;
			port = uri.Port;
			return uri.DnsSafeHost;
		}
		catch (Exception)
		{
			return "";
		}
	}
}
