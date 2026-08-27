using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.DeviceStopPlugin", "B3366D2A-ADD8-49ef-8019-209B698281D1", ActionTargetType.Device, "DeviceIds", "DeviceGroupIds")]
public sealed class DeviceStopPluginRuleActionFactory : GenericRecorderOnlyRuleActionFactory
{
	public DeviceStopPluginRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}
}
