using System.Net;
using VideoOS.Mobile.Portable.Http;

namespace VideoOS.Mobile.ReverseProxy;

public interface IRequestHandler
{
	HttpStatuses HandleRequest(HttpListenerContext context);

	HttpStatuses HandleRequest(HttpListenerContext context, bool requiresAuthentication);
}
