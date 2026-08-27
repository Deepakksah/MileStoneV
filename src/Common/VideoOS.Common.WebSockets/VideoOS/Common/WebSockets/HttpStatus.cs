using System.Collections.Specialized;
using System.Net;

namespace VideoOS.Common.WebSockets;

public struct HttpStatus
{
	public static readonly HttpStatus Success = new HttpStatus(HttpStatusCode.OK, string.Empty);

	public static HttpStatus UpgradeRequired = new HttpStatus(HttpStatusCode.UpgradeRequired, string.Empty, new NameValueCollection
	{
		["Connection"] = "Upgrade",
		["Upgrade"] = "websocket",
		["Sec-WebSocket-Version"] = "13"
	});

	public HttpStatusCode StatusCode { get; }

	public string StatusDescription { get; }

	public NameValueCollection Headers { get; }

	public HttpStatus(HttpStatusCode statusCode, string statusDescription)
		: this(statusCode, statusDescription, new NameValueCollection())
	{
	}

	private HttpStatus(HttpStatusCode statusCode, string statusDescription, NameValueCollection headers)
	{
		StatusCode = statusCode;
		StatusDescription = statusDescription;
		Headers = headers;
	}

	public bool IsSuccess()
	{
		int statusCode = (int)StatusCode;
		if (statusCode >= 100)
		{
			return statusCode <= 399;
		}
		return false;
	}
}
