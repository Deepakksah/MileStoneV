namespace VideoOS.Vms.BackupRestore;

public enum RestoreConfigError
{
	Undefined = 0,
	InvalidStartParameters = -200001,
	FileNotFound = -200002,
	FolderNotFound = -200003,
	ExceptionOccurred = -200004,
	CreateShareFailed = -200005,
	NoComponentSettingsFound = -200006,
	CorporateDBVersionDifferent = -200007,
	RegisteredServicesAreDifferent = -200008,
	SqlServerRunningAsLocal = -200009,
	ServicesFailedToStop = -200010,
	ServicesFailedToStart = -200011,
	MachineNameAreDifferent = -200012,
	EventServerDataIncompatible = -200013,
	EventServerNotRunning = -200014,
	SqlRestoreError = -200015,
	SqlAccessDatabaseError = -200016,
	SqlAuthenticationError = -200017,
	SqlNetworkError = -200018,
	InvalidConfigurationFile = -200019,
	SqlRestoreInvalidPathError = -200020,
	SqlPermissionError = -200021
}
