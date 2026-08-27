using System;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.IdentityServer.Common.Handlers.IdentityServerToken;

namespace VideoOS.GatewayService.InstallerPlugin.GatewayServiceInstallerUtils;

public class GatewayTokenCache : SimpleTokenCache, IGatewayTokenCache, ITokenCache, IDisposable
{
	public GatewayTokenCache(IIdentityServerTokenHandler tokenHandler)
		: base(tokenHandler)
	{
	}
}
