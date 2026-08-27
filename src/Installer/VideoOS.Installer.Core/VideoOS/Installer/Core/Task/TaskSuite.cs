using System.Collections.Generic;
using System.Threading;
using VideoOS.Installer.Core.Observer;

namespace VideoOS.Installer.Core.Task;

public class TaskSuite : Task
{
	protected List<Task> _tasks = new List<Task>();

	protected Task _currentTask;

	protected TaskSuiteObserver _tasksObserver;

	public Task CurrentTask => _currentTask;

	public List<Task> Tasks => _tasks;

	public TaskSuite()
	{
		_tasksObserver = new TaskSuiteObserver();
		_tasksObserver.UpdatedEvent += TasksObserverUpdatedEvent;
	}

	public void AddTask(Task task)
	{
		if (_status == TaskStatus.NotStarted)
		{
			AppendTask(task);
		}
	}

	public void RemoveTask(Task task)
	{
		if (_status == TaskStatus.NotStarted)
		{
			task.DetachObserver(_tasksObserver);
			_tasks.Remove(task);
		}
	}

	protected virtual void AppendTask(Task task)
	{
		_tasks.Add(task);
		task.AttachObserver(_tasksObserver);
	}

	protected virtual void InsertTask(int index, Task task)
	{
		_tasks.Insert(index, task);
		task.AttachObserver(_tasksObserver);
	}

	public void ClearAllTasks()
	{
		if (_status != TaskStatus.NotStarted)
		{
			return;
		}
		foreach (Task task in _tasks)
		{
			task.DetachObserver(_tasksObserver);
		}
		_tasks.Clear();
	}

	protected virtual void TasksObserverUpdatedEvent(Observable subject)
	{
		NotifyObservers();
	}

	public override void Pause()
	{
		base.Pause();
		if (_currentTask != null)
		{
			_currentTask.Pause();
		}
	}

	public override TaskResult Stop()
	{
		if (Status != TaskStatus.Completed)
		{
			TaskStatus status = Status;
			Status = TaskStatus.Terminating;
			if (_currentTask != null)
			{
				_currentTask.Stop();
			}
			if (status != TaskStatus.NotStarted)
			{
				_taskCompletedEvent.WaitOne();
			}
			if (_result != TaskResult.Error)
			{
				_result = TaskResult.TerminatedByUser;
			}
		}
		return _result;
	}

	protected override void DoRun()
	{
		for (int i = 0; i < _tasks.Count; i++)
		{
			Task task = _tasks[i];
			if (Status == TaskStatus.Terminating)
			{
				break;
			}
			while (Status == TaskStatus.Paused)
			{
				Thread.Sleep(100);
			}
			Interlocked.Exchange(ref _currentTask, task);
			TaskResult taskResult = task.Start();
			if (taskResult > _result || (i == _tasks.Count - 1 && _result == TaskResult.Unknown))
			{
				_result = taskResult;
			}
			DoRunUpdate(i);
			switch (taskResult)
			{
			default:
				return;
			case TaskResult.Warning:
				LastWarning = task.LastWarning;
				break;
			case TaskResult.Error:
				LastError = task.LastError;
				if (!task.ContinueOnError)
				{
					return;
				}
				break;
			case TaskResult.TerminatedByUser:
			case TaskResult.Unknown:
				return;
			case TaskResult.Success:
				break;
			}
		}
	}

	protected virtual void DoRunUpdate(int currentTask)
	{
	}

	protected override void PostRun()
	{
	}

	protected override void PreRun()
	{
	}
}
