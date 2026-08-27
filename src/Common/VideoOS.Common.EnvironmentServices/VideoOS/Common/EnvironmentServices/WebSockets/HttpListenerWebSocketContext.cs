using System.Net.WebSockets;

namespace VideoOS.Common.EnvironmentServices.WebSockets;

public class HttpListenerWebSocketContext : IHttpListenerWebSocketContext
{
	private readonly System.Net.WebSockets.HttpListenerWebSocketContext _context;

	private IWebSocket _webSocket;

	public IWebSocket WebSocket => _webSocket ?? (_webSocket = new WebSocket(_context.WebSocket));

	public HttpListenerWebSocketContext(System.Net.WebSockets.HttpListenerWebSocketContext context)
	{
		_context = context;
	}
}
