using System;
using System.Globalization;
using Microsoft.Extensions.Logging;
using VideoOS.Utilities.Telemetry.Configuration;

namespace VideoOS.Utilities.Telemetry;

public class LoggerFactoryProvider
{
	private readonly ILogConfigurationProvider _logConfigurationProvider;

	private readonly ILogProviderConfiguratorManager _loggingProviderFactory;

	public LoggerFactoryProvider(ILogConfigurationProvider logConfigurationProvider, ILogProviderConfiguratorManager logProviderConfiguratorManager)
	{
		_logConfigurationProvider = logConfigurationProvider ?? throw new ArgumentNullException("logConfigurationProvider", "Log configuration provider cannot be null.");
		_loggingProviderFactory = logProviderConfiguratorManager ?? throw new ArgumentNullException("logProviderConfiguratorManager", "Log provider factory cannot be null.");
	}

	public ILoggerFactory GetLoggerFactory(string loggerName, ILogger serviceLogger)
	{
		LoggerConfiguration loggerConfiguration = _logConfigurationProvider.GetLoggerConfiguration(loggerName);
		return DoConfigure(loggerConfiguration, loggerName, serviceLogger);
	}

	public ILocalizedLoggerFactory GetLocalizedLoggerFactory(string loggerName, ILogger serviceLogger)
	{
		LocalizedLoggerConfiguration localizedLoggerConfiguration = _logConfigurationProvider.GetLocalizedLoggerConfiguration(loggerName);
		CultureInfo cultureInfo = CultureInfo.InvariantCulture;
		if (localizedLoggerConfiguration != null)
		{
			if (string.IsNullOrEmpty(localizedLoggerConfiguration.LogLanguage))
			{
				throw new ArgumentNullException("Log language not found in the '" + loggerName + "' logger configuration");
			}
			cultureInfo = CultureInfo.GetCultureInfo(localizedLoggerConfiguration.LogLanguage);
		}
		LoggerExtensions.LogInformation(serviceLogger, "Log language of {LoggerName} set to {LogLanguage}", new object[2] { loggerName, cultureInfo.EnglishName });
		return new LocalizedLoggerFactory(DoConfigure(localizedLoggerConfiguration, loggerName, serviceLogger), cultureInfo);
	}

	private ILoggerFactory DoConfigure(LoggerConfiguration loggerConfiguration, string loggerName, ILogger serviceLogger)
	{
		return LoggerFactory.Create((Action<ILoggingBuilder>)delegate(ILoggingBuilder builder)
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			if (loggerConfiguration == null)
			{
				LoggerExtensions.LogWarning(serviceLogger, "No logger configuration found for {LoggerName}. Returning default factory (NullLoggerFactory).", new object[1] { loggerName });
				return;
			}
			if (!Enum.TryParse<LogLevel>(loggerConfiguration.LogLevel, out LogLevel result))
			{
				result = (LogLevel)2;
			}
			LoggerExtensions.LogInformation(serviceLogger, "Setting log level for {LoggerName} to {LogLevel}", new object[2] { loggerName, result });
			LoggingBuilderExtensions.SetMinimumLevel(builder, result);
			foreach (string provider in loggerConfiguration.Providers)
			{
				LoggerExtensions.LogInformation(serviceLogger, "Found a provider for {LoggerName} called {LogProviderName}", new object[2] { loggerName, provider });
				_loggingProviderFactory.ConfigureProvider(builder, provider, _logConfigurationProvider, serviceLogger);
			}
		});
	}
}
