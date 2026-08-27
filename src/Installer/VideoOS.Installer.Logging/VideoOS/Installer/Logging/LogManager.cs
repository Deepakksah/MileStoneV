using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using VideoOS.Installer.Logging.Properties;

namespace VideoOS.Installer.Logging;

public class LogManager
{
	private static ILogger _logger;

	public static string ApplicationPath { get; set; }

	public static ILogger InnerLog => _logger;

	public LogManager()
	{
	}

	public LogManager(ILogger logger)
	{
		_logger = logger;
	}

	public static void Init()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		Logger.SetLogWriter(new LogWriterFactory().Create(), true);
	}

	public static void Write(string logMessage, TraceEventType severity)
	{
		switch (severity)
		{
		case TraceEventType.Information:
			LoggerExtensions.LogInformation(_logger, logMessage, Array.Empty<object>());
			break;
		case TraceEventType.Warning:
			LoggerExtensions.LogWarning(_logger, logMessage, Array.Empty<object>());
			break;
		case TraceEventType.Error:
			LoggerExtensions.LogError(_logger, logMessage, Array.Empty<object>());
			break;
		case TraceEventType.Critical:
			LoggerExtensions.LogCritical(_logger, logMessage, Array.Empty<object>());
			break;
		default:
			LoggerExtensions.LogInformation(_logger, logMessage, Array.Empty<object>());
			break;
		}
	}

	public static void Write(string logMessage, string details, TraceEventType severity)
	{
		Write($"{logMessage}. Error details: {details}", severity);
	}

	public static void Write(string logMessage)
	{
		Write(logMessage, TraceEventType.Information);
	}

	public static void Write(Exception exception)
	{
		Write($"An error occurred: '{exception.TypeAndMessage()}'. Error details: {exception.StackTrace}", TraceEventType.Error);
		if (exception.InnerException != null)
		{
			Write(exception.InnerException);
		}
	}

	public static void SetLogInstance(ILogger logger)
	{
		_logger = logger;
	}

	public static string GetMsiLogPath()
	{
		return Path.GetDirectoryName(GetLogFilename());
	}

	private static RollingFlatFileTraceListenerData GetRollingFlatFileTraceListener()
	{
		ConfigurationSection section = ConfigurationManager.OpenExeConfiguration(ApplicationPath).GetSection("loggingConfiguration");
		LoggingSettings val = (LoggingSettings)(object)((section is LoggingSettings) ? section : null);
		if (val != null)
		{
			TraceListenerData obj = ((NamedElementCollection<TraceListenerData>)(object)val.TraceListeners).Get("Rolling Flat File Trace Listener");
			return (RollingFlatFileTraceListenerData)(object)((obj is RollingFlatFileTraceListenerData) ? obj : null);
		}
		return null;
	}

	public static void SetLogFilename(string logfile)
	{
		Configuration configuration = ConfigurationManager.OpenExeConfiguration(ApplicationPath);
		ConfigurationSection section = configuration.GetSection("loggingConfiguration");
		LoggingSettings val = (LoggingSettings)(object)((section is LoggingSettings) ? section : null);
		if (val != null)
		{
			TraceListenerData obj = ((NamedElementCollection<TraceListenerData>)(object)val.TraceListeners).Get("Rolling Flat File Trace Listener");
			RollingFlatFileTraceListenerData val2 = (RollingFlatFileTraceListenerData)(object)((obj is RollingFlatFileTraceListenerData) ? obj : null);
			if (val2 != null)
			{
				val2.FileName = logfile;
				configuration.Save(ConfigurationSaveMode.Modified);
				ConfigurationManager.RefreshSection("loggingConfiguration");
			}
		}
	}

	public static string GetLogFilename()
	{
		RollingFlatFileTraceListenerData rollingFlatFileTraceListener = GetRollingFlatFileTraceListener();
		if (rollingFlatFileTraceListener != null && !string.IsNullOrEmpty(rollingFlatFileTraceListener.FileName))
		{
			return rollingFlatFileTraceListener.FileName;
		}
		return Settings.Default.DefaultLogFilename;
	}

	public static void RemoveInstallLogs()
	{
		try
		{
			InstallerLog.Dispose();
			DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(GetLogFilename()));
			if (!directoryInfo.Exists)
			{
				return;
			}
			foreach (FileInfo item in directoryInfo.EnumerateFiles())
			{
				if (Path.GetExtension(item.Name) == ".log")
				{
					item.Delete();
				}
			}
		}
		catch (IOException inner)
		{
			throw new LogManagerFailedException(Resources.LogManagerError_FailToDeleteInstallerLogFiles, inner);
		}
		catch (SecurityException inner2)
		{
			throw new LogManagerFailedException(Resources.LogManagerError_FailToDeleteInstallerLogFiles, inner2);
		}
		catch (UnauthorizedAccessException inner3)
		{
			throw new LogManagerFailedException(Resources.LogManagerError_FailToDeleteInstallerLogFiles, inner3);
		}
	}
}
