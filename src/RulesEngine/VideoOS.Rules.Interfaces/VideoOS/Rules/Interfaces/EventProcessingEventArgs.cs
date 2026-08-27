using System;
using System.Diagnostics;

namespace VideoOS.Rules.Interfaces;

public sealed class EventProcessingEventArgs : EventArgs
{
	private TriggerEventBase triggerEventBase;

	public TriggerEventBase TriggerEventBase
	{
		[DebuggerStepThrough]
		get
		{
			return triggerEventBase;
		}
	}

	public EventProcessingEventArgs(TriggerEventBase triggerEventBase)
	{
		this.triggerEventBase = triggerEventBase;
	}
}
