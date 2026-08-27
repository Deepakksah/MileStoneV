using System;

namespace VideoOS.Common.Integration;

public class LogTimeRecurrenceHandler : ILogRecurrence
{
	private TimeSpan _timeBetweenLog;

	private DateTime _lastRun;

	private bool _initiated;

	public LogTimeRecurrenceHandler(TimeSpan timeBetweenLog)
	{
		_timeBetweenLog = timeBetweenLog;
	}

	public LogCheckResult CheckLog(LogEntry logEntry, bool setState)
	{
		if (_initiated)
		{
			DateTime now = DateTime.Now;
			if (now >= _lastRun.Add(_timeBetweenLog))
			{
				_lastRun = now;
				return LogCheckResult.Log;
			}
			return LogCheckResult.Nothing;
		}
		_initiated = true;
		_lastRun = DateTime.Now;
		return LogCheckResult.Log;
	}

	public ILogRecurrence Clone()
	{
		return new LogTimeRecurrenceHandler(_timeBetweenLog);
	}
}
