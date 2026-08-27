namespace VideoOS.Management.VmoClient;

public sealed class InputDevicePermissionInfo : PermissionInfo
{
	public InputDevicePermissionSet PermissionType => (InputDevicePermissionSet)base.PermissionTypeInternal;
}
