using System;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.IdentityServer.Common.Model.Responses;

namespace VideoOS.IdentityServer.Common;

public interface IDiscoveryClient : IDisposable
{
	Task<DiscoveryResponse> GetAsync(CancellationToken cancellationToken = default(CancellationToken));
}
