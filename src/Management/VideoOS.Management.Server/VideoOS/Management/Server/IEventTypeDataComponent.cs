using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public interface IEventTypeDataComponent : IDisposable
{
	IEnumerable<EventTypeData> QueryEventTypes();
}
