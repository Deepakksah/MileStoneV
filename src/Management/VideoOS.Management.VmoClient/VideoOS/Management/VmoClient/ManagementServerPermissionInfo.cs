namespace VideoOS.Management.VmoClient;

public class ManagementServerPermissionInfo : PermissionInfo
{
	public ManagementServerPermissionSet PermissionType => (ManagementServerPermissionSet)base.PermissionTypeInternal;
}
