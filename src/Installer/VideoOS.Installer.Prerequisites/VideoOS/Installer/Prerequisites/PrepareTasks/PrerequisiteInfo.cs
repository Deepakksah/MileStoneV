using System.Collections.Generic;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Configuration.Package;
using VideoOS.Installer.Sequencer.Prerequisite;

namespace VideoOS.Installer.Prerequisites.PrepareTasks;

public class PrerequisiteInfo
{
	public Dictionary<string, ProductInstallInfo> PackagesToInstall { get; set; }

	public Dictionary<string, string> PackagesCannotInstall { get; set; }

	public Dictionary<string, InstallerPrerequisiteCollectionType> InstallerPrerequisites { get; set; }

	public List<string> NotInstalledPrerequisites { get; set; }

	public Dictionary<string, PackageCollectionType> Prerequisites { get; set; }

	public Dictionary<string, InstallerTaskPluginProperties> InstallationTaskAdapter { get; set; }

	public PrerequisiteInfo()
	{
		Prerequisites = new Dictionary<string, PackageCollectionType>();
		InstallationTaskAdapter = new Dictionary<string, InstallerTaskPluginProperties>();
		PackagesToInstall = new Dictionary<string, ProductInstallInfo>();
		PackagesCannotInstall = new Dictionary<string, string>();
		InstallerPrerequisites = new Dictionary<string, InstallerPrerequisiteCollectionType>();
		NotInstalledPrerequisites = new List<string>();
	}
}
