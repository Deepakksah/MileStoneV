using VideoOS.Installer.Core.Observer;
using VideoOS.Installer.Core.Task;
using VideoOS.Installer.Interfaces;

namespace VideoOS.Installer.Sequencer.InstallerObserver;

public class InstallerTaskObserver : Observer
{
	private TaskSequencerSuite _parent;

	public InstallerTaskObserver(TaskSequencerSuite parent)
	{
		_parent = parent;
	}

	public override void Update(Observable subject)
	{
		TaskStatus status = ((Task)subject).Status;
		if (subject is InstallerTask)
		{
			InstallerTask installerTask = (InstallerTask)subject;
			if (status == TaskStatus.Running || status == TaskStatus.Terminating)
			{
				_parent.UpdateCurrentTaskProgress(installerTask.CurrentProgress, installerTask.MaximumProgress);
			}
			_parent.UpdateMessage(installerTask.Message);
			_parent.UpdateAction(installerTask.Action);
		}
	}
}
