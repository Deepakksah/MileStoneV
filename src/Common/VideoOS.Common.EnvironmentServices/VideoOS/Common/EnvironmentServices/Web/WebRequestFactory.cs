using System;
using System.Net;

namespace VideoOS.Common.EnvironmentServices.Web;

public class WebRequestFactory : IWebRequestFactory
{
	public IWebRequest Create(Uri uri)
	{
		return WrapRequest(System.Net.WebRequest.Create(uri));
	}

	public static IWebRequest WrapRequest(System.Net.WebRequest request)
	{
		if (request == null)
		{
			return null;
		}
		if (request is System.Net.HttpWebRequest httpWebRequest)
		{
			return new HttpWebRequest(httpWebRequest);
		}
		return new WebRequest(request);
	}
}
