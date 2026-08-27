using System;
using System.Threading;
using VideoOS.Utilities.Timers;

namespace VideoOS.Common.Integration;

public class ServiceDiscoveryEngine : IServiceDiscoveryEngine, IDisposable
{
	private TimeSpan _timeout;

	private ITimer _timer;

	private object _sync = new object();

	private IServiceConnectionStatusProvider ServiceProvider { get; set; }

	public ServiceDiscoveryEngine(TimeSpan timeout, IServiceConnectionStatusProvider serviceProvider)
		: this(timeout, serviceProvider, new ThreadingTimerWrapper())
	{
	}

	public ServiceDiscoveryEngine(TimeSpan timeout, IServiceConnectionStatusProvider serviceProvider, ITimer timer)
	{
		if (serviceProvider == null)
		{
			throw new ArgumentNullException("serviceProvider");
		}
		if (timeout.TotalMilliseconds <= 0.0)
		{
			throw new ArgumentOutOfRangeException("timeout", "Timespan needs to be greater than 0");
		}
		if (timeout.TotalMilliseconds > 4294967294.0)
		{
			throw new ArgumentOutOfRangeException("timeout", "Time-out interval must be less than 2^32-2 milliseconds.");
		}
		if (timer == null)
		{
			throw new ArgumentNullException("timer");
		}
		ServiceProvider = serviceProvider;
		_timeout = timeout;
		_timer = timer;
		_timer.Elapsed += TimerCallback;
		_timer.StartTimer(_timeout, Timeout.InfiniteTimeSpan);
	}

	public void CommunicationConfirmed(Guid serviceId)
	{
		ServiceProvider.GetService(serviceId)?.CommunicationConfirmed();
	}

	public void CommunicationError(Guid serviceId)
	{
		ServiceProvider.GetService(serviceId)?.CommunicationError();
	}

	private void OnTimer()
	{
		if (ServiceProvider.GetServices() != null)
		{
			IServiceConnectionStatus[] services = ServiceProvider.GetServices();
			for (int i = 0; i < services.Length; i++)
			{
				services[i].CheckTimeout();
			}
		}
	}

	private void TimerCallback(object sender, EventArgs e)
	{
		lock (_sync)
		{
			if (_timer != null)
			{
				_timer.StopTimer();
			}
		}
		try
		{
			OnTimer();
		}
		finally
		{
			lock (_sync)
			{
				if (_timer != null)
				{
					_timer.StartTimer(_timeout, Timeout.InfiniteTimeSpan);
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
		lock (_sync)
		{
			if (_timer != null)
			{
				_timer.Dispose();
				_timer = null;
			}
		}
	}
}
