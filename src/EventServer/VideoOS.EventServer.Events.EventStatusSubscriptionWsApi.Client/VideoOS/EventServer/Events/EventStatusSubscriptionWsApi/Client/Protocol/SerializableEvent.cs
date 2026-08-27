using System;
using Newtonsoft.Json.Linq;
using VideoOS.Platform;
using VideoOS.Platform.EventsAndState;

namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client.Protocol;

public class SerializableEvent : Event
{
	private JObject _dataAsJObject;

	public SerializableEvent(Guid id, Guid type, string source, Guid? stateGroupId, DateTime time, object data)
		: base(id, type, source, stateGroupId, time, ((data is JObject) ? data : null)?.ToString())
	{
		_dataAsJObject = (JObject)((data is JObject) ? data : null);
	}

	public override T GetData<T>()
	{
		try
		{
			if (_dataAsJObject == null)
			{
				return default(T);
			}
			return ((JToken)_dataAsJObject).ToObject<T>();
		}
		catch (Exception innerException)
		{
			throw new InvalidOperationMIPException("The received data is not of type of type " + typeof(T).Name, innerException);
		}
	}
}
