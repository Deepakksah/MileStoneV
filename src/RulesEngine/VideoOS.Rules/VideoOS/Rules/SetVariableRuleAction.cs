using System;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules;

public class SetVariableRuleAction : RuleActionBase
{
	private IRuleVariableManager ruleVariableManager;

	private Guid variableInstanceId;

	private Guid variableId;

	private string variableValue;

	public SetVariableRuleAction(IRuleEngine ruleEngine, IRuleVariableManager ruleVariableManager, Guid variableInstanceId, Guid variableId, string variableValue, TimeSpan delay)
		: base(ruleEngine, delay)
	{
		this.ruleVariableManager = ruleVariableManager;
		this.variableInstanceId = variableInstanceId;
		this.variableId = variableId;
		this.variableValue = variableValue;
	}

	public override void Execute(ActionTargetCollection expandedActionTargets, TriggerEvent triggerEvent)
	{
		ruleVariableManager.SetVariables(new Guid[1] { variableInstanceId }, variableId, variableValue);
	}
}
