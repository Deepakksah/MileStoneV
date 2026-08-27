using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.IdentityServer.Common;

namespace VideoOS.Management.VmoClient;

public interface ISiteIdpConnector
{
	Task<IdpInfo> GetIdpInfo(Uri address, bool useLongTimeout, CancellationToken cancellationToken);

	Task<IdpInfo> GetFastestIdpInfo(IEnumerable<Uri> addresses, bool useLongTimeout, bool allowUnsecure, CancellationToken cancellationToken);

	Task<IEnumerable<(Uri Address, IdpInfo IdpInfo, Exception Error)>> GetIdpInfoOrError(IEnumerable<Uri> addresses, bool useLongTimeout, bool allowUnsecure, CancellationToken cancellationToken);
}
