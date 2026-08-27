namespace VideoOS.Reporting.Web.Plugin.Business.Model;

public class RecorderStatus
{
	public bool Unknown { get; private set; }

	public bool ServerUnavailable { get; private set; }

	public RecorderAttachState AttachState { get; }

	public RecorderConnectionState ConnectionState { get; }

	public static RecorderStatus StatusUnknown { get; } = new RecorderStatus(RecorderAttachState.Unknown, RecorderConnectionState.Unknown)
	{
		Unknown = true
	};

	public static RecorderStatus StatusServerUnavailable { get; } = new RecorderStatus(RecorderAttachState.NotConnected, RecorderConnectionState.Disconnected)
	{
		ServerUnavailable = true
	};

	public RecorderStatus(RecorderAttachState attachState, RecorderConnectionState connectionState)
	{
		Unknown = false;
		ServerUnavailable = false;
		AttachState = attachState;
		ConnectionState = connectionState;
	}
}
