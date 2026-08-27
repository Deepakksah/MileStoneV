using VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;

namespace VideoOS.Platform.AccessControl.Internal.Data.Rule;

public abstract class RuleTimeProfileBase : RuleElementBase
{
	public static RuleTimeProfileBase Create(RuleConfigurationSerializeData.RuleTimeProfileBaseData ruleTimeProfileBaseData)
	{
		if (ruleTimeProfileBaseData is RuleConfigurationSerializeData.AlwaysRuleTimeProfileData)
		{
			return new AlwaysRuleTimeProfile((RuleConfigurationSerializeData.AlwaysRuleTimeProfileData)ruleTimeProfileBaseData);
		}
		if (ruleTimeProfileBaseData is RuleConfigurationSerializeData.MipRuleTimeProfileData)
		{
			return new MipRuleTimeProfile((RuleConfigurationSerializeData.MipRuleTimeProfileData)ruleTimeProfileBaseData);
		}
		return null;
	}
}
