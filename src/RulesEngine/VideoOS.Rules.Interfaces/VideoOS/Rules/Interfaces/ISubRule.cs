using System.Collections.Generic;

namespace VideoOS.Rules.Interfaces;

public interface ISubRule
{
	IRule ParentRule { get; set; }

	RuleType RuleType { get; }

	IRuleTrigger Trigger { get; set; }

	bool IsStartRule { get; set; }

	bool IsStopRule { get; set; }

	bool CanBeStartRule { get; }

	IRuleCondition Condition { get; set; }

	List<IRuleAction> Actions { get; set; }
}
