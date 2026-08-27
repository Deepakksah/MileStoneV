namespace VideoOS.Management.VmoClient;

public sealed class MicrophoneDevicePermissionInfo : PermissionInfo
{
	public MicrophoneDevicePermissionSet PermissionType => (MicrophoneDevicePermissionSet)base.PermissionTypeInternal;
}
