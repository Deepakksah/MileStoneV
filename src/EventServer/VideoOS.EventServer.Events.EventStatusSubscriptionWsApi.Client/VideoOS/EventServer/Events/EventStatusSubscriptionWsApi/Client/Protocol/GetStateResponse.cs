using System.Collections.Generic;
using VideoOS.Platform.EventsAndState;

namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client.Protocol;

public class GetStateResponse : ResponseBase
{
	public IEnumerable<State> States { get; set; }

	public GetStateResponse(ulong commandId, int status, IEnumerable<SerializableState> states)
		: base(commandId, status)
	{
		States = states;
	}
}
