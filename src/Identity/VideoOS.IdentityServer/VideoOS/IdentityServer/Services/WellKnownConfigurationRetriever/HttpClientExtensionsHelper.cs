using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace VideoOS.IdentityServer.Services.WellKnownConfigurationRetriever;

public class HttpClientExtensionsHelper : IHttpClientExtensionsHelper
{
	public async Task<DiscoveryDocumentResponse> GetDiscoveryDocumentAsync(HttpClient client, DiscoveryDocumentRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		return await client.GetDiscoveryDocumentAsync(request, cancellationToken);
	}
}
