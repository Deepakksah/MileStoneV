using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Common.WebSockets;

public interface IWebSocketReader<T>
{
	Task<T> ReadAsync(CancellationToken cancellationToken);
}
