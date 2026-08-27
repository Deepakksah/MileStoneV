namespace VideoOS.Management.VmoClient;

public enum ConnectionState
{
	None,
	Attached,
	AttachPending,
	Unauthorized,
	WaitRemoteAuthorize
}
