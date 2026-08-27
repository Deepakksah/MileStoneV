using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using VideoOS.Utilities.OemResource;

namespace VideoOS.Common.Integration;

public class RecorderResources : ApplicationResources, IRecorderResources
{
	public string ServiceName => GetResourceString("RecorderServiceName");

	public string FullName => GetResourceString("RecordingServerFullName").Replace("VERSION", Version);

	public string FullNameInstaller => GetResourceString("RecordingServerInstallerFullName").Replace("VERSION", Version);

	public string HttpHeaderServerName => GetResourceString("RecorderHttpHeaderServerName");

	public string ApplicationName => GetResourceString("RecorderApplicationName");

	public string ApplicationNameSimple => GetResourceString("RecorderApplicationNameSimple");

	public string ConfigFileName => "RecorderConfig.xml";

	public string SecureFolderName => "Secure";

	public string TableDatabaseName => "TablesDb";

	public string ConfigFilePath => Path.GetFullPath(Path.Combine(CommonApplicationDataFolder, ConfigFileName));

	public string CommonApplicationDataFolder => Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), OemResources.Instance.App.CompanyName), OemResources.Instance.Recorder.DefaultInstallDirectory);

	public string LocalApplicationDataFolder => Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), OemResources.Instance.App.CompanyName), OemResources.Instance.Recorder.DefaultInstallDirectory);

	public string SecureDataFolder => Path.Combine(CommonApplicationDataFolder, SecureFolderName);

	public string InstancesFolder => Path.Combine(CommonApplicationDataFolder, "Instances");

	public string StartMenuShortcutTrayControllerDisplayName => GetResourceString("RecorderStartMenuTrayControllerShortcutName");

	public string DefaultInstallDirectory => GetResourceString("RecorderInstallDir");

	public string DriverDefaultInstallDirectory => GetResourceString("RecorderDriversInstallDir");

	public string PluginsDefaultInstallDirectory => GetResourceString("RecorderPluginsInstallDir");

	public string NativeDriversDefaultInstallDirectory => GetResourceString("RecorderNativeDriversInstallDir");

	public string CurrentUserApplicationRegistryKeyName => string.Format(CultureInfo.InvariantCulture, "HKEY_CURRENT_USER\\SOFTWARE\\{0}\\{1}", OemResources.Instance.App.CompanyName, OemResources.Instance.Recorder.ApplicationNameSimple);

	public string LocalMachineApplicationRegistryKeyName => string.Format(CultureInfo.InvariantCulture, "HKEY_LOCAL_MACHINE\\SOFTWARE\\{0}\\{1}", OemResources.Instance.App.CompanyName, OemResources.Instance.Recorder.ApplicationNameSimple);

	public Icon RecorderIcon => GetResourceIcon("RecorderIcon");

	public Icon TrayIconIndeterminate => GetResourceIcon("RecorderTrayIconIndeterminate");

	public Icon TrayIconRunning => GetResourceIcon("RecorderTrayIconRunning");

	public Icon TrayIconRunningDisconnected => GetResourceIcon("RecorderTrayIconRunningDisconnected");

	public Icon TrayIconStarting => GetResourceIcon("RecorderTrayIconStarting");

	public Icon TrayIconStopped => GetResourceIcon("RecorderTrayIconStopped");

	public Icon TrayIconStopping => GetResourceIcon("RecorderTrayIconStopping");

	public RecorderResources(OemResourcePackage resourcePackage)
		: base(resourcePackage)
	{
	}
}
