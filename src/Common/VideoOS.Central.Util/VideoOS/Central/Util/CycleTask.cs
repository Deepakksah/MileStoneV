using System;

namespace VideoOS.Central.Util;

public abstract class CycleTask : IDisposable
{
	private CycleTaskMember _taskMember;

	public virtual void OnTaskStart()
	{
	}

	public virtual void OnTaskStop()
	{
	}

	public abstract TaskFuncResult TaskFunc();

	private void TaskStartFunc()
	{
		OnTaskStart();
	}

	private void TaskStopFunc()
	{
		OnTaskStop();
	}

	private TaskFuncResult TaskRunFunc()
	{
		return TaskFunc();
	}

	public void StartTask()
	{
		_taskMember = new CycleTaskMember(TaskRunFunc, TaskStartFunc, TaskStopFunc, TimeSpan.FromMilliseconds(10.0));
		_taskMember.StartTask();
	}

	public void SignalTaskToStop()
	{
		_taskMember.SignalTaskToStop();
	}

	public void WaitTaskToStop(TimeSpan waitTimeOut, bool cancelOnTimeout)
	{
		_taskMember.WaitTaskToStop(waitTimeOut, cancelOnTimeout);
	}

	public void Dispose()
	{
		Dispose(bDisposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool bDisposing)
	{
		if (bDisposing)
		{
			_taskMember.Dispose();
		}
	}
}
