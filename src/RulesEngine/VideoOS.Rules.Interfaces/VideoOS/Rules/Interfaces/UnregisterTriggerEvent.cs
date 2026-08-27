using System;

namespace VideoOS.Rules.Interfaces;

public sealed class UnregisterTriggerEvent : TriggerEventBase
{
	public UnregisterTriggerEvent(Guid sourceId, Guid eventTypeId, DateTime eventTimeUtc)
		: base(sourceId, eventTypeId, eventTimeUtc)
	{
	}

	public UnregisterTriggerEvent(Guid sourceId, Guid eventTypeId)
		: base(sourceId, eventTypeId)
	{
	}

	public UnregisterTriggerEvent Clone()
	{
		return new UnregisterTriggerEvent(base.SourceId, base.EventTypeId, base.Time);
	}
}
