using System;
using Microsoft.Extensions.Logging;
using NSpring.Logging;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Diagnostics.Logging.Loggers;

public abstract class LoggerBase : ILogger
{
	protected Logger _logger;

	public Logger LoggerInstance => _logger;

	public void Flush()
	{
		_logger.Flush();
	}

	public static Level NSpringLevel(LogLevel logLevel)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected I4, but got Unknown
		switch ((int)logLevel)
		{
		case 1:
			return Level.Debug;
		case 2:
			return Level.Info;
		case 3:
			return Level.Warning;
		case 4:
		case 5:
			return Level.Error;
		default:
			return Level.Verbose;
		}
	}

	protected LoggerBase(Logger loggerInstance)
	{
		_logger = loggerInstance ?? throw new ArgumentNullException("loggerInstance", "LoggerInstance passed to constructor in Logger is null.");
	}

	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		if (IsEnabled(logLevel))
		{
			string text = formatter(state, exception);
			if (exception != null)
			{
				text += $"\n{exception}";
			}
			_logger.Log(NSpringLevel(logLevel), text);
		}
	}

	public bool IsEnabled(LogLevel logLevel)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected I4, but got Unknown
		switch ((int)logLevel)
		{
		case 0:
			return _logger.Level <= Level.Verbose;
		case 1:
			return _logger.Level <= Level.Debug;
		case 2:
			return _logger.Level <= Level.Info;
		case 3:
			return _logger.Level <= Level.Warning;
		case 4:
		case 5:
			return _logger.Level <= Level.Error;
		default:
			return false;
		}
	}

	public IDisposable BeginScope<TState>(TState state) where TState : notnull
	{
		return new NullLogScope();
	}
}
