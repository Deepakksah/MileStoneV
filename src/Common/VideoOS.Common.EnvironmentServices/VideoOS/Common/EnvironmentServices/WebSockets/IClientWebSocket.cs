using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Common.EnvironmentServices.WebSockets;

public interface IClientWebSocket : IWebSocket, IDisposable
{
	IClientWebSocketOptions Options { get; }

	WebSocketCloseStatus? CloseStatus { get; }

	string CloseStatusDescription { get; }

	string SubProtocol { get; }

	Task ConnectAsync(Uri uri, CancellationToken cancellationToken);
}
