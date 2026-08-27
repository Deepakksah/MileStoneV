using System;
using System.Net;
using VideoOS.IdentityServer.Common.Model.Responses;

public class ExternalProviderDiscoveryResponseFactory : IGenericDiscoveryResponseBaseFactory<ExternalProviderDiscoveryResponse>
{
	public ExternalProviderDiscoveryResponse Create(HttpStatusCode statusCode, string reason)
	{
		return new ExternalProviderDiscoveryResponse(statusCode, reason);
	}

	public ExternalProviderDiscoveryResponse Create(Exception exception, string reason)
	{
		return new ExternalProviderDiscoveryResponse(exception, reason);
	}

	public ExternalProviderDiscoveryResponse Create(Uri identityProviderUri, string httpContent)
	{
		return new ExternalProviderDiscoveryResponse(identityProviderUri, httpContent);
	}

	public ExternalProviderDiscoveryResponse Create(string reason, ExternalProviderDiscoveryResponse[] errorResponses)
	{
		return new ExternalProviderDiscoveryResponse(reason, errorResponses);
	}
}
