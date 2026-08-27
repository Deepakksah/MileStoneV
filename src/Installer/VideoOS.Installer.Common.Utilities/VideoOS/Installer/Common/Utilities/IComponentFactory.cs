using Microsoft.Extensions.Logging;

namespace VideoOS.Installer.Common.Utilities;

public interface IComponentFactory
{
	IConfigurationDataComponent CreateConfigurationComponent(string connectionString, ILogger logger);
}
