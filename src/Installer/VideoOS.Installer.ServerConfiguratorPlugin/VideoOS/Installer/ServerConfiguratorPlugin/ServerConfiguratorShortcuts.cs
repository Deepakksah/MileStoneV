using System;
using System.Collections.Generic;
using System.IO;
using VideoOS.Common.Integration;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.ServerConfiguratorPlugin;

public static class ServerConfiguratorShortcuts
{
	public static void CreateShortcuts(Dictionary<ServerParameters, string> serverParameters)
	{
		LogManager.Write("CreateShortcuts: Started...");
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms);
		string companyName = OemResources.Instance.App.CompanyName;
		string path = Path.Combine(folderPath, companyName);
		string executable = Path.Combine(serverParameters[ServerParameters.DestinationDir], "VideoOS.Server.Service.TrayController.exe");
		string icon = Path.Combine(serverParameters[ServerParameters.DestinationDir], "Server.ico");
		new ShortcutManager(Path.Combine(path, OemResources.Instance.Server.StartMenuShortcutTrayControllerDisplayName + ".lnk")).Create(executable, string.Empty, icon);
		executable = Path.Combine(serverParameters[ServerParameters.DestinationDir], "Installation.url");
		new ShortcutManager(Path.Combine(path, OemResources.Instance.App.StartMenuShortcutInstallationPageDisplayName + ".lnk")).Create(executable, string.Empty, icon);
		executable = Path.Combine(serverParameters[ServerParameters.DestinationDir], "AdminInstallation.url");
		new ShortcutManager(Path.Combine(path, OemResources.Instance.App.StartMenuShortcutAdminInstallationPageDisplayName + ".lnk")).Create(executable, string.Empty, icon);
		executable = Path.Combine(serverParameters[ServerParameters.DestinationDir], "VideoOS.Server.Service.TrayController.exe");
		new ShortcutManager(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup), OemResources.Instance.Server.StartMenuShortcutTrayControllerDisplayName + ".lnk")).Create(executable, string.Empty, icon);
	}

	public static void RemoveShortcuts()
	{
		LogManager.Write("RemoveShortcuts: Started...");
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms);
		string companyName = OemResources.Instance.App.CompanyName;
		string path = Path.Combine(folderPath, companyName);
		new ShortcutManager(Path.Combine(path, OemResources.Instance.Server.StartMenuShortcutTrayControllerDisplayName + ".lnk")).Remove();
		new ShortcutManager(Path.Combine(path, OemResources.Instance.App.StartMenuShortcutInstallationPageDisplayName + ".lnk")).Remove();
		new ShortcutManager(Path.Combine(path, OemResources.Instance.App.StartMenuShortcutAdminInstallationPageDisplayName + ".lnk")).Remove();
		new ShortcutManager(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup), OemResources.Instance.Server.StartMenuShortcutTrayControllerDisplayName + ".lnk")).Remove();
	}
}
