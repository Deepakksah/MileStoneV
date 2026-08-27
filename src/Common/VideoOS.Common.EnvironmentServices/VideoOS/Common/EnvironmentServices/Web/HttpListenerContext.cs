using System.Net;
using System.Threading.Tasks;
using VideoOS.Common.EnvironmentServices.WebSockets;

namespace VideoOS.Common.EnvironmentServices.Web;

public class HttpListenerContext : IHttpListenerContext
{
	private System.Net.HttpListenerContext _responseMessage;

	private IHttpListenerRequest _request;

	private IHttpListenerResponse _response;

	public IHttpListenerRequest Request => _request ?? (_request = new HttpListenerRequest(_responseMessage.Request));

	public IHttpListenerResponse Response => _response ?? (_response = new HttpListenerResponse(_responseMessage.Response));

	public HttpListenerContext(System.Net.HttpListenerContext responseMessage)
	{
		_responseMessage = responseMessage;
	}

	public async Task<IHttpListenerWebSocketContext> AcceptWebSocketAsync(string subProtocol)
	{
		return new HttpListenerWebSocketContext(await _responseMessage.AcceptWebSocketAsync(subProtocol).ConfigureAwait(continueOnCapturedContext: false));
	}
}
