namespace VideoOS.Installer.Sequencer;

public interface ISequenceManagerWrapper
{
	ISequenceManager SequenceManager { get; set; }

	ITaskSequencer TaskSequencer { get; set; }
}
