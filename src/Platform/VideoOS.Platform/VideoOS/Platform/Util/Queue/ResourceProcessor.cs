using System;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Platform.Util.Queue;

public abstract class ResourceProcessor<TOutput> : IDisposable
{
	private bool _started;

	private bool _stopRequested;

	private bool _shuttingDown;

	private readonly object _idleMonitorLock = new object();

	private MultiLevelResourceQueueInput<TOutput> _item;

	private readonly AutoResetEvent _idleTimeoutEvent = new AutoResetEvent(initialState: false);

	private bool _requestProcessingDoneCalled;

	private readonly ManualResetEventSlim _processRequestFinishedEvent = new ManualResetEventSlim(initialState: false);

	private bool _disposed;

	private readonly object _disposedLock = new object();

	private MultiLevelResourceQueue<TOutput> _queue;

	private LimitManager _limitManager;

	private int _idleTimeTimeout;

	public bool Stopped { get; private set; }

	public bool Idle { get; private set; }

	public void Init(MultiLevelResourceQueue<TOutput> queue, LimitManager limitManager, int idleTimeTimeout)
	{
		_queue = queue;
		_limitManager = limitManager;
		_idleTimeTimeout = idleTimeTimeout;
	}

	public bool Process(MultiLevelResourceQueueInput<TOutput> item)
	{
		lock (_idleMonitorLock)
		{
			if (_stopRequested || _shuttingDown)
			{
				return false;
			}
			_item = item;
			Idle = false;
			if (!_started)
			{
				Task.Factory.StartNew(ProcessRequest, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default).ContinueWith(delegate(Task task)
				{
					if (!_requestProcessingDoneCalled)
					{
						_limitManager.RequestProcessingDone();
					}
					if (task.Exception != null)
					{
						_queue?.NotifyError(item, "Unhandled exception in background task", task.Exception);
					}
					_processRequestFinishedEvent.Set();
					Stop();
				}, TaskScheduler.Default);
			}
			_started = true;
			_idleTimeoutEvent.Set();
			return true;
		}
	}

	private void ProcessRequest()
	{
		while (true)
		{
			MultiLevelResourceQueueInput<TOutput> multiLevelResourceQueueInput = null;
			lock (_idleMonitorLock)
			{
				multiLevelResourceQueueInput = _item;
			}
			_requestProcessingDoneCalled = false;
			if (SafeShouldReinitialize(multiLevelResourceQueueInput))
			{
				SafeInitialize(multiLevelResourceQueueInput);
			}
			SafeProcessItem(multiLevelResourceQueueInput);
			lock (_idleMonitorLock)
			{
				if (multiLevelResourceQueueInput == _item)
				{
					if (!_stopRequested)
					{
						_idleTimeoutEvent.Reset();
					}
					Idle = true;
				}
			}
			_limitManager.RequestProcessingDone();
			_requestProcessingDoneCalled = true;
			_idleTimeoutEvent.WaitOne(_idleTimeTimeout);
			lock (_idleMonitorLock)
			{
				if (Idle)
				{
					_shuttingDown = true;
					break;
				}
			}
		}
	}

	public virtual void Stop()
	{
		Stop(-1);
	}

	public bool Stop(int timeout)
	{
		lock (_idleMonitorLock)
		{
			if (_stopRequested)
			{
				return false;
			}
			_stopRequested = true;
		}
		_idleTimeoutEvent.Set();
		Stopped = _processRequestFinishedEvent.Wait(timeout);
		return Stopped;
	}

	private bool SafeShouldReinitialize(MultiLevelResourceQueueInput<TOutput> item)
	{
		try
		{
			return ShouldReinitialize(item);
		}
		catch (Exception exception)
		{
			_queue?.NotifyError(item, "Unhandled exception in ShouldReinitialize()", exception);
		}
		return true;
	}

	private void SafeInitialize(MultiLevelResourceQueueInput<TOutput> item)
	{
		try
		{
			Initialize(item);
		}
		catch (Exception exception)
		{
			_queue?.NotifyError(item, "Unhandled exception in Initialize()", exception);
		}
	}

	private void SafeProcessItem(MultiLevelResourceQueueInput<TOutput> item)
	{
		try
		{
			ProcessItem(item);
		}
		catch (Exception exception)
		{
			_queue?.NotifyError(item, "Unhandled exception in ProcessItem()", exception);
		}
	}

	protected abstract bool ShouldReinitialize(MultiLevelResourceQueueInput<TOutput> item);

	protected abstract void Initialize(MultiLevelResourceQueueInput<TOutput> item);

	protected abstract void ProcessItem(MultiLevelResourceQueueInput<TOutput> item);

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (_disposed || !disposing)
		{
			return;
		}
		lock (_disposedLock)
		{
			if (_disposed)
			{
				return;
			}
			_disposed = true;
		}
		if (!Stopped)
		{
			Stop();
		}
		_idleTimeoutEvent.Dispose();
		_processRequestFinishedEvent.Dispose();
	}
}
