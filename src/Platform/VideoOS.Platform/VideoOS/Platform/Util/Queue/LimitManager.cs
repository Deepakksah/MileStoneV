using System;
using System.Threading;

namespace VideoOS.Platform.Util.Queue;

public class LimitManager : IDisposable
{
	private readonly object _concurrentRequestsLock = new object();

	private bool _disposed;

	public virtual ManualResetEventSlim AllowRequestNotification { get; } = new ManualResetEventSlim(initialState: true);

	public virtual QueueConfiguration Configuration { get; }

	public virtual int ConcurrentRequests { get; private set; }

	public LimitManager(QueueConfiguration configuration)
	{
		Configuration = configuration;
	}

	public virtual bool AllowRequestToBeProcessed()
	{
		lock (_concurrentRequestsLock)
		{
			if (ConcurrentRequests >= Configuration.MaxConcurrentRequests)
			{
				if (AllowRequestNotification.IsSet)
				{
					AllowRequestNotification.Reset();
				}
				return false;
			}
			int concurrentRequests = ConcurrentRequests;
			ConcurrentRequests = concurrentRequests + 1;
			return true;
		}
	}

	public virtual void RequestProcessingDone()
	{
		lock (_concurrentRequestsLock)
		{
			int concurrentRequests = ConcurrentRequests;
			ConcurrentRequests = concurrentRequests - 1;
			AllowRequestNotification.Set();
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (_disposed)
		{
			return;
		}
		_disposed = true;
		lock (_concurrentRequestsLock)
		{
			AllowRequestNotification.Dispose();
		}
	}
}
