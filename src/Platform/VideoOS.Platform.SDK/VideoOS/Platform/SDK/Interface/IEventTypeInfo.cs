using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IEventTypeInfo
{
	Guid EventTypeId { get; set; }

	string Name { get; set; }

	string Description { get; set; }
}
