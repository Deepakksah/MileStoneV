using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Common.EnvironmentServices.WebSockets;

public interface IWebSocket : IDisposable
{
	WebSocketState State { get; }

	Task<IWebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken);

	Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken);

	Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken);

	Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken);
}
