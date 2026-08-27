using System;
using System.Data;

namespace VideoOS.Common.Integration.Configuration;

public class RuleVariables : CollectionBase
{
	public new RuleVariable this[Guid id] => base[id] as RuleVariable;

	public RuleVariables(ConfigurationManager manager)
		: base(manager, manager.Dataset.RuleVariables)
	{
	}

	public RuleVariable Add(Guid ruleVariableId, string name, string description, string value, bool reset)
	{
		return Add(_configurationManager.Dataset.RuleVariables.AddRuleVariablesRow(DateTime.UtcNow, ruleVariableId.ToString(), name, description, value, reset)) as RuleVariable;
	}

	protected override IConfigurationItem GetInstance(DataRow row)
	{
		return new RuleVariable(_configurationManager, row as ConfigurationDataSet.RuleVariablesRow);
	}
}
