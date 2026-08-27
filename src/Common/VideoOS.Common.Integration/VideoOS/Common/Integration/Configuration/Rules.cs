using System;
using System.Data;

namespace VideoOS.Common.Integration.Configuration;

public class Rules : CollectionBase
{
	public new Rule this[Guid id] => base[id] as Rule;

	public Rules(ConfigurationManager manager)
		: base(manager, manager.Dataset.Rules)
	{
	}

	public Rule Add(Guid ruleId, string name, string description, string ruleXml, bool enabled)
	{
		return Add(_configurationManager.Dataset.Rules.AddRulesRow(DateTime.UtcNow, ruleId.ToString(), name, description, ruleXml, enabled, 100)) as Rule;
	}

	protected override IConfigurationItem GetInstance(DataRow row)
	{
		return new Rule(_configurationManager, row as ConfigurationDataSet.RulesRow);
	}
}
