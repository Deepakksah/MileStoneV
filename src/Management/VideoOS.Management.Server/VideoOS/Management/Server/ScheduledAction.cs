using System;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Management.Server;

public class ScheduledAction<T> : IScheduledAction<T>, IDisposable
{
	private Timer _timer;

	private CancellationTokenSource _tokenSource;

	private Func<object, System.Threading.Tasks.Task> _action;

	private ICancellationTokenSourceFactory _cancellationTokenSourceFactory;

	private ITimerFactory _timerFactory;

	private bool _disposed;

	public TimeSpan ScheduleTimeSpan { get; private set; }

	public ScheduledAction(Func<object, System.Threading.Tasks.Task> action, TimeSpan scheduleTimeSpan, ICancellationTokenSourceFactory cancellationTokenSourceFactory, ITimerFactory timerFactory)
	{
		_action = action;
		ScheduleTimeSpan = scheduleTimeSpan;
		_cancellationTokenSourceFactory = cancellationTokenSourceFactory;
		_timerFactory = timerFactory;
	}

	public ScheduledAction(Func<object, System.Threading.Tasks.Task> action, TimeSpan scheduleTimeSpan)
	{
		_action = action;
		ScheduleTimeSpan = scheduleTimeSpan;
		_cancellationTokenSourceFactory = new CancellationTokenSourceFactory();
		_timerFactory = new TimerFactory();
	}

	public void StartSchedule(T scheduleData)
	{
		StopSchedule();
		_tokenSource = _cancellationTokenSourceFactory.CreateCancellationTokenSource();
		_timer = _timerFactory.CreateScheduleTimer(delegate(object input)
		{
			_action(input);
		}, new ScheduledActionData<T>(scheduleData, _tokenSource.Token), 0L, (long)ScheduleTimeSpan.TotalMilliseconds);
	}

	public void StopSchedule()
	{
		if (_timer != null)
		{
			_timer.Change(-1, -1);
			_timer.Dispose();
			_timer = null;
		}
		if (_tokenSource != null)
		{
			_tokenSource.Cancel();
			_tokenSource.Dispose();
			_tokenSource = null;
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
		if (disposing)
		{
			if (_timer != null)
			{
				_timer.Change(-1, -1);
				_timer.Dispose();
				_timer = null;
			}
			if (_tokenSource != null)
			{
				_tokenSource.Cancel();
				_tokenSource.Dispose();
				_tokenSource = null;
			}
		}
		_disposed = true;
	}

	~ScheduledAction()
	{
		Dispose(disposing: false);
	}
}
