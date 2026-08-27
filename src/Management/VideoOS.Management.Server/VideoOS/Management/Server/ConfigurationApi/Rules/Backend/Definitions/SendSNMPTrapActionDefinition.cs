using System.Collections.Generic;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Definitions;

public class SendSNMPTrapActionDefinition : IActionDefinition
{
	public string ActionType => VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain.ActionType.SendSNMPTrap.ToString();

	public IDictionary<string, RuleParameterDefinition> Parameters => new Dictionary<string, RuleParameterDefinition>();

	public string CounteractionActionType => VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain.ActionType.None.ToString();

	public string DisplayName => ConfigurationApiResources.RuleActionSNMPTrap;

	public ActionPosition Position => ActionPosition.Start;

	public IEnumerable<string> RequiredFeatures => new string[1] { "SNMP" };

	public IEnumerable<TriggerType> ValidForTriggerTypes => new TriggerType[1] { TriggerType.Event };
}
