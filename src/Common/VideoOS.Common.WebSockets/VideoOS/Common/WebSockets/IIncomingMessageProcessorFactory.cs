using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Common.WebSockets;

public interface IIncomingMessageProcessorFactory<TIncoming, TOutgoing>
{
	Task<IIncomingServerMessageProcessor<TIncoming, TOutgoing>> CreateAsync(int connectionId, CancellationToken cancellationToken);
}
