using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using VideoOS.EventServer.Communication.Configuration;
using VideoOS.Platform;

namespace VideoOS.EventServer.Communication;

public static class ClientSessionManager
{
	private static readonly TimeSpan HeartbeatTimerRepeatDelay = TimeSpan.FromMinutes(1.0);

	private static readonly TimeSpan HeartbeatTimerInfiniteDelay = TimeSpan.FromMilliseconds(-1.0);

	private static readonly TimeSpan HeartbeatTimerNoDelay = TimeSpan.FromMilliseconds(0.0);

	private static readonly TimeSpan HeartbeatTimerRetryDelay = TimeSpan.FromSeconds(2.0);

	private static Timer _heartBeatTimer = new Timer(HeartBeatTimerCallback, null, HeartbeatTimerInfiniteDelay, HeartbeatTimerInfiniteDelay);

	private static Dictionary<ServerId, IList<HeartbeatData>> _heartbeatDataMap = new Dictionary<ServerId, IList<HeartbeatData>>();

	private static object _lock = new object();

	private static readonly ConfigurationClientManager _configurationClientManager = new ConfigurationClientManager();

	public static Guid ClientSessionId { get; private set; }

	public static void SetClientSession(Guid clientSessionId)
	{
		ClientSessionId = clientSessionId;
	}

	public static void ClearClientSession()
	{
		ClientSessionId = Guid.Empty;
		_heartBeatTimer.Change(HeartbeatTimerInfiniteDelay, HeartbeatTimerInfiniteDelay);
	}

	public static void AddHeartbeatData(ServerId serverId, HeartbeatData data)
	{
		if (serverId == null)
		{
			throw new ArgumentNullException("serverId");
		}
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		lock (_lock)
		{
			if (!_heartbeatDataMap.TryGetValue(serverId, out var value))
			{
				value = new List<HeartbeatData>();
				_heartbeatDataMap.Add(serverId, value);
			}
			value.Add(data);
		}
		_heartBeatTimer.Change(HeartbeatTimerNoDelay, HeartbeatTimerRepeatDelay);
	}

	public static bool RemoveHeartbeatData(ServerId serverId, HeartbeatData heartbeatData)
	{
		if (serverId == null)
		{
			throw new ArgumentNullException("serverId");
		}
		if (heartbeatData == null)
		{
			throw new ArgumentNullException("heartbeatData");
		}
		bool result = false;
		lock (_lock)
		{
			if (_heartbeatDataMap.TryGetValue(serverId, out var value))
			{
				result = value.Remove(heartbeatData);
			}
		}
		_heartBeatTimer.Change(HeartbeatTimerNoDelay, HeartbeatTimerRepeatDelay);
		return result;
	}

	public static bool RemoveHeartbeatData(ServerId serverId)
	{
		if (serverId == null)
		{
			throw new ArgumentNullException("serverId");
		}
		bool result = false;
		lock (_lock)
		{
			result = _heartbeatDataMap.Remove(serverId);
		}
		_heartBeatTimer.Change(HeartbeatTimerNoDelay, HeartbeatTimerRepeatDelay);
		return result;
	}

	public static void RemoveAllHeartbeatData()
	{
		lock (_lock)
		{
			_heartbeatDataMap.Clear();
		}
		_heartBeatTimer.Change(HeartbeatTimerNoDelay, HeartbeatTimerRepeatDelay);
	}

	public static MessageHeader CreateClientSessionIdHeader()
	{
		return CreateClientSessionIdHeader(ClientSessionId);
	}

	public static MessageHeader CreateClientSessionIdHeader(Guid clientSessionId)
	{
		if (clientSessionId != Guid.Empty)
		{
			return new MessageHeader<string>(clientSessionId.ToString()).GetUntypedHeader("client-session-id", "csid");
		}
		return null;
	}

	private static void HeartBeatTimerCallback(object state)
	{
		if (!EnvironmentManager.Instance.ApplicationLoggedOn)
		{
			_heartBeatTimer.Change(HeartbeatTimerRetryDelay, HeartbeatTimerRepeatDelay);
			return;
		}
		foreach (Item item in new ServerReceiver().ServersWithEventServer(new Version(3, 3, 0, 0)))
		{
			if (!(item.FQID != null))
			{
				continue;
			}
			ServerId serverId = item.FQID.ServerId;
			if (!(serverId != null))
			{
				continue;
			}
			HeartbeatData[] heartbeatData = Array.Empty<HeartbeatData>();
			lock (_lock)
			{
				if (_heartbeatDataMap.TryGetValue(serverId, out var value))
				{
					heartbeatData = value.ToArray();
				}
			}
			try
			{
				ConfigurationClient configurationClient = _configurationClientManager.GetConfigurationClient(serverId);
				HeartbeatParams heartbeatParams = new HeartbeatParams
				{
					HeartbeatData = heartbeatData
				};
				configurationClient.BeginClientSessionHeartbeat(0, heartbeatParams);
			}
			catch (Exception)
			{
			}
		}
	}
}
