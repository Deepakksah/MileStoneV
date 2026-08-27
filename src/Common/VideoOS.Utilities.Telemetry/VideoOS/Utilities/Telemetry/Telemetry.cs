using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VideoOS.Diagnostics.Logging;
using VideoOS.Utilities.Telemetry.OpenTelemetry;

namespace VideoOS.Utilities.Telemetry;

public static class Telemetry
{
	public static ILogger GetILogger(string name)
	{
		return (ILogger)(object)LogManager.GetLogger(name);
	}

	public static ILoggerFactory GetILoggerFactory(string loggerName, Func<HttpClient> httpClientFactory, ILogger serviceLogger, string configurationFilePath = "appsettings.json")
	{
		try
		{
			return GetLoggerFactoryProvider(serviceLogger, loggerName, httpClientFactory, configurationFilePath).GetLoggerFactory(loggerName, serviceLogger);
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogError(serviceLogger, ex, "Failed to create ILoggerFactory for logger '{LoggerName}'", new object[1] { loggerName });
			throw;
		}
	}

	public static ILocalizedLoggerFactory GetILocalizedLoggerFactory(string loggerName, Func<HttpClient> httpClientFactory, ILogger serviceLogger, string configurationFilePath = "appsettings.json")
	{
		try
		{
			return GetLoggerFactoryProvider(serviceLogger, loggerName, httpClientFactory, configurationFilePath).GetLocalizedLoggerFactory(loggerName, serviceLogger);
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogError(serviceLogger, ex, "Failed to create ILocalizedLoggerFactory for logger '{LoggerName}'", new object[1] { loggerName });
			throw;
		}
	}

	private static LoggerFactoryProvider GetLoggerFactoryProvider(ILogger serviceLogger, string loggerName, Func<HttpClient> httpClientFactory, string configurationFilePath = "appsettings.json")
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		if (string.IsNullOrEmpty(loggerName))
		{
			throw new ArgumentNullException("loggerName", "Logger name cannot be null or empty.");
		}
		if (httpClientFactory == null)
		{
			throw new ArgumentNullException("httpClientFactory", "The HTTP client factory cannot be null.");
		}
		if (string.IsNullOrEmpty(configurationFilePath))
		{
			throw new ArgumentNullException("configurationFilePath", "Configuration file path cannot be null or empty.");
		}
		LogConfigurationProvider logConfigurationProvider = new LogConfigurationProvider(JsonConfigurationExtensions.AddJsonFile((IConfigurationBuilder)new ConfigurationBuilder(), configurationFilePath, true).Build());
		LogProviderConfiguratorManager logProviderConfiguratorManager = new LogProviderConfiguratorManager();
		logProviderConfiguratorManager.AddProviderConfigurator(new OpenTelemetryConfigurator(httpClientFactory));
		return new LoggerFactoryProvider(logConfigurationProvider, logProviderConfiguratorManager);
	}
}
