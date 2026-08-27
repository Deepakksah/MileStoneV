using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.DeviceChangeSettings", "54D3F81B-9A9B-4daa-ACB3-5AA33B87889F", ActionTargetType.Device, "DeviceIds", "DeviceGroupIds")]
public sealed class DeviceChangeSettingsRuleActionFactory : GenericRecorderOnlyRuleActionFactory
{
	public DeviceChangeSettingsRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}
}
