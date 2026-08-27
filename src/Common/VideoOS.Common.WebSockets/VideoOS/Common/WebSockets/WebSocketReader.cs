using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Common.EnvironmentServices.WebSockets;

namespace VideoOS.Common.WebSockets;

public abstract class WebSocketReader<TIncoming> : IWebSocketReader<TIncoming>
{
	private const int _maxAllowedMessageSize = 10485760;

	private readonly byte[] _buffer = new byte[4096];

	private readonly IWebSocket _webSocket;

	public WebSocketReader(IWebSocket webSocket)
	{
		_webSocket = webSocket;
	}

	protected abstract Task<TIncoming> ReadFromStreamAsync(Stream stream, CancellationToken cancellationToken);

	public async Task<TIncoming> ReadAsync(CancellationToken cancellationToken)
	{
		ArraySegment<byte> bufferSegment = new ArraySegment<byte>(_buffer);
		MemoryStream stream = null;
		try
		{
			IWebSocketReceiveResult receiveResult;
			do
			{
				if (_webSocket.State != WebSocketState.Open && _webSocket.State != WebSocketState.CloseSent)
				{
					return default(TIncoming);
				}
				receiveResult = await _webSocket.ReceiveAsync(bufferSegment, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				if (await IsClosing(receiveResult, stream, cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
				{
					return default(TIncoming);
				}
				stream = await UpdateInputStream(stream, receiveResult, bufferSegment).ConfigureAwait(continueOnCapturedContext: false);
			}
			while (!receiveResult.EndOfMessage);
			stream.Seek(0L, SeekOrigin.Begin);
			return await ReadFromStreamAsync(stream, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		finally
		{
			stream?.Dispose();
		}
	}

	private async Task<bool> IsClosing(IWebSocketReceiveResult receiveResult, MemoryStream stream, CancellationToken cancellationToken)
	{
		if (receiveResult.MessageType == WebSocketMessageType.Close)
		{
			if (_webSocket.State == WebSocketState.CloseReceived)
			{
				await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			return true;
		}
		if (receiveResult.MessageType != WebSocketMessageType.Text)
		{
			await _webSocket.CloseAsync(WebSocketCloseStatus.InvalidMessageType, "", cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			return true;
		}
		if (stream != null && stream.Position + receiveResult.Count > 10485760)
		{
			await _webSocket.CloseAsync(WebSocketCloseStatus.MessageTooBig, "", cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			return true;
		}
		return false;
	}

	private async Task<MemoryStream> UpdateInputStream(MemoryStream stream, IWebSocketReceiveResult receiveResult, ArraySegment<byte> bufferSegment)
	{
		if (stream != null)
		{
			await stream.WriteAsync(bufferSegment.Array, bufferSegment.Offset, receiveResult.Count).ConfigureAwait(continueOnCapturedContext: false);
		}
		else if (receiveResult.EndOfMessage)
		{
			stream = new MemoryStream(bufferSegment.Array, 0, bufferSegment.Offset + receiveResult.Count);
		}
		else
		{
			stream = new MemoryStream();
			await stream.WriteAsync(bufferSegment.Array, bufferSegment.Offset, receiveResult.Count).ConfigureAwait(continueOnCapturedContext: false);
		}
		return stream;
	}

	public static IWebSocketReader<TIncoming> Create(IWebSocket webSocket)
	{
		if (typeof(TIncoming) == typeof(byte[]))
		{
			return (IWebSocketReader<TIncoming>)new WebSocketBinaryReader(webSocket);
		}
		Encoding uTF = Encoding.UTF8;
		if (typeof(TIncoming) == typeof(string))
		{
			return (IWebSocketReader<TIncoming>)new WebSocketTextReader(webSocket, uTF);
		}
		return new WebSocketJsonReader<TIncoming>(webSocket, uTF);
	}
}
