using System;
using System.Diagnostics;

namespace VideoOS.Rules.Interfaces;

[Serializable]
public sealed class RuleActionExecutionRequest : IRuleActionExecutionRequest
{
	private Guid requestID;

	private Guid ruleId;

	private Guid ruleActionInstanceId;

	private TriggerEvent triggerEvent;

	private ActionTargetCollection expandedTargets;

	public Guid ID
	{
		[DebuggerStepThrough]
		get
		{
			return requestID;
		}
		[DebuggerStepThrough]
		set
		{
			requestID = value;
		}
	}

	public Guid RuleId
	{
		[DebuggerStepThrough]
		get
		{
			return ruleId;
		}
		[DebuggerStepThrough]
		set
		{
			ruleId = value;
		}
	}

	public Guid RuleActionInstanceId
	{
		[DebuggerStepThrough]
		get
		{
			return ruleActionInstanceId;
		}
		[DebuggerStepThrough]
		set
		{
			ruleActionInstanceId = value;
		}
	}

	public TriggerEvent TriggerEvent
	{
		[DebuggerStepThrough]
		get
		{
			return triggerEvent;
		}
		[DebuggerStepThrough]
		set
		{
			triggerEvent = value;
		}
	}

	public ActionTargetCollection ExpandedTargets
	{
		[DebuggerStepThrough]
		get
		{
			return expandedTargets;
		}
		[DebuggerStepThrough]
		set
		{
			expandedTargets = value;
		}
	}

	public RuleActionExecutionRequest(Guid ruleId, Guid ruleActionInstanceId, TriggerEvent triggerEvent, ActionTargetCollection expandedTargets)
	{
		requestID = Guid.NewGuid();
		this.ruleId = ruleId;
		this.ruleActionInstanceId = ruleActionInstanceId;
		this.triggerEvent = triggerEvent;
		this.expandedTargets = expandedTargets;
	}
}
