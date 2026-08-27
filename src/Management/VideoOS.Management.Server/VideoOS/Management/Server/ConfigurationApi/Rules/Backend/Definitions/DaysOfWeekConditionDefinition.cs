using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Definitions;

public class DaysOfWeekConditionDefinition : IConditionDefinition
{
	public ConditionType ConditionType => ConditionType.DaysOfWeek;

	public IDictionary<string, RuleParameterDefinition> Parameters => CreateParameters();

	private static IDictionary<string, RuleParameterDefinition> CreateParameters()
	{
		return new RuleParameterDefinition[1]
		{
			new RuleParameterDefinition
			{
				Name = "DayOfWeekIsOneOf",
				ValueType = typeof(DayOfWeek[])
			}
		}.ToDictionary((RuleParameterDefinition s) => s.Name);
	}
}
