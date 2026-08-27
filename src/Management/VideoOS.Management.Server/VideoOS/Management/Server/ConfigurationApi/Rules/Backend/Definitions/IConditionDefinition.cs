using System.Collections.Generic;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Definitions;

public interface IConditionDefinition
{
	ConditionType ConditionType { get; }

	IDictionary<string, RuleParameterDefinition> Parameters { get; }
}
