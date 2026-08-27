using System;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace VideoOS.Common.Integration.Threading;

public class AsyncTaskManager
{
	private int _noofActiveTasks;

	private bool _abortRequested;

	public int NumberOfActiveTasks
	{
		get
		{
			lock (this)
			{
				return _noofActiveTasks;
			}
		}
	}

	public bool AbortRequested
	{
		get
		{
			lock (this)
			{
				return _abortRequested;
			}
		}
		set
		{
			lock (this)
			{
				_abortRequested = value;
			}
		}
	}

	public event EventHandler<TaskProgressEventArgs> TaskProgress;

	public event EventHandler<TaskEndedEventArgs> TaskEnded;

	public AsyncTaskManager()
	{
		_noofActiveTasks = 0;
		_abortRequested = false;
	}

	public void BeginTask(AsyncTask taskDelegate, TaskState taskState)
	{
		if (taskDelegate == null)
		{
			throw new ArgumentNullException("taskDelegate");
		}
		IncreaseNoofActiveTasks();
		taskDelegate.BeginInvoke(taskState, ReportProgress, IsAbortRequested, TaskCompleted, taskState);
	}

	public WaitForZeroActiveTasksResult WaitForZeroActiveTasks(int timeoutInMS)
	{
		DateTime dateTime = ((timeoutInMS >= 0) ? DateTime.UtcNow.AddMilliseconds(timeoutInMS) : DateTime.MaxValue);
		do
		{
			if (NumberOfActiveTasks == 0)
			{
				return new WaitForZeroActiveTasksResult(NumberOfActiveTasks, timeoutElapsed: false);
			}
			Thread.Sleep(100);
		}
		while (DateTime.UtcNow < dateTime);
		return new WaitForZeroActiveTasksResult(NumberOfActiveTasks, timeoutElapsed: true);
	}

	private void TaskCompleted(IAsyncResult result)
	{
		AsyncResult asyncResult = (AsyncResult)result;
		TaskState taskState = (TaskState)result.AsyncState;
		try
		{
			DecreaseNoofActiveTasks();
			((AsyncTask)asyncResult.AsyncDelegate).EndInvoke(result);
		}
		catch (Exception taskException)
		{
			taskState.ResultStatus = TaskResultStatus.Failed;
			taskState.TaskException = taskException;
		}
		OnTaskEnded(new TaskEndedEventArgs(taskState));
	}

	private void ReportProgress(string taskID, int currentProgress)
	{
		TaskProgressEventArgs e = new TaskProgressEventArgs(taskID, currentProgress);
		OnTaskProgress(e);
	}

	private void IncreaseNoofActiveTasks()
	{
		lock (this)
		{
			_noofActiveTasks++;
		}
	}

	private void DecreaseNoofActiveTasks()
	{
		lock (this)
		{
			_noofActiveTasks--;
		}
	}

	public bool CheckAndResetAbortRequest()
	{
		lock (this)
		{
			if (_abortRequested)
			{
				_abortRequested = false;
				return true;
			}
			return false;
		}
	}

	private bool IsAbortRequested()
	{
		return AbortRequested;
	}

	private void OnTaskProgress(TaskProgressEventArgs e)
	{
		EventHandler<TaskProgressEventArgs> eventHandler = this.TaskProgress;
		if (eventHandler != null)
		{
			try
			{
				eventHandler(this, e);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}

	private void OnTaskEnded(TaskEndedEventArgs e)
	{
		EventHandler<TaskEndedEventArgs> eventHandler = this.TaskEnded;
		if (eventHandler != null)
		{
			try
			{
				eventHandler(this, e);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
