using System;

namespace VideoOS.Vms.BackupRestore;

public class RestoreActionAbortedEventArgs : EventArgs
{
	public RestoreConfigActionStatus ActionStatus { get; private set; }

	public RestoreActionAbortedEventArgs(RestoreConfigActionStatus status)
	{
		ActionStatus = status;
	}
}
