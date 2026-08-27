using System.Collections.Concurrent;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Refresh.EventHandlers.RefreshActivePerformanceCounters;

public class WarningLoggedStateService : IWarningLoggedStateService
{
	private static readonly ConcurrentDictionary<string, bool> WarningLogged = new ConcurrentDictionary<string, bool>();

	public bool IsLogged(string subject, bool value)
	{
		if (!WarningLogged.TryGetValue(subject, out var value2))
		{
			WarningLogged.TryAdd(subject, value);
		}
		return value2;
	}

	public void Add(string subject, bool value)
	{
		WarningLogged.TryAdd(subject, value);
	}
}
