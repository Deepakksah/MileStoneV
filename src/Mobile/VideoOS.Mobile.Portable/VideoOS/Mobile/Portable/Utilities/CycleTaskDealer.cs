using System;
using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.Utilities;

public class CycleTaskDealer
{
	private readonly List<CycleTaskMember> _cycleTasks = new List<CycleTaskMember>();

	public void AddTask(CycleTaskMember task)
	{
		_cycleTasks.Add(task);
	}

	public void DealWithTasks(Action<CycleTaskMember> taskAction)
	{
		foreach (CycleTaskMember cycleTask in _cycleTasks)
		{
			taskAction(cycleTask);
		}
	}

	public void StartTasks()
	{
		DealWithTasks(delegate(CycleTaskMember task)
		{
			task.StartTask();
		});
	}

	public void SignalTasksToStop()
	{
		DealWithTasks(delegate(CycleTaskMember task)
		{
			task.SignalTaskToStop();
		});
	}

	public void WaitTasksToStop(int nTimeoutMs)
	{
		DealWithTasks(delegate(CycleTaskMember task)
		{
			task.WaitTaskToStop(nTimeoutMs);
		});
	}
}
