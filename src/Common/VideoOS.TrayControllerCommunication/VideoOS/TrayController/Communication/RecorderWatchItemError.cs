using Google.Protobuf.Reflection;

namespace VideoOS.TrayController.Communication;

public enum RecorderWatchItemError
{
	[OriginalName("None")]
	None,
	[OriginalName("SslCommunication")]
	SslCommunication,
	[OriginalName("Authorization")]
	Authorization
}
