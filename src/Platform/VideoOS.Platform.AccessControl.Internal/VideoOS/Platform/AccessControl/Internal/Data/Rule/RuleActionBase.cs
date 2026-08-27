using VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;

namespace VideoOS.Platform.AccessControl.Internal.Data.Rule;

public abstract class RuleActionBase : RuleElementBase
{
	public static RuleActionBase Create(RuleConfigurationSerializeData.RuleActionBaseData ruleActionBaseData)
	{
		if (ruleActionBaseData is RuleConfigurationSerializeData.SystemActionRuleActionData)
		{
			return new SystemActionRuleAction((RuleConfigurationSerializeData.SystemActionRuleActionData)ruleActionBaseData);
		}
		if (ruleActionBaseData is RuleConfigurationSerializeData.PtzRuleActionData)
		{
			return new PtzRuleAction((RuleConfigurationSerializeData.PtzRuleActionData)ruleActionBaseData);
		}
		if (ruleActionBaseData is RuleConfigurationSerializeData.StartRecordingRuleActionData)
		{
			return new StartRecordingRuleAction((RuleConfigurationSerializeData.StartRecordingRuleActionData)ruleActionBaseData);
		}
		if (ruleActionBaseData is RuleConfigurationSerializeData.AccessRequestRuleActionData)
		{
			return new AccessRequestRuleAction((RuleConfigurationSerializeData.AccessRequestRuleActionData)ruleActionBaseData);
		}
		return null;
	}
}
