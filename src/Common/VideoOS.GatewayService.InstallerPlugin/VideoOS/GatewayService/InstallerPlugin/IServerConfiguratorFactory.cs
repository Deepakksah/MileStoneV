using System.Collections.Generic;
using VideoOS.Installer.Interfaces;

namespace VideoOS.GatewayService.InstallerPlugin;

public interface IServerConfiguratorFactory
{
	IServerConfigurator CreateServerConfigurator(IEnumerable<IPackageParameter> installParameters, bool installation);
}
