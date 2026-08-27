using System;
using System.Net;

namespace VideoOS.GatewayService.InstallerPlugin.GatewayServiceInstallerUtils;

public interface ITokenHandlerFactory
{
	IGatewayIdentityServerTokenHandler CreateTokenHandler(ICredentials credentials, Uri idpAddress);
}
