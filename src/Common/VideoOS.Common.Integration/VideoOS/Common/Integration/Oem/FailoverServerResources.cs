using System;
using System.Drawing;
using System.IO;
using VideoOS.Utilities.OemResource;

namespace VideoOS.Common.Integration.Oem;

public class FailoverServerResources : ApplicationResources, IFailoverServerResources
{
	public string ServiceName => GetResourceString("FailoverServiceName");

	public string ApplicationName => GetResourceString("FailoverApplicationName");

	public string ConfigFileName => "FailoverServerConfig.xml";

	public string ConfigFilePath => Path.GetFullPath(Path.Combine(CommonApplicationDataFolder, ConfigFileName));

	public string CommonApplicationDataFolder => Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), OemResources.Instance.App.CompanyName), OemResources.Instance.FailoverServer.DefaultInstallDir);

	public string LocalApplicationDataFolder => Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), OemResources.Instance.App.CompanyName), OemResources.Instance.FailoverServer.DefaultInstallDir);

	public string SecureDataFolder => Path.Combine(CommonApplicationDataFolder, "Secure");

	public string StartMenuShortcutTrayControllerDisplayName => GetResourceString("FailoverStartMenuShortcutTrayControllerDisplayName");

	public string DefaultInstallDir => GetResourceString("FailoverInstallDir");

	public Icon FailoverServerIcon => GetResourceIcon("FailoverServerIcon");

	public Icon TrayIconIndeterminate => GetResourceIcon("FailoverTrayIconIndeterminate");

	public Icon TrayIconRunning => GetResourceIcon("FailoverTrayIconRunning");

	public Icon TrayIconRunningDisconnected => GetResourceIcon("FailoverTrayIconRunningDisconnected");

	public Icon TrayIconStarting => GetResourceIcon("FailoverTrayIconStarting");

	public Icon TrayIconStopped => GetResourceIcon("FailoverTrayIconStopped");

	public Icon TrayIconStopping => GetResourceIcon("FailoverTrayIconStopping");

	public FailoverServerResources(OemResourcePackage resourcePackage)
		: base(resourcePackage)
	{
	}
}
