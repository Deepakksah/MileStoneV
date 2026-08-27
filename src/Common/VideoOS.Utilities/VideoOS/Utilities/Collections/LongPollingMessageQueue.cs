using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Utilities.Collections;

public class LongPollingMessageQueue<TMessage>
{
	private ConcurrentQueue<TMessage> _eventQueue = new ConcurrentQueue<TMessage>();

	private TaskCompletionSource<TMessage[]> _tcs;

	private DateTime _lastDequeueTime = DateTime.MinValue;

	private TimeSpan _timeout;

	private TimeSpan _minimumDelay;

	public LongPollingMessageQueue(TimeSpan timeout, TimeSpan minimumDelay)
	{
		_timeout = timeout;
		_minimumDelay = minimumDelay;
	}

	public LongPollingMessageQueue()
		: this(TimeSpan.FromMinutes(1.0), TimeSpan.Zero)
	{
	}

	public void Enqueue(TMessage newMessage)
	{
		bool flag = false;
		if (DateTime.UtcNow - _lastDequeueTime > _minimumDelay)
		{
			TaskCompletionSource<TMessage[]> taskCompletionSource = Interlocked.Exchange(ref _tcs, null);
			if (taskCompletionSource != null)
			{
				_eventQueue.Enqueue(newMessage);
				taskCompletionSource.SetResult(DequeueAllAvailable());
				flag = true;
			}
		}
		if (!flag)
		{
			_eventQueue.Enqueue(newMessage);
		}
	}

	public Task<TMessage[]> DequeueAsync(TimeSpan timeout, TimeSpan minimumDelay)
	{
		return DequeueAsync(timeout, minimumDelay, CancellationToken.None);
	}

	public Task<TMessage[]> DequeueAsync(TimeSpan timeout, TimeSpan minimumDelay, CancellationToken token)
	{
		_minimumDelay = minimumDelay;
		TaskCompletionSource<TMessage[]> tcs = Interlocked.Exchange(ref _tcs, null);
		if (tcs != null)
		{
			tcs.TrySetResult(new TMessage[0]);
		}
		tcs = new TaskCompletionSource<TMessage[]>();
		Task<TMessage[]> task = tcs.Task;
		if (token != CancellationToken.None)
		{
			CancellationTokenRegistration cancellationRegistration = token.Register(delegate
			{
				tcs.TrySetResult(new TMessage[0]);
			});
			Action<Task<TMessage[]>> continuationAction = delegate
			{
				cancellationRegistration.Dispose();
			};
			task.ContinueWith(continuationAction);
		}
		task.ContinueWith(delegate(Task<TMessage[]> t)
		{
			if (t.Result.Length != 0)
			{
				_lastDequeueTime = DateTime.UtcNow;
			}
		}, TaskContinuationOptions.ExecuteSynchronously);
		TimeSpan timeSpan = DateTime.UtcNow - _lastDequeueTime;
		if (timeSpan < minimumDelay)
		{
			_tcs = tcs;
			StartTimeoutTimer(minimumDelay - timeSpan, task);
		}
		else
		{
			TMessage[] array = DequeueAllAvailable();
			if (array.Length != 0)
			{
				tcs.SetResult(array);
			}
			else
			{
				_tcs = tcs;
				StartTimeoutTimer(timeout, task);
			}
		}
		return task;
	}

	public Task<TMessage[]> DequeueAsync()
	{
		return DequeueAsync(_timeout, _minimumDelay);
	}

	private TMessage[] DequeueAllAvailable()
	{
		List<TMessage> list = new List<TMessage>();
		TMessage result;
		while (_eventQueue.TryDequeue(out result))
		{
			list.Add(result);
		}
		return list.ToArray();
	}

	private void StartTimeoutTimer(TimeSpan timeout, Task task)
	{
		Timer timer = new Timer(delegate
		{
			Interlocked.Exchange(ref _tcs, null)?.TrySetResult(DequeueAllAvailable());
		}, null, timeout, TimeSpan.FromMilliseconds(-1.0));
		task.ContinueWith(delegate
		{
			timer.Dispose();
		}, TaskContinuationOptions.ExecuteSynchronously);
	}
}
