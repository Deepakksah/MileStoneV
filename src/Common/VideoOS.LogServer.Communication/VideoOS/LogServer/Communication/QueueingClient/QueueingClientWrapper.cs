using System;
using System.Collections.Concurrent;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VideoOS.LogServer.Communication.QueueingClient;

public class QueueingClientWrapper<T> : IDisposable where T : ICommunicationObject
{
	private readonly IClientWrapper<T> _client;

	private readonly ILogger _logger;

	private readonly TimeSpan[] _retryDelays;

	private readonly TimeSpan _loopCycleTime;

	private readonly int _maxQueueLength;

	private object _queueLock = new object();

	private ConcurrentQueue<Entry> _activeQueue = new ConcurrentQueue<Entry>();

	private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

	private readonly object _workerLock = new object();

	private bool _workerRunning;

	private bool disposedValue;

	private bool IsClosed => _cancellationTokenSource.IsCancellationRequested;

	private int MaxRetries => _retryDelays.Length;

	public QueueingClientWrapper(IClientWrapper<T> client, TimeSpan[] retryDelays, TimeSpan loopCycleTime, int maxQueueLength, ILogger logger)
	{
		_client = client;
		_logger = logger;
		_loopCycleTime = loopCycleTime;
		_retryDelays = (retryDelays.Any() ? retryDelays : new TimeSpan[1] { _loopCycleTime });
		_maxQueueLength = maxQueueLength;
	}

	public QueueingClientWrapper(IClientWrapper<T> client, int numberOfRetries, TimeSpan loopCycleTime, int maxQueueLength, ILogger logger)
		: this(client, Enumerable.Repeat(loopCycleTime, numberOfRetries).ToArray(), loopCycleTime, maxQueueLength, logger)
	{
	}

	public async Task CallWithRetryAsync<TRes>(Func<T, Task<TRes>> func)
	{
		if (IsClosed)
		{
			LoggerExtensions.LogWarning(_logger, "Client is used after being closed. Requests are thrown away.", Array.Empty<object>());
			return;
		}
		try
		{
			await _client.CallWithRetryAsync(func);
		}
		catch (CommunicationServiceException)
		{
			LoggerExtensions.LogDebug(_logger, "Original request failed. Request is being queued.", Array.Empty<object>());
			ScheduleTask(1, async delegate
			{
				await _client.CallWithRetryAsync(func);
			});
		}
	}

	public void Close()
	{
		_cancellationTokenSource.Cancel();
	}

	private void StartWorker()
	{
		lock (_workerLock)
		{
			if (!_workerRunning)
			{
				_workerRunning = true;
				Task.Run(() => DoWork(_cancellationTokenSource.Token));
			}
		}
	}

	private async Task DoWork(CancellationToken cancellationToken)
	{
		while (!cancellationToken.IsCancellationRequested)
		{
			ConcurrentQueue<Entry> queuedEntries = null;
			lock (_queueLock)
			{
				queuedEntries = _activeQueue;
				_activeQueue = new ConcurrentQueue<Entry>();
			}
			Entry entry;
			while (queuedEntries.TryDequeue(out entry))
			{
				if (entry.IsFutureWork)
				{
					lock (_queueLock)
					{
						_activeQueue.Enqueue(entry);
					}
					continue;
				}
				try
				{
					await entry.Callback().ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception)
				{
					if (entry.NeedsRetry(MaxRetries))
					{
						ScheduleTask(entry.NextRetries, entry.Callback);
					}
					else
					{
						LoggerExtensions.LogWarning(_logger, "Request being discarded because it reached the maximum retries.", Array.Empty<object>());
					}
				}
			}
			lock (_workerLock)
			{
				lock (_queueLock)
				{
					if (_activeQueue.IsEmpty)
					{
						_workerRunning = false;
						break;
					}
				}
			}
			try
			{
				await Task.Delay(_loopCycleTime, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (ObjectDisposedException)
			{
				break;
			}
			catch (OperationCanceledException)
			{
				break;
			}
			catch (InvalidOperationException)
			{
				break;
			}
		}
	}

	private void ScheduleTask(int nextRetry, Func<Task> task)
	{
		if (IsClosed)
		{
			return;
		}
		lock (_queueLock)
		{
			if (_activeQueue.Count < _maxQueueLength)
			{
				int retryIndex = nextRetry - 1;
				_activeQueue.Enqueue(new Entry(nextRetry, task, GetDueTime(retryIndex)));
			}
			else
			{
				LoggerExtensions.LogWarning(_logger, "Request being discarded because queue is full.", Array.Empty<object>());
			}
		}
		StartWorker();
	}

	private DateTime GetDueTime(int retryIndex)
	{
		if (_retryDelays.Length > retryIndex && retryIndex >= 0)
		{
			return DateTime.Now.Add(_retryDelays[retryIndex]);
		}
		return DateTime.Now;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				_cancellationTokenSource.Cancel();
			}
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
