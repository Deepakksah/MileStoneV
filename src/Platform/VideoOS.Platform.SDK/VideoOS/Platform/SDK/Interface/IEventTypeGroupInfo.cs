using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IEventTypeGroupInfo : IGroupInfo
{
	IEventTypeGroupInfo[] EventTypeGroups { get; set; }

	Guid[] EventTypes { get; set; }
}
