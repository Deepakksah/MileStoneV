using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.RecordAllFrames", "F7FCF9A5-A50F-4572-A985-AFA80D5E9DB1", ActionTargetType.Device, "DeviceIds", "DeviceGroupIds")]
public sealed class DeviceRecordAllFramesActionFactory : GenericRecorderOnlyRuleActionFactory
{
	public DeviceRecordAllFramesActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}
}
