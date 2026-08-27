using System.Collections.Generic;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

public class ActionModel
{
	public string ActionType { get; set; }

	public IDictionary<string, RuleParameter> Parameters { get; set; }
}
