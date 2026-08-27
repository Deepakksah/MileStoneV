namespace VideoOS.Common.Integration;

public interface ILogRecurrence
{
	LogCheckResult CheckLog(LogEntry logEntry, bool setState);

	ILogRecurrence Clone();
}
