using System;
using System.Threading;

namespace VideoOS.Management.Server;

public abstract class PeriodicTimerServiceBase : IGeminiServerService, IDisposable
{
	private object sync = new object();

	private Timer timer;

	protected abstract TimeSpan InitialTimeInterval { get; }

	protected abstract TimeSpan PeriodicTimeInterval { get; }

	protected abstract void OnTimer(object state);

	void IGeminiServerService.StartService(IGeminiRequestContext requestContext)
	{
		StartService(requestContext);
	}

	void IGeminiServerService.StopService(IGeminiRequestContext requestContext)
	{
		StopService(requestContext);
	}

	protected virtual void StartService(IGeminiRequestContext requestContext)
	{
		lock (sync)
		{
			if (timer == null)
			{
				timer = new Timer(TimerCallback, null, InitialTimeInterval, Timeout.InfiniteTimeSpan);
			}
		}
	}

	protected virtual void StopService(IGeminiRequestContext requestContext)
	{
		lock (sync)
		{
			if (timer != null)
			{
				timer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
				timer = null;
			}
		}
	}

	private void TimerCallback(object state)
	{
		try
		{
			OnTimer(state);
		}
		finally
		{
			lock (sync)
			{
				if (timer != null)
				{
					timer.Change(PeriodicTimeInterval, Timeout.InfiniteTimeSpan);
				}
			}
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposing)
		{
			return;
		}
		lock (sync)
		{
			if (timer != null)
			{
				timer.Dispose();
				timer = null;
			}
		}
	}
}
