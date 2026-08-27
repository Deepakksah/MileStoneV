using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using VideoOS.Platform;
using VideoOS.Platform.EventsAndState;

namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client.Protocol;

public class EventsMessage
{
	public IEnumerable<Event> Events { get; }

	public EventsMessage(IEnumerable<SerializableEvent> events)
	{
		Events = events;
	}

	public static EventsMessage FromJson(JObject jsonData)
	{
		return FromJson<EventsMessage>(jsonData);
	}

	public static T FromJson<T>(JObject jsonData)
	{
		try
		{
			return ((JToken)jsonData).ToObject<T>();
		}
		catch (Exception innerException)
		{
			throw new CommunicationContentMIPException("Unexpected response. Response was not of type " + typeof(T).Name, innerException);
		}
	}
}
