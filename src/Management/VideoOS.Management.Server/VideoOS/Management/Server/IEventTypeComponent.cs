using System;
using System.Collections.Generic;
using VideoOS.Management.Server.BusinessObjects.EventTypes;

namespace VideoOS.Management.Server;

public interface IEventTypeComponent : IDisposable
{
	EventTypeInfo GetEventType(Guid eventTypeId);

	IEnumerable<EventTypeInfo> GetEventTypes(string[] generatorTypes, string idGenerator);
}
