using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public sealed class EventTypeData
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public string Description => string.Empty;

	public string GeneratorType { get; set; }

	public string GeneratorSubType { get; set; }

	public bool BuiltIn { get; set; }

	public static IEnumerable<EventTypeData> QueryEventTypes(IGeminiRequestContext requestContext)
	{
		using IEventTypeDataComponent eventTypeDataComponent = requestContext.CreateEventTypeDataComponent();
		return eventTypeDataComponent.QueryEventTypes();
	}
}
