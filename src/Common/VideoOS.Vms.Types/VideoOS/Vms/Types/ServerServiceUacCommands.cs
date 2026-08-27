namespace VideoOS.Vms.Types;

public static class ServerServiceUacCommands
{
	public const string StartService = "START";

	public const string StopService = "STOP";

	public const string ChangeEncryptionConfiguration = "CHANGEENCRYPTIONCONFIGURATION";

	public const string ChangeLicense = "CHANGELICENSE";

	public const string RestoreBackupFolder = "RESTOREBACKUPFOLDER";

	public const string Restore = "RESTORE";

	public const string SetSystemRecoveryPassword = "SETSYSTEMRECOVERYPASSWORD";

	public const string ChangeSystemRecoveryPassword = "CHANGESYSTEMRECOVERYPASSWORD";

	public const string ConfigureManagementServerFailover = "CONFIGUREMANAGEMENTSERVERFAILOVER";
}
