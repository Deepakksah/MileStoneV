using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.GotoDefaultPreset", "41C10FB7-2647-4ecb-93C9-5E737BC3C4EA", ActionTargetType.Device, "DeviceIds", "DeviceGroupIds")]
public sealed class GotoDefaultPresetRuleActionFactory : GenericRecorderOnlyRuleActionFactory
{
	public GotoDefaultPresetRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}
}
