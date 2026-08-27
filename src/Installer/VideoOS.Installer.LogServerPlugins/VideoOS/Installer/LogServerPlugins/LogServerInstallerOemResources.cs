using System;
using System.IO;
using VideoOS.Utilities.Database;
using VideoOS.Utilities.OemResource;

namespace VideoOS.Installer.LogServerPlugins;

public sealed class LogServerInstallerOemResources : OemResourceReaderBase, ISqlRegistryPathResources
{
	public string CommonApplicationDataFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), CompanyNameShort, LogServerServerInstallDir);

	public string OemCompanyNameShort => CompanyNameShort;

	public string OemInstallDir => LogServerServerInstallDir;

	private string CompanyNameShort => GetResourceString("CompanyNameShort");

	private string LogServerServerInstallDir => GetResourceString("LogServerServerInstallDir");

	public LogServerInstallerOemResources(IResourcesFolderLocator resourcesFolderLocator)
		: base(resourcesFolderLocator)
	{
	}
}
