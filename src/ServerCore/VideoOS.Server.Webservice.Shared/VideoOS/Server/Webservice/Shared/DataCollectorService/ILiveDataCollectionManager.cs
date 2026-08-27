using System;
using System.Threading.Tasks;
using VideoOS.WebService.Common.DataCollectorService;

namespace VideoOS.Server.Webservice.Shared.DataCollectorService;

public interface ILiveDataCollectionManager : IDisposable
{
	Task<int> GetDataCollectorId();

	string[] GetPerformanceCategories();

	PerformanceCounterValue[] GetPerformanceCounterValues(string categoryName, string counterName, string instanceName, int sampleFrequency);

	void PostUpdatedCounterStateRuleConfigurations(CounterStateRulesConfigurationUpdate[] ruleConfigUpdates);

	Task<bool> IsAlive();
}
