using System;

namespace VideoOS.Server;

public abstract class SessionData : IDisposable
{
	private static long _sessionDataIdNext = 1L;

	private static object _lockObject = new object();

	private bool disposedValue;

	public long SessionDataId { get; private set; }

	public DateTime Created { get; private set; }

	public DateTime LastAccess { get; set; }

	public TimeSpan MaxIdleToDispose { get; private set; }

	public SessionData(TimeSpan maxIdleToDispose)
	{
		MaxIdleToDispose = maxIdleToDispose;
		Created = DateTime.UtcNow;
		LastAccess = Created;
		lock (_lockObject)
		{
			SessionDataId = _sessionDataIdNext++;
		}
		SessionDataManager.Instance.Store(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				SessionDataManager.Instance.Remove(this);
			}
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
