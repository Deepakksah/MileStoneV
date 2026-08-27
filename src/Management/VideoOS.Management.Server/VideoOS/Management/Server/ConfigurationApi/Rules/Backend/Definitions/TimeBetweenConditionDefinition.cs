using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Definitions;

public class TimeBetweenConditionDefinition : IConditionDefinition
{
	public ConditionType ConditionType => ConditionType.TimeBetween;

	public IDictionary<string, RuleParameterDefinition> Parameters => CreateParameters();

	private static IDictionary<string, RuleParameterDefinition> CreateParameters()
	{
		return new RuleParameterDefinition[2]
		{
			new RuleParameterDefinition
			{
				Name = "StartTime",
				ValueType = typeof(TimeSpan),
				ValidatorType = typeof(TimeParameterValueValidator)
			},
			new RuleParameterDefinition
			{
				Name = "EndTime",
				ValueType = typeof(TimeSpan),
				ValidatorType = typeof(TimeParameterValueValidator)
			}
		}.ToDictionary((RuleParameterDefinition s) => s.Name);
	}
}
