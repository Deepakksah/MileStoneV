using VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;

namespace VideoOS.Platform.AccessControl.Internal.Data.Rule;

public class AlwaysRuleTimeProfile : RuleTimeProfileBase
{
	public AlwaysRuleTimeProfile(RuleConfigurationSerializeData.AlwaysRuleTimeProfileData alwaysRuleTimeProfileData)
	{
	}

	public AlwaysRuleTimeProfile()
	{
	}

	public override object ToDataClass()
	{
		return new RuleConfigurationSerializeData.AlwaysRuleTimeProfileData();
	}
}
