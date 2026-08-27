using VideoOS.DataCollector.Service.Entities.DataCollection;

namespace VideoOS.DataCollector.Service;

public interface ICounterRuleFactory
{
	CounterRule CreateCounterRule(int datacollectoId, CounterStateRuleConfiguration ruleConfiguration, bool isDefault, bool triggerPerValue, bool counterDirection, IStateHandlerFactory stateHandlerFactory);

	bool ValidateCounter(CounterStateRuleConfiguration ruleConfiguration, string counterPath);
}
