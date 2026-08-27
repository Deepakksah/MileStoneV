using System.Collections.Generic;
using VideoOS.Installer.Interfaces;

namespace VideoOS.GatewayService.InstallerPlugin;

public class ServerConfiguratorFactory : IServerConfiguratorFactory
{
	public IServerConfigurator CreateServerConfigurator(IEnumerable<IPackageParameter> installParameters, bool installation)
	{
		return new ServerConfigurator(installParameters, installation);
	}
}
