namespace VideoOS.Management.VmoClient;

public sealed class VideoWallPermissionInfo : PermissionInfo
{
	public VideoWallPermissionSet PermissionType => (VideoWallPermissionSet)base.PermissionTypeInternal;
}
