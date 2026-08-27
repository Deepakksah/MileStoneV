using System;
using System.Collections.Generic;

namespace VideoOS.Platform.EventsAndState;

public class EventTypes : InternalSpecifier<Guid>
{
	public static EventTypes Any { get; } = new EventTypes();

	private EventTypes()
	{
	}

	public EventTypes(IEnumerable<Guid> ids)
		: base(ids)
	{
	}
}
