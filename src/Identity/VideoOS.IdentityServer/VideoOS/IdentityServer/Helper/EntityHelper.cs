using System.Collections.Generic;
using Duende.IdentityServer.EntityFramework.Entities;

namespace VideoOS.IdentityServer.Helper;

public static class EntityHelper
{
	public static Client InstantiateClient()
	{
		return new Client
		{
			AllowedCorsOrigins = new List<ClientCorsOrigin>(),
			AllowedScopes = new List<ClientScope>(),
			AllowedGrantTypes = new List<ClientGrantType>(),
			Claims = new List<ClientClaim>(),
			ClientSecrets = new List<ClientSecret>(),
			IdentityProviderRestrictions = new List<ClientIdPRestriction>(),
			PostLogoutRedirectUris = new List<ClientPostLogoutRedirectUri>(),
			Properties = new List<ClientProperty>(),
			RedirectUris = new List<ClientRedirectUri>()
		};
	}
}
