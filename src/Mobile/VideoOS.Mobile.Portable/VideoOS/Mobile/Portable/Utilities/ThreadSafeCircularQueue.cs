using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.Utilities;

public class ThreadSafeCircularQueue<T> where T : new()
{
	private readonly Queue<T> _queue = new Queue<T>();

	private readonly int _length;

	private readonly object _locker = new object();

	public ThreadSafeCircularQueue(int length)
	{
		_length = length;
	}

	public T Enqueue(T value)
	{
		T result = default(T);
		lock (_locker)
		{
			_queue.Enqueue(value);
			if (_queue.Count > _length)
			{
				result = _queue.Dequeue();
				return result;
			}
		}
		return result;
	}

	public T Dequeue()
	{
		lock (_locker)
		{
			if (_queue.Count > 0)
			{
				return _queue.Dequeue();
			}
		}
		return default(T);
	}
}
