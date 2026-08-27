using System;
using System.IO;
using VideoOS.Utilities.Database;
using VideoOS.Vms.BackupRestore;

namespace VideoOS.Server;

public abstract class BackupConfigComponentBase : IConfigBackup
{
	private readonly string _backupFileName;

	public string ComponentName { get; }

	protected ISqlConnectionManager SqlConnectionManager { get; }

	protected BackupConfigComponentBase(ISqlConnectionManager sqlConnectionManager, string backupFileName, string componentName)
	{
		SqlConnectionManager = sqlConnectionManager;
		_backupFileName = backupFileName;
		ComponentName = componentName;
	}

	public abstract BackupValidationResult ValidateForBackup(ReportComponentStatus reportStatus, AbortRequested abortRequested);

	public BackupComponentResult Backup(string folderPath, ReportComponentStatus reportStatus, AbortRequested abortRequested)
	{
		if (reportStatus == null)
		{
			throw new ArgumentNullException("reportStatus");
		}
		if (abortRequested == null)
		{
			throw new ArgumentNullException("abortRequested");
		}
		reportStatus(ComponentName, 0);
		CorporateSqlData corporateSqlData = new CorporateSqlData(SqlConnectionManager);
		SqlServerInfo sqlServerInfo;
		try
		{
			sqlServerInfo = corporateSqlData.SqlServiceAccount();
		}
		catch (SqlPermissionOnConnectException ex)
		{
			return BackupComponentResult.CreateErrorResult(BackupConfigManager.BackupConfigError.SqlBackupError, ex.InnerException.Message, null, ex);
		}
		catch (DatabaseException ex2)
		{
			return BackupComponentResult.CreateErrorResult(BackupConfigManager.BackupConfigError.SqlBackupError, ex2.Message, null, ex2);
		}
		string empty = string.Empty;
		empty = ((string.Compare(sqlServerInfo.MachineName, Environment.MachineName, StringComparison.OrdinalIgnoreCase) != 0) ? Path.Combine("\\\\" + Environment.MachineName, ServerConfig.Instance.BackupShareFolderName, _backupFileName) : Path.Combine(folderPath, _backupFileName));
		try
		{
			corporateSqlData.DoBackup(empty);
		}
		catch (DatabaseException ex3)
		{
			DatabaseError[] errors = ex3.Errors;
			foreach (DatabaseError databaseError in errors)
			{
				switch (databaseError.Number)
				{
				case 40:
					return BackupComponentResult.CreateErrorResult(BackupConfigManager.BackupConfigError.SqlNetworkError, ex3.Message, null, ex3);
				case 121:
					return BackupComponentResult.CreateErrorResult(BackupConfigManager.BackupConfigError.SqlNetworkError, ex3.Message, null, ex3);
				case 3201:
					return BackupComponentResult.CreateErrorResult(BackupConfigManager.BackupConfigError.SqlAuthenticationError, ex3.Message, null, ex3);
				}
			}
			return BackupComponentResult.CreateErrorResult(BackupConfigManager.BackupConfigError.SqlBackupError, ex3.Message, null, ex3);
		}
		reportStatus(ComponentName, 50);
		if (abortRequested())
		{
			return BackupComponentResult.CreateErrorResult(BackupConfigManager.BackupConfigError.Undefined, "Corporate backup terminated due to an abort request.", null, null);
		}
		reportStatus(ComponentName, 100);
		return BackupComponentResult.CreateSuccessResult(new string[1] { _backupFileName });
	}

	public abstract string ConfigData();
}
