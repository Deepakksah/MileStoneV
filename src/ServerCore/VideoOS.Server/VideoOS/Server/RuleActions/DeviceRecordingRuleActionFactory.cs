using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.DeviceRecording", "37C1780F-34CC-43C4-BA21-6DFAE3A4F93F", ActionTargetType.Device, "DeviceIds", "DeviceGroupIds")]
public sealed class DeviceRecordingRuleActionFactory : GenericRecorderOnlyRuleActionFactory
{
	public DeviceRecordingRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}
}
