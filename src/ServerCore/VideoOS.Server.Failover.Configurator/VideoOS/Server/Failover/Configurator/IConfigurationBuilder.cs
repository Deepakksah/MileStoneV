using System.Collections.Generic;

namespace VideoOS.Server.Failover.Configurator;

public interface IConfigurationBuilder
{
	IEnumerable<IConfigurationStep> Build();
}
