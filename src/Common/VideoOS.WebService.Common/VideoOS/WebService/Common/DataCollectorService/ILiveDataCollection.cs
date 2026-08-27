using System.ServiceModel;
using System.Threading.Tasks;

namespace VideoOS.WebService.Common.DataCollectorService;

[ServiceContract(Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
public interface ILiveDataCollection
{
	[OperationContract]
	string[] GetPerformanceCategories();

	[OperationContract]
	PerformanceCounterInstance[] GetPerformanceCounterInstances(string categoryName);

	[OperationContract]
	PerformanceCounterValue[] GetPerformanceCounterValues(string categoryName, string counterName, string instanceName, int sampleFrequency);

	[OperationContract]
	void PostUpdatedCounterStateRuleConfigurations(CounterStateRulesConfigurationUpdate[] ruleConfigUpdates);

	[OperationContract]
	Task<int> GetDataCollectorId();

	[OperationContract]
	Task<bool> IsAlive();
}
