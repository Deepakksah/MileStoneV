using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.GotoPreset", "868297FB-D739-4BB8-AA17-04667E37334C", ActionTargetType.Device, "DeviceId")]
public sealed class GotoPresetRuleActionFactory : GenericRecorderOnlyRuleActionFactory
{
	public GotoPresetRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}
}
