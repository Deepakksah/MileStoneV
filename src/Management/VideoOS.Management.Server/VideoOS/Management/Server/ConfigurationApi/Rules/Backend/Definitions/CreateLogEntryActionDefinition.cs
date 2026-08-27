using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Definitions;

public class CreateLogEntryActionDefinition : IActionDefinition
{
	public string ActionType => VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain.ActionType.CreateLogEntry.ToString();

	public IDictionary<string, RuleParameterDefinition> Parameters => CreateParameters();

	public string CounteractionActionType => VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain.ActionType.None.ToString();

	public string DisplayName => ConfigurationApiResources.RuleActionLogEntry;

	public ActionPosition Position => ActionPosition.Start;

	public IEnumerable<string> RequiredFeatures => new string[0];

	public IEnumerable<TriggerType> ValidForTriggerTypes => new TriggerType[3]
	{
		TriggerType.Event,
		TriggerType.TimeInterval,
		TriggerType.RecurringTime
	};

	private static IDictionary<string, RuleParameterDefinition> CreateParameters()
	{
		return new RuleParameterDefinition[1]
		{
			new RuleParameterDefinition
			{
				Name = "Text",
				ValueType = typeof(string)
			}
		}.ToDictionary((RuleParameterDefinition s) => s.Name);
	}
}
