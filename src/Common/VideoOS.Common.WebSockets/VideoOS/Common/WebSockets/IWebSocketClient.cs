using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Common.EnvironmentServices.Web;

namespace VideoOS.Common.WebSockets;

public interface IWebSocketClient<TOutgoing> : IWebSocketConnection<TOutgoing>, IMessageProcessor<TOutgoing>, IDisposable
{
	CloseStatus CloseStatus { get; }

	Task<IHttpResponseMessage> ConnectAsync(Uri uri, CancellationToken cancellationToken);

	Task<IHttpResponseMessage> ConnectAsync(Uri uri, IEnumerable<KeyValuePair<string, string>> headers, CancellationToken cancellationToken);

	Task DisconnectAsync(int millisecondsTimeout);

	Task SendAsync(TOutgoing message, CancellationToken cancellationToken);
}
