using Microsoft.Extensions.Logging;

namespace VideoOS.Utilities.Telemetry;

public interface ILogProviderConfiguratorManager
{
	void AddProviderConfigurator(ILogProviderConfigurator logProviderConfigurator);

	void ConfigureProvider(ILoggingBuilder builder, string logProviderName, ILogConfigurationProvider logConfigurationProvider, ILogger serviceLogger);
}
