using System;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Platform.Messaging;

public interface IMessageClient
{
	ClientState State { get; }

	event EventHandler<ClientState> StateChanged;

	Task<Guid> SubscribeAsync<T>(string topic, MessageReceiverAsync receiver, CancellationToken cancellationToken);

	Task UnsubscribeAsync(Guid subscriptionId, CancellationToken cancellationToken);

	Task PublishAsync(string topic, object data, CancellationToken cancellationToken);
}
