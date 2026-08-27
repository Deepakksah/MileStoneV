using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using VideoOS.Utilities.OemResource;
using VideoOS.Vms.Utilities.Properties;

namespace VideoOS.Vms.Utilities.Oem;

public class ServerResources : ApplicationResources, IServerResources
{
	public string ConfigFilePath => Path.GetFullPath(Path.Combine(CommonApplicationDataFolder, ConfigFileName));

	public string ConfigFileName => "ServerConfig.xml";

	public string IISWebApplicationName => GetResourceString("ServerIISWebApplicationName");

	public string ServiceName => GetResourceString("ServerServiceName");

	public string FullName => GetResourceString("ServerFullName").Replace("VERSION", Version);

	public string DefaultInstallDir => GetResourceString("ServerInstallDir");

	public string OldDefaultInstallDir => GetResourceString("OldServerProgramFolderName");

	public string DesktopShortcutDisplayName => GetResourceString("ServerDesktopShortcutDisplayName");

	public string StartMenuShortcutTrayControllerDisplayName => GetResourceString("ServerStartMenuShortcutTrayControllerDisplayName");

	public string ICPApplicationName => GetResourceString("ICPApplicationName");

	public string AddRemoveProgramComment => GetResourceString("ServerAddRemoveProgramComment");

	public string ApplicationName => GetResourceString("ServerApplicationName");

	public string ApplicationNameSimple => GetResourceString("ServerApplicationNameSimple");

	public Icon ChangeLicenseIcon => GetResourceIcon("ChangeLicenseIcon");

	public Icon ChangeSqlAddressIcon => GetResourceIcon("ChangeSqlAddressIcon");

	public Icon ServerIcon => GetResourceIcon("ServerIcon");

	public Icon TrayIconIndeterminate => GetResourceIcon("ServerTrayIconIndeterminate");

	public Icon TrayIconRunning => GetResourceIcon("ServerTrayIconRunning");

	public Icon TrayIconStarting => GetResourceIcon("ServerTrayIconStarting");

	public Icon TrayIconStopped => GetResourceIcon("ServerTrayIconStopped");

	public Icon TrayIconStopping => GetResourceIcon("ServerTrayIconStopping");

	public string RegistrySubPath => string.Format(CultureInfo.InvariantCulture, "SOFTWARE\\{0}\\{1}", OemResources.Instance.App.CompanyName, ApplicationNameSimple);

	public string RegistrySubPathVersioned => string.Format(CultureInfo.InvariantCulture, "SOFTWARE\\{0}\\{1}\\{2}.{3}", OemResources.Instance.App.CompanyName, ApplicationNameSimple, BuildInfo.Major, BuildInfo.Minor);

	public string LocalMachineRegistryPath => string.Format(CultureInfo.InvariantCulture, "HKEY_LOCAL_MACHINE\\{0}", RegistrySubPath);

	public string CurrentUserRegistryPath => string.Format(CultureInfo.InvariantCulture, "HKEY_CURRENT_USER\\{0}", RegistrySubPath);

	public string CommonApplicationDataFolder => Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), OemResources.Instance.App.CompanyName), OemResources.Instance.Server.DefaultInstallDir);

	public string LocalApplicationDataFolder => Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), OemResources.Instance.App.CompanyName), OemResources.Instance.Server.DefaultInstallDir);

	public Uri OnlineActivationUrl
	{
		get
		{
			if (string.IsNullOrEmpty(GetResourceString("ManagerOnlineActivationUrl")))
			{
				return null;
			}
			try
			{
				return new Uri(GetResourceString("ManagerOnlineActivationUrl"));
			}
			catch (UriFormatException)
			{
				return null;
			}
		}
	}

	public ServerResources(OemResourcePackage resourcePackage)
		: base(resourcePackage)
	{
	}
}
