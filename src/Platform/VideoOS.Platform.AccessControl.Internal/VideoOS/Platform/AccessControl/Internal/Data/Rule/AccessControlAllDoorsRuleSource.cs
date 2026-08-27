using VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;

namespace VideoOS.Platform.AccessControl.Internal.Data.Rule;

public class AccessControlAllDoorsRuleSource : RuleSourceBase
{
	public AccessControlAllDoorsRuleSource(RuleConfigurationSerializeData.AccessControlAllDoorsRuleSourceData accessControlAllDoorsRuleSourceData)
	{
	}

	public AccessControlAllDoorsRuleSource()
	{
	}

	public override object ToDataClass()
	{
		return new RuleConfigurationSerializeData.AccessControlAllDoorsRuleSourceData();
	}
}
