namespace VideoOS.Vms.BackupRestore;

public interface IConfigBackup
{
	string ComponentName { get; }

	string ConfigData();

	BackupValidationResult ValidateForBackup(ReportComponentStatus reportStatus, AbortRequested abortRequested);

	BackupComponentResult Backup(string folderPath, ReportComponentStatus reportStatus, AbortRequested abortRequested);
}
