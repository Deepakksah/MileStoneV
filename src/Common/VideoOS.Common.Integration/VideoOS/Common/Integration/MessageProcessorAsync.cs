using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration;

public class MessageProcessorAsync<T> : IThreadPoolMessageProcessor<T>
{
	public delegate Task ProcessDelegate(T obj);

	private int maxProcessingMessages;

	public IEventTrace eventTracingObj = new EventTracer();

	private ProcessDelegate processDelegate;

	private volatile bool stopped;

	private volatile bool enabled;

	private object internalSync = new object();

	private LinkedList<T> pendingItems = new LinkedList<T>();

	private List<LinkedListNode<T>> processingItems = new List<LinkedListNode<T>>();

	public int MaxProcessingMessages
	{
		[DebuggerStepThrough]
		get
		{
			return maxProcessingMessages;
		}
		[DebuggerStepThrough]
		set
		{
			if (value <= 0)
			{
				throw new ArgumentOutOfRangeException("value", "Value must be greater than zero.");
			}
			maxProcessingMessages = value;
		}
	}

	public int PendingItemsCount => pendingItems.Count;

	public int ProcessingItemsCount => processingItems.Count;

	public LinkedList<T> PendingItems => pendingItems;

	public MessageProcessorAsync(ProcessDelegate processDelegate)
	{
		maxProcessingMessages = 1;
		this.processDelegate = processDelegate;
	}

	public MessageProcessorAsync(ProcessDelegate processDelegate, IEventTrace eventTracing)
		: this(processDelegate)
	{
		eventTracingObj = eventTracing;
	}

	public void Start(bool enable)
	{
		if (enable)
		{
			EnableProcessing(enable: true);
		}
	}

	public void Stop()
	{
		lock (internalSync)
		{
			stopped = true;
			pendingItems.Clear();
		}
	}

	public void EnableProcessing(bool enable)
	{
		lock (internalSync)
		{
			enabled = enable;
			if (stopped)
			{
				return;
			}
		}
		EnsureItemProcessing();
	}

	public void QueueMessage(T message)
	{
		lock (internalSync)
		{
			if (stopped)
			{
				return;
			}
			pendingItems.AddLast(message);
			eventTracingObj.Trace("Pending Queue size is {0}", PendingItemsCount);
		}
		EnsureItemProcessing();
	}

	public void QueueMessages(T[] messages)
	{
		if (messages == null)
		{
			throw new ArgumentNullException("messages");
		}
		lock (internalSync)
		{
			if (stopped)
			{
				return;
			}
			foreach (T value in messages)
			{
				pendingItems.AddLast(value);
			}
		}
		EnsureItemProcessing();
	}

	private void EnsureItemProcessing()
	{
		if (stopped || !enabled)
		{
			return;
		}
		List<LinkedListNode<T>> list = new List<LinkedListNode<T>>();
		lock (internalSync)
		{
			if (processingItems.Count >= maxProcessingMessages)
			{
				return;
			}
			for (LinkedListNode<T> first = pendingItems.First; first != null; first = pendingItems.First)
			{
				if (stopped || !enabled)
				{
					return;
				}
				if (processingItems.Count >= maxProcessingMessages)
				{
					break;
				}
				first = ProcessMessage(first);
				processingItems.Add(first);
			}
			foreach (LinkedListNode<T> processingItem in processingItems)
			{
				list.Add(processingItem);
			}
		}
		foreach (LinkedListNode<T> node in list)
		{
			Task.Run(delegate
			{
				ProcessItem(node);
			});
		}
	}

	public virtual LinkedListNode<T> ProcessMessage(LinkedListNode<T> node)
	{
		pendingItems.Remove(node);
		return node;
	}

	private async void ProcessItem(object state)
	{
		LinkedListNode<T> node = state as LinkedListNode<T>;
		T value = node.Value;
		try
		{
			await processDelegate(value);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
		}
		lock (internalSync)
		{
			processingItems.Remove(node);
		}
		EnsureItemProcessing();
	}
}
