using System;

namespace VideoOS.Rules.Interfaces;

public class TriggerEventEventArgs : EventArgs
{
	public TriggerEvent TriggerEvent { get; }

	public TriggerEventEventArgs(TriggerEvent triggerEvent)
	{
		TriggerEvent = triggerEvent;
	}
}
