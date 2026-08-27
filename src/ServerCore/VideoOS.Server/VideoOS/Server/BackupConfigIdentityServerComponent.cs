using VideoOS.Utilities.Database;
using VideoOS.Vms.BackupRestore;

namespace VideoOS.Server;

public sealed class BackupConfigIdentityServerComponent : BackupConfigComponentBase
{
	public BackupConfigIdentityServerComponent(ISqlConnectionManager sqlConnectionManager)
		: base(sqlConnectionManager, BackupResources.SurveillanceIdentityServerBackupFileName, "IdentityServer")
	{
	}

	public override string ConfigData()
	{
		return "";
	}

	public override BackupValidationResult ValidateForBackup(ReportComponentStatus reportStatus, AbortRequested abortRequested)
	{
		return BackupValidationResult.CreateSuccessResult();
	}
}
