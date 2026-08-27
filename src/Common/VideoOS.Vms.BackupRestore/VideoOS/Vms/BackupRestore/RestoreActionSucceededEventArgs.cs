using System;

namespace VideoOS.Vms.BackupRestore;

public class RestoreActionSucceededEventArgs : EventArgs
{
	public RestoreConfigActionStatus ActionStatus { get; private set; }

	public RestoreActionSucceededEventArgs(RestoreConfigActionStatus status)
	{
		ActionStatus = status;
	}
}
