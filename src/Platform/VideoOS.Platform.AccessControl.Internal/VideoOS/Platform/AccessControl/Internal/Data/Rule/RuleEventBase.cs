using VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;

namespace VideoOS.Platform.AccessControl.Internal.Data.Rule;

public abstract class RuleEventBase : RuleElementBase
{
	public static RuleEventBase Create(RuleConfigurationSerializeData.RuleEventBaseData ruleEventBaseData)
	{
		if (ruleEventBaseData is RuleConfigurationSerializeData.SystemEventRuleEventData)
		{
			return new SystemEventRuleEvent((RuleConfigurationSerializeData.SystemEventRuleEventData)ruleEventBaseData);
		}
		if (ruleEventBaseData is RuleConfigurationSerializeData.AccessControlEventRuleEventData)
		{
			return new AccessControlEventRuleEvent((RuleConfigurationSerializeData.AccessControlEventRuleEventData)ruleEventBaseData);
		}
		if (ruleEventBaseData is RuleConfigurationSerializeData.AccessControlEventCategoryRuleEventData)
		{
			return new AccessControlEventCategoryRuleEvent((RuleConfigurationSerializeData.AccessControlEventCategoryRuleEventData)ruleEventBaseData);
		}
		return null;
	}

	public RuleEventBase()
	{
	}
}
