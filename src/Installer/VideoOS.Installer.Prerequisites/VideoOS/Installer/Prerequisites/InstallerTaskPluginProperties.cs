using System.Collections.Generic;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Interfaces;

namespace VideoOS.Installer.Prerequisites;

public class InstallerTaskPluginProperties
{
	public List<IPackageParameter> PackageParameters { get; set; }

	public string Filename { get; set; }

	public InstallerPackageTypeInstallationTaskPlugin InstallationTaskPlugin { get; set; }
}
