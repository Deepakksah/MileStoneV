using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Common.EnvironmentServices.WebSockets;

public class WebSocket : IWebSocket, IDisposable
{
	private System.Net.WebSockets.WebSocket _webSocket;

	public WebSocketState State => _webSocket.State;

	public WebSocket(System.Net.WebSockets.WebSocket webSocket)
	{
		_webSocket = webSocket;
	}

	public async Task<IWebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
	{
		return new WebSocketReceiveResult(await _webSocket.ReceiveAsync(buffer, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
	}

	public Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
	{
		return _webSocket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
	}

	public Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
	{
		return _webSocket.CloseAsync(closeStatus, statusDescription, cancellationToken);
	}

	public Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
	{
		return _webSocket.CloseOutputAsync(closeStatus, statusDescription, cancellationToken);
	}

	public void Dispose()
	{
		_webSocket.Dispose();
		GC.SuppressFinalize(this);
	}
}
