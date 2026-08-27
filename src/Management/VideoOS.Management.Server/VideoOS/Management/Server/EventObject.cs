using System.Collections.Generic;

namespace VideoOS.Management.Server;

public class EventObject
{
	public Dictionary<string, string> _eventData = new Dictionary<string, string>();

	public IGeminiRequestContext RequestContext { get; set; }

	public string EventTypeName { get; set; }

	public string EventObjectId { get; set; }

	public EventAction Action { get; set; }

	public EventObject(IGeminiRequestContext requestContext, string eventTypeName, string eventObjectId, EventAction action, Dictionary<string, string> data)
	{
		RequestContext = requestContext;
		EventTypeName = eventTypeName;
		EventObjectId = eventObjectId;
		Action = action;
		if (data == null || data.Keys.Count <= 0)
		{
			return;
		}
		foreach (string key in data.Keys)
		{
			_eventData.Add(key, data[key]);
		}
	}

	public void AddEventData(string name, string value)
	{
		_eventData.Add(name, value);
	}

	public static int Save()
	{
		return 0;
	}
}
