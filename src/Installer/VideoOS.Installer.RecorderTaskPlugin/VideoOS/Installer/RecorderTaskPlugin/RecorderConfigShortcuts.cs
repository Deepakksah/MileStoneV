using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VideoOS.Common.Integration;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.RecorderTaskPlugin;

public static class RecorderConfigShortcuts
{
	public static void CreateRecorderAndFailoverShortcuts(Dictionary<string, string> parameters)
	{
		LogManager.Write("CreateRecorderAndFailoverShortcuts: Started...");
		string text = parameters["INSTALLLOCATION"];
		if (string.IsNullOrEmpty(text))
		{
			throw new ArgumentException("INSTALLLOCATION parameter is not set");
		}
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms);
		string companyName = OemResources.Instance.App.CompanyName;
		string text2 = Path.Combine(folderPath, companyName);
		string executable = Path.Combine(text, "VideoOS.Recorder.Service.TrayController.exe");
		string icon = Path.Combine(text, "Recorder.ico");
		if (!Directory.Exists(text2))
		{
			Directory.CreateDirectory(text2);
		}
		new ShortcutManager(Path.Combine(text2, OemResources.Instance.Recorder.StartMenuShortcutTrayControllerDisplayName + ".lnk")).Create(executable, string.Empty, icon);
		new ShortcutManager(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup), OemResources.Instance.Recorder.StartMenuShortcutTrayControllerDisplayName + ".lnk")).Create(executable, string.Empty, icon);
		string text3 = parameters["ISFAILOVER"];
		if (text3 != null && string.Equals(text3, bool.TrueString, StringComparison.OrdinalIgnoreCase))
		{
			text = text.Replace(OemResources.Instance.Recorder.FullNameInstaller, OemResources.Instance.FailoverServer.DefaultInstallDir);
			executable = Path.Combine(text, "VideoOS.Failover.Service.TrayController.exe");
			icon = Path.Combine(text, "FailoverServer.ico");
			new ShortcutManager(Path.Combine(text2, OemResources.Instance.FailoverServer.StartMenuShortcutTrayControllerDisplayName + ".lnk")).Create(executable, string.Empty, icon);
			new ShortcutManager(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup), OemResources.Instance.FailoverServer.StartMenuShortcutTrayControllerDisplayName + ".lnk")).Create(executable, string.Empty, icon);
		}
	}

	public static void RemoveRecorderAndFailoverShortcuts()
	{
		LogManager.Write("RemoveRecorderAndFailoverShortcuts: Started...");
		try
		{
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms);
			string companyName = OemResources.Instance.App.CompanyName;
			string path = Path.Combine(folderPath, companyName);
			new ShortcutManager(Path.Combine(path, OemResources.Instance.Recorder.StartMenuShortcutTrayControllerDisplayName + ".lnk")).Remove();
			new ShortcutManager(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup), OemResources.Instance.Recorder.StartMenuShortcutTrayControllerDisplayName + ".lnk")).Remove();
			new ShortcutManager(Path.Combine(path, OemResources.Instance.FailoverServer.StartMenuShortcutTrayControllerDisplayName + ".lnk")).Remove();
			new ShortcutManager(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup), OemResources.Instance.FailoverServer.StartMenuShortcutTrayControllerDisplayName + ".lnk")).Remove();
		}
		catch (Exception ex)
		{
			LogManager.Write("Failed to delete recorder and failover shortcuts during uninstallation", ex.ToString(), TraceEventType.Error);
		}
	}

	public static void RemoveStartMenuFolder()
	{
		LogManager.Write("RemoveStartMenuFolder: Started...");
		try
		{
			string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms), OemResources.Instance.App.CompanyName);
			if (Directory.Exists(path) && Directory.GetFiles(path).Count() == 0)
			{
				Directory.Delete(path, recursive: true);
			}
		}
		catch (Exception ex)
		{
			LogManager.Write("Failed to delete the start menu folder during uninstallation", ex.ToString(), TraceEventType.Error);
		}
	}
}
