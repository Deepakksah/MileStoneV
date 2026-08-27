using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Common.WebSockets;

public interface IWebSocketWriter<T>
{
	Task WriteAsync(T message, CancellationToken cancellationToken);
}
