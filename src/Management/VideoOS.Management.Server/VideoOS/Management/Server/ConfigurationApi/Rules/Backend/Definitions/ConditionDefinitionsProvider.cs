using System.Collections.Generic;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Definitions;

public class ConditionDefinitionsProvider
{
	public IDictionary<ConditionType, IConditionDefinition> Definitions { get; } = new Dictionary<ConditionType, IConditionDefinition>();

	public ConditionDefinitionsProvider()
	{
		AddDefinitions();
	}

	private void AddDefinitions()
	{
		Definitions[ConditionType.WithinTimeProfile] = new WithinTimeProfileConditionDefinition();
		Definitions[ConditionType.OutsideTimeProfile] = new OutsideTimeProfileConditionDefinition();
		Definitions[ConditionType.TimeBetween] = new TimeBetweenConditionDefinition();
		Definitions[ConditionType.DaysOfWeek] = new DaysOfWeekConditionDefinition();
		Definitions[ConditionType.WithinWindow] = new WithinWindowConditionDefinition();
		Definitions[ConditionType.FailoverActive] = new FailoverActiveConditionDefinition();
		Definitions[ConditionType.FailoverInactive] = new FailoverInactiveConditionDefinition();
	}

	public IConditionDefinition Get(ConditionType key)
	{
		if (!Definitions.TryGetValue(key, out var value))
		{
			return null;
		}
		return value;
	}
}
