using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Services;
using VideoOS.Management.Common;
using VideoOS.WebService.Common.DataCollectorService;

namespace VideoOS.Management.Server;

public sealed class SystemMonitorStateManager : IDisposable
{
	private class StateCacheHelper
	{
		public ConcurrentDictionary<Guid, Guid> Sources { get; set; }

		public CounterState State { get; set; }

		public string HostName { get; set; }

		public HashSet<int> SessionsUpdated { get; set; }

		public StateCacheHelper()
		{
			Sources = new ConcurrentDictionary<Guid, Guid>();
			State = CounterState.Normal;
			SessionsUpdated = new HashSet<int>();
		}
	}

	private int _nextSessionId;

	private readonly ConcurrentDictionary<int, StateSession> _sessions;

	private System.Threading.Timer _sessionCleanTimer;

	private const int SessionCleanMinutes = 1;

	private const int SessionTimeoutMinutes = 10;

	private const long PingInterval = 600000000L;

	private System.Timers.Timer _timer;

	private static ConcurrentDictionary<int, StateCacheHelper> _statesCacheHelper = new ConcurrentDictionary<int, StateCacheHelper>();

	private static DateTime _lastEndedPingSync = DateTime.MinValue;

	private static SemaphoreSlim _syncLock = new SemaphoreSlim(1);

	public SystemMonitorStateManager()
	{
		_nextSessionId = 1;
		_sessions = new ConcurrentDictionary<int, StateSession>();
	}

	private void CleanSessions(object state)
	{
		foreach (KeyValuePair<int, StateSession> item in _sessions.Where((KeyValuePair<int, StateSession> s) => !s.Value.IsAlive(TimeSpan.FromMinutes(10.0))))
		{
			RemoveFromStatesCache(item.Value.Subscribtions.SelectMany((KeyValuePair<int, List<Guid>> i) => i.Value).ToList());
			_sessions.TryRemove(item.Key, out var value);
			value.Dispose();
		}
		if (_sessions.Count == 0)
		{
			_statesCacheHelper = new ConcurrentDictionary<int, StateCacheHelper>();
			_timer.Enabled = false;
		}
	}

	public int StartStateSession(GeminiRequestContext requestContext)
	{
		int newSessionId = GetNewSessionId();
		_sessions[newSessionId] = new StateSession(requestContext.AuthenticatedUserSid);
		_timer.Enabled = true;
		return newSessionId;
	}

	public void StopStateSession(IGeminiRequestContext requestContext, int sessionId)
	{
		if (_sessions.TryGetValue(sessionId, out var value) && !(value.User != requestContext.AuthenticatedUserSid))
		{
			RemoveFromStatesCache(_sessions[sessionId].Subscribtions.SelectMany((KeyValuePair<int, List<Guid>> i) => i.Value).ToList());
			_sessions.TryRemove(sessionId, out value);
			value.Dispose();
		}
	}

	public CurrentState[] SubscribeServerStates(IGeminiRequestContext requestContext, int sessionId, int[] eventTypes, Guid[] itemGuids)
	{
		if (!_sessions.TryGetValue(sessionId, out var value))
		{
			return null;
		}
		if (value.User != requestContext.AuthenticatedUserSid)
		{
			return null;
		}
		value.SubscribeServerStates(requestContext, eventTypes, itemGuids);
		return GetCurrentServerStates(requestContext, eventTypes, itemGuids);
	}

	public CurrentState[] SubscribeCameraStates(IGeminiRequestContext requestContext, int sessionId, int[] eventTypes, Guid[] itemGuids)
	{
		if (!_sessions.TryGetValue(sessionId, out var value))
		{
			return null;
		}
		if (value.User != requestContext.AuthenticatedUserSid)
		{
			return null;
		}
		value.SubscribeCameraStates(requestContext, eventTypes, itemGuids);
		return GetCurrentCameraStates(requestContext, eventTypes, itemGuids);
	}

	public void UnsubscribeStates(IGeminiRequestContext requestContext, int sessionId, int[] eventTypes, Guid[] itemGuids)
	{
		if (_sessions.TryGetValue(sessionId, out var value) && !(value.User != requestContext.AuthenticatedUserSid))
		{
			RemoveFromStatesCache(itemGuids.ToList());
			value.UnsubscribeStates(eventTypes, itemGuids);
		}
	}

	public async Task<CurrentState[]> GetStates(IGeminiRequestContext requestContext, int sessionId, int timeout)
	{
		if (!_sessions.TryGetValue(sessionId, out var value) || !value.User.Equals(requestContext.AuthenticatedUserSid))
		{
			return null;
		}
		return (await value.GetStates(timeout)).ToArray();
	}

	public static CurrentState[] GetCurrentServerStates(IGeminiRequestContext requestContext, int[] eventTypes, Guid[] itemGuids)
	{
		object ids;
		if (!itemGuids.Contains(ServerGroupings.AllServers))
		{
			ids = itemGuids;
		}
		else
		{
			ids = PerformanceRuleServiceTypes.RuleServiceTypes;
		}
		List<SystemMonitorManager.EventSource> list = SystemMonitorManager.GetServerSourcesByIds(requestContext, (IEnumerable<Guid>)ids).ToList();
		SystemMonitorStateSourceComponent.StateData[] source;
		using (SystemMonitorStateSourceComponent systemMonitorStateSourceComponent = requestContext.CreateComponent<SystemMonitorStateSourceComponent>())
		{
			source = systemMonitorStateSourceComponent.QuerySystemMonitorServerStates(list.Select((SystemMonitorManager.EventSource s) => s.DataCollectorId).Distinct().ToArray(), eventTypes);
		}
		List<CurrentState> list2 = new List<CurrentState>();
		foreach (SystemMonitorManager.EventSource so in list)
		{
			foreach (SystemMonitorStateSourceComponent.StateData item in source.Where((SystemMonitorStateSourceComponent.StateData s) => s.DataCollectorId.Equals(so.DataCollectorId) && (s.InstanceId == Guid.Empty || s.InstanceId.Equals(new Guid(so.ServerType)) || s.InstanceId.Equals(new Guid(so.SourceId)))))
			{
				DateTime utcNow = DateTime.UtcNow;
				Guid guid = Guid.Parse(so.ServerType);
				if ((!PerformanceStateRuleConfigType.RecordingServerOnly.Contains(item.EventType) || guid.Equals(ServiceTypes.ServiceType_RecordingService) || guid.Equals(ServiceTypes.ServiceType_FailoverService)) && (!PerformanceStateRuleConfigType.GPUEvents.Contains(item.EventType) || guid.Equals(ServiceTypes.ServiceType_RecordingService) || guid.Equals(ServiceTypes.ServiceType_FailoverService) || guid.Equals(ServiceTypes.ServiceType_MobileService)))
				{
					list2.Add(new CurrentState(Guid.Parse(so.SourceId), guid, item.EventType, item.State, item.TriggerValue, utcNow, so.DataCollectorId));
					AddToCache(new Guid(so.SourceId), so.DataCollectorId, guid);
				}
			}
		}
		foreach (KeyValuePair<int, StateCacheHelper> item2 in _statesCacheHelper)
		{
			foreach (KeyValuePair<Guid, Guid> source2 in item2.Value.Sources)
			{
				DateTime utcNow2 = DateTime.UtcNow;
				list2.Add(new CurrentState(source2.Key, source2.Value, 9, item2.Value.State, (double)item2.Value.State, utcNow2, item2.Key));
			}
		}
		return list2.ToArray();
	}

	public static CurrentState[] GetCurrentCameraStates(IGeminiRequestContext requestContext, int[] eventTypes, Guid[] itemGuids)
	{
		Dictionary<string, int> cameraSourcesByIds = SystemMonitorManager.GetCameraSourcesByIds(requestContext, itemGuids);
		SystemMonitorStateSourceComponent.StateData[] array;
		using (SystemMonitorStateSourceComponent systemMonitorStateSourceComponent = requestContext.CreateComponent<SystemMonitorStateSourceComponent>())
		{
			array = systemMonitorStateSourceComponent.QuerySystemMonitorCameraStates(eventTypes, cameraSourcesByIds.Select((KeyValuePair<string, int> s) => Guid.Parse(s.Key)).ToArray());
		}
		List<CurrentState> list = new List<CurrentState>();
		DateTime utcNow = DateTime.UtcNow;
		SystemMonitorStateSourceComponent.StateData[] array2 = array;
		foreach (SystemMonitorStateSourceComponent.StateData stateData in array2)
		{
			list.Add(new CurrentState(stateData.InstanceId, CameraGroupings.SingleCamera, stateData.EventType, stateData.State, stateData.TriggerValue, utcNow, stateData.DataCollectorId));
			AddToCache(stateData.InstanceId, stateData.DataCollectorId, CameraGroupings.SingleCamera);
		}
		return list.ToArray();
	}

	public void AddStateToSessions(IGeminiRequestContext requestContext, CounterStateChangeData changeData, List<SystemMonitorManager.EventSource> sources, SystemMonitorManager.RuleEventType type)
	{
		IEnumerable<CurrentState> state = CurrentStateFromCounterStateChangeData(changeData, sources, type);
		AddStateToSessions(requestContext, state);
	}

	public void AddStateToSessions(IGeminiRequestContext requestContext, IEnumerable<CurrentState> state)
	{
		foreach (KeyValuePair<int, StateSession> session in _sessions)
		{
			foreach (CurrentState item in state)
			{
				if (item.ParentId == CameraGroupings.SingleCamera)
				{
					if (SystemMonitorManager.GetActiveRecorders(requestContext).Contains(item.DataCollectorId))
					{
						session.Value.AddCameraState(requestContext, item);
					}
				}
				else
				{
					session.Value.AddServerState(requestContext, item);
				}
				UpdateCache(item);
			}
		}
	}

	private int GetNewSessionId()
	{
		if (_nextSessionId == int.MaxValue)
		{
			_nextSessionId = 1;
		}
		return _nextSessionId++;
	}

	private static IEnumerable<CurrentState> CurrentStateFromCounterStateChangeData(CounterStateChangeData data, List<SystemMonitorManager.EventSource> sources, SystemMonitorManager.RuleEventType type)
	{
		List<CurrentState> list = new List<CurrentState>();
		DateTime updateTime = DateTime.UtcNow;
		int eventType = type.EventType;
		if ((uint)(eventType - 3) <= 2u || eventType == 10)
		{
			foreach (SystemMonitorManager.EventSource source in sources)
			{
				list.Add(new CurrentState(Guid.Parse(source.SourceId), CameraGroupings.SingleCamera, type.EventType, (CounterState)data.State, data.TriggerValue, updateTime, source.DataCollectorId));
			}
			return list;
		}
		if (sources != null)
		{
			IEnumerable<CurrentState> collection = sources.Select((SystemMonitorManager.EventSource eventSource) => new CurrentState(Guid.Parse(eventSource.SourceId), Guid.Parse(eventSource.ServerType), type.EventType, (CounterState)data.State, data.TriggerValue, updateTime, data.DataCollectorId));
			list.AddRange(collection);
		}
		return list;
	}

	private static void AddToCache(Guid sourceId, int dataCollectorId, Guid type)
	{
		if (!_statesCacheHelper.TryGetValue(dataCollectorId, out var value))
		{
			value = new StateCacheHelper();
		}
		if (!value.Sources.ContainsKey(sourceId))
		{
			value.Sources.TryAdd(sourceId, type);
		}
		_statesCacheHelper[dataCollectorId] = value;
	}

	private static void RemoveFromStatesCache(List<Guid> obsoleteSources)
	{
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, StateCacheHelper> item in _statesCacheHelper)
		{
			foreach (Guid obsoleteSource in obsoleteSources)
			{
				item.Value.Sources.TryRemove(obsoleteSource, out var _);
			}
			if (item.Value.Sources.Count == 0)
			{
				list.Add(item.Key);
			}
		}
		foreach (int item2 in list)
		{
			_statesCacheHelper.TryRemove(item2, out var _);
		}
	}

	private static void UpdateCache(CurrentState state)
	{
		foreach (KeyValuePair<int, StateCacheHelper> item in _statesCacheHelper)
		{
			if (item.Value.Sources.ContainsKey(state.Id))
			{
				if (state.StateType == 9)
				{
					item.Value.State = state.State;
				}
				break;
			}
		}
	}

	public void Start()
	{
		_sessionCleanTimer = new System.Threading.Timer(CleanSessions, null, TimeSpan.FromMinutes(1.0), TimeSpan.FromMinutes(1.0));
		_timer = new System.Timers.Timer(new TimeSpan(600000000L).TotalMilliseconds);
		_timer.Elapsed += delegate
		{
			PingServersAsync();
		};
	}

	public void Stop()
	{
		_sessionCleanTimer.Dispose();
		_timer.Dispose();
		foreach (KeyValuePair<int, StateSession> session in _sessions)
		{
			session.Value.Dispose();
		}
		_statesCacheHelper.Clear();
		_sessions.Clear();
	}

	private async void PingServersAsync()
	{
		_ = 1;
		try
		{
			await _syncLock.WaitAsync(default(CancellationToken));
			if (DateTime.Now - _lastEndedPingSync < TimeSpan.FromSeconds(1.0))
			{
				return;
			}
			Dictionary<string, Task<bool>> tasks = new Dictionary<string, Task<bool>>();
			using (GeminiRequestContext requestContext = GeminiApplicationCore.RuntimeServiceHost.CreateSystemContext())
			{
				foreach (KeyValuePair<int, StateCacheHelper> stateCacheHelper in _statesCacheHelper)
				{
					if (stateCacheHelper.Value.HostName == null)
					{
						DataCollectorHost dataCollectorHost = DataCollectorHost.GetDataCollectorHosts(requestContext, stateCacheHelper.Key).FirstOrDefault((DataCollectorHost h) => h.DataCollectorId == stateCacheHelper.Key);
						if (dataCollectorHost == null)
						{
							continue;
						}
						stateCacheHelper.Value.HostName = dataCollectorHost.HostName;
					}
					if (!tasks.ContainsKey(stateCacheHelper.Value.HostName))
					{
						tasks.Add(stateCacheHelper.Value.HostName, LiveDataCollector.IsDataCollectorAlive(requestContext, stateCacheHelper.Value.HostName));
					}
				}
			}
			if (tasks.Count == 0)
			{
				return;
			}
			Task<bool> ti;
			do
			{
				ti = await System.Threading.Tasks.Task.WhenAny(tasks.Select((KeyValuePair<string, Task<bool>> i) => i.Value));
				string hostName = tasks.FirstOrDefault((KeyValuePair<string, Task<bool>> i) => i.Value.Id == ti.Id).Key;
				tasks.Remove(hostName);
				KeyValuePair<int, StateCacheHelper> keyValuePair = _statesCacheHelper.SingleOrDefault((KeyValuePair<int, StateCacheHelper> h) => h.Value.HostName != null && h.Value.HostName.Equals(hostName, StringComparison.OrdinalIgnoreCase));
				if (keyValuePair.Value != null && (!AllSessionsUpdated(keyValuePair.Value.SessionsUpdated) || (ti.Result && keyValuePair.Value.State != CounterState.Normal) || (!ti.Result && keyValuePair.Value.State != CounterState.Warning)))
				{
					keyValuePair.Value.SessionsUpdated = new HashSet<int>(_sessions.Keys);
					using GeminiRequestContext systemRequestContext = GeminiApplicationCore.RuntimeServiceHost.CreateSystemContext();
					AddServerOnlineSessionStates(systemRequestContext, ti.Result, keyValuePair.Value.Sources, keyValuePair.Key);
				}
			}
			while (tasks.Count > 0);
		}
		finally
		{
			_lastEndedPingSync = DateTime.Now;
			_syncLock.Release();
		}
	}

	private bool AllSessionsUpdated(HashSet<int> sessionsUpdated)
	{
		foreach (int key in _sessions.Keys)
		{
			if (!sessionsUpdated.Contains(key))
			{
				return false;
			}
		}
		return true;
	}

	private void AddServerOnlineSessionStates(GeminiRequestContext systemRequestContext, bool result, ConcurrentDictionary<Guid, Guid> sources, int dataCollectorId)
	{
		List<CurrentState> list = new List<CurrentState>();
		foreach (KeyValuePair<Guid, Guid> source in sources)
		{
			DateTime utcNow = DateTime.UtcNow;
			CounterState counterState = (result ? CounterState.Normal : CounterState.Warning);
			CurrentState item = new CurrentState(source.Key, source.Value, 9, counterState, (double)counterState, utcNow, dataCollectorId);
			list.Add(item);
		}
		AddStateToSessions(systemRequestContext, list);
	}

	public void Dispose()
	{
		_sessionCleanTimer.Dispose();
		_timer.Dispose();
		foreach (KeyValuePair<int, StateSession> session in _sessions)
		{
			session.Value.Dispose();
		}
		_statesCacheHelper.Clear();
		_sessions.Clear();
	}
}
