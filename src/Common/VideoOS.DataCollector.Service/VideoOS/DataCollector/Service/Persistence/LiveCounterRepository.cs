using System.Collections.Concurrent;
using VideoOS.DataCollector.Service.Entities;

namespace VideoOS.DataCollector.Service.Persistence;

public class LiveCounterRepository : ILiveCounterRepository
{
	public ConcurrentDictionary<string, PerformanceCounterCollector> Counters { get; } = new ConcurrentDictionary<string, PerformanceCounterCollector>();
}
