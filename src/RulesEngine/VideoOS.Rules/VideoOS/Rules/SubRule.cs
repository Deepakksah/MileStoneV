using System.Collections.Generic;
using System.Diagnostics;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules;

[DebuggerDisplay("SubRule. Type: {DebugSubRuleType} Parent: {parent.Name}")]
public abstract class SubRule : ISubRule
{
	protected IRule Parent { get; set; }

	protected RuleType Type { get; }

	protected IRuleTrigger RuleTrigger { get; set; }

	protected IRuleCondition RuleCondition { get; set; }

	protected List<IRuleAction> RuleActions { get; set; }

	protected bool IsStartSubRule { get; set; }

	public RuleType RuleType => Type;

	public IRule ParentRule
	{
		get
		{
			return Parent;
		}
		set
		{
			Parent = value;
		}
	}

	public IRuleTrigger Trigger
	{
		get
		{
			return RuleTrigger;
		}
		set
		{
			RuleTrigger = value;
		}
	}

	public IRuleCondition Condition
	{
		get
		{
			return RuleCondition;
		}
		set
		{
			RuleCondition = value;
		}
	}

	public List<IRuleAction> Actions
	{
		get
		{
			return RuleActions;
		}
		set
		{
			RuleActions = value;
		}
	}

	public bool IsStartRule
	{
		get
		{
			return IsStartSubRule;
		}
		set
		{
			IsStartSubRule = value;
		}
	}

	public bool IsStopRule
	{
		get
		{
			return !IsStartSubRule;
		}
		set
		{
			IsStartSubRule = !value;
		}
	}

	private string DebugSubRuleType
	{
		get
		{
			if (!IsStartSubRule)
			{
				return "Stop sub-rule";
			}
			return "Start sub-rule";
		}
	}

	public virtual bool CanBeStartRule => true;

	protected SubRule(RuleType ruleType)
	{
		Type = ruleType;
		IsStartSubRule = false;
	}
}
