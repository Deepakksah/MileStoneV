using System.Collections.Generic;
using System.Threading;

namespace VideoOS.Platform.Util.Queue;

public class ConcurrentQueueSlim<TQueueItem>
{
	private readonly object _queueLock = new object();

	private readonly List<TQueueItem> _queue = new List<TQueueItem>();

	public bool IsStopped { get; private set; }

	public bool IsStopping { get; private set; }

	public int Count => _queue.Count;

	public bool Enqueue(TQueueItem item)
	{
		lock (_queueLock)
		{
			if (IsStopping || IsStopped)
			{
				return false;
			}
			_queue.Add(item);
			Monitor.Pulse(_queueLock);
		}
		return true;
	}

	public void Remove(TQueueItem item)
	{
		lock (_queueLock)
		{
			_queue.Remove(item);
			if (IsStopping && _queue.Count == 0)
			{
				IsStopped = true;
			}
		}
	}

	public TQueueItem Dequeue(int timeout)
	{
		bool flag = timeout >= 0;
		lock (_queueLock)
		{
			if (IsStopped)
			{
				return default(TQueueItem);
			}
			while (_queue.Count == 0)
			{
				Monitor.Wait(_queueLock, flag ? timeout : (-1));
				if ((flag && _queue.Count == 0) || IsStopped)
				{
					return default(TQueueItem);
				}
			}
			TQueueItem val = _queue[0];
			_queue.Remove(val);
			if (IsStopping && _queue.Count == 0)
			{
				IsStopped = true;
			}
			return val;
		}
	}

	public TQueueItem Peek(int timeout)
	{
		bool flag = timeout >= 0;
		lock (_queueLock)
		{
			if (IsStopped)
			{
				return default(TQueueItem);
			}
			while (_queue.Count == 0)
			{
				Monitor.Wait(_queueLock, flag ? timeout : (-1));
				if ((flag && _queue.Count == 0) || IsStopped)
				{
					return default(TQueueItem);
				}
			}
			return _queue[0];
		}
	}

	public void Stop()
	{
		lock (_queueLock)
		{
			if (!IsStopping)
			{
				IsStopping = true;
				if (_queue.Count == 0)
				{
					IsStopped = true;
				}
			}
		}
	}

	public void Clear()
	{
		lock (_queueLock)
		{
			_queue.Clear();
		}
	}
}
