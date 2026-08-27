using System.Collections.Generic;
using System.Diagnostics;
using VideoOS.DataCollector.Service.Entities.DataCollection;

namespace VideoOS.DataCollector.Service.Services;

public interface IPerformanceCounterInstancesService
{
	Dictionary<string, PerformanceCounterInstance> GetConfiguredPerformanceCounterInstances(PerformanceCounterCategory category, PerformanceCounterConfiguration counterConfiguration);

	string[] GetCategoryNames();

	PerformanceCounterInstance[] GetPerformanceCounterInstances(string categoryName);
}
