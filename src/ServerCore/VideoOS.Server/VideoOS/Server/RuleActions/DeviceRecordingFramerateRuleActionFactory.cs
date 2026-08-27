using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.DeviceRecordingFramerate", "6F3434A7-5C76-477e-8C16-B519417D17A4", ActionTargetType.Device, "DeviceIds", "DeviceGroupIds")]
public sealed class DeviceRecordingFramerateRuleActionFactory : GenericRecorderOnlyRuleActionFactory
{
	public DeviceRecordingFramerateRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}
}
