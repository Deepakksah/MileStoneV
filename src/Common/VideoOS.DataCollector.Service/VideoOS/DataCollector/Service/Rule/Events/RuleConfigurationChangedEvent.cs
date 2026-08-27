using VideoOS.DataCollector.Service.Common;
using VideoOS.DataCollector.Service.Entities.DataCollection;

namespace VideoOS.DataCollector.Service.Rule.Events;

public class RuleConfigurationChangedEvent : BaseEvent
{
	public CounterStateRulesConfigurationUpdate[] RuleConfigUpdates { get; }

	public RuleConfigurationChangedEvent(CounterStateRulesConfigurationUpdate[] ruleConfigUpdates)
	{
		RuleConfigUpdates = ruleConfigUpdates;
	}
}
