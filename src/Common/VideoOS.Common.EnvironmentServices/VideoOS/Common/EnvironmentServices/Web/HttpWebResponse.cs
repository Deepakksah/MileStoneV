using System;
using System.Net;

namespace VideoOS.Common.EnvironmentServices.Web;

public class HttpWebResponse : WebResponse, IHttpWebResponse, IWebResponse, IDisposable
{
	private readonly System.Net.HttpWebResponse _webResponse;

	public HttpStatusCode StatusCode => _webResponse.StatusCode;

	public HttpWebResponse(System.Net.HttpWebResponse webResponse)
		: base(webResponse)
	{
		_webResponse = webResponse;
	}
}
