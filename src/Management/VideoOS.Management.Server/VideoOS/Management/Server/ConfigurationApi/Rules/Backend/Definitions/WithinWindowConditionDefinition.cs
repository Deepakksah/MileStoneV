using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Definitions;

public class WithinWindowConditionDefinition : IConditionDefinition
{
	public ConditionType ConditionType => ConditionType.WithinWindow;

	public IDictionary<string, RuleParameterDefinition> Parameters => CreateParameters();

	private static IDictionary<string, RuleParameterDefinition> CreateParameters()
	{
		return new RuleParameterDefinition[1]
		{
			new RuleParameterDefinition
			{
				Name = "EventIsWithinWindow",
				ValueType = typeof(string)
			}
		}.ToDictionary((RuleParameterDefinition s) => s.Name);
	}
}
