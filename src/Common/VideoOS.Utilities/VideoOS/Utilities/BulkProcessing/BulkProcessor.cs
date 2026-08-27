using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace VideoOS.Utilities.BulkProcessing;

public class BulkProcessor<T> : IDisposable
{
	private volatile bool isClosing;

	private object internalSync;

	private Queue<T> pendingWorkItems;

	private bool isWorkItemRunning;

	private ManualResetEvent noWorkItemRunningEvent;

	private int maxBulkSize;

	private Action<T[]> processDelegate;

	private Action<Exception, T[]> failedProcessDelegate;

	private readonly ITaskFactory taskFactory = new TaskFactory();

	private bool disposedValue;

	public BulkProcessor(Action<T[]> processDelegate, int maxBulkSize)
		: this(processDelegate, (Action<Exception, T[]>)null, maxBulkSize)
	{
	}

	public BulkProcessor(Action<T[]> processDelegate, Action<Exception, T[]> failedProcessDelegate, int maxBulkSize)
	{
		this.maxBulkSize = maxBulkSize;
		this.processDelegate = processDelegate;
		this.failedProcessDelegate = failedProcessDelegate;
		isClosing = false;
		internalSync = new object();
		pendingWorkItems = new Queue<T>();
		isWorkItemRunning = false;
		noWorkItemRunningEvent = new ManualResetEvent(initialState: true);
	}

	public BulkProcessor(Action<T[]> processDelegate, int maxBulkSize, ITaskFactory taskFactory)
		: this(processDelegate, (Action<Exception, T[]>)null, maxBulkSize, taskFactory)
	{
	}

	public BulkProcessor(Action<T[]> processDelegate, Action<Exception, T[]> failedProcessDelegate, int maxBulkSize, ITaskFactory taskFactory)
		: this(processDelegate, failedProcessDelegate, maxBulkSize)
	{
		this.taskFactory = taskFactory;
	}

	public void Close()
	{
		isClosing = true;
		noWorkItemRunningEvent.WaitOne(10000, exitContext: false);
	}

	public void EnqueueItem(T workItem)
	{
		if (isClosing)
		{
			return;
		}
		lock (internalSync)
		{
			if (isClosing)
			{
				return;
			}
			pendingWorkItems.Enqueue(workItem);
		}
		EnsureWorkItemProcessing();
	}

	private void EnsureWorkItemProcessing()
	{
		if (isClosing)
		{
			return;
		}
		lock (internalSync)
		{
			if (isClosing || isWorkItemRunning)
			{
				return;
			}
			int num = 0;
			Collection<T> workItems = new Collection<T>();
			while (pendingWorkItems.Count > 0 && num < maxBulkSize)
			{
				T item = pendingWorkItems.Dequeue();
				workItems.Add(item);
				num++;
			}
			if (num != 0)
			{
				isWorkItemRunning = true;
				noWorkItemRunningEvent.Reset();
				taskFactory.StartTask(delegate
				{
					ProcessWorkItems(workItems.ToArray());
				});
			}
		}
	}

	private void ProcessWorkItems(T[] workItems)
	{
		try
		{
			processDelegate(workItems);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception arg)
		{
			if (failedProcessDelegate != null)
			{
				failedProcessDelegate(arg, workItems);
			}
		}
		finally
		{
			lock (internalSync)
			{
				isWorkItemRunning = false;
				noWorkItemRunningEvent.Set();
			}
			EnsureWorkItemProcessing();
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing && noWorkItemRunningEvent != null)
			{
				noWorkItemRunningEvent.Dispose();
				noWorkItemRunningEvent = null;
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
