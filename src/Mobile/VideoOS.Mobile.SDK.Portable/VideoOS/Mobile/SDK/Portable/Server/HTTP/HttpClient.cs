using VideoOS.Mobile.Portable.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.HTTP;

public class HttpClient : IClientContext
{
	private static IHttpWebRequestFactory _httpWebRequestFactory;

	public string Protocol { get; private set; }

	public static IHttpWebRequestFactory HttpWebRequestFactory
	{
		get
		{
			return _httpWebRequestFactory ?? (_httpWebRequestFactory = new HttpWebRequestFactory());
		}
		set
		{
			_httpWebRequestFactory = value;
		}
	}

	public IRequest CreateRequest(string webAddress)
	{
		return new HttpClientObject(webAddress);
	}

	public HttpClient(bool isSecured)
	{
		Protocol = (isSecured ? "https://" : "http://");
	}
}
