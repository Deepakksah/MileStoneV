using System.Collections.Concurrent;
using VideoOS.DataCollector.Service.Common;
using VideoOS.DataCollector.Service.Entities;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Refresh.Events.ActivePerformanceCountersRefreshed;

public class ActiveRefreshedEvent : BaseEvent
{
	public ConcurrentDictionary<string, PerformanceCounterCollector> Item { get; }

	public ActiveRefreshedEvent(ConcurrentDictionary<string, PerformanceCounterCollector> item)
	{
		Item = item;
	}
}
