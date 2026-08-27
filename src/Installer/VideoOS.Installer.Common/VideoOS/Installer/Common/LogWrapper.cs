using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.Common;

public class LogWrapper : ILogger
{
	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected I4, but got Unknown
		if (IsEnabled(logLevel))
		{
			string entry = formatter(state, exception);
			switch (logLevel - 1)
			{
			case 0:
				WriteLog(TraceEventType.Verbose, entry);
				break;
			case 3:
				WriteLog(TraceEventType.Error, entry);
				break;
			case 1:
				WriteLog(TraceEventType.Information, entry);
				break;
			case 2:
				WriteLog(TraceEventType.Warning, entry);
				break;
			default:
				WriteLog(TraceEventType.Verbose, entry);
				break;
			}
		}
	}

	public bool IsEnabled(LogLevel logLevel)
	{
		return true;
	}

	public IDisposable BeginScope<TState>(TState state)
	{
		return null;
	}

	private static void WriteLog(TraceEventType eventType, string entry)
	{
		LogManager.Write(entry, eventType);
	}
}
