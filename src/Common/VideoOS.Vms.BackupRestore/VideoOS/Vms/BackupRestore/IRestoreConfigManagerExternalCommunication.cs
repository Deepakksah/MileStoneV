using VideoOS.Utilities.Database;

namespace VideoOS.Vms.BackupRestore;

public interface IRestoreConfigManagerExternalCommunication
{
	ISqlConnectionManager SqlConnectionManager { get; }

	bool DirectoryExists(string path);

	void DeleteFiles(string folderName);

	bool DeleteFolder(string fullFolderPath);

	void CreateSharedFolderSqlServerAccount(string fullBackupFolderPath, string shareName, CorporateSqlData corporateSqlData);

	void RemoveShare(string shareName);

	void CopyFileWithProgress(string sourceFileName, string targetFolderName, ReportComponentStatus reportComponentStatus);

	void DecompressFileWithProgress(string fileName, string targetFolderName, ReportComponentStatus reportComponentProgress);

	BackupConfigSettings ReadConfigFromFile(string folder);
}
