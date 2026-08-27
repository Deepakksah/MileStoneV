using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Platform.Util;

public class WorkStealingTaskScheduler : TaskScheduler, IDisposable
{
	private readonly int m_concurrencyLevel;

	private readonly Queue<Task> m_queue = new Queue<Task>();

	private WorkStealingQueue<Task>[] m_wsQueues = new WorkStealingQueue<Task>[Environment.ProcessorCount];

	private readonly Lazy<Thread[]> m_threads;

	private int m_threadsWaiting;

	private bool m_shutdown;

	[ThreadStatic]
	private static WorkStealingQueue<Task> m_wsq;

	public override int MaximumConcurrencyLevel => m_concurrencyLevel;

	public WorkStealingTaskScheduler()
		: this(Environment.ProcessorCount * 2)
	{
	}

	public WorkStealingTaskScheduler(int concurrencyLevel)
	{
		if (concurrencyLevel <= 0)
		{
			throw new ArgumentOutOfRangeException("concurrencyLevel");
		}
		m_concurrencyLevel = concurrencyLevel;
		m_threads = new Lazy<Thread[]>(delegate
		{
			Thread[] array = new Thread[m_concurrencyLevel];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new Thread(DispatchLoop)
				{
					IsBackground = true
				};
				array[i].Start();
			}
			return array;
		});
	}

	protected override void QueueTask(Task task)
	{
		m_threads.Force();
		if ((task.CreationOptions & TaskCreationOptions.LongRunning) != TaskCreationOptions.None)
		{
			Thread thread = new Thread(delegate(object state)
			{
				TryExecuteTask((Task)state);
			});
			thread.IsBackground = true;
			thread.Start(task);
			return;
		}
		WorkStealingQueue<Task> wsq = m_wsq;
		if (wsq != null && (task.CreationOptions & TaskCreationOptions.PreferFairness) == 0)
		{
			wsq.LocalPush(task);
			if (m_threadsWaiting <= 0)
			{
				return;
			}
			lock (m_queue)
			{
				Monitor.Pulse(m_queue);
				return;
			}
		}
		lock (m_queue)
		{
			m_queue.Enqueue(task);
			if (m_threadsWaiting > 0)
			{
				Monitor.Pulse(m_queue);
			}
		}
	}

	protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
	{
		return TryExecuteTask(task);
	}

	protected override IEnumerable<Task> GetScheduledTasks()
	{
		List<Task> list = new List<Task>();
		bool lockTaken = false;
		try
		{
			Monitor.TryEnter(m_queue, ref lockTaken);
			if (!lockTaken)
			{
				throw new NotSupportedException();
			}
			list.AddRange(m_queue.ToArray());
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(m_queue);
			}
		}
		WorkStealingQueue<Task>[] wsQueues = m_wsQueues;
		foreach (WorkStealingQueue<Task> workStealingQueue in wsQueues)
		{
			if (workStealingQueue != null)
			{
				list.AddRange(workStealingQueue.ToArray());
			}
		}
		return list;
	}

	private void AddWsq(WorkStealingQueue<Task> wsq)
	{
		lock (m_wsQueues)
		{
			int i;
			for (i = 0; i < m_wsQueues.Length; i++)
			{
				if (m_wsQueues[i] == null)
				{
					m_wsQueues[i] = wsq;
					return;
				}
			}
			WorkStealingQueue<Task>[] array = new WorkStealingQueue<Task>[i * 2];
			Array.Copy(m_wsQueues, array, i);
			array[i] = wsq;
			m_wsQueues = array;
		}
	}

	private void RemoveWsq(WorkStealingQueue<Task> wsq)
	{
		lock (m_wsQueues)
		{
			for (int i = 0; i < m_wsQueues.Length; i++)
			{
				if (m_wsQueues[i] == wsq)
				{
					m_wsQueues[i] = null;
				}
			}
		}
	}

	private void DispatchLoop()
	{
		WorkStealingQueue<Task> workStealingQueue = (m_wsq = new WorkStealingQueue<Task>());
		AddWsq(workStealingQueue);
		try
		{
			while (true)
			{
				Task obj = null;
				if (!workStealingQueue.LocalPop(ref obj))
				{
					bool flag = false;
					while (true)
					{
						lock (m_queue)
						{
							if (m_shutdown)
							{
								return;
							}
							if (m_queue.Count != 0)
							{
								obj = m_queue.Dequeue();
								break;
							}
							if (flag)
							{
								m_threadsWaiting++;
								try
								{
									Monitor.Wait(m_queue);
								}
								finally
								{
									m_threadsWaiting--;
								}
								if (m_shutdown)
								{
									return;
								}
								flag = false;
								continue;
							}
						}
						WorkStealingQueue<Task>[] wsQueues = m_wsQueues;
						int i;
						for (i = 0; i < wsQueues.Length; i++)
						{
							WorkStealingQueue<Task> workStealingQueue2 = wsQueues[i];
							if (workStealingQueue2 != null && workStealingQueue2 != workStealingQueue && workStealingQueue2.TrySteal(ref obj))
							{
								break;
							}
						}
						if (i != wsQueues.Length)
						{
							break;
						}
						flag = true;
					}
				}
				TryExecuteTask(obj);
			}
		}
		finally
		{
			RemoveWsq(workStealingQueue);
		}
	}

	public void Dispose()
	{
		m_shutdown = true;
		if (m_queue != null && m_threads.IsValueCreated)
		{
			Thread[] value = m_threads.Value;
			lock (m_queue)
			{
				Monitor.PulseAll(m_queue);
			}
			for (int i = 0; i < value.Length; i++)
			{
				value[i].Join();
			}
		}
	}
}
