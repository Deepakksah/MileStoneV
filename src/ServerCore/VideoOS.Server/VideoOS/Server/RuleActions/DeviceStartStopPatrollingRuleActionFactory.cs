using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.DeviceStartStopPatrolling", "AD0BBA03-E87B-49E6-A686-B008525EF9B6", ActionTargetType.Device, "DeviceId")]
public sealed class DeviceStartStopPatrollingRuleActionFactory : GenericRecorderOnlyRuleActionFactory
{
	public DeviceStartStopPatrollingRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}
}
