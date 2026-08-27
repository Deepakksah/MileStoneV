using System;
using System.Net;
using Newtonsoft.Json;

namespace VideoOS.IdentityServer.Common.Model.Responses;

public class ExternalProviderDiscoveryResponse : DiscoveryResponseBase
{
	public ExternalProviderModel[] ExternalProviders { get; }

	public ExternalProviderDiscoveryResponse(HttpStatusCode statusCode, string reason)
		: base(statusCode, reason)
	{
	}

	public ExternalProviderDiscoveryResponse(Exception exception, string reason)
		: base(exception, reason)
	{
	}

	public ExternalProviderDiscoveryResponse(Uri identityProviderUri, string httpContent)
		: base(identityProviderUri, httpContent)
	{
		ExternalProviders = JsonConvert.DeserializeObject<ExternalProviderModel[]>(httpContent);
	}

	public ExternalProviderDiscoveryResponse(string reason, ExternalProviderDiscoveryResponse[] errorResponses)
		: base(reason, errorResponses)
	{
	}
}
