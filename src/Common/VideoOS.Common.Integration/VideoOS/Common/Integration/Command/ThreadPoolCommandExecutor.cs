using System;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

public class ThreadPoolCommandExecutor : ICommandExecutor
{
	private volatile bool _isClosing;

	private int _workItemCount;

	private ManualResetEvent _allWorkItemsDoneEvent = new ManualResetEvent(initialState: true);

	public void Close(TimeSpan timeout)
	{
		_isClosing = true;
		_allWorkItemsDoneEvent.WaitOne(timeout, exitContext: false);
	}

	public void Process(CommandBase command, object state, ICommandHandler commandHandler, IResponseSink responseSink)
	{
		if (_isClosing)
		{
			return;
		}
		lock (this)
		{
			_workItemCount++;
			_allWorkItemsDoneEvent.Reset();
			Task.Factory.StartNew(delegate
			{
				try
				{
					commandHandler.Execute(command, state, responseSink);
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					CommandProcessor.LogUnhandledCommandHandlerException(command, ex2);
					responseSink.AddResponse(new ExceptionResponse(ex2));
				}
				finally
				{
					lock (this)
					{
						if (--_workItemCount == 0)
						{
							_allWorkItemsDoneEvent.Set();
						}
					}
				}
			}, TaskCreationOptions.LongRunning);
		}
	}
}
