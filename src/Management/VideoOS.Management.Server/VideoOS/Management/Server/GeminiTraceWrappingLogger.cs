using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Management.Server;

public class GeminiTraceWrappingLogger : ILogger
{
	public static GeminiTraceWrappingLogger Instance { get; } = new GeminiTraceWrappingLogger();

	private GeminiTraceWrappingLogger()
	{
	}

	public IDisposable BeginScope<TState>(TState state)
	{
		return new NullLogScope();
	}

	public bool IsEnabled(LogLevel logLevel)
	{
		TraceLevel? traceLevel = null;
		switch (logLevel)
		{
		case LogLevel.Debug:
			traceLevel = TraceLevel.Verbose;
			break;
		case LogLevel.Information:
			traceLevel = TraceLevel.Info;
			break;
		case LogLevel.Warning:
			traceLevel = TraceLevel.Warning;
			break;
		case LogLevel.Error:
			traceLevel = TraceLevel.Error;
			break;
		case LogLevel.Critical:
			traceLevel = TraceLevel.Error;
			break;
		}
		if (!traceLevel.HasValue)
		{
			return false;
		}
		return GeminiTrace.IsTracing(TraceType.General, traceLevel.Value);
	}

	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
	{
		if (!IsEnabled(logLevel))
		{
			return;
		}
		string text = formatter(state, exception);
		if (eventId.Id != 0)
		{
			text = string.Concat(eventId, " ", text);
		}
		switch (logLevel)
		{
		case LogLevel.Debug:
			GeminiTrace.Verbose(text);
			if (exception != null)
			{
				GeminiTrace.Verbose(exception.ToString());
			}
			break;
		case LogLevel.Information:
			GeminiTrace.Info(text);
			if (exception != null)
			{
				GeminiTrace.Info(exception.ToString());
			}
			break;
		case LogLevel.Warning:
			GeminiTrace.Warning(text);
			if (exception != null)
			{
				GeminiTrace.Warning(exception.ToString());
			}
			break;
		case LogLevel.Error:
		case LogLevel.Critical:
			GeminiTrace.Error(text);
			if (exception != null)
			{
				GeminiTrace.Error(exception.ToString());
			}
			break;
		}
	}
}
