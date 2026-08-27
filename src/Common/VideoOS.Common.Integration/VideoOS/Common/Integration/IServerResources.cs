using System;
using System.Drawing;

namespace VideoOS.Common.Integration;

public interface IServerResources
{
	string ConfigFilePath { get; }

	string ConfigFileName { get; }

	string IISWebApplicationName { get; }

	string ServiceName { get; }

	string FullName { get; }

	string DefaultInstallDir { get; }

	string OldDefaultInstallDir { get; }

	string DesktopShortcutDisplayName { get; }

	string StartMenuShortcutTrayControllerDisplayName { get; }

	string ICPApplicationName { get; }

	string VSaaSApplicationName { get; }

	string AddRemoveProgramComment { get; }

	string ApplicationName { get; }

	string ApplicationNameSimple { get; }

	Icon ChangeLicenseIcon { get; }

	Icon ChangeSqlAddressIcon { get; }

	Icon ServerIcon { get; }

	Icon TrayIconIndeterminate { get; }

	Icon TrayIconRunning { get; }

	Icon TrayIconStarting { get; }

	Icon TrayIconStopped { get; }

	Icon TrayIconStopping { get; }

	string RegistrySubPath { get; }

	string RegistrySubPathVersioned { get; }

	string LocalMachineRegistryPath { get; }

	string CurrentUserRegistryPath { get; }

	string CommonApplicationDataFolder { get; }

	string LocalApplicationDataFolder { get; }

	Uri OnlineActivationUrl { get; }

	string ManagementServerServiceName { get; }

	string LogServerServiceName { get; }

	string EventServerServiceName { get; }

	string DataCollectorServiceName { get; }
}
