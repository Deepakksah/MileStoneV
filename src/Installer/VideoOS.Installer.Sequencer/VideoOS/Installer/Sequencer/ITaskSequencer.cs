using VideoOS.Installer.Common;

namespace VideoOS.Installer.Sequencer;

public interface ITaskSequencer
{
	TaskSequencerSuite TaskSuite { get; set; }

	event ProgressEventHandler CurrentProgress;

	event ProgressEventHandler OverallProgress;

	event MessageEventHandler MessageChanged;

	event ActionEventHandler ActionChanged;

	void UpdateCurrentProgress(int current, int maximum);

	void UpdateOverallProgress(int current, int maximum);

	void UpdateMessage(string message);

	void UpdateAction(string action);

	void EnsureResults();
}
