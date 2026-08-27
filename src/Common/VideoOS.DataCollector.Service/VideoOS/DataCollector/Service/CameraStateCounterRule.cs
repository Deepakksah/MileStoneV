using Microsoft.Extensions.Options;
using VideoOS.DataCollector.Service.Configuration;
using VideoOS.DataCollector.Service.Entities.DataCollection;
using VideoOS.DataCollector.Service.Rule.Entities.State;

namespace VideoOS.DataCollector.Service;

public class CameraStateCounterRule : CameraCounterRule
{
	public override StateChange RecorderDownState => new StateChange(State.Critical, 7f);

	public CameraStateCounterRule(int datacollectoId, CounterStateRuleConfiguration ruleConfiguration, bool isDefault, bool triggerPerValue, bool counterDirection, IStateHandlerFactory stateHandlerFactory, IOptions<RuleOptions> options)
		: base(datacollectoId, ruleConfiguration, isDefault, triggerPerValue, counterDirection, stateHandlerFactory, options)
	{
	}
}
