using VideoOS.Utilities.Telemetry.Configuration;

namespace VideoOS.Utilities.Telemetry;

public interface ILogConfigurationProvider
{
	LoggerConfiguration GetLoggerConfiguration(string loggerName);

	LocalizedLoggerConfiguration GetLocalizedLoggerConfiguration(string loggerName);

	T GetLogProviderConfiguration<T>(string providerName);
}
