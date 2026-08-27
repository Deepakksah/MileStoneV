using System;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules;

public class UnregisterTriggerEventEventArgs : EventArgs
{
	public UnregisterTriggerEvent UnregisterTriggerEvent { get; }

	public UnregisterTriggerEventEventArgs(UnregisterTriggerEvent unregisterTriggerEvent)
	{
		UnregisterTriggerEvent = unregisterTriggerEvent;
	}
}
