namespace VideoOS.Vms.Types;

public enum ConnectStatus
{
	Disconnected,
	Reconnecting,
	Connecting,
	Connected,
	ConnectedAttachPending,
	SslCommunicationError
}
