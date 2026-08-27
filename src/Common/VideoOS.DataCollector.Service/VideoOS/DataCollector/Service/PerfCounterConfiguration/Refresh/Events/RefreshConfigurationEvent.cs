using VideoOS.DataCollector.Service.Common;
using VideoOS.DataCollector.Service.Entities.DataCollection;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Refresh.Events;

public class RefreshConfigurationEvent : BaseEvent
{
	public DataCollectorConfiguration Item { get; }

	public RefreshConfigurationEvent(DataCollectorConfiguration item)
	{
		Item = item;
	}
}
