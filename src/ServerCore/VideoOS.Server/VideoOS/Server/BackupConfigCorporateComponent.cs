using System;
using System.IO;
using VideoOS.Utilities.Database;
using VideoOS.Vms.BackupRestore;

namespace VideoOS.Server;

public sealed class BackupConfigCorporateComponent : BackupConfigComponentBase
{
	public BackupConfigCorporateComponent(ISqlConnectionManager sqlConnectionManager)
		: base(sqlConnectionManager, BackupResources.SurveillanceBackupFileName, "Corporate")
	{
	}

	public override string ConfigData()
	{
		CorporateSqlData corporateSqlData = new CorporateSqlData(base.SqlConnectionManager);
		string dbVersion = corporateSqlData.QueryVersion();
		CorporateConfigData configData = new CorporateConfigData(dbVersion, Environment.MachineName);
		return CorporateConfigData.WriteCorporateConfigDataAsXml(configData);
	}

	public override BackupValidationResult ValidateForBackup(ReportComponentStatus reportStatus, AbortRequested abortRequested)
	{
		if (reportStatus == null)
		{
			throw new ArgumentNullException("reportStatus");
		}
		if (abortRequested == null)
		{
			throw new ArgumentNullException("abortRequested");
		}
		reportStatus(base.ComponentName, 0);
		CorporateSqlData corporateSqlData = new CorporateSqlData(base.SqlConnectionManager);
		SqlServerInfo sqlServerInfo;
		try
		{
			sqlServerInfo = corporateSqlData.SqlServiceAccount();
		}
		catch (SqlPermissionOnConnectException)
		{
			return BackupValidationResult.CreateErrorResult(BackupConfigManager.BackupConfigError.SqlBackupError, "The system can not access the master database on the SQL Server. Backup cannot be done.", null);
		}
		catch (DatabaseException)
		{
			return BackupValidationResult.CreateErrorResult(BackupConfigManager.BackupConfigError.Undefined, "Unknown error occurred when accessing master database. Backup cannot be done.", null);
		}
		if (string.Compare(sqlServerInfo.MachineName, Environment.MachineName, StringComparison.OrdinalIgnoreCase) != 0 && (string.Compare(sqlServerInfo.SqlAccountName, "LocalSystem", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(sqlServerInfo.SqlAccountName, "NT AUTHORITY\\LocalService", StringComparison.OrdinalIgnoreCase) == 0))
		{
			return BackupValidationResult.CreateErrorResult(BackupConfigManager.BackupConfigError.SqlServerRunningAsLocal, "Backup cannot be done if SQL Server is running on a remote machine and under the 'LocalSystem' or 'LocalService' accounts.", new string[1] { sqlServerInfo.MachineName });
		}
		string folderPath = Path.Combine(ServerConfig.Instance.BackupShareFolderPath, ServerConfig.Instance.BackupShareFolderName);
		long num = DirectoryUtilities.AvailableFreeSpace(folderPath);
		if (num >= 0 && num <= 1000000000)
		{
			return BackupValidationResult.CreateErrorResult(BackupConfigManager.BackupConfigError.LowOnDiskSpace, "Disk contains less that 1 GB available free space.", null);
		}
		reportStatus(base.ComponentName, 50);
		if (abortRequested())
		{
			return BackupValidationResult.CreateErrorResult(BackupConfigManager.BackupConfigError.Undefined, "Corporate backup validation terminated due to an abort request.", null);
		}
		reportStatus(base.ComponentName, 100);
		return BackupValidationResult.CreateSuccessResult();
	}
}
