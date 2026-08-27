using System;
using VideoOS.Platform.AccessControl.Plugin;

namespace VideoOS.Platform.AccessControl.Internal.PluginWrappers;

public class ACEventManagerWrapper
{
	private ACEventManager _acEventManager;

	public event EventHandler<ACEventsOccurredEventArgs> EventsOccurred = delegate
	{
	};

	public ACEventManagerWrapper(ACEventManager acEventManager)
	{
		_acEventManager = acEventManager;
		_acEventManager.EventsOccurred += _acEventManager_EventsOccurred;
	}

	private void _acEventManager_EventsOccurred(object sender, ACEventsOccurredEventArgs e)
	{
		ACPluginWrapper.TryHandleEvent(delegate
		{
			this.EventsOccurred(this, e);
		}, _acEventManager.GetType(), "EventsOccurred");
	}
}
