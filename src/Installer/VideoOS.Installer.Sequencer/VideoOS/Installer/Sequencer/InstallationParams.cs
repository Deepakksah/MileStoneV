using System;
using System.Collections.Generic;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Interfaces;

namespace VideoOS.Installer.Sequencer;

public class InstallationParams
{
	public string InstallerSourceDir { get; private set; }

	public InstallerConfig InstallerConfiguration { get; private set; }

	public InstallInfo InstallInfo { get; private set; }

	public Dictionary<string, List<IPackageParameter>> PackageParameters { get; set; }

	public InstallationParams(string installerSourceDir, InstallerConfig installerConfiguration, InstallInfo installInfo, Dictionary<string, List<IPackageParameter>> packageParameters)
	{
		InstallerSourceDir = installerSourceDir ?? throw new ArgumentNullException("installerSourceDir");
		InstallerConfiguration = installerConfiguration ?? throw new ArgumentNullException("installerConfiguration");
		InstallInfo = installInfo ?? throw new ArgumentNullException("installInfo");
		PackageParameters = packageParameters ?? throw new ArgumentNullException("packageParameters");
	}
}
