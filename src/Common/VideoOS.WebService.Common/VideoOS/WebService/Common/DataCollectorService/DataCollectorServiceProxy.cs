using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.WebService.Common.DataCollectorService;

public sealed class DataCollectorServiceProxy : WebServiceManagerBase<ManagementDataCollectorServiceClient, IDataCollection>, IDataCollection
{
	private readonly object _locker;

	public static DataCollectorServiceProxy CreateNew(string hostName, int port, bool runSecure, ITokenCache tokenCache, IWebServiceManagerExceptionHandler exceptionHandler = null)
	{
		return new DataCollectorServiceProxy(hostName, port, runSecure, tokenCache, exceptionHandler);
	}

	private DataCollectorServiceProxy(string hostName, int port, bool runSecure, ITokenCache tokenCache, IWebServiceManagerExceptionHandler exceptionHandler)
		: base(new UriBuilder(runSecure ? Uri.UriSchemeHttps : Uri.UriSchemeHttp, hostName, port, "ManagementServer/DataCollectorServiceOAuth.svc").Uri, "http://schemas.videoos/DataCollectorServerService/2020/03/Vmo/01", tokenCache, exceptionHandler)
	{
		_locker = new object();
	}

	protected override ManagementDataCollectorServiceClient CreateClientObject(Binding binding, EndpointAddress remoteAddress)
	{
		return new ManagementDataCollectorServiceClient(binding, remoteAddress);
	}

	public int RegisterDataCollector(string applicationId, string hostName)
	{
		lock (_locker)
		{
			return CallWithRetry((IDataCollection c) => c.RegisterDataCollector(applicationId, hostName));
		}
	}

	public int RegisterDataCollectorWithVersion(string applicationId, string hostName, int version)
	{
		lock (_locker)
		{
			return CallWithRetry((IDataCollection c) => c.RegisterDataCollectorWithVersion(applicationId, hostName, version));
		}
	}

	public int RegisterDataCollectorNewApplicationId(string applicationId, string oldApplicationId, string hostName, int version)
	{
		lock (_locker)
		{
			return CallWithRetry((IDataCollection c) => c.RegisterDataCollectorNewApplicationId(applicationId, oldApplicationId, hostName, version));
		}
	}

	public DataCollectorConfiguration GetDataCollectorConfiguration(int dataCollectorId)
	{
		lock (_locker)
		{
			return CallWithRetry((IDataCollection c) => c.GetDataCollectorConfiguration(dataCollectorId));
		}
	}

	public CounterStateRulesConfiguration GetCounterStateRulesConfiguration(int dataCollectorId)
	{
		lock (_locker)
		{
			return CallWithRetry((IDataCollection c) => c.GetCounterStateRulesConfiguration(dataCollectorId));
		}
	}

	public void AddPerformanceCounterData(PerformanceCounterData counterData)
	{
		lock (_locker)
		{
			CallWithRetry(delegate(IDataCollection c)
			{
				c.AddPerformanceCounterData(counterData);
			});
		}
	}

	public void AddPerformanceCounterDataWithConfigRefresh(PerformanceCounterData counterData)
	{
		lock (_locker)
		{
			CallWithRetry(delegate(IDataCollection c)
			{
				c.AddPerformanceCounterDataWithConfigRefresh(counterData);
			});
		}
	}

	public void MarkPerformanceCounterDataAsObsolete(string[] instances)
	{
		lock (_locker)
		{
			CallWithRetry(delegate(IDataCollection c)
			{
				c.MarkPerformanceCounterDataAsObsolete(instances);
			});
		}
	}

	public ServiceApplication[] GetServiceApplicationList(int dataCollectorId)
	{
		lock (_locker)
		{
			return CallWithRetry((IDataCollection c) => c.GetServiceApplicationList(dataCollectorId));
		}
	}

	public void RegisterServiceApplication(int dataCollectorId, ServiceApplication[] serviceApplications)
	{
		lock (_locker)
		{
			CallWithRetry(delegate(IDataCollection c)
			{
				c.RegisterServiceApplication(dataCollectorId, serviceApplications);
			});
		}
	}

	public void UpdateMachineProperties(int dataCollectorId, MachineProperty[] machineProperties)
	{
		lock (_locker)
		{
			CallWithRetry(delegate(IDataCollection c)
			{
				c.UpdateMachineProperties(dataCollectorId, machineProperties);
			});
		}
	}

	public HistoricSample[] GetHistoricSamples(HistoricSampleRequestData data)
	{
		lock (_locker)
		{
			return CallWithRetry((IDataCollection c) => c.GetHistoricSamples(data));
		}
	}

	public void RegisterStateChangeData(CounterStateChangeData data)
	{
		lock (_locker)
		{
			CallWithRetry(delegate(IDataCollection c)
			{
				c.RegisterStateChangeData(data);
			});
		}
	}

	public void DeleteStateData(CounterStateChangeData data)
	{
		lock (_locker)
		{
			CallWithRetry(delegate(IDataCollection c)
			{
				c.DeleteStateData(data);
			});
		}
	}

	public void DeleteStateDataFromDataCollector(int dataCollectorId, string[] activePaths)
	{
		lock (_locker)
		{
			CallWithRetry(delegate(IDataCollection c)
			{
				c.DeleteStateDataFromDataCollector(dataCollectorId, activePaths);
			});
		}
	}
}
