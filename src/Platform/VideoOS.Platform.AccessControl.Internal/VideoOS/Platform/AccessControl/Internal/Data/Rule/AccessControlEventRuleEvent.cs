using System;
using VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;

namespace VideoOS.Platform.AccessControl.Internal.Data.Rule;

public class AccessControlEventRuleEvent : RuleEventBase
{
	public Guid EventTypeId { get; private set; }

	public AccessControlEventRuleEvent(RuleConfigurationSerializeData.AccessControlEventRuleEventData accessControlEventRuleEventData)
	{
		EventTypeId = accessControlEventRuleEventData.EventTypeId;
	}

	public AccessControlEventRuleEvent(Guid eventTypeId)
	{
		EventTypeId = eventTypeId;
	}

	public override object ToDataClass()
	{
		return new RuleConfigurationSerializeData.AccessControlEventRuleEventData
		{
			EventTypeId = EventTypeId
		};
	}
}
