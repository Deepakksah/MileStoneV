using System.Net.Http;

namespace VideoOS.Management.Server.RestApi;

public class RestApiRequestContextFactory : IRestApiRequestContextFactory
{
	public IRestApiRequestContext GetRequestContext(HttpRequestMessage httpRequest)
	{
		return (IRestApiRequestContext)GeminiApplicationCore.RuntimeServiceHost.CreateContext(RequestContextType.UserContext, HostRequestType.RestApi, httpRequest);
	}
}
