using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Services;
using VideoOS.Management.Common;
using VideoOS.Utilities.Collections;

namespace VideoOS.Management.Server;

public class StateSession : IDisposable
{
	private class UserRightCache
	{
		private const int MaxCacheSize = 64;

		private const int TimeToLiveSeconds = 3600;

		private readonly ConcurrentQueue<KeyValuePair<DateTime, Guid>> _cache;

		public UserRightCache()
		{
			_cache = new ConcurrentQueue<KeyValuePair<DateTime, Guid>>();
		}

		public bool IsAllowed(Guid id)
		{
			CleanUp();
			return _cache.Any((KeyValuePair<DateTime, Guid> p) => p.Value == id);
		}

		public void AddAllowed(Guid id)
		{
			_cache.Enqueue(new KeyValuePair<DateTime, Guid>(DateTime.UtcNow, id));
		}

		private void CleanUp()
		{
			if (_cache.Count != 0)
			{
				while (_cache.Count > 64 || IsObsolate(DateTime.UtcNow))
				{
					_cache.TryDequeue(out var _);
				}
			}
		}

		private bool IsObsolate(DateTime dateTime)
		{
			if (_cache.TryPeek(out var result))
			{
				return (dateTime - result.Key).TotalSeconds > 3600.0;
			}
			return false;
		}
	}

	private readonly LongPollingMessageQueue<CurrentState> _states;

	private readonly object _lock;

	private DateTime _accessTime;

	private readonly Dictionary<int, List<Guid>> _subscriptions;

	private readonly UserRightCache _userRightCache;

	private ReaderWriterLockSlim _userRightsLock = new ReaderWriterLockSlim();

	private CancellationTokenSource _tokenSource = new CancellationTokenSource();

	private bool _disposed;

	public string User { get; private set; }

	public Dictionary<int, List<Guid>> Subscribtions => _subscriptions;

	public StateSession(string user)
	{
		User = user;
		_states = new LongPollingMessageQueue<CurrentState>();
		_lock = new object();
		_accessTime = DateTime.UtcNow;
		_subscriptions = new Dictionary<int, List<Guid>>();
		_userRightCache = new UserRightCache();
	}

	public void AddServerState(IGeminiRequestContext requestContext, CurrentState state)
	{
		lock (_lock)
		{
			if (!_subscriptions.ContainsKey(state.StateType) || (!_subscriptions[state.StateType].Contains(ServerGroupings.AllServers) && !_subscriptions[state.StateType].Contains(state.ParentId) && !_subscriptions[state.StateType].Contains(state.Id)))
			{
				return;
			}
		}
		if (CheckUserRights(requestContext, state))
		{
			_states.Enqueue(state);
		}
	}

	public void AddCameraState(IGeminiRequestContext requestContext, CurrentState state)
	{
		lock (_lock)
		{
			if (!_subscriptions.ContainsKey(state.StateType) || (!_subscriptions[state.StateType].Contains(WellKnownDevicesGroupIds.AllCameras) && !_subscriptions[state.StateType].Contains(state.Id)))
			{
				return;
			}
		}
		if (CheckUserRights(requestContext, state))
		{
			_states.Enqueue(state);
		}
	}

	public void SubscribeServerStates(IGeminiRequestContext requestContext, int[] eventTypes, Guid[] itemGuids)
	{
		IEnumerable<int> enumerable = FilterKnownServerEventTypes(eventTypes);
		VerifyServers(requestContext, ref itemGuids);
		lock (_lock)
		{
			foreach (int item2 in enumerable)
			{
				if (!_subscriptions.ContainsKey(item2))
				{
					_subscriptions.Add(item2, new List<Guid>());
				}
				Guid[] array = itemGuids;
				foreach (Guid item in array)
				{
					if (!_subscriptions[item2].Contains(item))
					{
						_subscriptions[item2].Add(item);
					}
				}
			}
		}
	}

	public void SubscribeCameraStates(IGeminiRequestContext requestContext, int[] eventTypes, Guid[] itemGuids)
	{
		IEnumerable<int> enumerable = FilterKnownCameraEventTypes(eventTypes);
		VerifyCameras(requestContext, ref itemGuids);
		lock (_lock)
		{
			foreach (int item2 in enumerable)
			{
				if (!_subscriptions.ContainsKey(item2))
				{
					_subscriptions.Add(item2, new List<Guid>());
				}
				Guid[] array = itemGuids;
				foreach (Guid item in array)
				{
					if (!_subscriptions[item2].Contains(item))
					{
						_subscriptions[item2].Add(item);
					}
				}
			}
		}
	}

	public void UnsubscribeStates(int[] eventTypes, Guid[] itemGuids)
	{
		lock (_lock)
		{
			foreach (int key in eventTypes)
			{
				if (_subscriptions.ContainsKey(key))
				{
					_subscriptions[key].RemoveAll(((IEnumerable<Guid>)itemGuids).Contains<Guid>);
					if (_subscriptions[key].Count == 0)
					{
						_subscriptions.Remove(key);
					}
				}
			}
		}
	}

	public async Task<CurrentState[]> GetStates(int timeout)
	{
		CurrentState[] result = await _states.DequeueAsync(TimeSpan.FromMilliseconds(timeout), TimeSpan.Zero, _tokenSource.Token);
		lock (_lock)
		{
			_accessTime = DateTime.UtcNow;
			return result;
		}
	}

	private bool CheckUserRights(IGeminiRequestContext requestContext, CurrentState state)
	{
		_userRightsLock.EnterReadLock();
		try
		{
			if (_userRightCache.IsAllowed(state.Id))
			{
				return true;
			}
		}
		finally
		{
			_userRightsLock.ExitReadLock();
		}
		if (state.ParentId == CameraGroupings.SingleCamera)
		{
			return CheckCameraUserRights(requestContext, state.Id);
		}
		if (state.ParentId == ServiceTypes.ServiceType_RecordingService)
		{
			return CheckServerUserRights(requestContext, state.Id);
		}
		return true;
	}

	private bool CheckCameraUserRights(IGeminiRequestContext requestContext, Guid cameraId)
	{
		if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, User, "GENERIC_READ", cameraId.ToString()))
		{
			return false;
		}
		_userRightsLock.EnterWriteLock();
		try
		{
			_userRightCache.AddAllowed(cameraId);
		}
		finally
		{
			_userRightsLock.ExitWriteLock();
		}
		return true;
	}

	private bool CheckServerUserRights(IGeminiRequestContext requestContext, Guid serverId)
	{
		if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, User, "GENERIC_READ", serverId.ToString()))
		{
			return false;
		}
		_userRightsLock.EnterWriteLock();
		try
		{
			_userRightCache.AddAllowed(serverId);
		}
		finally
		{
			_userRightsLock.ExitWriteLock();
		}
		return true;
	}

	public bool IsAlive(TimeSpan sessionTimeout)
	{
		lock (_lock)
		{
			return _accessTime + sessionTimeout > DateTime.UtcNow;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (!_disposed && disposing)
		{
			if (_tokenSource != null)
			{
				_tokenSource.Cancel();
				_tokenSource.Dispose();
			}
			if (_userRightsLock != null)
			{
				_userRightsLock.Dispose();
			}
			_disposed = true;
		}
	}

	private static IEnumerable<int> FilterKnownServerEventTypes(IEnumerable<int> types)
	{
		return types.Where((int t) => PerformanceStateRuleConfigType.ServerTypes.Contains(t) || PerformanceStateRuleConfigType.HiddenServerTypes.Contains(t));
	}

	private void VerifyServers(IGeminiRequestContext requestContext, ref Guid[] servers)
	{
		if (servers == null)
		{
			throw new ArgumentNullException("servers");
		}
		IEnumerable<Guid> enumerable = servers.Where((Guid s) => s != ServerGroupings.AllServers && !PerformanceRuleServiceTypes.RuleServiceTypes.Contains(s));
		IEnumerable<SystemMonitorManager.EventSource> serverSourcesByIds = SystemMonitorManager.GetServerSourcesByIds(requestContext, enumerable);
		List<Guid> list = new List<Guid>();
		foreach (Guid server in enumerable)
		{
			SystemMonitorManager.EventSource eventSource = serverSourcesByIds.FirstOrDefault((SystemMonitorManager.EventSource s) => Guid.Parse(s.SourceId) == server);
			if (eventSource != null && !(Guid.Parse(eventSource.ServerType) != ServiceTypes.ServiceType_RecordingService) && !CheckServerUserRights(requestContext, server))
			{
				list.Add(server);
			}
		}
		List<Guid> list2 = servers.ToList();
		foreach (Guid item in list)
		{
			list2.Remove(item);
		}
		servers = list2.ToArray();
	}

	private void VerifyCameras(IGeminiRequestContext requestContext, ref Guid[] cameras)
	{
		if (cameras == null)
		{
			throw new ArgumentNullException("cameras");
		}
		IEnumerable<Guid> enumerable = cameras.Where((Guid c) => c != WellKnownDevicesGroupIds.AllCameras);
		List<Guid> list = new List<Guid>();
		foreach (Guid item in enumerable)
		{
			if (!CheckCameraUserRights(requestContext, item))
			{
				list.Add(item);
			}
		}
		List<Guid> list2 = cameras.ToList();
		foreach (Guid item2 in list)
		{
			list2.Remove(item2);
		}
		cameras = list2.ToArray();
	}

	private static IEnumerable<int> FilterKnownCameraEventTypes(IEnumerable<int> types)
	{
		return types.Where((int t) => PerformanceStateRuleConfigType.CameraTypes.Contains(t) || PerformanceStateRuleConfigType.HiddenCameraTypes.Contains(t));
	}
}
