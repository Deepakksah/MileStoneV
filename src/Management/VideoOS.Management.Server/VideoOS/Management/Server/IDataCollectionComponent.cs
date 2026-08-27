using System;
using VideoOS.WebService.Common.DataCollectorService;

namespace VideoOS.Management.Server;

public interface IDataCollectionComponent : IDisposable
{
	void AddPerformanceCounterData(PerformanceCounterData counterData);

	DataCollectorConfiguration GetDataCollectorConfiguration(int dataCollectorId);

	void MarkPerformanceCounterDataAsObsolete(string[] instances);

	ServiceApplication[] GetServiceApplicationList(int dataCollectorId);

	int RegisterDataCollector(string applicationId, string hostName, int version);

	int RegisterDataCollectorNewApplicationId(string applicationId, string oldApplicationId, string hostName, int version);

	void RegisterServiceApplications(int dataCollectorId, ServiceApplication[] serviceApplications);

	void UpdateMachineProperties(int dataCollectorId, MachineProperty[] machineProperties);
}
