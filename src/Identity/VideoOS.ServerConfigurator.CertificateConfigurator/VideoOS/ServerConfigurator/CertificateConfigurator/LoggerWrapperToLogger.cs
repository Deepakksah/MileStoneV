using System;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public class LoggerWrapperToLogger : MarshalByRefObject, ILogger
{
	private readonly IWrapperLogger _logger;

	public string LogFilePath => null;

	public LoggerWrapperToLogger(IWrapperLogger logger)
	{
		_logger = logger;
	}

	public void LogDebug(string entry)
	{
		_logger?.LogDebug(entry);
	}

	public void LogError(string entry)
	{
		_logger?.LogError(entry);
	}

	public void LogInfo(string entry)
	{
		_logger?.LogInfo(entry);
	}

	public void LogWarning(string entry)
	{
		_logger?.LogWarning(entry);
	}
}
