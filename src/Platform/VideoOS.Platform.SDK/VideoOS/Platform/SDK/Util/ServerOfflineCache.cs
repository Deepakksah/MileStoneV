using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Timers;
using VideoOS.Common.Proxy.Server.WCF.Net;
using VideoOS.Platform.SDK.Server;

namespace VideoOS.Platform.SDK.Util;

public static class ServerOfflineCache
{
	public class Server
	{
		public Uri Address { get; set; }

		public bool CheckInProgress { get; set; }

		public ServerCommandServiceClient ServerCommandServiceClient { get; set; }

		public Server(Uri address)
		{
			Address = address;
			CheckInProgress = false;
			TimeSpan timeSpan = TimeSpan.FromSeconds(ConnectionCheck.Timeout);
			if (address.Scheme == Uri.UriSchemeHttps)
			{
				ServerCommandServiceClient = new ServerCommandServiceClient(new BasicHttpsBinding
				{
					OpenTimeout = timeSpan,
					CloseTimeout = timeSpan,
					SendTimeout = timeSpan
				}, new EndpointAddress(address));
			}
			else
			{
				ServerCommandServiceClient = new ServerCommandServiceClient(new BasicHttpBinding
				{
					OpenTimeout = timeSpan,
					CloseTimeout = timeSpan,
					SendTimeout = timeSpan
				}, new EndpointAddress(address));
			}
		}
	}

	public delegate void ServerOfflineCacheEventHandler(object sender, EventArgs e);

	private static ConcurrentDictionary<Uri, Server> _offlineServers = new ConcurrentDictionary<Uri, Server>();

	private static Timer _refreshTimer = null;

	private static object _refreshTimerLock = new object();

	private static readonly TimeSpan _refreshInterval = TimeSpan.FromSeconds(20.0);

	public static IReadOnlyDictionary<Uri, Server> OfflineServers => _offlineServers;

	public static bool IsTimerRunning
	{
		get
		{
			if (_refreshTimer != null)
			{
				return _refreshTimer.Enabled;
			}
			return false;
		}
	}

	public static event ServerOfflineCacheEventHandler ServerOfflineCacheChanged;

	public static void AddServer(Uri serverAddress)
	{
		if (_offlineServers.TryAdd(serverAddress, new Server(serverAddress)))
		{
			ServerOfflineCache.ServerOfflineCacheChanged?.Invoke(null, EventArgs.Empty);
			StartTimer();
		}
	}

	public static void RemoveServer(Uri serverAddress)
	{
		bool flag = false;
		foreach (KeyValuePair<Uri, Server> item in _offlineServers.Where((KeyValuePair<Uri, Server> x) => x.Value.Address.DnsSafeHost == serverAddress.DnsSafeHost).ToList())
		{
			if (_offlineServers.TryRemove(item.Key, out var _) && !flag)
			{
				flag = true;
			}
		}
		if (flag)
		{
			ServerOfflineCache.ServerOfflineCacheChanged?.Invoke(null, EventArgs.Empty);
		}
		if (_offlineServers.IsEmpty)
		{
			StopTimer();
		}
	}

	public static bool IsServerOnline(Uri serverAddress)
	{
		bool flag = !_offlineServers.ContainsKey(serverAddress);
		ServerTrace.Info("ServerOfflineCache", "IsServerOnline", "Server:" + serverAddress?.ToString() + ", is considered " + (flag ? "online" : "offline"));
		return flag;
	}

	public static bool IsServerOffline(Uri serverAddress)
	{
		return !IsServerOnline(serverAddress);
	}

	public static void Clear()
	{
		_offlineServers.Clear();
		ServerOfflineCache.ServerOfflineCacheChanged?.Invoke(null, EventArgs.Empty);
		StopTimer();
	}

	private static void StartTimer()
	{
		lock (_refreshTimerLock)
		{
			if (_refreshTimer == null)
			{
				_refreshTimer = new Timer(_refreshInterval.TotalMilliseconds);
				_refreshTimer.Elapsed += CheckOfflineServersStatus;
			}
			_refreshTimer.Start();
		}
	}

	private static void StopTimer()
	{
		lock (_refreshTimerLock)
		{
			if (_refreshTimer != null && _offlineServers.IsEmpty)
			{
				_refreshTimer?.Stop();
				_refreshTimer?.Dispose();
				_refreshTimer = null;
			}
		}
	}

	private static async void CheckOfflineServersStatus(object sender, ElapsedEventArgs e)
	{
		List<KeyValuePair<Uri, Server>> list = _offlineServers.Where((KeyValuePair<Uri, Server> server) => !server.Value.CheckInProgress).ToList();
		foreach (KeyValuePair<Uri, Server> item in list)
		{
			if (_offlineServers.TryGetValue(item.Key, out var value))
			{
				value.CheckInProgress = true;
			}
		}
		await Task.WhenAll(list.Select((KeyValuePair<Uri, Server> server) => CheckServerStatusAsync(server.Value, new ConnectionCheck())));
		StartTimer();
	}

	public static async Task CheckServerStatusAsync(Server server, ConnectionCheck connectionCheck)
	{
		bool online = false;
		try
		{
			online = await connectionCheck.IsOAuthServerOnline(server.Address.ToString());
			if (!online)
			{
				try
				{
					server.ServerCommandServiceClient.GetServerVersion();
				}
				catch (Exception)
				{
				}
			}
		}
		catch (Exception ex2)
		{
			string uriOrId = $"Exception occurred for server: {server.Address}. Error: {ex2.Message}";
			ServerTrace.Info("ServerOfflineCache", "CheckServerStatusAsync", uriOrId);
		}
		Server value;
		if (online)
		{
			RemoveServer(server.Address);
		}
		else if (_offlineServers.TryGetValue(server.Address, out value))
		{
			value.CheckInProgress = false;
		}
	}
}
