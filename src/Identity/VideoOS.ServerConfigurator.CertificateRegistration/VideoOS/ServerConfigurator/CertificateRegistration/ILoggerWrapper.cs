using System;
using Microsoft.Extensions.Logging;

namespace VideoOS.ServerConfigurator.CertificateRegistration;

public class ILoggerWrapper : ILogger
{
	private class NullLogScope : IDisposable
	{
		public void Dispose()
		{
		}
	}

	private readonly ILogger _logger;

	public ILoggerWrapper(ILogger logger)
	{
		_logger = logger;
	}

	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected I4, but got Unknown
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		if (_logger == null || !IsEnabled(logLevel))
		{
			return;
		}
		if (formatter == null)
		{
			throw new ArgumentNullException("formatter");
		}
		string text = formatter(state, exception);
		if (((EventId)(ref eventId)).Id != 0)
		{
			text = string.Concat(eventId, " ", text);
		}
		switch (logLevel - 1)
		{
		case 0:
			_logger.LogDebug(text);
			if (exception != null)
			{
				_logger.LogDebug(exception.ToString());
			}
			break;
		case 2:
			_logger.LogWarning(text);
			if (exception != null)
			{
				_logger.LogWarning(exception.ToString());
			}
			break;
		case 3:
			_logger.LogError(text);
			if (exception != null)
			{
				_logger.LogError(exception.ToString());
			}
			break;
		case 1:
			_logger.LogInfo(text);
			if (exception != null)
			{
				_logger.LogInfo(exception.ToString());
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
