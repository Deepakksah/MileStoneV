using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.RetrieveEdgeStorageRecording", "CF47BDC9-DDCE-4E2F-BE2D-2A5AA730262B", ActionTargetType.Device, "DeviceIds", "DeviceGroupIds")]
public sealed class DeviceEdgeRetrievalRuleActionFactory : GenericRecorderOnlyRuleActionFactory
{
	public DeviceEdgeRetrievalRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}
}
