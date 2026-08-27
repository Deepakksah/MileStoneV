using VideoOS.Rules;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

public sealed class GenericRecorderOnlyRuleAction : RuleActionBase
{
	public GenericRecorderOnlyRuleAction(IRuleEngine ruleEngine)
		: base(ruleEngine)
	{
	}

	public override void Execute(ActionTargetCollection expandedActionTargets, TriggerEvent triggerEvent)
	{
	}
}
