namespace VideoOS.Management.Server;

public interface IPermissionAuditLogService
{
	bool GetDisablePermissionLogsSetting { get; }
}
