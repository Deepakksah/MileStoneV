#define TRACE
using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace VideoOS.LogServer.Communication.Log;

public class TraceLogger : ILogger
{
	public IDisposable BeginScope<TState>(TState state)
	{
		return new LegacyLogScope();
	}

	public void Flush()
	{
		Trace.Flush();
	}

	public bool IsEnabled(LogLevel logLevel)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		return (int)logLevel == 4;
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
		if (((EventId)(ref eventId)).Id != 0)
		{
			text = string.Concat(eventId, " ", text);
		}
		switch (logLevel - 1)
		{
		case 0:
			Trace.WriteLine(text);
			if (exception != null)
			{
				Trace.WriteLine(exception.ToString());
			}
			break;
		case 2:
			Trace.TraceWarning(text);
			if (exception != null)
			{
				Trace.TraceWarning(exception.ToString());
			}
			break;
		case 3:
			Trace.TraceError(text);
			if (exception != null)
			{
				Trace.TraceError(exception.ToString());
			}
			break;
		case 1:
			Trace.TraceInformation(text);
			if (exception != null)
			{
				Trace.TraceInformation(exception.ToString());
			}
			break;
		}
	}
}
