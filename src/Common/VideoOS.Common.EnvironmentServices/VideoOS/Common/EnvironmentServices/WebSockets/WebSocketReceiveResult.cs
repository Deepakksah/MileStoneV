using System.Net.WebSockets;

namespace VideoOS.Common.EnvironmentServices.WebSockets;

public class WebSocketReceiveResult : IWebSocketReceiveResult
{
	private System.Net.WebSockets.WebSocketReceiveResult _receiveResult;

	public int Count => _receiveResult.Count;

	public WebSocketMessageType MessageType => _receiveResult.MessageType;

	public bool EndOfMessage => _receiveResult.EndOfMessage;

	public WebSocketReceiveResult(System.Net.WebSockets.WebSocketReceiveResult receiveResult)
	{
		_receiveResult = receiveResult;
	}
}
