using Microsoft.Extensions.Logging;
using VideoOS.Common.EnvironmentServices.WebSockets;

namespace VideoOS.Common.WebSockets;

public class WebSocketConnectionFactory<TIncoming, TOutgoing> : IWebSocketConnectionFactory<TIncoming, TOutgoing>
{
	private readonly ILogger _logger;

	public WebSocketConnectionFactory(ILogger logger)
	{
		_logger = logger;
	}

	public IWebSocketConnection<TOutgoing> Create(IMessageProcessor<TIncoming> messageProcessor, IWebSocket webSocket, long id)
	{
		return new WebSocketConnection<TIncoming, TOutgoing>(messageProcessor, webSocket, WebSocketReader<TIncoming>.Create(webSocket), WebSocketWriter<TOutgoing>.Create(webSocket), id, _logger);
	}
}
