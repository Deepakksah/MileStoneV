namespace VideoOS.Management.VmoClient;

public sealed class PublicViewGroupPermissionInfo : PermissionInfo
{
	public PublicViewGroupPermissionSet PermissionType => (PublicViewGroupPermissionSet)base.PermissionTypeInternal;
}
