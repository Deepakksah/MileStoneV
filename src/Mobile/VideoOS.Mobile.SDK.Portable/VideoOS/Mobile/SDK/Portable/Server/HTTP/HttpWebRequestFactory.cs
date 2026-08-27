using System.Net;

namespace VideoOS.Mobile.SDK.Portable.Server.HTTP;

public class HttpWebRequestFactory : IHttpWebRequestFactory
{
	public HttpWebRequest Create(string webAddress)
	{
		HttpWebRequest httpWebRequest = WebRequest.CreateHttp(webAddress);
		httpWebRequest.AllowReadStreamBuffering = false;
		return httpWebRequest;
	}
}
