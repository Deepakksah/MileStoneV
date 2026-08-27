using System;
using VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;

namespace VideoOS.Platform.AccessControl.Internal.Data.Rule;

public class AccessControlEventCategoryRuleEvent : RuleEventBase
{
	public Guid CategoryId { get; private set; }

	public AccessControlEventCategoryRuleEvent(RuleConfigurationSerializeData.AccessControlEventCategoryRuleEventData accessControlEventCategoryRuleEventData)
	{
		CategoryId = accessControlEventCategoryRuleEventData.CategoryId;
	}

	public AccessControlEventCategoryRuleEvent(Guid categoryId)
	{
		CategoryId = categoryId;
	}

	public override object ToDataClass()
	{
		return new RuleConfigurationSerializeData.AccessControlEventCategoryRuleEventData
		{
			CategoryId = CategoryId
		};
	}
}
