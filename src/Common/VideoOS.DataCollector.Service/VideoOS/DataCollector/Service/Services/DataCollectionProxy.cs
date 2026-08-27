using AutoMapper;
using VideoOS.DataCollector.Service.Entities.DataCollection;
using VideoOS.WebService.Common.DataCollectorService;

namespace VideoOS.DataCollector.Service.Services;

public class DataCollectionProxy : VideoOS.DataCollector.Service.Entities.DataCollection.IDataCollection
{
	private readonly VideoOS.WebService.Common.DataCollectorService.IDataCollection _proxy;

	private readonly IMapper _mapper;

	public DataCollectionProxy(VideoOS.WebService.Common.DataCollectorService.IDataCollection proxy, IMapper mapper)
	{
		_proxy = proxy;
		_mapper = mapper;
	}

	public void AddPerformanceCounterData(VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterData counterData)
	{
		VideoOS.WebService.Common.DataCollectorService.PerformanceCounterData counterData2 = _mapper.Map<VideoOS.WebService.Common.DataCollectorService.PerformanceCounterData>(counterData);
		_proxy.AddPerformanceCounterData(counterData2);
	}

	public void AddPerformanceCounterDataWithConfigRefresh(VideoOS.DataCollector.Service.Entities.DataCollection.PerformanceCounterData counterData)
	{
		VideoOS.WebService.Common.DataCollectorService.PerformanceCounterData counterData2 = _mapper.Map<VideoOS.WebService.Common.DataCollectorService.PerformanceCounterData>(counterData);
		_proxy.AddPerformanceCounterDataWithConfigRefresh(counterData2);
	}

	public void DeleteStateData(VideoOS.DataCollector.Service.Entities.DataCollection.CounterStateChangeData data)
	{
		VideoOS.WebService.Common.DataCollectorService.CounterStateChangeData data2 = _mapper.Map<VideoOS.WebService.Common.DataCollectorService.CounterStateChangeData>(data);
		_proxy.DeleteStateData(data2);
	}

	public void DeleteStateDataFromDataCollector(int dataCollectorId, string[] activePaths)
	{
		_proxy.DeleteStateDataFromDataCollector(dataCollectorId, activePaths);
	}

	public VideoOS.DataCollector.Service.Entities.DataCollection.CounterStateRulesConfiguration GetCounterStateRulesConfiguration(int dataCollectorId)
	{
		VideoOS.WebService.Common.DataCollectorService.CounterStateRulesConfiguration counterStateRulesConfiguration = _proxy.GetCounterStateRulesConfiguration(dataCollectorId);
		return _mapper.Map<VideoOS.DataCollector.Service.Entities.DataCollection.CounterStateRulesConfiguration>(counterStateRulesConfiguration);
	}

	public VideoOS.DataCollector.Service.Entities.DataCollection.DataCollectorConfiguration GetDataCollectorConfiguration(int dataCollectorId)
	{
		VideoOS.WebService.Common.DataCollectorService.DataCollectorConfiguration dataCollectorConfiguration = _proxy.GetDataCollectorConfiguration(dataCollectorId);
		return _mapper.Map<VideoOS.DataCollector.Service.Entities.DataCollection.DataCollectorConfiguration>(dataCollectorConfiguration);
	}

	public VideoOS.DataCollector.Service.Entities.DataCollection.HistoricSample[] GetHistoricSamples(VideoOS.DataCollector.Service.Entities.DataCollection.HistoricSampleRequestData data)
	{
		VideoOS.WebService.Common.DataCollectorService.HistoricSampleRequestData data2 = _mapper.Map<VideoOS.WebService.Common.DataCollectorService.HistoricSampleRequestData>(data);
		VideoOS.WebService.Common.DataCollectorService.HistoricSample[] historicSamples = _proxy.GetHistoricSamples(data2);
		return _mapper.Map<VideoOS.DataCollector.Service.Entities.DataCollection.HistoricSample[]>(historicSamples);
	}

	public VideoOS.DataCollector.Service.Entities.DataCollection.ServiceApplication[] GetServiceApplicationList(int dataCollectorId)
	{
		VideoOS.WebService.Common.DataCollectorService.ServiceApplication[] serviceApplicationList = _proxy.GetServiceApplicationList(dataCollectorId);
		return _mapper.Map<VideoOS.DataCollector.Service.Entities.DataCollection.ServiceApplication[]>(serviceApplicationList);
	}

	public void MarkPerformanceCounterDataAsObsolete(string[] instances)
	{
		_proxy.MarkPerformanceCounterDataAsObsolete(instances);
	}

	public int RegisterDataCollector(string applicationId, string hostName)
	{
		return _proxy.RegisterDataCollector(applicationId, hostName);
	}

	public int RegisterDataCollectorNewApplicationId(string applicationId, string oldApplicationId, string hostName, int version)
	{
		return _proxy.RegisterDataCollectorNewApplicationId(applicationId, oldApplicationId, hostName, version);
	}

	public int RegisterDataCollectorWithVersion(string applicationId, string hostName, int version)
	{
		return _proxy.RegisterDataCollectorWithVersion(applicationId, hostName, version);
	}

	public void RegisterServiceApplication(int dataCollectorId, VideoOS.DataCollector.Service.Entities.DataCollection.ServiceApplication[] serviceApplications)
	{
		VideoOS.WebService.Common.DataCollectorService.ServiceApplication[] serviceApplications2 = _mapper.Map<VideoOS.WebService.Common.DataCollectorService.ServiceApplication[]>(serviceApplications);
		_proxy.RegisterServiceApplication(dataCollectorId, serviceApplications2);
	}

	public void RegisterStateChangeData(VideoOS.DataCollector.Service.Entities.DataCollection.CounterStateChangeData data)
	{
		VideoOS.WebService.Common.DataCollectorService.CounterStateChangeData data2 = _mapper.Map<VideoOS.WebService.Common.DataCollectorService.CounterStateChangeData>(data);
		_proxy.RegisterStateChangeData(data2);
	}

	public void UpdateMachineProperties(int dataCollectorId, VideoOS.DataCollector.Service.Entities.DataCollection.MachineProperty[] machineProperties)
	{
		VideoOS.WebService.Common.DataCollectorService.MachineProperty[] machineProperties2 = _mapper.Map<VideoOS.WebService.Common.DataCollectorService.MachineProperty[]>(machineProperties);
		_proxy.UpdateMachineProperties(dataCollectorId, machineProperties2);
	}
}
