using System;
using System.Diagnostics;

namespace VideoOS.Rules.Interfaces;

public sealed class RuleProcessingErrorEventArgs : EventArgs
{
	private IRule rule;

	private TriggerEventBase triggerEventBase;

	private Exception exception;

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

	public Exception Exception
	{
		[DebuggerStepThrough]
		get
		{
			return exception;
		}
	}

	public RuleProcessingErrorEventArgs(IRule rule, TriggerEventBase triggerEventBase, Exception exception)
	{
		this.rule = rule;
		this.triggerEventBase = triggerEventBase;
		this.exception = exception;
	}
}
