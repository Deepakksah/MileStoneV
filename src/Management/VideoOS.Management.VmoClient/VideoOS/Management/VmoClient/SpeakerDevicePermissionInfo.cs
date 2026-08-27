namespace VideoOS.Management.VmoClient;

public sealed class SpeakerDevicePermissionInfo : PermissionInfo
{
	public SpeakerDevicePermissionSet PermissionType => (SpeakerDevicePermissionSet)base.PermissionTypeInternal;
}
