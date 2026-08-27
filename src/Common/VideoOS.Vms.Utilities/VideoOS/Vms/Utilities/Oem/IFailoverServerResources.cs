using System.Drawing;

namespace VideoOS.Vms.Utilities.Oem;

public interface IFailoverServerResources
{
	string ApplicationName { get; }

	string CommonApplicationDataFolder { get; }

	string ConfigFileName { get; }

	string ConfigFilePath { get; }

	string DefaultInstallDir { get; }

	Icon FailoverServerIcon { get; }

	string LocalApplicationDataFolder { get; }

	string ServiceName { get; }

	string StartMenuShortcutTrayControllerDisplayName { get; }

	Icon TrayIconIndeterminate { get; }

	Icon TrayIconRunning { get; }

	Icon TrayIconRunningDisconnected { get; }

	Icon TrayIconStarting { get; }

	Icon TrayIconStopped { get; }

	Icon TrayIconStopping { get; }
}
