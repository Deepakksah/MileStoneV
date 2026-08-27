namespace VideoOS.Vms.BackupRestore;

public class WaitForZeroActiveActionsSucceededEventArgs : RestoreActionSucceededEventArgs
{
	public int NumberOfActiveActions { get; private set; }

	public bool TimeoutElapsed { get; private set; }

	public WaitForZeroActiveActionsSucceededEventArgs(int numberOfActiveActions, bool timeoutElapsed)
		: base(RestoreConfigActionStatus.WaitingForZeroActiveActions)
	{
		NumberOfActiveActions = numberOfActiveActions;
		TimeoutElapsed = timeoutElapsed;
	}
}
