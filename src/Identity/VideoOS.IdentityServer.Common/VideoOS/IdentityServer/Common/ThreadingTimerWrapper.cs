using System;
using System.Threading;

namespace VideoOS.IdentityServer.Common;

public class ThreadingTimerWrapper : ITimer, IDisposable
{
	private Timer _timer;

	public event EventHandler Elapsed;

	public ThreadingTimerWrapper()
	{
		_timer = new Timer(TimerCallback);
	}

	public void StartTimer(TimeSpan delay, TimeSpan period)
	{
		_timer?.Change(delay, period);
	}

	public void StopTimer()
	{
		_timer?.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
	}

	public void TimerCallback(object state)
	{
		if (this.Elapsed != null)
		{
			this.Elapsed(this, new EventArgs());
		}
	}

	private void StopAndDisposeTimer()
	{
		try
		{
			_timer?.Change(-1, -1);
		}
		catch (ObjectDisposedException)
		{
		}
		WaitHandle waitHandle = null;
		try
		{
			if (_timer != null)
			{
				waitHandle = new AutoResetEvent(initialState: false);
				_timer?.Dispose(waitHandle);
				waitHandle.WaitOne();
			}
			_timer = null;
		}
		finally
		{
			waitHandle?.Dispose();
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
			StopAndDisposeTimer();
		}
	}
}
