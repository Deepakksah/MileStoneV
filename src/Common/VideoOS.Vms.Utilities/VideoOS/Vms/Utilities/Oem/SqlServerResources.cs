using VideoOS.Utilities.OemResource;

namespace VideoOS.Vms.Utilities.Oem;

public class SqlServerResources : ApplicationResources
{
	public string ApplicationName => GetResourceString("DatabaseServerApplicationName");

	public string AddRemoveProgramComment => GetResourceString("DatabaseServerAddRemoveProgramComment");

	public string DefaultInstallDir => GetResourceString("DatabaseServerInstallDir");

	public string FullName => GetResourceString("DatabaseServerFullName").Replace("VERSION", OemResources.Instance.App.Version);

	public string FullNameInstaller => GetResourceString("DatabaseServerInstallerFullName").Replace("VERSION", OemResources.Instance.App.Version);

	public SqlServerResources(OemResourcePackage resourcePackage)
		: base(resourcePackage)
	{
	}
}
