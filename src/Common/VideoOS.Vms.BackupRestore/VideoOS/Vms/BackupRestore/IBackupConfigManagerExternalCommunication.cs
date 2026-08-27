using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace VideoOS.Vms.BackupRestore;

public interface IBackupConfigManagerExternalCommunication
{
	IList<RegisteredService> GetRegisteredServices { get; }

	ILogger Logger { get; }

	bool DirectoryExists(string directoryPath);

	void DeleteFiles(string directoryPath);

	void DeleteFiles(string directoryPath, IList<string> fileNames);

	void WriteConfigToFile(BackupConfigSettings backupConfigSettings, string path);

	string CompressFiles(string destinationPath, IList<string> sourceFiles, ReportProgress reportStatus, AbortRequested abortRequested);
}
