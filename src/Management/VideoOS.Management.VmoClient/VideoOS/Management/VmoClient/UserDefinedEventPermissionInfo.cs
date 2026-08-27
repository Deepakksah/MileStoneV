namespace VideoOS.Management.VmoClient;

public class UserDefinedEventPermissionInfo : PermissionInfo
{
	public UserDefinedEventPermissionSet PermissionType => (UserDefinedEventPermissionSet)base.PermissionTypeInternal;
}
