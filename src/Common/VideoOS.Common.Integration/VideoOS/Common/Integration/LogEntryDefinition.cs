using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[Serializable]
[DataContract]
public class LogEntryDefinition : ILogEntryDefinition
{
	[DataMember]
	public int LogId { get; private set; }

	[DataMember]
	public string LogIdString { get; private set; }

	[DataMember]
	public LogLevel LogLevel { get; set; }

	[DataMember]
	public LogType LogEntryType { get; set; }

	[DataMember]
	public string Description { get; private set; }

	[DataMember]
	public int NumberOfParameters { get; private set; }

	public LogEntryDefinition(int logId, LogLevel logLevel, LogType logEntryType, string description, int numberOfParameters)
	{
		LogId = logId;
		LogIdString = string.Empty;
		LogLevel = logLevel;
		LogEntryType = logEntryType;
		Description = description;
		NumberOfParameters = numberOfParameters;
	}
}
