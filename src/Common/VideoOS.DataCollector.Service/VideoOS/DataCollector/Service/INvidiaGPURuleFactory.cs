using VideoOS.DataCollector.Service.Entities.DataCollection;

namespace VideoOS.DataCollector.Service;

public interface INvidiaGPURuleFactory
{
	NvidiaGPURule CreateGPURule(int dataCollectorId, CounterStateRuleConfiguration ruleConfig, IStateHandlerFactory stateFactory);
}
