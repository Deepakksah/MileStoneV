using System;

namespace VideoOS.Management.Server;

public class ScheduleLogger : IScheduleLogger
{
	public void LogInfo(string logMessage)
	{
		GeminiTrace.Info(logMessage);
	}

	public void LogException(string logMessage, Exception ex)
	{
		GeminiTrace.Error(logMessage, ex);
	}
}
