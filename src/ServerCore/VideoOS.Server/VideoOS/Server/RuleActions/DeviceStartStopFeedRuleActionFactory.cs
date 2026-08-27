using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.DeviceStartStopFeed", "2E0DACDD-7457-4a99-AC68-1F2298DE57ED", ActionTargetType.Device, "DeviceIds", "DeviceGroupIds")]
public sealed class DeviceStartStopFeedRuleActionFactory : GenericRecorderOnlyRuleActionFactory
{
	public DeviceStartStopFeedRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}
}
