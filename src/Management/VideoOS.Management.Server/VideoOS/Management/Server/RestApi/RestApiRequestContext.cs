using System;
using System.Net.Http;
using System.Web;

namespace VideoOS.Management.Server.RestApi;

public class RestApiRequestContext : GeminiRequestContext, IRestApiRequestContext, IGeminiRequestContext, IDisposable
{
	public IRestApiRequestParameters RestApiRequestParameters { get; }

	public RestApiRequestContext(GeminiApplicationHost applicationHost, HttpRequestMessage httpRequest)
		: base(applicationHost, RequestContextType.UserContext)
	{
		RestApiRequestParameters = new RestApiRequestParameters(httpRequest);
		SetAuthenticationPropertiesUsingAccessToken(httpRequest);
		FillRemote(httpRequest);
		RestApiCache.Initialize(this);
	}

	public RestApiRequestContext(GeminiApplicationHost applicationHost, Uri uri)
		: base(applicationHost, RequestContextType.UserContext)
	{
		RestApiRequestParameters = new RestApiRequestParameters(uri);
	}

	private void FillRemote(HttpRequestMessage httpRequest)
	{
		if (httpRequest.Properties.ContainsKey("MS_HttpContext"))
		{
			HttpRequestBase request = ((HttpContextWrapper)httpRequest.Properties["MS_HttpContext"]).Request;
			base.RemotePort = request.ServerVariables["REMOTE_PORT"];
			base.RemoteIPAddress = request.ServerVariables["REMOTE_ADDR"];
			base.LocalIPAddress = request.ServerVariables["LOCAL_ADDR"];
		}
		else
		{
			base.RemotePort = "0";
			base.RemoteIPAddress = "1.1.1.1";
			base.LocalIPAddress = "0.0.0.0";
		}
	}
}
