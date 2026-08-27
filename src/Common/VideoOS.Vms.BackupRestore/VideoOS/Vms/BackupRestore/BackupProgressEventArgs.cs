using System;

namespace VideoOS.Vms.BackupRestore;

public class BackupProgressEventArgs : EventArgs
{
	public BackupConfigManager.BackupConfigActionStatus ActionStatus { get; private set; }

	public string ComponentName { get; private set; }

	public int CurrentActionProgress { get; private set; }

	public BackupProgressEventArgs(BackupConfigManager.BackupConfigActionStatus status, string componentName, int currentActionProgress)
	{
		ActionStatus = status;
		ComponentName = componentName;
		CurrentActionProgress = currentActionProgress;
	}
}
