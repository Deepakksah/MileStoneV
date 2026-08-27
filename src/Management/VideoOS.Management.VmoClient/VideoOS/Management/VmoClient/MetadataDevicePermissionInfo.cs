namespace VideoOS.Management.VmoClient;

public sealed class MetadataDevicePermissionInfo : PermissionInfo
{
	public MetadataDevicePermissionSet PermissionType => (MetadataDevicePermissionSet)base.PermissionTypeInternal;
}
