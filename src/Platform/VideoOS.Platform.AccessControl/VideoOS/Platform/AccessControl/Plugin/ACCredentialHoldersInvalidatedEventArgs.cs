using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace VideoOS.Platform.AccessControl.Plugin;

public sealed class ACCredentialHoldersInvalidatedEventArgs : EventArgs
{
	public IEnumerable<string> ACCredentialHolderIds { get; private set; }

	public ACCredentialHoldersInvalidatedEventArgs(IEnumerable<string> acCredentialHolderIds)
	{
		ACCredentialHolderIds = new ReadOnlyCollection<string>((acCredentialHolderIds != null) ? acCredentialHolderIds.ToList() : new List<string>());
	}
}
