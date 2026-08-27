using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace VideoOS.Platform.AccessControl.Plugin;

public sealed class ACEventsOccurredEventArgs : EventArgs
{
	public IEnumerable<ACEvent> ACEvents { get; private set; }

	public ACEventsOccurredEventArgs(IEnumerable<ACEvent> acEvents)
	{
		ACEvents = new ReadOnlyCollection<ACEvent>((acEvents != null) ? acEvents.ToList() : new List<ACEvent>());
	}
}
