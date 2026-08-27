using System;
using VideoOS.Platform.EventsAndState;

namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client;

public class StateAsEventWrapper : Event
{
	public StateAsEventWrapper(State state)
		: base(default(Guid), state.Type, state.Source, state.StateGroupId, state.Time, null)
	{
	}

	public override T GetData<T>()
	{
		return default(T);
	}
}
