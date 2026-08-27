using VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;

namespace VideoOS.Platform.AccessControl.Internal.Data.Rule;

public class SystemEventRuleEvent : RuleEventBase
{
	public SystemEventRuleEvent(RuleConfigurationSerializeData.SystemEventRuleEventData systemEventRuleEventData)
	{
	}

	public SystemEventRuleEvent()
	{
	}

	public override object ToDataClass()
	{
		return new RuleConfigurationSerializeData.SystemEventRuleEventData();
	}
}
