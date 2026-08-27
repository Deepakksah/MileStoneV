using System;
using System.Collections.Generic;
using System.IdentityModel.Configuration;
using System.ServiceModel.Description;
using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.IdentityServer.Common.ServiceModel;

public static class ServiceModelExtensions
{
	public static void AddBearerTokenBehavior(this ServiceEndpoint serviceEndpoint, ITokenCache tokenCache)
	{
		serviceEndpoint.Behaviors.Add(new BearerTokenBehaviour(tokenCache));
	}

	public static void AddOpenIdAuthentication(this ServiceCredentials serviceCredentials, Uri idpAddress, IEnumerable<string> audiences)
	{
		OpenIdTokenHandler openIdTokenHandler = new OpenIdTokenHandler(idpAddress, audiences);
		serviceCredentials.IdentityConfiguration = new IdentityConfiguration();
		serviceCredentials.IdentityConfiguration.ClaimsAuthenticationManager = new OpenIdClaimsAuthenticationManager(openIdTokenHandler);
		serviceCredentials.IdentityConfiguration.ClaimsAuthorizationManager = new OpenIdClaimsAuthorizationManager();
		serviceCredentials.UseIdentityConfiguration = true;
	}
}
