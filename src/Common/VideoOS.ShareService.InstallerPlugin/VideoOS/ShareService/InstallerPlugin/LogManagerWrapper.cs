using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using VideoOS.Installer.Logging;

namespace VideoOS.ShareService.InstallerPlugin;

public class LogManagerWrapper : ILogger
{
	private string _componentName;

	public bool Enable
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public LogManagerWrapper(string componentName)
	{
		_componentName = componentName;
	}

	public void WriteDebugLog(string log)
	{
		Write(log);
	}

	public void Write(string log, TraceEventType eventType = TraceEventType.Information)
	{
		LogManager.Write(_componentName + ": " + log, eventType);
	}

	public void Write(Exception ex, TraceEventType eventType = TraceEventType.Error)
	{
		LogManager.Write(_componentName ?? "", eventType);
		LogManager.Write(ex);
	}

	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		LogManager.InnerLog.Log<TState>(logLevel, eventId, state, exception, formatter);
	}

	public bool IsEnabled(LogLevel logLevel)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return LogManager.InnerLog.IsEnabled(logLevel);
	}

	public IDisposable BeginScope<TState>(TState state)
	{
		return LogManager.InnerLog.BeginScope<TState>(state);
	}
}
