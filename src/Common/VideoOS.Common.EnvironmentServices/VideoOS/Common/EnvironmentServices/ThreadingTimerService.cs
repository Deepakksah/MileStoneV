using System;
using System.Threading;

namespace VideoOS.Common.EnvironmentServices;

public class ThreadingTimerService : ITimer, IDisposable
{
	private Timer _timer;

	public event EventHandler Elapsed;

	public ThreadingTimerService()
	{
		_timer = new Timer(TimerCallback);
	}

	public void StartTimer()
	{
		throw new NotImplementedException();
	}

	public void StartTimer(TimeSpan delay, TimeSpan period)
	{
		_timer.Change(delay, period);
	}

	public void Change(TimeSpan delay, TimeSpan period)
	{
		_timer.Change(delay, period);
	}

	public void StopTimer()
	{
		_timer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
	}

	public void TimerCallback(object state)
	{
		if (this.Elapsed != null)
		{
			this.Elapsed(this, new EventArgs());
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			_timer.Dispose();
		}
	}

	public bool Dispose(WaitHandle notifyObject)
	{
		return _timer.Dispose(notifyObject);
	}
}
