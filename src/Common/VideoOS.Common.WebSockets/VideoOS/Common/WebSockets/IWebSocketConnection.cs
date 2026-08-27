using System;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Common.WebSockets;

public interface IWebSocketConnection<TOutgoing> : IMessageProcessor<TOutgoing>, IDisposable
{
	long Id { get; }

	Task RunAsync(CancellationToken cancellationToken);

	Task CloseAsync(CloseStatus closeStatus, int millisecondsTimeout);
}
