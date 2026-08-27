using System;
using System.Net;

namespace VideoOS.Common.EnvironmentServices.Web;

public class WebRequest : IWebRequest
{
	private readonly System.Net.WebRequest _webRequest;

	public bool UseDefaultCredentials
	{
		get
		{
			return _webRequest.UseDefaultCredentials;
		}
		set
		{
			_webRequest.UseDefaultCredentials = value;
		}
	}

	public int Timeout
	{
		get
		{
			return _webRequest.Timeout;
		}
		set
		{
			_webRequest.Timeout = value;
		}
	}

	public WebRequest(System.Net.WebRequest webRequest)
	{
		_webRequest = webRequest;
	}

	public IWebResponse GetResponse()
	{
		try
		{
			return WrapResponse(_webRequest.GetResponse());
		}
		catch (System.Net.WebException nativeException)
		{
			throw new WebException(nativeException);
		}
	}

	public static IWebResponse WrapResponse(System.Net.WebResponse response)
	{
		if (response == null)
		{
			return null;
		}
		if (response is System.Net.HttpWebResponse webResponse)
		{
			return new HttpWebResponse(webResponse);
		}
		return new WebResponse(response);
	}

	private static Exception WrapException(System.Net.WebException webException)
	{
		return new WebException(webException);
	}
}
