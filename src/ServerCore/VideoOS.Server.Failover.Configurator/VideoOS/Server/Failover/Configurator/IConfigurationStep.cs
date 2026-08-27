using System.Threading.Tasks;

namespace VideoOS.Server.Failover.Configurator;

public interface IConfigurationStep
{
	Task ExecuteAsync();
}
