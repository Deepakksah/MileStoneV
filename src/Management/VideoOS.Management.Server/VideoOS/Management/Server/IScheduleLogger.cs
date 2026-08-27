using System;

namespace VideoOS.Management.Server;

public interface IScheduleLogger
{
	void LogInfo(string logMessage);

	void LogException(string logMessage, Exception ex);
}
