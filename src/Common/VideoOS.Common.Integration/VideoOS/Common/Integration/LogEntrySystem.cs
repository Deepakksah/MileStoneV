using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[Serializable]
public class LogEntrySystem : LogEntry, ISerializable
{
	private Guid _eventTypeId;

	private string _eventType;

	public Guid EventTypeId
	{
		get
		{
			return _eventTypeId;
		}
		set
		{
			_eventTypeId = value;
		}
	}

	public string EventType
	{
		get
		{
			return _eventType;
		}
		set
		{
			_eventType = value;
		}
	}

	public LogEntrySystem(LogEntryId logEntryId, params string[] arguments)
		: base((int)logEntryId, arguments)
	{
	}

	public LogEntrySystem(LogEntryId logEntryId, Guid sourceId, SourceType sourceType, string sourceName, params string[] arguments)
		: base((int)logEntryId, sourceId, sourceType, sourceName, arguments)
	{
		_eventType = string.Empty;
		_eventTypeId = Guid.Empty;
	}

	public LogEntrySystem(LogEntryId logEntryId, Guid sourceId, SourceType sourceType, string sourceName, Guid eventTypeId, string eventType, params string[] arguments)
		: base((int)logEntryId, sourceId, sourceType, sourceName, arguments)
	{
		_eventType = eventType;
		_eventTypeId = eventTypeId;
	}

	public LogEntrySystem(Guid sourceId, SourceType sourceType, string sourceName, Guid eventTypeId, string eventType, LogEntryDefinition definition)
		: base(sourceId, sourceType, sourceName, definition)
	{
		_eventType = eventType;
		_eventTypeId = eventTypeId;
	}

	protected LogEntrySystem(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		EventTypeId = (Guid)info.GetValue("EventTypeId", typeof(Guid));
		EventType = info.GetString("EventType");
	}

	protected override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		base.GetObjectData(info, context);
		info.AddValue("EventTypeId", EventTypeId);
		info.AddValue("EventType", EventType);
	}
}
