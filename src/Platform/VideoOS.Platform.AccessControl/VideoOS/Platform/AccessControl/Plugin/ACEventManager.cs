using System;
using System.Collections.Generic;

namespace VideoOS.Platform.AccessControl.Plugin;

public abstract class ACEventManager
{
	public event EventHandler<ACEventsOccurredEventArgs> EventsOccurred = delegate
	{
	};

	protected void FireEventsOccurred(IEnumerable<ACEvent> acEvents)
	{
		this.EventsOccurred(this, new ACEventsOccurredEventArgs(acEvents));
	}
}
