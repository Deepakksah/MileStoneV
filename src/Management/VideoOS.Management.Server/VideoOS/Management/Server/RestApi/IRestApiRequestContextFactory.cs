using System.Net.Http;

namespace VideoOS.Management.Server.RestApi;

public interface IRestApiRequestContextFactory
{
	IRestApiRequestContext GetRequestContext(HttpRequestMessage httpRequest);
}
