using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace VideoOS.IdentityServer.Services.WellKnownConfigurationRetriever;

public interface IHttpClientExtensionsHelper
{
	Task<DiscoveryDocumentResponse> GetDiscoveryDocumentAsync(HttpClient client, DiscoveryDocumentRequest request, CancellationToken cancellationToken = default(CancellationToken));
}
