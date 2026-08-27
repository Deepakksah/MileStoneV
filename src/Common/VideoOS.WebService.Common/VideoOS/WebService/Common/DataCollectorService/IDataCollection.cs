using System.ServiceModel;

namespace VideoOS.WebService.Common.DataCollectorService;

[ServiceContract(Namespace = "http://schemas.videoos/DataCollectorServerService/2020/03/Vmo/01")]
public interface IDataCollection
{
	[OperationContract]
	int RegisterDataCollector(string applicationId, string hostName);

	[OperationContract]
	int RegisterDataCollectorWithVersion(string applicationId, string hostName, int version);

	[OperationContract]
	int RegisterDataCollectorNewApplicationId(string applicationId, string oldApplicationId, string hostName, int version);

	[OperationContract]
	DataCollectorConfiguration GetDataCollectorConfiguration(int dataCollectorId);

	[OperationContract]
	CounterStateRulesConfiguration GetCounterStateRulesConfiguration(int dataCollectorId);

	[OperationContract]
	void AddPerformanceCounterData(PerformanceCounterData counterData);

	[OperationContract]
	void AddPerformanceCounterDataWithConfigRefresh(PerformanceCounterData counterData);

	[OperationContract]
	void MarkPerformanceCounterDataAsObsolete(string[] instances);

	[OperationContract]
	ServiceApplication[] GetServiceApplicationList(int dataCollectorId);

	[OperationContract]
	void RegisterServiceApplication(int dataCollectorId, ServiceApplication[] serviceApplications);

	[OperationContract]
	void UpdateMachineProperties(int dataCollectorId, MachineProperty[] machineProperties);

	[OperationContract]
	void RegisterStateChangeData(CounterStateChangeData data);

	[OperationContract]
	void DeleteStateData(CounterStateChangeData data);

	[OperationContract]
	void DeleteStateDataFromDataCollector(int dataCollectorId, string[] activePaths);

	[OperationContract]
	HistoricSample[] GetHistoricSamples(HistoricSampleRequestData data);
}
