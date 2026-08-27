using Google.Protobuf.Reflection;

namespace VideoOS.TrayController.Communication;

public enum ConnectStatus_Communication
{
	[OriginalName("Disconnected")]
	Disconnected,
	[OriginalName("Reconnecting")]
	Reconnecting,
	[OriginalName("Connecting")]
	Connecting,
	[OriginalName("Connected")]
	Connected,
	[OriginalName("ConnectedAttachPending")]
	ConnectedAttachPending,
	[OriginalName("SslCommunicationError")]
	SslCommunicationError
}
