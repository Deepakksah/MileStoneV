using System;
using VideoOS.Reporting.Web.Plugin.Settings;

namespace VideoOS.Reporting.Web.Plugin.Base;

public sealed class ServiceLogger : IServiceLogger
{
	private static IServiceLogger _logger;

	public static IServiceLogger Instance
	{
		get
		{
			if (_logger == null)
			{
				ServiceLogger serviceLogger = new ServiceLogger();
				((IServiceLogger)serviceLogger).InitiateLogger("ReportServerLog", ConfigSettings.ServerLogFolder, ConfigSettings.WinEventLogSourceName);
				_logger = serviceLogger;
			}
			return _logger;
		}
	}

	private string ServerLogConfigName { get; set; }

	private string ServerLogFolder { get; set; }

	private string EventSourceName { get; set; }

	void IServiceLogger.InitiateLogger(string serverLogConfigName, string serverLogFolder, string eventSourceName)
	{
		ServerLogConfigName = serverLogConfigName;
		ServerLogFolder = serverLogFolder;
		EventSourceName = eventSourceName;
	}

	void IServiceLogger.LogToServerLog(Exception ex, string additionalInfo, LogType logType)
	{
		if (!(ex is ReportServerException))
		{
			try
			{
				new EventLogger(EventSourceName).Log(ex, additionalInfo, LogType.Error);
			}
			catch (Exception)
			{
			}
		}
		new TextLogger(ServerLogConfigName, ServerLogFolder).Log(ex, additionalInfo, logType);
	}

	void IServiceLogger.LogToServerLog(string logText, LogType logType)
	{
		new TextLogger(ServerLogConfigName, ServerLogFolder).Log(0, logText, logType);
	}
}
