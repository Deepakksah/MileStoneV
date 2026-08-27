using System;
using System.Diagnostics;
using System.Threading;

namespace VideoOS.Utilities.Threading;

public class ThreadPoolTask
{
	private Action<object> action;

	private ManualResetEvent doneEvent;

	public WaitHandle WaitHandle
	{
		[DebuggerStepThrough]
		get
		{
			return doneEvent;
		}
	}

	public Exception Exception { get; private set; }

	private ThreadPoolTask(Action<object> action)
	{
		this.action = action;
		doneEvent = new ManualResetEvent(initialState: false);
	}

	public bool Wait()
	{
		return Wait(-1);
	}

	public bool Wait(int millisecondsTimeout)
	{
		if (!doneEvent.WaitOne(millisecondsTimeout, exitContext: false))
		{
			return false;
		}
		if (Exception != null)
		{
			throw new ThreadPoolTaskExecuteException("Task ended with an exception", Exception);
		}
		return true;
	}

	public static ThreadPoolTask Create(Action<object> action)
	{
		return Create(action, null);
	}

	public static ThreadPoolTask Create(Action<object> action, object state)
	{
		ThreadPoolTask task = new ThreadPoolTask(action);
		ThreadPool.QueueUserWorkItem(delegate(object s)
		{
			try
			{
				action(s);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception exception)
			{
				task.Exception = exception;
			}
			task.doneEvent.Set();
		}, state);
		return task;
	}
}
