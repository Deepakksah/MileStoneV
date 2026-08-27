using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[Serializable]
public class LogEntryRule : LogEntry, ISerializable
{
	private string _serviceName;

	private string _ruleName;

	private Guid _eventTypeId;

	private string _eventType;

	private Guid _generatorId;

	private string _generatorName;

	public string ServiceName
	{
		get
		{
			return _serviceName;
		}
		set
		{
			_serviceName = value;
		}
	}

	public string RuleName
	{
		get
		{
			return _ruleName;
		}
		set
		{
			_ruleName = value;
		}
	}

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

	public Guid GeneratorId
	{
		get
		{
			return _generatorId;
		}
		set
		{
			_generatorId = value;
		}
	}

	public string GeneratorTypeValue { get; set; }

	public string GeneratorName
	{
		get
		{
			return _generatorName;
		}
		set
		{
			_generatorName = value;
		}
	}

	public LogEntryRule(LogEntryId logEntryId, params string[] arguments)
		: base((int)logEntryId, arguments)
	{
	}

	public LogEntryRule(LogEntryId logEntryId, string serviceName, Guid sourceId, SourceType sourceType, string sourceName, Guid eventTypeId, string eventType, string ruleName, Guid generatorId, EventGeneratorType generatorType, string generatorName, params string[] arguments)
		: base((int)logEntryId, sourceId, sourceType, sourceName, arguments)
	{
		_serviceName = serviceName;
		_eventTypeId = eventTypeId;
		_eventType = eventType;
		_ruleName = ruleName;
		_generatorId = generatorId;
		GeneratorTypeValue = generatorType.ToString();
		_generatorName = generatorName;
	}

	protected LogEntryRule(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		ServiceName = info.GetString("ServiceName");
		RuleName = info.GetString("RuleName");
		EventTypeId = (Guid)info.GetValue("EventTypeId", typeof(Guid));
		EventType = info.GetString("EventType");
		GeneratorId = (Guid)info.GetValue("GeneratorId", typeof(Guid));
		GeneratorTypeValue = info.GetString("GeneratorTypeValue");
		GeneratorName = info.GetString("GeneratorName");
	}

	protected override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		base.GetObjectData(info, context);
		info.AddValue("ServiceName", ServiceName);
		info.AddValue("RuleName", RuleName);
		info.AddValue("EventTypeId", EventTypeId);
		info.AddValue("EventType", EventType);
		info.AddValue("GeneratorId", GeneratorId);
		info.AddValue("GeneratorTypeValue", GeneratorTypeValue);
		info.AddValue("GeneratorName", GeneratorName);
	}
}
