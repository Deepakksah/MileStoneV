using System;
using System.Net;
using VideoOS.IdentityServer.Common;

namespace VideoOS.GatewayService.InstallerPlugin.GatewayServiceInstallerUtils;

public class TokenHandlerFactory : ITokenHandlerFactory
{
	public IGatewayIdentityServerTokenHandler CreateTokenHandler(ICredentials credentials, Uri idpAddress)
	{
		return new GatewayIdentityServerTokenHandler(credentials, new IdentityServerClientSettings(idpAddress, "GrantValidatorClient"));
	}
}
