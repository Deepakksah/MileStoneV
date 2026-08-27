using System.Threading;
using System.Threading.Tasks;
using VideoOS.IdentityServer.Model.Result;

namespace VideoOS.IdentityServer.Services.WellKnownConfigurationRetriever;

public interface IWellKnownConfigurationRetriever
{
	Task<Result<WellKnownConfigurationResponse>> GetWellKnownConfiguration(string address, bool requireHttps, CancellationToken cancellationToken = default(CancellationToken));
}
