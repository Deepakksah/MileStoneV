using System.Threading.Tasks;
using VideoOS.Common.EnvironmentServices.WebSockets;

namespace VideoOS.Common.EnvironmentServices.Web;

public interface IHttpListenerContext
{
	IHttpListenerRequest Request { get; }

	IHttpListenerResponse Response { get; }

	Task<IHttpListenerWebSocketContext> AcceptWebSocketAsync(string subProtocol);
}
