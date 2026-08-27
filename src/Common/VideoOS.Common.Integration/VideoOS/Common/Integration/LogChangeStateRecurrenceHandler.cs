using System;

namespace VideoOS.Common.Integration;

public class LogChangeStateRecurrenceHandler : ILogRecurrence
{
	private int currentLogEntryId = int.MinValue;

	public LogCheckResult CheckLog(LogEntry logEntry, bool setState)
	{
		if (logEntry == null)
		{
			throw new ArgumentNullException("logEntry");
		}
		if (currentLogEntryId != logEntry.LogId)
		{
			currentLogEntryId = logEntry.LogId;
			return LogCheckResult.Log;
		}
		return LogCheckResult.Nothing;
	}

	public ILogRecurrence Clone()
	{
		return new LogChangeStateRecurrenceHandler();
	}
}
