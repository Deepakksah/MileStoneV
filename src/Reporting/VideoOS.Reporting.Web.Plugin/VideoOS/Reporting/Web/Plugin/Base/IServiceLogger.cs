using System;

namespace VideoOS.Reporting.Web.Plugin.Base;

public interface IServiceLogger
{
	void LogToServerLog(Exception ex, string additionalInfo, LogType logType);

	void LogToServerLog(string logText, LogType logType);

	void InitiateLogger(string serverLogConfigName, string serverLogFolder, string eventSourceName);
}
