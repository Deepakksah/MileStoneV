namespace VideoOS.Common.Integration.Threading;

public class WaitForZeroActiveTasksResult
{
	public int NumberOfActiveActions { get; private set; }

	public bool TimeoutElapsed { get; private set; }

	public WaitForZeroActiveTasksResult(int numberOfActiveActions, bool timeoutElapsed)
	{
		NumberOfActiveActions = numberOfActiveActions;
		TimeoutElapsed = timeoutElapsed;
	}
}
