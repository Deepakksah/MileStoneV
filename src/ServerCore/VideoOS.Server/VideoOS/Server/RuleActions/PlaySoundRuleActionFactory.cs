using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.ActionPlaySound", "B8E40A14-5F8B-4999-AD29-021287F271B8", ActionTargetType.Device, "DeviceIds", "DeviceGroupIds")]
public sealed class PlaySoundRuleActionFactory : GenericRecorderOnlyRuleActionFactory
{
	public PlaySoundRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}
}
