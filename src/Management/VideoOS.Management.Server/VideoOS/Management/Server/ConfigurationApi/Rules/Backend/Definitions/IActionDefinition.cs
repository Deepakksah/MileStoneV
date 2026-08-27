using System.Collections.Generic;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Definitions;

public interface IActionDefinition
{
	string ActionType { get; }

	IDictionary<string, RuleParameterDefinition> Parameters { get; }

	string CounteractionActionType { get; }

	string DisplayName { get; }

	ActionPosition Position { get; }

	IEnumerable<string> RequiredFeatures { get; }

	IEnumerable<TriggerType> ValidForTriggerTypes { get; }
}
