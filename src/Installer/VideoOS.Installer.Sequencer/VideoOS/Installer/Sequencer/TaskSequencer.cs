using System.Globalization;
using VideoOS.Installer.Common;
using VideoOS.Installer.Core.Task;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.Sequencer;

public class TaskSequencer : ITaskSequencer
{
	public TaskSequencerSuite TaskSuite { get; set; }

	public event ProgressEventHandler CurrentProgress;

	public event ProgressEventHandler OverallProgress;

	public event MessageEventHandler MessageChanged;

	public event ActionEventHandler ActionChanged;

	public void UpdateCurrentProgress(int current, int maximum)
	{
		if (this.CurrentProgress != null)
		{
			this.CurrentProgress(this, new ProgressEventArgs(current, maximum));
		}
	}

	public void UpdateOverallProgress(int current, int maximum)
	{
		if (this.OverallProgress != null)
		{
			this.OverallProgress(this, new ProgressEventArgs(current, maximum));
		}
	}

	public void UpdateMessage(string message)
	{
		if (this.MessageChanged != null)
		{
			this.MessageChanged(this, new MessageEventArgs(message));
		}
	}

	public void UpdateAction(string action)
	{
		if (this.ActionChanged != null)
		{
			this.ActionChanged(this, new ActionEventArgs(action));
		}
	}

	public void EnsureResults()
	{
		foreach (Task task in TaskSuite.Tasks)
		{
			if (task.LastError != null)
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Last error: {0}", task.LastError.TypeAndMessage()));
			}
			if (task.LastWarning != null)
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Last warning: {0}", task.LastWarning.TypeAndMessage()));
			}
		}
	}
}
