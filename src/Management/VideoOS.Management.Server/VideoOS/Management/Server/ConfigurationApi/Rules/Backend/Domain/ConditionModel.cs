using System.Collections.Generic;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

public class ConditionModel
{
	public ConditionType ConditionType { get; set; }

	public IDictionary<string, RuleParameter> Parameters { get; set; }
}
