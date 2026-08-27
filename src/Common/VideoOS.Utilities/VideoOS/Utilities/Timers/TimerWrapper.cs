using System;
using System.Timers;

namespace VideoOS.Utilities.Timers;

public class TimerWrapper : IEventTimer, ITimer, IDisposable
{
	private Timer _timer;

	private bool _isLive;

	public bool AutoReset
	{
		get
		{
			return _timer.AutoReset;
		}
		set
		{
			_timer.AutoReset = value;
		}
	}

	public double Interval
	{
		get
		{
			return _timer.Interval;
		}
		set
		{
			_timer.Interval = value;
		}
	}

	public event EventHandler Elapsed;

	public TimerWrapper(double interval)
	{
		_timer = new Timer(interval);
		_timer.Elapsed += TimerOnElapsed;
		_isLive = true;
	}

	private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
	{
		if (_isLive && this.Elapsed != null)
		{
			this.Elapsed(this, elapsedEventArgs);
		}
	}

	public void StartTimer()
	{
		_timer.Start();
		_isLive = true;
	}

	public void StartTimer(TimeSpan delay, TimeSpan period)
	{
		throw new NotImplementedException();
	}

	public void StopTimer()
	{
		_timer.Stop();
		_isLive = false;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing && _timer != null)
		{
			_isLive = false;
			_timer.Elapsed -= TimerOnElapsed;
			_timer.Dispose();
			_timer = null;
		}
	}
}
