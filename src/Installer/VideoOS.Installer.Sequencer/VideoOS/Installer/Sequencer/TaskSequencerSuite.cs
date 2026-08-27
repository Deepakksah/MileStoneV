using System;
using VideoOS.Installer.Core.Task;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Sequencer.InstallerObserver;

namespace VideoOS.Installer.Sequencer;

public class TaskSequencerSuite : TaskSuite
{
	private const int TASK_TICKS = 50;

	private int _taskNo;

	private TaskSeqencerDirection _direction;

	private bool _continue;

	public int OverallProgress { get; set; }

	public int OverallProgressMax { get; set; }

	public int CurrentTaskProgress { get; set; }

	public int CurrentTaskProgressMax { get; set; }

	public string Message { get; set; }

	public string Action { get; set; }

	public bool Canceled
	{
		set
		{
			foreach (InstallerTask task in base.Tasks)
			{
				task.Canceled = value;
			}
		}
	}

	public TaskSequencerSuite(TaskSeqencerDirection direction)
	{
		OverallProgress = 0;
		OverallProgressMax = 0;
		CurrentTaskProgress = 0;
		CurrentTaskProgressMax = 0;
		_taskNo = -1;
		_direction = direction;
	}

	public TaskSequencerSuite(TaskSeqencerDirection direction, int overallProgress, int overallProgressMax)
	{
		OverallProgress = overallProgress;
		OverallProgressMax = overallProgressMax;
		CurrentTaskProgress = 0;
		CurrentTaskProgressMax = 0;
		_direction = direction;
		_continue = true;
	}

	public override TaskResult Start()
	{
		if (!_continue)
		{
			OverallProgress = 0;
			OverallProgressMax = 0;
			CurrentTaskProgress = 0;
			CurrentTaskProgressMax = 0;
			_taskNo = ((_direction == TaskSeqencerDirection.Forward) ? (-1) : _tasks.Count);
		}
		else if (_direction == TaskSeqencerDirection.Forward)
		{
			_taskNo = (int)Math.Ceiling((double)OverallProgress / 50.0) - 1;
		}
		else
		{
			_taskNo = _tasks.Count - (int)Math.Ceiling((double)OverallProgress / 50.0);
		}
		Status = TaskStatus.NotStarted;
		return base.Start();
	}

	protected override void PreRun()
	{
		if (!_continue)
		{
			OverallProgressMax = 0;
			foreach (Task task in _tasks)
			{
				if (task is IInstallationAdapter)
				{
					OverallProgressMax += 50;
				}
				OverallProgressMax += 50;
				task.AttachObserver(new InstallerTaskObserver(this));
			}
		}
		else
		{
			foreach (Task task2 in _tasks)
			{
				task2.AttachObserver(new InstallerTaskObserver(this));
			}
		}
		base.PreRun();
	}

	public void UpdateCurrentTaskProgress(int current, int maximum)
	{
		if (_direction == TaskSeqencerDirection.Backward)
		{
			current = maximum - current;
		}
		CurrentTaskProgress = current;
		CurrentTaskProgressMax = maximum;
		if (maximum != 0 && current == 0 && _direction == TaskSeqencerDirection.Forward)
		{
			_taskNo++;
		}
		if (maximum != 0 && maximum == current && _direction == TaskSeqencerDirection.Backward)
		{
			_taskNo--;
		}
		if (maximum != 0)
		{
			double num = (double)current / (double)maximum * 100.0;
			if (num > 98.0)
			{
				num = 100.0;
			}
			int num2 = (int)(50.0 * num / 100.0);
			if (num2 >= 0)
			{
				OverallProgress = _taskNo * 50 + num2;
			}
		}
		NotifyObservers();
	}

	public void UpdateMessage(string message)
	{
		Message = message;
		NotifyObservers();
	}

	public void UpdateAction(string action)
	{
		Action = action;
		NotifyObservers();
	}
}
