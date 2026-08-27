using System;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Common.WebSockets;

public interface IIncomingServerMessageProcessor<TIncoming, TOutgoing> : IMessageProcessor<TIncoming>, IDisposable
{
	Task<HttpStatus> ValidateClientAsync(NameValueCollection headers, CancellationToken cancellationToken);

	void SetResponseMessageProcessor(IMessageProcessor<TOutgoing> responseMessageProcessor);

	Task ClientDisconnectedAsync();

	void SetServerDisconnect(Func<CloseStatus, Task> requestDisconnect);
}
