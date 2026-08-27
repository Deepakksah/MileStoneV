using System;

namespace VideoOS.Rules.Interfaces;

public interface IRuleVariableManager
{
	string GetVariableValue(Guid variableValueId, Guid variableId);

	void ResetVariables(Guid[] variableValueIds, Guid variableId);

	void SetConfiguration(IRuleConfiguration ruleConfiguration);

	void SetVariables(Guid[] variableValueIds, Guid variableId, string variableValue);

	bool UpdateConfiguration(IRuleConfiguration rulesInfo);
}
