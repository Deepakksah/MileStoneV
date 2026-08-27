using System;

namespace VideoOS.Vms.BackupRestore;

public class RestoreActionProgressEventArgs : EventArgs
{
	public RestoreConfigActionStatus ActionStatus { get; private set; }

	public string ComponentName { get; private set; }

	public int ActionProgress { get; private set; }

	public RestoreActionProgressEventArgs(RestoreConfigActionStatus status, string componentName, int currentActionProgress)
	{
		ActionStatus = status;
		ComponentName = componentName;
		ActionProgress = currentActionProgress;
	}
}
