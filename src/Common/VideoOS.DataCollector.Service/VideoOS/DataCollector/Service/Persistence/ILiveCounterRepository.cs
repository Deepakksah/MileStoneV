using System.Collections.Concurrent;
using VideoOS.DataCollector.Service.Entities;

namespace VideoOS.DataCollector.Service.Persistence;

public interface ILiveCounterRepository
{
	ConcurrentDictionary<string, PerformanceCounterCollector> Counters { get; }
}
