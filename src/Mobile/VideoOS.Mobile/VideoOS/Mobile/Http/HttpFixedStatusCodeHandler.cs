using System.Net;
using VideoOS.Mobile.Portable.Http;

namespace VideoOS.Mobile.Http;

public class HttpFixedStatusCodeHandler
{
	private readonly HttpStatuses _statusCode = HttpStatuses.Ok;

	public HttpFixedStatusCodeHandler(HttpStatuses status)
	{
		_statusCode = status;
	}

	public HttpStatuses Process(HttpListenerContext context)
	{
		return _statusCode;
	}
}
