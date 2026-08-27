using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.GatewayService.InstallerPlugin.GatewayServiceInstallerUtils;

public class GatewayTokenCacheFactory : IGatewayTokenCacheFactory
{
	public IGatewayTokenCache CreateTokenCache(IIdentityServerTokenHandler tokenHandler)
	{
		return new GatewayTokenCache(tokenHandler);
	}
}
