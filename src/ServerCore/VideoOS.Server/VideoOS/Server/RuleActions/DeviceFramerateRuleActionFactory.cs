using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.DeviceFramerate", "9B37CEB2-58FC-4960-A716-B2F1527C7E38", ActionTargetType.Device, "DeviceIds", "DeviceGroupIds")]
public sealed class DeviceFramerateRuleActionFactory : GenericRecorderOnlyRuleActionFactory
{
	public DeviceFramerateRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}
}
