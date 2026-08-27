using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.Common.EnvironmentServices.Web;
using VideoOS.Common.EnvironmentServices.WebSockets;

namespace VideoOS.Common.WebSockets;

public class WebSocketClient<TIncoming, TOutgoing> : WebSocketConnection<TIncoming, TOutgoing>, IWebSocketClient<TOutgoing>, IWebSocketConnection<TOutgoing>, IMessageProcessor<TOutgoing>, IDisposable
{
	private readonly IClientWebSocket _webSocket;

	public CloseStatus CloseStatus { get; private set; }

	public WebSocketClient(IMessageProcessor<TIncoming> messageProcessor, IClientWebSocket webSocket, IWebSocketReader<TIncoming> reader, IWebSocketWriter<TOutgoing> writer, long id, ILogger logger)
		: base(messageProcessor, (IWebSocket)webSocket, reader, writer, id, logger)
	{
		_webSocket = webSocket;
	}

	public async Task<IHttpResponseMessage> ConnectAsync(Uri uri, CancellationToken cancellationToken)
	{
		await _webSocket.ConnectAsync(uri, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		return null;
	}

	public async Task<IHttpResponseMessage> ConnectAsync(Uri uri, IEnumerable<KeyValuePair<string, string>> headers, CancellationToken cancellationToken)
	{
		foreach (KeyValuePair<string, string> header in headers)
		{
			_webSocket.Options.SetRequestHeader(header.Key, header.Value);
		}
		return await ConnectAsync(uri, cancellationToken);
	}

	public override async Task RunAsync(CancellationToken cancellationToken)
	{
		await base.RunAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		SetCloseStatus();
	}

	public async Task DisconnectAsync(int millisecondsTimeout)
	{
		await CloseAsync(CloseStatus.NormalClosure, millisecondsTimeout).ConfigureAwait(continueOnCapturedContext: false);
		SetCloseStatus();
	}

	public async Task SendAsync(TOutgoing message, CancellationToken cancellationToken)
	{
		await ProcessMessageAsync(message, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	private void SetCloseStatus()
	{
		WebSocketCloseStatus webSocketCloseStatus = _webSocket.CloseStatus ?? WebSocketCloseStatus.Empty;
		if (webSocketCloseStatus != WebSocketCloseStatus.Empty)
		{
			string closeStatusDescription = _webSocket.CloseStatusDescription;
			CloseStatus = new CloseStatus(webSocketCloseStatus, closeStatusDescription);
		}
		else if (_webSocket.State == WebSocketState.Aborted)
		{
			CloseStatus = new CloseStatus(WebSocketCloseStatus.EndpointUnavailable, "Aborted connection");
		}
		else
		{
			CloseStatus = new CloseStatus(WebSocketCloseStatus.InternalServerError, "Connection lost for unknonw reasons");
		}
	}
}
