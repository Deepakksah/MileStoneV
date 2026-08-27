using System;

namespace VideoOS.Rules.Interfaces;

public interface IRuleActionExecutionRequest
{
	Guid ID { get; set; }

	Guid RuleId { get; set; }

	Guid RuleActionInstanceId { get; set; }

	ActionTargetCollection ExpandedTargets { get; set; }

	TriggerEvent TriggerEvent { get; set; }
}
