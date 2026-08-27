using System;
using System.Threading;

namespace VideoOS.Utilities.Threading;

public abstract class BaseTimeDelayedThread
{
	private const int AwaitDueTimeThreadSleepMilliSeconds = 10;

	private readonly object _lockObject = new object();

	private readonly int _delayMilliseconds;

	private readonly string _threadName;

	private DateTime _nextRun = DateTime.MinValue;

	private bool _runRequested;

	private bool _running;

	private Thread _thread;

	protected BaseTimeDelayedThread(int delayMilliseconds, string threadName)
	{
		if (delayMilliseconds < 1)
		{
			throw new ArgumentOutOfRangeException("delayMilliseconds", "Must be greater than 1 millisecond");
		}
		if (string.IsNullOrEmpty(threadName))
		{
			throw new ArgumentNullException("threadName");
		}
		_delayMilliseconds = delayMilliseconds;
		_threadName = threadName;
	}

	protected abstract void OnTimeElapsed();

	protected abstract bool OnHandleException(Exception err);

	protected abstract bool CanContinueWaiting();

	protected void ScheduleNextRun()
	{
		lock (_lockObject)
		{
			_nextRun = DateTime.UtcNow.AddMilliseconds(_delayMilliseconds);
			_runRequested = true;
			if (_running)
			{
				return;
			}
			_running = true;
		}
		_thread = new Thread(Run);
		_thread.Name = _threadName;
		_thread.Priority = ThreadPriority.Normal;
		_thread.IsBackground = true;
		_thread.Start();
	}

	protected bool IsRunning()
	{
		lock (_lockObject)
		{
			return _running;
		}
	}

	public void Cancel()
	{
		lock (_lockObject)
		{
			_runRequested = false;
		}
	}

	private void Run()
	{
		do
		{
			if (!HasStartTimeElapsed())
			{
				continue;
			}
			try
			{
				OnTimeElapsed();
			}
			catch (Exception err)
			{
				lock (_lockObject)
				{
					_runRequested = _runRequested || OnHandleException(err);
					if (!_runRequested)
					{
						break;
					}
				}
			}
		}
		while (ContinueRunning());
	}

	private bool ContinueRunning()
	{
		lock (_lockObject)
		{
			if (_runRequested && CanContinueWaiting())
			{
				return true;
			}
			_running = false;
			_nextRun = DateTime.MinValue;
			return false;
		}
	}

	private bool HasStartTimeElapsed()
	{
		lock (_lockObject)
		{
			if (DateTime.UtcNow >= _nextRun)
			{
				_runRequested = false;
				return true;
			}
		}
		Thread.Sleep(10);
		return false;
	}
}
