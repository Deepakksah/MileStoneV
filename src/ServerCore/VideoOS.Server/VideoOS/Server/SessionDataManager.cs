using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace VideoOS.Server;

public sealed class SessionDataManager : ISessionDataManager, IDisposable
{
	private Dictionary<long, SessionData> _sessionDataDictionary;

	private volatile Timer _pollTimer;

	private bool disposedValue;

	public static ISessionDataManager Instance { get; set; }

	static SessionDataManager()
	{
		Instance = new SessionDataManager();
	}

	public SessionDataManager()
	{
		_sessionDataDictionary = new Dictionary<long, SessionData>();
	}

	public void Store(SessionData sessionData)
	{
		if (sessionData == null)
		{
			throw new ArgumentNullException("sessionData");
		}
		sessionData.LastAccess = DateTime.UtcNow;
		lock (_sessionDataDictionary)
		{
			_sessionDataDictionary[sessionData.SessionDataId] = sessionData;
			if (_pollTimer == null)
			{
				_pollTimer = new Timer(IdleCheck, null, 5000, 5000);
			}
		}
	}

	public SessionData Query(long sessionDataId)
	{
		SessionData value = null;
		lock (_sessionDataDictionary)
		{
			if (_sessionDataDictionary.TryGetValue(sessionDataId, out value))
			{
				return value;
			}
		}
		return null;
	}

	public void Remove(SessionData sessionData)
	{
		if (sessionData == null)
		{
			throw new ArgumentNullException("sessionData");
		}
		lock (_sessionDataDictionary)
		{
			if (_sessionDataDictionary.ContainsKey(sessionData.SessionDataId))
			{
				_sessionDataDictionary.Remove(sessionData.SessionDataId);
			}
			if (!_sessionDataDictionary.Any())
			{
				_pollTimer.Change(-1, -1);
				_pollTimer.Dispose();
				_pollTimer = null;
			}
		}
	}

	private void IdleCheck(object state)
	{
		lock (_sessionDataDictionary)
		{
			DateTime utcNow = DateTime.UtcNow;
			List<long> list = new List<long>();
			foreach (long key in _sessionDataDictionary.Keys)
			{
				SessionData sessionData = _sessionDataDictionary[key];
				if (sessionData.LastAccess + sessionData.MaxIdleToDispose < utcNow)
				{
					list.Add(key);
				}
			}
			foreach (long item in list)
			{
				SessionData sessionData2 = _sessionDataDictionary[item];
				sessionData2.Dispose();
			}
		}
	}

	private void Dispose(bool disposing)
	{
		if (disposedValue)
		{
			return;
		}
		if (disposing)
		{
			_sessionDataDictionary.Clear();
			if (_pollTimer != null)
			{
				_pollTimer.Change(-1, -1);
				_pollTimer.Dispose();
				_pollTimer = null;
			}
			Instance = null;
		}
		disposedValue = true;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
