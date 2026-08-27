using System;
using System.Collections.Generic;
using System.Threading;
using VideoOS.Common.Integration.Threading;

namespace VideoOS.Common.Integration.Command;

public class AdvancedThreadPoolCommandExecutor : ICommandExecutor, IDisposable
{
	private class WorkItem
	{
		public Guid Source;

		public CommandBase Command { get; private set; }

		public object State { get; private set; }

		public ICommandHandler CommandHandler { get; private set; }

		public IResponseSink ResponseSink { get; private set; }

		public WorkItem(CommandBase command, object state, ICommandHandler commandHandler, IResponseSink responseSink)
		{
			Command = command;
			State = state;
			CommandHandler = commandHandler;
			ResponseSink = responseSink;
			if (Command is ICommandWithSource)
			{
				Source = ((ICommandWithSource)Command).SourceId;
			}
			else
			{
				Source = Command.GetType().GUID;
			}
		}
	}

	private volatile bool isClosing;

	private ManualResetEvent allWorkItemsDoneEvent;

	private SmartThreadPool defaultThreadPool;

	private object internalSync;

	private LinkedList<WorkItem> pendingWorkItems;

	private Dictionary<Guid, List<WorkItem>> runningWorkItems;

	private bool disposedValue;

	public AdvancedThreadPoolCommandExecutor()
	{
		isClosing = false;
		allWorkItemsDoneEvent = new ManualResetEvent(initialState: true);
		defaultThreadPool = new SmartThreadPool(60000, 100, 4)
		{
			Name = "Advanced thread pool command executor"
		};
		internalSync = new object();
		pendingWorkItems = new LinkedList<WorkItem>();
		runningWorkItems = new Dictionary<Guid, List<WorkItem>>();
	}

	public void Close(TimeSpan timeout)
	{
		isClosing = true;
		allWorkItemsDoneEvent.WaitOne(timeout, exitContext: false);
		defaultThreadPool.Dispose();
	}

	public void Process(CommandBase command, object state, ICommandHandler commandHandler, IResponseSink responseSink)
	{
		if (isClosing)
		{
			return;
		}
		WorkItem value = new WorkItem(command, state, commandHandler, responseSink);
		lock (internalSync)
		{
			if (isClosing)
			{
				return;
			}
			pendingWorkItems.AddLast(value);
			allWorkItemsDoneEvent.Reset();
		}
		EnsureWorkItemProcessing();
	}

	public void EnsureWorkItemProcessing()
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
			LinkedListNode<WorkItem> linkedListNode = pendingWorkItems.First;
			while (linkedListNode != null)
			{
				WorkItem value = linkedListNode.Value;
				SmartThreadPool threadPool = defaultThreadPool;
				if (value.CommandHandler is IAdvancedThreadPoolCommandHandler { ThreadPool: not null } advancedThreadPoolCommandHandler)
				{
					threadPool = advancedThreadPoolCommandHandler.ThreadPool;
				}
				if (!runningWorkItems.TryGetValue(value.CommandHandler.GetType().GUID, out var value2))
				{
					value2 = new List<WorkItem>();
					runningWorkItems.Add(value.CommandHandler.GetType().GUID, value2);
				}
				if (value2.Count >= threadPool.MaxThreads || IsWorkItemProcessingForSource(value.Source))
				{
					linkedListNode = linkedListNode.Next;
					continue;
				}
				pendingWorkItems.Remove(linkedListNode);
				value2.Add(linkedListNode.Value);
				threadPool.QueueWorkItem((Action<object>)ProcessWorkItem, (object)linkedListNode.Value);
				break;
			}
		}
	}

	private bool IsWorkItemProcessingForSource(Guid source)
	{
		lock (internalSync)
		{
			foreach (List<WorkItem> value in runningWorkItems.Values)
			{
				foreach (WorkItem item in value)
				{
					if (item.Source == source)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	private void ProcessWorkItem(object state)
	{
		WorkItem workItem = (WorkItem)state;
		try
		{
			workItem.CommandHandler.Execute(workItem.Command, workItem.State, workItem.ResponseSink);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			CommandProcessor.LogUnhandledCommandHandlerException(workItem.Command, ex2);
			workItem.ResponseSink.AddResponse(new ExceptionResponse(ex2));
		}
		finally
		{
			lock (internalSync)
			{
				if (runningWorkItems.TryGetValue(workItem.CommandHandler.GetType().GUID, out var value))
				{
					value.Remove(workItem);
					if (value.Count == 0)
					{
						runningWorkItems.Remove(workItem.CommandHandler.GetType().GUID);
					}
				}
				if (runningWorkItems.Count == 0)
				{
					allWorkItemsDoneEvent.Set();
				}
			}
			EnsureWorkItemProcessing();
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposedValue)
		{
			return;
		}
		if (disposing)
		{
			if (allWorkItemsDoneEvent != null)
			{
				allWorkItemsDoneEvent.Dispose();
				allWorkItemsDoneEvent = null;
			}
			if (defaultThreadPool != null)
			{
				defaultThreadPool.Dispose();
				defaultThreadPool = null;
			}
		}
		disposedValue = true;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
