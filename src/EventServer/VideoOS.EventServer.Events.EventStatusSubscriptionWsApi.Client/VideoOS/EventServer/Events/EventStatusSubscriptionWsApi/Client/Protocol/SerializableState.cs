using System;
using VideoOS.Platform.EventsAndState;

namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client.Protocol;

public class SerializableState : State
{
	public SerializableState(Guid Type, string Source, Guid? StateGroupId, DateTime Time)
		: base(Type, Source, StateGroupId, Time)
	{
	}
}
