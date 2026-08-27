using VideoOS.Common.EnvironmentServices.WebSockets;

namespace VideoOS.Common.WebSockets;

public class WebSocketBinaryWriter : WebSocketWriter<byte[]>
{
	public WebSocketBinaryWriter(IWebSocket webSocket)
		: base(webSocket)
	{
	}

	protected override byte[] FillBuffer(byte[] message)
	{
		return message;
	}
}
