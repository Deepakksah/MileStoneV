using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using VideoOS.Common.Integration.Properties;

namespace VideoOS.Common.Integration;

[Serializable]
[KnownType(typeof(LogEntryAudit))]
[KnownType(typeof(LogEntrySystem))]
[KnownType(typeof(LogEntryRule))]
public abstract class LogEntry : ISerializable
{
	private string[] _arguments;

	private DateTime _timeStamp;

	private DateTime _timeStampUtc;

	private int _logId;

	private LogLevel _logLevel;

	private Guid _sourceId;

	private string _description;

	public string[] Arguments
	{
		get
		{
			return _arguments;
		}
		set
		{
			_arguments = value;
		}
	}

	public DateTime TimeStampLocal
	{
		get
		{
			return _timeStamp;
		}
		set
		{
			_timeStamp = value;
		}
	}

	public DateTime TimeStampUtc
	{
		get
		{
			return _timeStampUtc;
		}
		set
		{
			_timeStampUtc = value;
		}
	}

	public int LogId
	{
		get
		{
			return _logId;
		}
		set
		{
			_logId = value;
		}
	}

	public LogLevel LogLevel
	{
		get
		{
			return _logLevel;
		}
		set
		{
			_logLevel = value;
		}
	}

	public string LogLevelValue { get; set; }

	public Guid SourceId
	{
		get
		{
			return _sourceId;
		}
		set
		{
			_sourceId = value;
		}
	}

	public SourceType SourceType { get; private set; }

	public string SourceTypeValue { get; set; }

	public string SourceName { get; set; }

	public string Description
	{
		get
		{
			if (string.IsNullOrEmpty(_description))
			{
				return string.Empty;
			}
			return _description;
		}
		set
		{
			_description = value;
		}
	}

	public string MigratedLogDescription { get; set; }

	public LogEntryDefinition LogEntryDefinition { get; private set; }

	public bool HasDescription => _description != null;

	public LogEntry(int logId, params string[] arguments)
	{
		_timeStampUtc = DateTime.UtcNow;
		_timeStamp = _timeStampUtc.ToLocalTime();
		_logId = logId;
		_arguments = arguments;
		LogLevelValue = null;
		LogEntryDefinition = null;
		MigratedLogDescription = null;
	}

	public LogEntry(int logId, Guid sourceId, SourceType sourceType, string sourceName, params string[] arguments)
	{
		_timeStampUtc = DateTime.UtcNow;
		_timeStamp = _timeStampUtc.ToLocalTime();
		_logId = logId;
		_sourceId = sourceId;
		LogLevelValue = null;
		SourceType = sourceType;
		SourceTypeValue = sourceType.ToString();
		SourceName = sourceName;
		_arguments = arguments;
		LogEntryDefinition = null;
		MigratedLogDescription = null;
	}

	public LogEntry(Guid sourceId, SourceType sourceType, string sourceName, LogEntryDefinition definition)
	{
		if (definition == null)
		{
			throw new ArgumentNullException("definition");
		}
		_timeStampUtc = DateTime.UtcNow;
		_timeStamp = _timeStampUtc.ToLocalTime();
		_logId = definition.LogId;
		_sourceId = sourceId;
		LogLevelValue = null;
		SourceType = sourceType;
		SourceTypeValue = sourceType.ToString();
		SourceName = sourceName;
		_arguments = new string[0];
		Description = definition.Description;
		LogEntryDefinition = definition;
		_logLevel = definition.LogLevel;
		MigratedLogDescription = null;
	}

	protected LogEntry(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		BinaryFormatter binaryFormatter = new BinaryFormatter().Wrap();
		MemoryStream memoryStream = new MemoryStream((byte[])info.GetValue("Arguments", typeof(byte[])));
		Arguments = (string[])binaryFormatter.Deserialize(memoryStream);
		memoryStream.Close();
		TimeStampLocal = DateTime.ParseExact(info.GetString("TimeStampLocal"), "o", CultureInfo.InvariantCulture);
		TimeStampUtc = DateTime.Parse(info.GetString("TimeStampUtc"), CultureInfo.InvariantCulture);
		TimeStampUtc = TimeStampUtc.ToUniversalTime();
		LogId = info.GetInt32("LogId");
		LogLevel = (LogLevel)Enum.Parse(typeof(LogLevel), info.GetString("LogLevel"), ignoreCase: true);
		LogLevelValue = info.GetString("LogLevelValue");
		SourceId = (Guid)info.GetValue("SourceId", typeof(Guid));
		SourceTypeValue = info.GetString("SourceTypeValue");
		SourceName = info.GetString("SourceName");
		Description = info.GetString("Description");
		MigratedLogDescription = info.GetString("MigratedLogDescription");
		LogEntryDefinition = (LogEntryDefinition)info.GetValue("LogEntryDefinition", typeof(LogEntryDefinition));
	}

	protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		MemoryStream memoryStream = new MemoryStream();
		new BinaryFormatter().Serialize(memoryStream, Arguments);
		byte[] value = memoryStream.ToArray();
		memoryStream.Close();
		info.AddValue("Arguments", value);
		info.AddValue("TimeStampLocal", TimeStampLocal.ToString("o", CultureInfo.InvariantCulture));
		info.AddValue("TimeStampUtc", TimeStampUtc.ToLocalTime().ToString("o", CultureInfo.InvariantCulture));
		info.AddValue("LogId", LogId);
		info.AddValue("LogLevel", LogLevel.ToString());
		info.AddValue("LogLevelValue", LogLevelValue);
		info.AddValue("SourceId", SourceId);
		info.AddValue("SourceTypeValue", SourceTypeValue);
		info.AddValue("SourceName", SourceName);
		info.AddValue("Description", Description);
		info.AddValue("MigratedLogDescription", MigratedLogDescription);
		info.AddValue("LogEntryDefinition", LogEntryDefinition);
	}

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		GetObjectData(info, context);
	}

	public static string GetEnumName(string resourceKey)
	{
		return Resources.ResourceManager.GetString(resourceKey);
	}

	public static string GetEnumName(string resourceKey, CultureInfo culture)
	{
		return Resources.ResourceManager.GetString(resourceKey, culture);
	}

	public static string GetEnumName(AuditType auditType, CultureInfo culture)
	{
		string name = Enum.GetName(typeof(AuditType), auditType);
		return GetEnumName(string.Format(CultureInfo.InvariantCulture, "{0}{1}", typeof(AuditType).Name, name), culture);
	}

	public static string GetEnumName(AuditType auditType)
	{
		return GetEnumName(auditType, Thread.CurrentThread.CurrentUICulture);
	}

	public static string GetEnumName(AccessGranted accessGranted, CultureInfo culture)
	{
		string name = Enum.GetName(typeof(AccessGranted), accessGranted);
		return GetEnumName(string.Format(CultureInfo.InvariantCulture, "{0}{1}", typeof(AccessGranted).Name, name), culture);
	}

	public static string GetEnumName(AccessGranted accessGranted)
	{
		return GetEnumName(accessGranted, Thread.CurrentThread.CurrentUICulture);
	}

	public static string GetEnumName(ResourceType resourceType, CultureInfo culture)
	{
		string name = Enum.GetName(typeof(ResourceType), resourceType);
		return GetEnumName(string.Format(CultureInfo.InvariantCulture, "{0}{1}", typeof(ResourceType).Name, name), culture);
	}

	public static string GetEnumName(ResourceType resourceType)
	{
		return GetEnumName(resourceType, Thread.CurrentThread.CurrentUICulture);
	}

	public static string GetEnumName(SourceType sourceType, CultureInfo culture)
	{
		string name = Enum.GetName(typeof(SourceType), sourceType);
		return GetEnumName(string.Format(CultureInfo.InvariantCulture, "{0}{1}", typeof(SourceType).Name, name), culture);
	}

	public static string GetEnumName(SourceType sourceType)
	{
		return GetEnumName(sourceType, Thread.CurrentThread.CurrentUICulture);
	}

	public static string GetEnumName(EventGeneratorType eventGeneratorType, CultureInfo culture)
	{
		string name = Enum.GetName(typeof(EventGeneratorType), eventGeneratorType);
		return GetEnumName(string.Format(CultureInfo.InvariantCulture, "{0}{1}", typeof(EventGeneratorType).Name, name), culture);
	}

	public static string GetEnumName(EventGeneratorType eventGeneratorType)
	{
		return GetEnumName(eventGeneratorType, Thread.CurrentThread.CurrentUICulture);
	}

	public static SourceType GetSourceType(string sourceTypeName)
	{
		if (Enum.TryParse<SourceType>(sourceTypeName, ignoreCase: true, out var result))
		{
			return result;
		}
		return SourceType.Unknown;
	}

	public static EventGeneratorType GetEventGeneratorType(string eventGeneratorTypeName)
	{
		if (Enum.TryParse<EventGeneratorType>(eventGeneratorTypeName, ignoreCase: true, out var result))
		{
			return result;
		}
		return EventGeneratorType.Unknown;
	}
}
