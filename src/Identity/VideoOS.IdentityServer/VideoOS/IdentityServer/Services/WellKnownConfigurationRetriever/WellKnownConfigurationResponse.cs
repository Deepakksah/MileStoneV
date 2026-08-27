using System.Collections.Generic;
using IdentityModel.Client;

namespace VideoOS.IdentityServer.Services.WellKnownConfigurationRetriever;

public class WellKnownConfigurationResponse
{
	public DiscoveryDocumentResponse DiscoveryDocumentResponse { get; set; }

	public IEnumerable<LoginProvider> LoginProviders { get; set; }
}
