namespace VideoOS.Management.Server;

public sealed class PermissionAuditLogService : IPermissionAuditLogService, IGeminiServerService
{
	private bool _disablePermissionLogs;

	public bool GetDisablePermissionLogsSetting => _disablePermissionLogs;

	public void StartService(IGeminiRequestContext systemRequestContext)
	{
		_disablePermissionLogs = bool.Parse(Config.Instance.GetAppSetting("DisablePermissionAuditLogs", "false"));
		using IManagementServerComponent managementServerComponent = systemRequestContext.CreateManagementServerComponent();
		managementServerComponent.SetSystemSetting("DisablePermissionAuditLogs", _disablePermissionLogs.ToString());
	}

	public void StopService(IGeminiRequestContext systemRequestContext)
	{
	}
}
