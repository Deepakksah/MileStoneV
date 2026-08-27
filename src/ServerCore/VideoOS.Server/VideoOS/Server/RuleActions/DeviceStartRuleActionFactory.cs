using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.DeviceStart", "19081FB0-FAF9-45f8-96FF-6A9B4D26B442", ActionTargetType.Device, "DeviceIds", "DeviceGroupIds")]
public sealed class DeviceStartRuleActionFactory : GenericRecorderOnlyRuleActionFactory
{
	public DeviceStartRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}
}
