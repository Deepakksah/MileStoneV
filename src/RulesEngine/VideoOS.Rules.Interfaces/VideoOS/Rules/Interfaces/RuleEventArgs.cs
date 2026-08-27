using System;
using System.Diagnostics;

namespace VideoOS.Rules.Interfaces;

public class RuleEventArgs : EventArgs
{
	private Guid ruleId;

	public Guid RuleId
	{
		[DebuggerStepThrough]
		get
		{
			return ruleId;
		}
	}

	public RuleEventArgs(Guid ruleId)
	{
		this.ruleId = ruleId;
	}
}
