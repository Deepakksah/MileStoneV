namespace VideoOS.Management.VmoClient;

public sealed class OutputDevicePermissionInfo : PermissionInfo
{
	public OutputDevicePermissionSet PermissionType => (OutputDevicePermissionSet)base.PermissionTypeInternal;
}
