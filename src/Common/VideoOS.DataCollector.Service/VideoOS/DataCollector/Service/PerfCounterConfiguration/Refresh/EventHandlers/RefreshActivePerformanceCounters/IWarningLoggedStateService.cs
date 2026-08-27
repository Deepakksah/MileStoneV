namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Refresh.EventHandlers.RefreshActivePerformanceCounters;

public interface IWarningLoggedStateService
{
	bool IsLogged(string categoryName, bool value);

	void Add(string subject, bool value);
}
