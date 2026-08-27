using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace VideoOS.Platform.AccessControl.Plugin;

public sealed class ACStatesChangedEventArgs : EventArgs
{
	public IEnumerable<ACState> ACStates { get; private set; }

	public ACStatesChangedEventArgs(IEnumerable<ACState> acStates)
	{
		ACStates = new ReadOnlyCollection<ACState>((acStates != null) ? acStates.ToList() : new List<ACState>());
	}
}
