using System;
using System.Diagnostics;

namespace VideoOS.Rules.Interfaces;

public sealed class RuleProcessingEventArgs : EventArgs
{
	private IRule rule;

	private TriggerEventBase triggerEventBase;

	public IRule Rule
	{
		[DebuggerStepThrough]
		get
		{
			return rule;
		}
	}

	public TriggerEventBase TriggerEventBase
	{
		[DebuggerStepThrough]
		get
		{
			return triggerEventBase;
		}
	}

	public RuleProcessingEventArgs(IRule rule, TriggerEventBase triggerEventBase)
	{
		this.rule = rule;
		this.triggerEventBase = triggerEventBase;
	}
}
