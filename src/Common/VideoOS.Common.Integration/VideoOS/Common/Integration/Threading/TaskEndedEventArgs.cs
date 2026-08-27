using System;

namespace VideoOS.Common.Integration.Threading;

public class TaskEndedEventArgs : EventArgs
{
	public TaskState State { get; private set; }

	public TaskEndedEventArgs(TaskState state)
	{
		State = state;
	}
}
