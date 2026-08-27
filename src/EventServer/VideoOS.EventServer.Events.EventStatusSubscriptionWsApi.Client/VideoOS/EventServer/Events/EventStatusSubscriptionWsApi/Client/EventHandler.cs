using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VideoOS.Common.WebSockets;
using VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client.Protocol;
using VideoOS.Platform.EventsAndState;

namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client;

public class EventHandler : IEventHandler, IEventReceiver, IMessageProcessor<JObject>, IDisposable
{
	private object _eventLock = new object();

	private List<IEventReceiver> _eventReceivers = new List<IEventReceiver>();

	public EventHandler(IMessageDispatcher messageDispatcher)
	{
		messageDispatcher.RegisterMessageReceiver(MessageType.Event, this);
	}

	public async Task ProcessMessageAsync(JObject message, CancellationToken cancellationToken)
	{
		EventsMessage eventsMessage = EventsMessage.FromJson(message);
		await OnEventsReceivedAsync(eventsMessage.Events);
	}

	public Task OnEventsReceivedAsync(IEnumerable<Event> events)
	{
		if (events.Any())
		{
			lock (_eventLock)
			{
				foreach (IEventReceiver eventReceiver in _eventReceivers)
				{
					eventReceiver.OnEventsReceivedAsync(events);
				}
			}
		}
		return Task.CompletedTask;
	}

	public void Dispose()
	{
	}

	public void AddEventReceiver(IEventReceiver eventReceiver)
	{
		lock (_eventLock)
		{
			_eventReceivers.Add(eventReceiver);
		}
	}

	public bool RemoveEventReceiver(IEventReceiver eventReceiver)
	{
		lock (_eventLock)
		{
			return _eventReceivers.Remove(eventReceiver);
		}
	}

	public Task OnConnectionStateChangedAsync(ConnectionState newState)
	{
		return Task.CompletedTask;
	}
}
