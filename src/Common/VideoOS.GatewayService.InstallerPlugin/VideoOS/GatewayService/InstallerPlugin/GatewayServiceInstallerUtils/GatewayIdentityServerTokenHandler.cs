using System;
using System.Net;
using VideoOS.IdentityServer.Common;
using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.GatewayService.InstallerPlugin.GatewayServiceInstallerUtils;

public class GatewayIdentityServerTokenHandler : IdentityServerWindowsTokenHandler, IGatewayIdentityServerTokenHandler, IIdentityServerTokenHandler, IDisposable
{
	public GatewayIdentityServerTokenHandler(IIdentityServerClientSettings identityServerClientSettings)
		: base(identityServerClientSettings)
	{
	}

	public GatewayIdentityServerTokenHandler(ICredentials credentials, IIdentityServerClientSettings identityServerClientSettings)
		: base(credentials, identityServerClientSettings)
	{
	}
}
