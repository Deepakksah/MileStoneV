using System.Collections.Concurrent;
using VideoOS.DataCollector.Service.Entities;

namespace VideoOS.DataCollector.Service.Persistence;

public class ActivePerformanceCounterRepository : IActivePerformanceCounterRepository
{
	public ConcurrentDictionary<string, PerformanceCounterCollector> Counters { get; } = new ConcurrentDictionary<string, PerformanceCounterCollector>();
}
