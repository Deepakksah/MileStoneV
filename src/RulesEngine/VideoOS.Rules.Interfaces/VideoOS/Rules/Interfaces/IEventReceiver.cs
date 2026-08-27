using System;

namespace VideoOS.Rules.Interfaces;

public interface IEventReceiver
{
	event EventHandler<TriggerEventEventArgs> TriggerEventReceived;
}
