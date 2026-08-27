using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;
using VideoOS.Management.VmoClient;

namespace VideoOS.Server;

public sealed class AsyncTaskMonitor : IAsyncTaskMonitor
{
	private readonly IServer _server;

	private readonly int TaskPollInterval = 1000;

	private volatile bool _started;

	private volatile bool _finished;

	private volatile bool _initialized;

	private volatile int _allTasksCount;

	private volatile int _completedTasksCount;

	private volatile object _contextState;

	private long _expirationTicks;

	private volatile List<IAsyncTask> _tasks = new List<IAsyncTask>();

	private volatile Timer _monitorTimer;

	private object _pollLock = new object();

	public bool Initialized => _initialized;

	public bool Started => _started;

	public bool Finished => _finished;

	public object ContextState
	{
		get
		{
			return _contextState;
		}
		set
		{
			_contextState = value;
		}
	}

	public event EventHandler TasksCompleted;

	public event EventHandler TimeoutElapsed;

	public event EventHandler<IAsyncTask> TaskCompleted;

	public AsyncTaskMonitor(IServer server)
	{
		if (server == null)
		{
			throw new ArgumentNullException("server");
		}
		_server = server;
	}

	private void TaskStatusPoller(object state)
	{
		if (!System.Threading.Monitor.TryEnter(_pollLock, 100))
		{
			return;
		}
		try
		{
			if (_finished)
			{
				return;
			}
			long ticks = DateTime.Now.Ticks;
			if (ticks > Interlocked.Read(ref _expirationTicks))
			{
				this.TimeoutElapsed?.Invoke(this, EventArgs.Empty);
				StopAndDisposeMonitorTimer();
				return;
			}
			List<IAsyncTask> list = new List<IAsyncTask>();
			foreach (IAsyncTask task in _tasks)
			{
				if (_server.IsClosing)
				{
					StopAndDisposeMonitorTimer();
					return;
				}
				try
				{
					task.Refresh();
				}
				catch (VmoException)
				{
					_server.Logger.LogError("Error occurred while obtaining task status.");
					continue;
				}
				if (task.Status == AsyncTaskStatus.Succeeded || task.Status == AsyncTaskStatus.Failed || task.Status == AsyncTaskStatus.Stopped)
				{
					Interlocked.Increment(ref _completedTasksCount);
					list.Add(task);
				}
			}
			foreach (IAsyncTask item in list)
			{
				this.TaskCompleted?.Invoke(this, item);
				_tasks.Remove(item);
			}
			if (_completedTasksCount == _allTasksCount)
			{
				this.TasksCompleted?.Invoke(this, EventArgs.Empty);
				StopAndDisposeMonitorTimer();
			}
		}
		finally
		{
			System.Threading.Monitor.Exit(_pollLock);
		}
	}

	private void StopAndDisposeMonitorTimer()
	{
		if (!_finished)
		{
			_monitorTimer.Change(-1, -1);
			_monitorTimer.Dispose();
			_finished = true;
		}
	}

	public void Initialize(IEnumerable<IAsyncTask> tasks)
	{
		if (!_initialized && !_started)
		{
			_tasks.AddRange(tasks);
			_allTasksCount = _tasks.Count;
			_initialized = true;
			return;
		}
		throw new InvalidOperationException("Can not add tasks once the monitor has been initialized.");
	}

	public void Start(TimeSpan threadTimeout)
	{
		if (!_initialized)
		{
			throw new InvalidOperationException("Monitor is not initialized.");
		}
		if (_started)
		{
			throw new InvalidOperationException("Monitor is already started.");
		}
		DateTime now = DateTime.Now;
		Interlocked.Exchange(ref _expirationTicks, (now + threadTimeout).Ticks);
		lock (_pollLock)
		{
			_monitorTimer = new Timer(TaskStatusPoller, null, 0, TaskPollInterval);
			_started = true;
		}
	}
}
