using System;

namespace VideoOS.Platform.SDK.Interface;

public interface ISystemEventTypeInfo
{
	Guid EventTypeId { get; set; }

	string Name { get; set; }

	string Description { get; set; }

	EventSource EventSource { get; set; }
}
