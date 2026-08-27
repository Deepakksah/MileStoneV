using System;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Platform.Messaging.Internal;

public interface IManagedMessageClient : IMessageClient, IDisposable
{
	Task StartAsync(CancellationToken cancellationToken);

	Task CloseAsync();
}
