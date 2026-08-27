using System;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules;

public class ResetVariableRuleAction : RuleActionBase
{
	private IRuleVariableManager ruleVariableManager;

	private Guid variableInstanceId;

	private Guid variableId;

	public ResetVariableRuleAction(IRuleEngine ruleEngine, IRuleVariableManager ruleVariableManager, Guid variableInstanceId, Guid variableId, TimeSpan delay)
		: base(ruleEngine, delay)
	{
		this.ruleVariableManager = ruleVariableManager;
		this.variableInstanceId = variableInstanceId;
		this.variableId = variableId;
	}

	public override void Execute(ActionTargetCollection expandedActionTargets, TriggerEvent triggerEvent)
	{
		ruleVariableManager.ResetVariables(new Guid[1] { variableInstanceId }, variableId);
	}
}
