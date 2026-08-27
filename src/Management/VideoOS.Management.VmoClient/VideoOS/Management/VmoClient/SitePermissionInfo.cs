namespace VideoOS.Management.VmoClient;

public class SitePermissionInfo : PermissionInfo
{
	public SitePermissionSet PermissionType => (SitePermissionSet)base.PermissionTypeInternal;
}
