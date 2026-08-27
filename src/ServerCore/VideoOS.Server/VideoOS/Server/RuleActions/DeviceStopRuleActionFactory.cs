using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.DeviceStop", "182FB842-A049-473F-9AAA-B3C087A31BC0", ActionTargetType.Device, "DeviceIds", "DeviceGroupIds")]
public sealed class DeviceStopRuleActionFactory : GenericRecorderOnlyRuleActionFactory
{
	public DeviceStopRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}
}
