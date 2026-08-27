using System;

namespace VideoOS.Common.Integration;

public class LogCountRecurrenceHandler : ILogRecurrence
{
	public LogCheckResult CheckLog(LogEntry logEntry, bool setState)
	{
		throw new NotImplementedException("The method or operation is not implemented.");
	}

	public ILogRecurrence Clone()
	{
		throw new NotImplementedException("The method or operation is not implemented.");
	}
}
