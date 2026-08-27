using System.Net.WebSockets;

namespace VideoOS.Common.EnvironmentServices.WebSockets;

public interface IWebSocketReceiveResult
{
	int Count { get; }

	WebSocketMessageType MessageType { get; }

	bool EndOfMessage { get; }
}
