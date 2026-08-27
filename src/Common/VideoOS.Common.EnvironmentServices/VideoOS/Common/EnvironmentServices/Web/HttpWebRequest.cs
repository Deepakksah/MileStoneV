using System.Net;

namespace VideoOS.Common.EnvironmentServices.Web;

public class HttpWebRequest : WebRequest, IHttpWebRequest, IWebRequest
{
	private readonly System.Net.HttpWebRequest _httpWebRequest;

	public bool HaveResponse => _httpWebRequest.HaveResponse;

	public HttpWebRequest(System.Net.HttpWebRequest httpWebRequest)
		: base(httpWebRequest)
	{
		_httpWebRequest = httpWebRequest;
	}
}
