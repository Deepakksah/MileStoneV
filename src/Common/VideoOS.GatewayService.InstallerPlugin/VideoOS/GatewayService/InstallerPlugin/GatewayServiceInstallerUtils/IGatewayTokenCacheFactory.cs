using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.GatewayService.InstallerPlugin.GatewayServiceInstallerUtils;

public interface IGatewayTokenCacheFactory
{
	IGatewayTokenCache CreateTokenCache(IIdentityServerTokenHandler tokenHandler);
}
