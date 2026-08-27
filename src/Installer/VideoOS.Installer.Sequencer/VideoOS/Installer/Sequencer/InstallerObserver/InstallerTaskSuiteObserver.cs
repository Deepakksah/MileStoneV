using VideoOS.Installer.Core.Observer;
using VideoOS.Installer.Core.Task;
using VideoOS.Installer.Sequencer.Properties;

namespace VideoOS.Installer.Sequencer.InstallerObserver;

public class InstallerTaskSuiteObserver : Observer
{
	private ITaskSequencer _parent;

	public InstallerTaskSuiteObserver(ITaskSequencer parent)
	{
		_parent = parent;
	}

	public override void Update(Observable subject)
	{
		TaskStatus status = ((Task)subject).Status;
		if (subject is TaskSequencerSuite)
		{
			TaskSequencerSuite taskSequencerSuite = (TaskSequencerSuite)subject;
			switch (status)
			{
			case TaskStatus.NotStarted:
				_parent.UpdateMessage(Resources.InstallerStatus_NotStarted);
				break;
			case TaskStatus.Started:
				_parent.UpdateMessage(Resources.InstallerStatus_Started);
				break;
			case TaskStatus.Running:
				_parent.UpdateMessage(taskSequencerSuite.Message);
				_parent.UpdateAction(taskSequencerSuite.Action);
				_parent.UpdateCurrentProgress(taskSequencerSuite.CurrentTaskProgress, taskSequencerSuite.CurrentTaskProgressMax);
				_parent.UpdateOverallProgress(taskSequencerSuite.OverallProgress, taskSequencerSuite.OverallProgressMax);
				break;
			case TaskStatus.Completed:
				_parent.UpdateMessage(Resources.InstallerStatus_Completed);
				break;
			case TaskStatus.Paused:
				_parent.UpdateMessage(Resources.InstallerStatus_Paused);
				break;
			case TaskStatus.Terminating:
				_parent.UpdateMessage(Resources.InstallerStatus_Terminating);
				break;
			}
		}
	}
}
