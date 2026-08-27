using System;
using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.Management.Server;

public interface ITokenCacheFactory
{
	ITokenCache CreateTokenCache(string secret, Uri authorizationServerUri, string clientId, string grantType, string scope);
}
