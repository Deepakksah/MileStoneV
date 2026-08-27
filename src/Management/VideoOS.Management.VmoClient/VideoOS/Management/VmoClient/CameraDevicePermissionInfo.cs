namespace VideoOS.Management.VmoClient;

public sealed class CameraDevicePermissionInfo : PermissionInfo
{
	public CameraDevicePermissionSet PermissionType => (CameraDevicePermissionSet)base.PermissionTypeInternal;
}
