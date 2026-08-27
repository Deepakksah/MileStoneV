using System;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.IdentityServer.Common;

public interface IIdpDiscoveryService
{
	Task<IdpInfo> ValidateIdpServer(Uri address, int timeout, CancellationToken cancellationToken);

	Task<IdpInfo> ValidateFasterIdpServerOnBothSchemas(Uri address, int timeout, CancellationToken cancellationToken, int extraTimeoutForHttps = 0, int defaultPortHttps = 443);

	Task<(IdpInfo IdpInfo, Exception HttpError, Exception HttpsError)> ValidateFasterIdpServerOnBothSchemasWithErrors(Uri address, int timeout, CancellationToken cancellationToken, int extraTimeoutForHttps = 0, int defaultPortHttps = 443);
}
