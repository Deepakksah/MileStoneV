using System;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Common.WebSockets;

public interface IMessageProcessor<TMessage> : IDisposable
{
	Task ProcessMessageAsync(TMessage message, CancellationToken cancellationToken);
}
