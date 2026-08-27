using System.Collections.Generic;

namespace VideoOS.Mobile.Push.Apns.Notifications;

public class ThreadSafeQueue<T>
{
	private Queue<T> queue;

	private object lockObj;

	public int Count => queue.Count;

	public ThreadSafeQueue()
	{
		queue = new Queue<T>();
		lockObj = new object();
	}

	public T Dequeue()
	{
		T val = default(T);
		lock (lockObj)
		{
			return queue.Dequeue();
		}
	}

	public void Enqueue(T item)
	{
		lock (lockObj)
		{
			queue.Enqueue(item);
		}
	}
}
