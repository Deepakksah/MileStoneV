using System;
using System.Diagnostics;

namespace VideoOS.Rules.Interfaces;

public sealed class RuleActionProcessingEventArgs : EventArgs
{
	private IRule rule;

	private IRuleAction ruleAction;

	private ActionTargetCollection expandedTargets;

	private TriggerEvent triggerEvent;

	public IRule Rule
	{
		[DebuggerStepThrough]
		get
		{
			return rule;
		}
	}

	public IRuleAction RuleAction
	{
		[DebuggerStepThrough]
		get
		{
			return ruleAction;
		}
	}

	public ActionTargetCollection ExpandedTargets
	{
		[DebuggerStepThrough]
		get
		{
			return expandedTargets;
		}
	}

	public TriggerEvent TriggerEvent
	{
		[DebuggerStepThrough]
		get
		{
			return triggerEvent;
		}
	}

	public RuleActionProcessingEventArgs(IRule rule, IRuleAction ruleAction, ActionTargetCollection expandedTargets, TriggerEvent triggerEvent)
	{
		this.rule = rule;
		this.ruleAction = ruleAction;
		this.expandedTargets = expandedTargets;
		this.triggerEvent = triggerEvent;
	}
}
