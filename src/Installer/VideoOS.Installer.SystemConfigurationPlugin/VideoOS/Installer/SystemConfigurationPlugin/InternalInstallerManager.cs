using System.Collections.Generic;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Manager;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class InternalInstallerManager : IInstallerManager
{
	public Dictionary<string, string> Parameters => InstallerManager.Instance.Parameters;

	public IEnumerable<InstallerPackageType> InstallerPackages => InstallerManager.Instance.InstallerConfig.InstallerPackages;
}
