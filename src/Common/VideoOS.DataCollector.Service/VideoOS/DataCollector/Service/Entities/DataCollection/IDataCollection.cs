namespace VideoOS.DataCollector.Service.Entities.DataCollection;

public interface IDataCollection
{
	int RegisterDataCollector(string applicationId, string hostName);

	int RegisterDataCollectorWithVersion(string applicationId, string hostName, int version);

	int RegisterDataCollectorNewApplicationId(string applicationId, string oldApplicationId, string hostName, int version);

	DataCollectorConfiguration GetDataCollectorConfiguration(int dataCollectorId);

	CounterStateRulesConfiguration GetCounterStateRulesConfiguration(int dataCollectorId);

	void AddPerformanceCounterData(PerformanceCounterData counterData);

	void AddPerformanceCounterDataWithConfigRefresh(PerformanceCounterData counterData);

	void MarkPerformanceCounterDataAsObsolete(string[] instances);

	ServiceApplication[] GetServiceApplicationList(int dataCollectorId);

	void RegisterServiceApplication(int dataCollectorId, ServiceApplication[] serviceApplications);

	void UpdateMachineProperties(int dataCollectorId, MachineProperty[] machineProperties);

	void RegisterStateChangeData(CounterStateChangeData data);

	void DeleteStateData(CounterStateChangeData data);

	void DeleteStateDataFromDataCollector(int dataCollectorId, string[] activePaths);

	HistoricSample[] GetHistoricSamples(HistoricSampleRequestData data);
}
