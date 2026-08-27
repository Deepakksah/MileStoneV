using System.Collections.Generic;
using VideoOS.Installer.Configuration;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public interface IInstallerManager
{
	Dictionary<string, string> Parameters { get; }

	IEnumerable<InstallerPackageType> InstallerPackages { get; }
}
