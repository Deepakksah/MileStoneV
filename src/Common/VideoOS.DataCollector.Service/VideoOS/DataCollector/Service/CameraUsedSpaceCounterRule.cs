using Microsoft.Extensions.Options;
using VideoOS.DataCollector.Service.Configuration;
using VideoOS.DataCollector.Service.Entities.DataCollection;
using VideoOS.DataCollector.Service.Rule.Entities.State;

namespace VideoOS.DataCollector.Service;

public class CameraUsedSpaceCounterRule : CameraCounterRule
{
	public override StateChange RecorderDownState => new StateChange(State.Undefined, -2.1474836E+09f);

	public CameraUsedSpaceCounterRule(int dataCollectorId, CounterStateRuleConfiguration ruleConfiguration, bool isDefault, bool triggerPerValue, bool counterDirection, IStateHandlerFactory stateHandlerFactory, IOptions<RuleOptions> options)
		: base(dataCollectorId, ruleConfiguration, isDefault, triggerPerValue, counterDirection, stateHandlerFactory, options)
	{
	}
}
