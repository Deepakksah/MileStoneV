using System;

namespace VideoOS.Common.Integration.Threading;

public class TaskProgressEventArgs : EventArgs
{
	public string TaskID { get; private set; }

	public int Progress { get; private set; }

	public TaskProgressEventArgs(string taskID, int progress)
	{
		TaskID = taskID;
		Progress = progress;
	}
}
