using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.IdentityServer.Common;

public interface IIdpServerValidation
{
	Task<IIdpServerInformation> ValidateIdpServerAsync(Uri serverUri);

	Task<IIdpServerInformation> ValidateIdpServerAsync(Uri serverUri, CancellationToken cancellationToken);

	Task<List<IIdpServerInformation>> ValidateIdpServerAsync(Uri[] serverUriList);

	Task<List<IIdpServerInformation>> ValidateIdpServerAsync(Uri[] serverUriList, CancellationToken cancellationToken);

	Task ValidateIdpServerAsync(Uri[] serverUriList, Func<IIdpServerInformation, bool> idpValidationDelegate);
}
