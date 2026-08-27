using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace VideoOS.Platform.AccessControl.Plugin;

public sealed class ACCredentialHoldersChangedEventArgs : EventArgs
{
	public IEnumerable<ACCredentialHolder> ACCredentialHolders { get; private set; }

	public ACCredentialHoldersChangedEventArgs(IEnumerable<ACCredentialHolder> acCredentialHolders)
	{
		ACCredentialHolders = new ReadOnlyCollection<ACCredentialHolder>((acCredentialHolders != null) ? acCredentialHolders.ToList() : new List<ACCredentialHolder>());
	}
}
