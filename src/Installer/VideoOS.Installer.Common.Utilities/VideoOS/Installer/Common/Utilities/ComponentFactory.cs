using Microsoft.Extensions.Logging;

namespace VideoOS.Installer.Common.Utilities;

public class ComponentFactory : IComponentFactory
{
	public IConfigurationDataComponent CreateConfigurationComponent(string connectionString, ILogger logger)
	{
		return new ConfigurationDataComponent(connectionString, logger);
	}
}
