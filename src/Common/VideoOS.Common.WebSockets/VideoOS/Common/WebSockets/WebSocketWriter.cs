using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Common.EnvironmentServices.WebSockets;

namespace VideoOS.Common.WebSockets;

public abstract class WebSocketWriter<TOutgoing> : IWebSocketWriter<TOutgoing>
{
	private readonly IWebSocket _webSocket;

	private readonly SemaphoreSlim _sendLock = new SemaphoreSlim(1);

	public WebSocketWriter(IWebSocket webSocket)
	{
		_webSocket = webSocket ?? throw new ArgumentNullException("webSocket");
	}

	protected abstract byte[] FillBuffer(TOutgoing message);

	public async Task WriteAsync(TOutgoing message, CancellationToken cancellationToken)
	{
		byte[] buffer = FillBuffer(message);
		await _sendLock.WaitAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		try
		{
			await _webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, endOfMessage: true, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		finally
		{
			_sendLock.Release();
		}
	}

	public static IWebSocketWriter<TOutgoing> Create(IWebSocket webSocket)
	{
		if (typeof(TOutgoing) == typeof(byte[]))
		{
			return (IWebSocketWriter<TOutgoing>)new WebSocketBinaryWriter(webSocket);
		}
		UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
		if (typeof(TOutgoing) == typeof(string))
		{
			return (IWebSocketWriter<TOutgoing>)new WebSocketTextWriter(webSocket, encoding);
		}
		return new WebSocketJsonWriter<TOutgoing>(webSocket, encoding);
	}
}
