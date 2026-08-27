using System;
using VideoOS.IdentityServer.Common;
using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.Management.Server;

public class TokenCacheFactory : ITokenCacheFactory
{
	public ITokenCache CreateTokenCache(string secret, Uri authorizationServerUri, string clientId, string grantType, string scope)
	{
		if (string.IsNullOrEmpty(secret))
		{
			throw new ArgumentNullException("secret");
		}
		if (authorizationServerUri == null)
		{
			throw new ArgumentNullException("authorizationServerUri");
		}
		if (string.IsNullOrEmpty(clientId))
		{
			throw new ArgumentNullException("clientId");
		}
		if (string.IsNullOrEmpty(scope))
		{
			throw new ArgumentNullException("scope");
		}
		return new TokenCache(new IdentityServerTokenHandler(new IdentityServerClientSettings(authorizationServerUri, clientId, grantType), scope, secret));
	}
}
