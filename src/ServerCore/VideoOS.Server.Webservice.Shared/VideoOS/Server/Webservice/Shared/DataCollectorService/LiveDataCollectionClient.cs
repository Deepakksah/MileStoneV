using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using VideoOS.WebService.Common.DataCollectorService;

namespace VideoOS.Server.Webservice.Shared.DataCollectorService;

public class LiveDataCollectionClient : ClientBase<ILiveDataCollection>, ILiveDataCollection
{
	public LiveDataCollectionClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public string[] GetCategories()
	{
		return base.Channel.GetPerformanceCategories();
	}

	public PerformanceCounterInstance[] GetPerformanceCounterInstances(string categoryName)
	{
		return base.Channel.GetPerformanceCounterInstances(categoryName);
	}

	public PerformanceCounterValue[] GetPerformanceCounterValues(string categoryName, string counterName, string instanceName, int sampleFrequency)
	{
		return base.Channel.GetPerformanceCounterValues(categoryName, counterName, instanceName, sampleFrequency);
	}

	public void PostUpdatedCounterStateRuleConfigurations(CounterStateRulesConfigurationUpdate[] ruleConfigUpdates)
	{
		base.Channel.PostUpdatedCounterStateRuleConfigurations(ruleConfigUpdates);
	}

	public string[] GetPerformanceCategories()
	{
		return base.Channel.GetPerformanceCategories();
	}

	public Task<int> GetDataCollectorId()
	{
		return base.Channel.GetDataCollectorId();
	}

	public Task<bool> IsAlive()
	{
		return base.Channel.IsAlive();
	}
}
