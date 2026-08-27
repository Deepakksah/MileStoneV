using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Definitions;

public class WithinTimeProfileConditionDefinition : IConditionDefinition
{
	public ConditionType ConditionType => ConditionType.WithinTimeProfile;

	public IDictionary<string, RuleParameterDefinition> Parameters => CreateParameters();

	private static IDictionary<string, RuleParameterDefinition> CreateParameters()
	{
		return new RuleParameterDefinition[1]
		{
			new RuleParameterDefinition
			{
				Name = "WithinTimeProfile",
				ValueType = typeof(Guid),
				AllowedItemTypes = new string[1] { "TimeProfile" },
				ValidatorType = typeof(TimeProfileParameterValueValidator)
			}
		}.ToDictionary((RuleParameterDefinition s) => s.Name);
	}
}
