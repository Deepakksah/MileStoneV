using System.Drawing;

namespace VideoOS.Common.Integration.Oem;

public interface IFailoverServerResources
{
	string ServiceName { get; }

	string ApplicationName { get; }

	string ConfigFileName { get; }

	string ConfigFilePath { get; }

	string CommonApplicationDataFolder { get; }

	string LocalApplicationDataFolder { get; }

	string SecureDataFolder { get; }

	string StartMenuShortcutTrayControllerDisplayName { get; }

	string DefaultInstallDir { get; }

	Icon FailoverServerIcon { get; }

	Icon TrayIconIndeterminate { get; }

	Icon TrayIconRunning { get; }

	Icon TrayIconRunningDisconnected { get; }

	Icon TrayIconStarting { get; }

	Icon TrayIconStopped { get; }

	Icon TrayIconStopping { get; }
}
