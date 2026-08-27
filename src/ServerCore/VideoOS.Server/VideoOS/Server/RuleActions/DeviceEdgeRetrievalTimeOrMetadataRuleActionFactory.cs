using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.RetrieveEdgeStorageRecordingTimeOrMetadata", "8BD18946-9F8F-4B0F-A62C-CD6EC00AF3EE", ActionTargetType.Device, "DeviceIds", "DeviceGroupIds")]
public sealed class DeviceEdgeRetrievalTimeOrMetadataRuleActionFactory : GenericRecorderOnlyRuleActionFactory
{
	public DeviceEdgeRetrievalTimeOrMetadataRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}
}
