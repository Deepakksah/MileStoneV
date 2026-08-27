using System;
using Microsoft.Extensions.Logging;
using VideoOS.Platform.SDK.Platform;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Platform.SDK.Telemetry;

public class SDKTelemetryLogger : ILogger
{
	private readonly bool _enabled;

	public SDKTelemetryLogger(bool isEnabled)
	{
		_enabled = isEnabled;
	}

	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected I4, but got Unknown
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		if (!IsEnabled(logLevel))
		{
			return;
		}
		string text = formatter(state, exception);
		if (eventId.Id != 0)
		{
			text = string.Concat(eventId, " ", text);
		}
		switch (logLevel - 1)
		{
		case 0:
			Logger.DebugLog(text);
			if (exception != null)
			{
				Logger.DebugLog(exception.ToString());
			}
			break;
		case 2:
			Logger.DebugLog(text);
			if (exception != null)
			{
				Logger.DebugLog(exception.ToString());
			}
			break;
		case 3:
			Logger.DebugLog(text);
			if (exception != null)
			{
				Logger.DebugLog(exception.ToString());
			}
			break;
		case 1:
			Logger.DebugLog(text);
			if (exception != null)
			{
				Logger.DebugLog(exception.ToString());
			}
			break;
		}
	}

	public bool IsEnabled(LogLevel logLevel)
	{
		return _enabled;
	}

	public IDisposable BeginScope<TState>(TState state)
	{
		return new NullLogScope();
	}
}
