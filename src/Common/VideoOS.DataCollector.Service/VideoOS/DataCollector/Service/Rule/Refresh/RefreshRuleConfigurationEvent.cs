using VideoOS.DataCollector.Service.Common;
using VideoOS.DataCollector.Service.Entities.DataCollection;

namespace VideoOS.DataCollector.Service.Rule.Refresh;

public class RefreshRuleConfigurationEvent : BaseEvent
{
	public CounterStateRulesConfiguration Item { get; }

	public RefreshRuleConfigurationEvent(CounterStateRulesConfiguration item)
	{
		Item = item;
	}
}
