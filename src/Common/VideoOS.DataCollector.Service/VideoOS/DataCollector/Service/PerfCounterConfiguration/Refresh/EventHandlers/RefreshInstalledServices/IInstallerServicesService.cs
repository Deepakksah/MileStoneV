using System.Collections.Concurrent;
using System.Collections.Generic;
using VideoOS.DataCollector.Service.Entities.DataCollection;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Refresh.EventHandlers.RefreshInstalledServices;

public interface IInstallerServicesService
{
	List<PerformanceCounterValue> UpdateInstallerServices(ConcurrentDictionary<string, string> installedServices);
}
