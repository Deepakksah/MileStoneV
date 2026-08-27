using VideoOS.LogServer.Communication.LogConfigurationServiceReference;

namespace VideoOS.Management.Server;

public interface ILogServerConfigurationComponent
{
	LogRetentions QueryLogServerConfiguration();

	void UpdateLogServerConfiguration(LogRetentions logServerConfiguration);
}
