using System.Collections.Generic;
using VideoOS.Installer.Core.Task;
using VideoOS.Installer.Interfaces;

namespace VideoOS.Installer.Sequencer.InstallerObserver;

public class InstallerTaskSuite : TaskSequencerSuite
{
	public bool NeedsReboot
	{
		get
		{
			foreach (InstallerTask task in _tasks)
			{
				if (task.NeedsReboot)
				{
					return true;
				}
			}
			return false;
		}
	}

	public List<string> FailedPackages
	{
		get
		{
			List<string> list = new List<string>();
			foreach (Task task in _tasks)
			{
				if (task.Result == TaskResult.Error)
				{
					_ = task.Id;
					if (!((InstallerTask)task).NeedsReboot)
					{
						list.Add(task.Id.ToString());
					}
				}
			}
			return list;
		}
	}

	public List<string> InstalledPackages
	{
		get
		{
			List<string> list = new List<string>();
			foreach (Task task in _tasks)
			{
				if (task.Result != TaskResult.Success)
				{
					if (!((InstallerTask)task).NeedsReboot)
					{
						continue;
					}
					_ = task.Id;
				}
				list.Add(task.Id.ToString());
			}
			return list;
		}
	}

	public List<string> UnknownStatePackages
	{
		get
		{
			List<string> list = new List<string>();
			foreach (Task task in _tasks)
			{
				if (task.Result == TaskResult.Unknown)
				{
					_ = task.Id;
					list.Add(task.Id.ToString());
				}
			}
			return list;
		}
	}

	public InstallerTaskSuite(TaskSeqencerDirection direction)
		: base(direction)
	{
	}
}
