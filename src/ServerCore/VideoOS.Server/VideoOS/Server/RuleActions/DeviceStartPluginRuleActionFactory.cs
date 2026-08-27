using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.DeviceStartPlugin", "416888E8-E7B1-465e-999A-730FF3E0BBF4", ActionTargetType.Device, "DeviceIds", "DeviceGroupIds")]
public sealed class DeviceStartPluginRuleActionFactory : GenericRecorderOnlyRuleActionFactory
{
	public DeviceStartPluginRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}
}
