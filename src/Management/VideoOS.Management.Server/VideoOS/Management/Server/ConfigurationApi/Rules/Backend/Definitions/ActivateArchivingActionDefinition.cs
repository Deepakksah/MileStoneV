using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Definitions;

public class ActivateArchivingActionDefinition : IActionDefinition
{
	public string ActionType => VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain.ActionType.ActivateArchiving.ToString();

	public IDictionary<string, RuleParameterDefinition> Parameters => CreateParameters();

	public string CounteractionActionType => VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain.ActionType.None.ToString();

	public string DisplayName => ConfigurationApiResources.RuleActionActivateArchiving;

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
				Name = "ArchiveIds",
				ValueType = typeof(Guid[]),
				SpecialPropertyType = "PathList",
				AllowedItemTypes = new string[1] { "ArchiveStorage" },
				ValidatorType = typeof(ArchiveParameterValueValidator)
			}
		}.ToDictionary((RuleParameterDefinition s) => s.Name);
	}
}
