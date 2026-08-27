using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using VideoOS.Installer.Logging.Properties;

namespace VideoOS.Installer.Logging;

public class InstallerLog : ILogger
{
	private sealed class EmptyScope : IDisposable
	{
		public void Dispose()
		{
		}
	}

	private static void LogWrite(string logMessage, TraceEventType severity)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		try
		{
			Logger.Write(new LogEntry
			{
				Message = logMessage,
				Severity = severity,
				EventId = Settings.Default.DefaultLoggingEventId,
				Priority = Settings.Default.DefaultLoggingPriority,
				Categories = { Settings.Default.DefaultLoggingCategory }
			});
		}
		catch (Exception)
		{
		}
	}

	public static void Dispose()
	{
		Logger.Reset();
	}

	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected I4, but got Unknown
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		if (IsEnabled(logLevel))
		{
			string text = formatter(state, exception);
			if (((EventId)(ref eventId)).Id != 0)
			{
				text = string.Concat(eventId, " ", text);
			}
			LogWrite(text, (int)logLevel switch
			{
				2 => TraceEventType.Information, 
				3 => TraceEventType.Warning, 
				4 => TraceEventType.Error, 
				5 => TraceEventType.Critical, 
				_ => TraceEventType.Verbose, 
			});
		}
	}

	public bool IsEnabled(LogLevel logLevel)
	{
		return true;
	}

	public IDisposable BeginScope<TState>(TState state)
	{
		return new EmptyScope();
	}
}
