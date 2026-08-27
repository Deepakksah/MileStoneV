using System.Drawing;

namespace VideoOS.Common.Integration;

public interface IRecorderResources
{
	string ServiceName { get; }

	string FullName { get; }

	string FullNameInstaller { get; }

	string HttpHeaderServerName { get; }

	string ApplicationName { get; }

	string ApplicationNameSimple { get; }

	string ConfigFileName { get; }

	string SecureFolderName { get; }

	string TableDatabaseName { get; }

	string ConfigFilePath { get; }

	string CommonApplicationDataFolder { get; }

	string LocalApplicationDataFolder { get; }

	string SecureDataFolder { get; }

	string InstancesFolder { get; }

	string StartMenuShortcutTrayControllerDisplayName { get; }

	string DefaultInstallDirectory { get; }

	string DriverDefaultInstallDirectory { get; }

	string PluginsDefaultInstallDirectory { get; }

	string NativeDriversDefaultInstallDirectory { get; }

	string CurrentUserApplicationRegistryKeyName { get; }

	string LocalMachineApplicationRegistryKeyName { get; }

	Icon RecorderIcon { get; }

	Icon TrayIconIndeterminate { get; }

	Icon TrayIconRunning { get; }

	Icon TrayIconRunningDisconnected { get; }

	Icon TrayIconStarting { get; }

	Icon TrayIconStopped { get; }

	Icon TrayIconStopping { get; }
}
