using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server.BusinessObjects.EventTypes;

public sealed class EventType
{
	private EventType()
	{
	}

	public static EventTypeInfo GetEventType(IGeminiRequestContext requestContext, Guid eventTypeId)
	{
		using IEventTypeComponent eventTypeComponent = requestContext.CreateEventTypeComponent();
		return eventTypeComponent.GetEventType(eventTypeId);
	}

	public static IEnumerable<EventTypeInfo> GetEventTypes(IGeminiRequestContext requestContext, string[] generatorTypes, string idGenerator)
	{
		using IEventTypeComponent eventTypeComponent = requestContext.CreateEventTypeComponent();
		return eventTypeComponent.GetEventTypes(generatorTypes, idGenerator);
	}
}
