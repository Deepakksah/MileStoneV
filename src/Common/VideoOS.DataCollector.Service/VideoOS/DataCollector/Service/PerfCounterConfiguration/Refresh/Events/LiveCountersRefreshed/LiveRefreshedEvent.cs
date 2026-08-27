using System.Collections.Concurrent;
using VideoOS.DataCollector.Service.Common;
using VideoOS.DataCollector.Service.Entities;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Refresh.Events.LiveCountersRefreshed;

public class LiveRefreshedEvent : BaseEvent
{
	public ConcurrentDictionary<string, PerformanceCounterCollector> Item { get; }

	public LiveRefreshedEvent(ConcurrentDictionary<string, PerformanceCounterCollector> item)
	{
		Item = item;
	}
}
