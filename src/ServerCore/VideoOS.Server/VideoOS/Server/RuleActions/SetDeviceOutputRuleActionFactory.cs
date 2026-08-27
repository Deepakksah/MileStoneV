using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.SetDeviceOutput", "B8106540-F385-4087-A1FE-B690AB9EBBD9", ActionTargetType.Device, "DeviceIds", "DeviceGroupIds")]
public sealed class SetDeviceOutputRuleActionFactory : GenericRecorderOnlyRuleActionFactory
{
	public SetDeviceOutputRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}
}
