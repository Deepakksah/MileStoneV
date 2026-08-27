using System;
using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.Management.VmoClient;

public interface ISiteVmoConnector
{
	IVmoClient TryConnect(Guid siteId, Uri address, ITokenCache tokenCache);

	IVmoClient TryConnectLegacy(Guid siteId, Uri address);

	IVmoClient TryConnectLegacyAsUser(Guid siteId, Uri address, string username, string password);
}
