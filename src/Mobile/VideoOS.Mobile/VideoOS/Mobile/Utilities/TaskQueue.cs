using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Utilities;

public class TaskQueue<T>
{
	private readonly List<TaskObject<T>> _tasksList = new List<TaskObject<T>>();

	private readonly object _lockObject = new object();

	private readonly CycleTaskMember _cycleTask;

	public TaskQueue()
	{
		_cycleTask = new CycleTaskMember((Action)TaskFunc, (Action)null, (Action)null, 10);
	}

	private void TaskFunc()
	{
		TaskFuncExt();
	}

	private void TaskFuncExt(bool action = true)
	{
		List<TaskObject<T>> list;
		lock (_lockObject)
		{
			list = new List<TaskObject<T>>(_tasksList);
			_tasksList.Clear();
		}
		foreach (TaskObject<T> item in list)
		{
			try
			{
				if (action)
				{
					item.Action(item.State);
				}
				else
				{
					item.Failed(item.State);
				}
			}
			catch (Exception ex)
			{
				Log.Error("WebSocketExt.TaskFunc", $"Error with message: {ex.Message}");
			}
		}
	}

	public void Enqueue(TaskObject<T> taskObject)
	{
		lock (_lockObject)
		{
			_tasksList.Add(taskObject);
		}
		if (!_cycleTask.IsCycling)
		{
			TaskFuncExt(action: false);
		}
	}

	public void Stop()
	{
		_cycleTask.SignalTaskToStop();
		TaskFuncExt(action: false);
	}

	public void Start()
	{
		_cycleTask.StartTask();
	}
}
