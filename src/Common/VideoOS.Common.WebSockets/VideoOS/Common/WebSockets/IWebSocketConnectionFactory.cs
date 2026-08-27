using VideoOS.Common.EnvironmentServices.WebSockets;

namespace VideoOS.Common.WebSockets;

public interface IWebSocketConnectionFactory<TIncoming, TOutgoing>
{
	IWebSocketConnection<TOutgoing> Create(IMessageProcessor<TIncoming> messageProcessor, IWebSocket webSocket, long id);
}
