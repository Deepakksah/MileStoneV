using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.DevicePauseResumePatrolling", "DDA4E41F-1CBC-4C55-AE44-1A8A73E40CAC", ActionTargetType.Device, "DeviceIds", "DeviceGroupIds")]
public sealed class DevicePauseResumePatrollingRuleActionFactory : GenericRecorderOnlyRuleActionFactory
{
	public DevicePauseResumePatrollingRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}
}
