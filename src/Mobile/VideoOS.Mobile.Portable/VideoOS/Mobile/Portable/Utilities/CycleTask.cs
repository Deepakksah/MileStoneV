using System;

namespace VideoOS.Mobile.Portable.Utilities;

public abstract class CycleTask
{
	private CycleTaskMember _task;

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
		_task = new CycleTaskMember((Func<TaskFuncResult>)TaskRunFunc, (Action)TaskStartFunc, (Action)TaskStopFunc, 10);
		_task.StartTask();
	}

	public void SignalTaskToStop()
	{
		_task.SignalTaskToStop();
	}

	public void WaitTaskToStop(int nTimeOutMs)
	{
		_task.WaitTaskToStop(nTimeOutMs);
	}
}
