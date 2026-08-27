using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Common.EnvironmentServices.WebSockets;

public class ClientWebSocket : WebSocket, IClientWebSocket, IWebSocket, IDisposable
{
	private System.Net.WebSockets.ClientWebSocket _inner;

	private IClientWebSocketOptions _options;

	public IClientWebSocketOptions Options => _options ?? (_options = new ClientWebSocketOptions(_inner.Options));

	public WebSocketCloseStatus? CloseStatus => _inner.CloseStatus;

	public string CloseStatusDescription => _inner.CloseStatusDescription;

	public string SubProtocol => _inner.SubProtocol;

	public ClientWebSocket()
		: this(new System.Net.WebSockets.ClientWebSocket())
	{
	}

	public ClientWebSocket(System.Net.WebSockets.ClientWebSocket webSocket)
		: base(webSocket)
	{
		_inner = webSocket;
	}

	public async Task ConnectAsync(Uri uri, CancellationToken cancellationToken)
	{
		await _inner.ConnectAsync(uri, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}
}
