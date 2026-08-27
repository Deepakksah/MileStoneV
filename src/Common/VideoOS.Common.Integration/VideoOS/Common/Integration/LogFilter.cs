using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[Serializable]
[DataContract]
public class LogFilter
{
	private LogFilterCondition _logFilterCondition;

	[DataMember]
	public LogType LogType { get; private set; }

	[DataMember]
	public LogFilterEntry LogFilterEntry { get; private set; }

	[DataMember]
	public string Value { get; private set; }

	[DataMember]
	public string CultureValue { get; private set; }

	[DataMember]
	public LogFilterCondition Condition
	{
		get
		{
			return _logFilterCondition;
		}
		set
		{
			_logFilterCondition = value;
		}
	}

	[DataMember]
	public string CultureName { get; set; }

	public string ConditionAsString => _logFilterCondition switch
	{
		LogFilterCondition.Equal => " = ", 
		LogFilterCondition.GreaterThan => " > ", 
		LogFilterCondition.GreaterThanOrEqual => " >= ", 
		LogFilterCondition.LessThan => " < ", 
		LogFilterCondition.LessThanOrEqual => " <= ", 
		LogFilterCondition.NotEqual => " <> ", 
		LogFilterCondition.Like => " LIKE ", 
		_ => null, 
	};

	public LogFilter(LogType logType, LogFilterEntry logFilterEntry)
	{
		LogType = logType;
		LogFilterEntry = logFilterEntry;
		CultureName = "en-US";
	}

	public LogFilter(LogType logType, LogFilterEntry logFilterEntry, string value, string cultureValue)
		: this(logType, logFilterEntry)
	{
		Value = value;
		CultureValue = cultureValue;
	}

	public LogFilter(LogType logType, LogFilterEntry logFilterEntry, string value, string cultureValue, LogFilterCondition condition)
		: this(logType, logFilterEntry, value, cultureValue)
	{
		_logFilterCondition = condition;
	}

	public LogFilter Clone()
	{
		return new LogFilter(LogType, LogFilterEntry, Value, CultureValue, _logFilterCondition)
		{
			CultureName = CultureName
		};
	}
}
