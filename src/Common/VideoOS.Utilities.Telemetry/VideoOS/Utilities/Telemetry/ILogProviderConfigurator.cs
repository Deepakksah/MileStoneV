using Microsoft.Extensions.Logging;

namespace VideoOS.Utilities.Telemetry;

public interface ILogProviderConfigurator
{
	string Name { get; }

	void Configure(ILoggingBuilder builder, string logProviderName, ILogConfigurationProvider logConfigurationProvider, ILogger serviceLogger);
}
