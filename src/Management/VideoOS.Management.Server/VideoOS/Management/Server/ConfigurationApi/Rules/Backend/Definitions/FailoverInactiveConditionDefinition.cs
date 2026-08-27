using System.Collections.Generic;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Definitions;

public class FailoverInactiveConditionDefinition : IConditionDefinition
{
	public ConditionType ConditionType => ConditionType.FailoverInactive;

	public IDictionary<string, RuleParameterDefinition> Parameters => new Dictionary<string, RuleParameterDefinition>();
}
