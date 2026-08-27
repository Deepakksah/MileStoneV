using VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;

namespace VideoOS.Platform.AccessControl.Internal.Data.Rule;

public abstract class RuleSourceBase : RuleElementBase
{
	public static RuleSourceBase Create(RuleConfigurationSerializeData.RuleSourceBaseData ruleSourceBaseData)
	{
		if (ruleSourceBaseData is RuleConfigurationSerializeData.AccessControlAllDoorsRuleSourceData)
		{
			return new AccessControlAllDoorsRuleSource((RuleConfigurationSerializeData.AccessControlAllDoorsRuleSourceData)ruleSourceBaseData);
		}
		if (ruleSourceBaseData is RuleConfigurationSerializeData.AccessControlDoorRuleSourceData)
		{
			return new AccessControlDoorRuleSource((RuleConfigurationSerializeData.AccessControlDoorRuleSourceData)ruleSourceBaseData);
		}
		if (ruleSourceBaseData is RuleConfigurationSerializeData.AccessControlOperationableInstanceRuleSourceData)
		{
			return new AccessControlOperationableInstanceRuleSource((RuleConfigurationSerializeData.AccessControlOperationableInstanceRuleSourceData)ruleSourceBaseData);
		}
		if (ruleSourceBaseData is RuleConfigurationSerializeData.SystemItemRuleSourceData)
		{
			return new SystemItemRuleSource((RuleConfigurationSerializeData.SystemItemRuleSourceData)ruleSourceBaseData);
		}
		return null;
	}
}
