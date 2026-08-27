using System.Collections.Generic;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

public class FrequencyModel
{
	public FrequencyType FrequencyType { get; set; }

	public int Frequency { get; set; }

	public Dictionary<string, RuleParameter> Parameters { get; } = new Dictionary<string, RuleParameter>();
}
