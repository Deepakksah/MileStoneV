using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Common.WebSockets;

public interface IServerEndpoint
{
	Task RunAsync(IEnumerable<string> urlPrefixes, CancellationToken cancellationToken);
}
