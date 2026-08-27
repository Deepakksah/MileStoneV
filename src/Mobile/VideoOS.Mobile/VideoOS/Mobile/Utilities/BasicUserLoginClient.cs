using System;
using System.Net;
using VideoOS.IdentityServer.Common;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.IdentityServer.Common.Handlers.IdentityServerToken;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Mobile.Utilities;

public class BasicUserLoginClient : ILoginClient
{
	public Uri Url { get; set; }

	public NetworkCredential Credentials { get; set; }

	public void Login(Guid instanceId, string currentToken)
	{
		using ResourceOwnerTokenHandler tokenHandler = new ResourceOwnerTokenHandler(Credentials.UserName, Credentials.Password, new IdentityServerClientSettings(new Uri(Url, "IDP"), "GrantValidatorClient", "password"));
		using SimpleTokenCache tokenCache = new SimpleTokenCache(tokenHandler);
		using IServerCommandServiceProxy serverCommandServiceProxy = ServerCommandServiceProxy.CreateNew(Url.Host, Url.Port, Url.Scheme.Equals(Uri.UriSchemeHttps), tokenCache);
		Guid instanceId2 = Guid.NewGuid();
		serverCommandServiceProxy.Login(instanceId2, null);
		serverCommandServiceProxy.Logout(instanceId2, null);
	}
}
