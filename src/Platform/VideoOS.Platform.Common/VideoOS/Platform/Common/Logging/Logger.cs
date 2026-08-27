using System;
using Microsoft.Extensions.Logging;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Platform.Common.Logging;

public class Logger : ILogger
{
	private readonly string _componentName;

	public Logger()
		: this(string.Empty)
	{
	}

	public Logger(string componentName)
	{
		_componentName = componentName;
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
			EnvironmentManager.Instance.Log(error: false, _componentName, text);
			if (exception != null)
			{
				EnvironmentManager.Instance.Log(error: false, _componentName, exception.ToString());
			}
			break;
		case 2:
			EnvironmentManager.Instance.Log(error: false, _componentName, text);
			if (exception != null)
			{
				EnvironmentManager.Instance.Log(error: false, _componentName, exception.ToString());
			}
			break;
		case 3:
			EnvironmentManager.Instance.Log(error: true, _componentName, text);
			if (exception != null)
			{
				EnvironmentManager.Instance.Log(error: true, _componentName, exception.ToString());
			}
			break;
		case 1:
			EnvironmentManager.Instance.Log(error: false, _componentName, text);
			if (exception != null)
			{
				EnvironmentManager.Instance.Log(error: false, _componentName, exception.ToString());
			}
			break;
		}
	}

	public bool IsEnabled(LogLevel logLevel)
	{
		return true;
	}

	public IDisposable BeginScope<TState>(TState state)
	{
		return new NullLogScope();
	}
}
