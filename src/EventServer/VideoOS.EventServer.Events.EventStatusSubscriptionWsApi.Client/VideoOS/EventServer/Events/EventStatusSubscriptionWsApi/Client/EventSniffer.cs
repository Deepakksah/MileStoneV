using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoOS.Platform.EventsAndState;

namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client;

public sealed class EventSniffer : IEventReceiver
{
	public Guid? LastKnownGoodEventId { get; private set; }

	public Task OnConnectionStateChangedAsync(ConnectionState newState)
	{
		return Task.CompletedTask;
	}

	public Task OnEventsReceivedAsync(IEnumerable<Event> events)
	{
		Event obj = events.LastOrDefault();
		if (obj != null && obj.Id != default(Guid))
		{
			LastKnownGoodEventId = obj.Id;
		}
		return Task.CompletedTask;
	}
}
