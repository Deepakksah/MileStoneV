namespace VideoOS.Common.Integration;

public class LogIgnoreRecurrenceHandler : ILogRecurrence
{
	private bool _initiated;

	public LogCheckResult CheckLog(LogEntry logEntry, bool setState)
	{
		if (!setState)
		{
			return LogCheckResult.LogAndTerminate;
		}
		if (_initiated)
		{
			return LogCheckResult.Nothing;
		}
		_initiated = true;
		return LogCheckResult.Log;
	}

	public ILogRecurrence Clone()
	{
		return new LogIgnoreRecurrenceHandler();
	}
}
