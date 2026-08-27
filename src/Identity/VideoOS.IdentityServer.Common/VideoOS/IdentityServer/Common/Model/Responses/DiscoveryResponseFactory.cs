using System;
using System.Net;

namespace VideoOS.IdentityServer.Common.Model.Responses;

public class DiscoveryResponseFactory : IGenericDiscoveryResponseBaseFactory<DiscoveryResponse>
{
	public DiscoveryResponse Create(HttpStatusCode statusCode, string reason)
	{
		return new DiscoveryResponse(statusCode, reason);
	}

	public DiscoveryResponse Create(Exception exception, string reason)
	{
		return new DiscoveryResponse(exception, reason);
	}

	public DiscoveryResponse Create(Uri identityProviderUri, string httpContent)
	{
		return new DiscoveryResponse(identityProviderUri, httpContent);
	}

	public DiscoveryResponse Create(string reason, DiscoveryResponse[] errorResponses)
	{
		return new DiscoveryResponse(reason, errorResponses);
	}
}
