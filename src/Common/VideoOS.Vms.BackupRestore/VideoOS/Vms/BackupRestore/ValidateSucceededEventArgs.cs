using System.Collections.Generic;

namespace VideoOS.Vms.BackupRestore;

public class ValidateSucceededEventArgs : RestoreActionSucceededEventArgs
{
	public IList<string> ServiceUsers { get; private set; }

	public ValidateSucceededEventArgs(IList<string> serviceUsers)
		: base(RestoreConfigActionStatus.Validating)
	{
		ServiceUsers = serviceUsers;
	}
}
