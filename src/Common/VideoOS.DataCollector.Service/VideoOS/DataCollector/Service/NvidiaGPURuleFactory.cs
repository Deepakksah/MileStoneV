using Microsoft.Extensions.Options;
using VideoOS.DataCollector.Service.Configuration;
using VideoOS.DataCollector.Service.Entities.DataCollection;

namespace VideoOS.DataCollector.Service;

public class NvidiaGPURuleFactory : INvidiaGPURuleFactory
{
	private readonly IOptions<RuleOptions> _options;

	public NvidiaGPURuleFactory(IOptions<RuleOptions> options)
	{
		_options = options;
	}

	public NvidiaGPURule CreateGPURule(int dataCollectorId, CounterStateRuleConfiguration ruleConfig, IStateHandlerFactory stateFactory)
	{
		return new NvidiaGPURule(dataCollectorId, ruleConfig, stateFactory, _options);
	}
}
