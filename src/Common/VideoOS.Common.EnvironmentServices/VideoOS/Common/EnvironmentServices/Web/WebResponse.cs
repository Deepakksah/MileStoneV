using System;
using System.IO;
using System.Net;

namespace VideoOS.Common.EnvironmentServices.Web;

public class WebResponse : IWebResponse, IDisposable
{
	private readonly System.Net.WebResponse _webResponse;

	public WebResponse(System.Net.WebResponse webResponse)
	{
		_webResponse = webResponse;
	}

	public void Dispose()
	{
		_webResponse.Dispose();
	}

	public Stream GetResponseStream()
	{
		return _webResponse.GetResponseStream();
	}
}
