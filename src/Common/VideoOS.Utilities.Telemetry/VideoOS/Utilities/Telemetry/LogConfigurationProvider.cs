using System;
using Microsoft.Extensions.Configuration;
using VideoOS.Utilities.Telemetry.Configuration;

namespace VideoOS.Utilities.Telemetry;

public class LogConfigurationProvider : ILogConfigurationProvider
{
	private readonly IConfigurationRoot _logConfigurationRoot;

	public LogConfigurationProvider(IConfigurationRoot logConfigurationRoot)
	{
		_logConfigurationRoot = logConfigurationRoot ?? throw new ArgumentNullException("logConfigurationRoot");
	}

	public LoggerConfiguration GetLoggerConfiguration(string loggerName)
	{
		return GetLoggerConfiguration<LoggerConfiguration>(((IConfiguration)_logConfigurationRoot).GetSection("LogConfiguration:Loggers"), loggerName);
	}

	public LocalizedLoggerConfiguration GetLocalizedLoggerConfiguration(string loggerName)
	{
		return GetLoggerConfiguration<LocalizedLoggerConfiguration>(((IConfiguration)_logConfigurationRoot).GetSection("LogConfiguration:LocalizedLoggers"), loggerName);
	}

	public T GetLogProviderConfiguration<T>(string providerName)
	{
		if (string.IsNullOrEmpty(providerName))
		{
			throw new ArgumentNullException("providerName", "Provider name cannot be null or empty.");
		}
		return ConfigurationBinder.Get<T>((IConfiguration)(object)ConfigurationExtensions.GetRequiredSection((IConfiguration)(object)ConfigurationExtensions.GetRequiredSection((IConfiguration)(object)_logConfigurationRoot, "LogConfiguration:Providers"), providerName));
	}

	private static T GetLoggerConfiguration<T>(IConfigurationSection loggerConfiguration, string loggerName)
	{
		if (string.IsNullOrEmpty(loggerName))
		{
			throw new ArgumentNullException("loggerName", "Logger name cannot be null or empty.");
		}
		if (!ConfigurationExtensions.Exists(loggerConfiguration))
		{
			return default(T);
		}
		return ConfigurationBinder.Get<T>((IConfiguration)(object)ConfigurationExtensions.GetRequiredSection((IConfiguration)(object)loggerConfiguration, loggerName));
	}
}
