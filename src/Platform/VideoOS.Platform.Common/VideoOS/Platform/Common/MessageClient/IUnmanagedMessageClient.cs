using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Platform.Common.MessageClient;

public interface IUnmanagedMessageClient
{
	event EventHandler<Guid> SessionIdReceived;

	event EventHandler<WebSocketCloseStatus> UnexpectedlyDisconnected;

	event EventHandler<NewMessageArgs> MessageReceived;

	Task SubscribeAsync(string topic, CancellationToken cancellationToken);

	Task ConnectAsync(Uri uri, string token, Guid? sessionId, CancellationToken cancellationToken);

	Task DisconnectAsync();

	Task UnsubscribeAsync(string topic, CancellationToken cancellationToken);

	Task PublishAsync(string topic, object data, CancellationToken cancellationToken);
}
