using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace VideoOS.Platform.AccessControl.Plugin;

public sealed class ACStatesInvalidatedEventArgs : EventArgs
{
	public IEnumerable<string> OperationableInstanceIds { get; private set; }

	public ACStatesInvalidatedEventArgs(IEnumerable<string> operationableInstanceIds)
	{
		OperationableInstanceIds = new ReadOnlyCollection<string>((operationableInstanceIds != null) ? operationableInstanceIds.ToList() : new List<string>());
	}
}
