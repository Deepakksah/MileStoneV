using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

public class QueuedCommandExecutor : ICommandExecutor, IDisposable
{
	private class WorkItem
	{
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
		}
	}

	private volatile bool isClosing;

	private object internalSync;

	private Queue<WorkItem> pendingWorkItems;

	private bool isWorkItemRunning;

	private ManualResetEvent noWorkItemRunningEvent;

	private bool disposedValue;

	public QueuedCommandExecutor()
	{
		isClosing = false;
		internalSync = new object();
		pendingWorkItems = new Queue<WorkItem>();
		isWorkItemRunning = false;
		noWorkItemRunningEvent = new ManualResetEvent(initialState: true);
	}

	public void Close(TimeSpan timeout)
	{
		isClosing = true;
		noWorkItemRunningEvent.WaitOne(timeout, exitContext: false);
	}

	public void Process(CommandBase command, object state, ICommandHandler commandHandler, IResponseSink responseSink)
	{
		if (isClosing)
		{
			return;
		}
		WorkItem item = new WorkItem(command, state, commandHandler, responseSink);
		lock (internalSync)
		{
			if (isClosing)
			{
				return;
			}
			pendingWorkItems.Enqueue(item);
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
			if (!isClosing && !isWorkItemRunning && pendingWorkItems.Count > 0)
			{
				WorkItem workItem = pendingWorkItems.Dequeue();
				isWorkItemRunning = true;
				noWorkItemRunningEvent.Reset();
				Task.Run(delegate
				{
					ProcessWorkItem(workItem);
				});
			}
		}
	}

	private void ProcessWorkItem(WorkItem workItem)
	{
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
