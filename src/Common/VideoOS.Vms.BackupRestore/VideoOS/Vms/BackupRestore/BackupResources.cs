using VideoOS.Vms.BackupRestore.Properties;
using VideoOS.Vms.Utilities.Oem;

namespace VideoOS.Vms.BackupRestore;

public static class BackupResources
{
	public static string DefaultBackupFolderName => BackupRestoreResources.BackupDefaultShareName;

	public static string DefaultBackupFolderPath => OemResources.Instance.Server.CommonApplicationDataFolder;

	public static string BackupSettingsFileName => "CorporateBackupSettings.xml";

	public static string SurveillanceBackupFileName => "Surveillance.bak";

	public static string RestoreShareName => "RestoreFolder";

	public static string SurveillanceIdentityServerBackupFileName => "Surveillance_idp.bak";
}
