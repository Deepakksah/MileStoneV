using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VideoOS.WebService.Common.DataCollectorService;

public class ManagementDataCollectorServiceClient : ClientBase<IDataCollection>, IDataCollection
{
	public ManagementDataCollectorServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public int RegisterDataCollector(string applicationId, string hostName)
	{
		return base.Channel.RegisterDataCollector(applicationId, hostName);
	}

	public int RegisterDataCollectorWithVersion(string applicationId, string hostName, int version)
	{
		return base.Channel.RegisterDataCollectorWithVersion(applicationId, hostName, version);
	}

	public int RegisterDataCollectorNewApplicationId(string applicationId, string oldApplicationId, string hostName, int version)
	{
		return base.Channel.RegisterDataCollectorNewApplicationId(applicationId, oldApplicationId, hostName, version);
	}

	public DataCollectorConfiguration GetDataCollectorConfiguration(int dataCollectorId)
	{
		return base.Channel.GetDataCollectorConfiguration(dataCollectorId);
	}

	public CounterStateRulesConfiguration GetCounterStateRulesConfiguration(int dataCollectorId)
	{
		return base.Channel.GetCounterStateRulesConfiguration(dataCollectorId);
	}

	public void AddPerformanceCounterData(PerformanceCounterData counterData)
	{
		base.Channel.AddPerformanceCounterData(counterData);
	}

	public void AddPerformanceCounterDataWithConfigRefresh(PerformanceCounterData counterData)
	{
		base.Channel.AddPerformanceCounterDataWithConfigRefresh(counterData);
	}

	public void MarkPerformanceCounterDataAsObsolete(string[] instances)
	{
		base.Channel.MarkPerformanceCounterDataAsObsolete(instances);
	}

	public ServiceApplication[] GetServiceApplicationList(int dataCollectorId)
	{
		return base.Channel.GetServiceApplicationList(dataCollectorId);
	}

	public void RegisterServiceApplication(int dataCollectorId, ServiceApplication[] serviceApplications)
	{
		base.Channel.RegisterServiceApplication(dataCollectorId, serviceApplications);
	}

	public void UpdateMachineProperties(int dataCollectorId, MachineProperty[] machineProperties)
	{
		base.Channel.UpdateMachineProperties(dataCollectorId, machineProperties);
	}

	public void RegisterStateChangeData(CounterStateChangeData data)
	{
		base.Channel.RegisterStateChangeData(data);
	}

	public void DeleteStateData(CounterStateChangeData data)
	{
		base.Channel.DeleteStateData(data);
	}

	public void DeleteStateDataFromDataCollector(int dataCollectorId, string[] activePaths)
	{
		base.Channel.DeleteStateDataFromDataCollector(dataCollectorId, activePaths);
	}

	public HistoricSample[] GetHistoricSamples(HistoricSampleRequestData data)
	{
		return base.Channel.GetHistoricSamples(data);
	}
}
