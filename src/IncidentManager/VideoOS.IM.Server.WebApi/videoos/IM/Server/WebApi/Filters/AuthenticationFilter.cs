using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using VideoOS.IM.Server.Logger;
using VideoOS.IM.Server.Service.ConfigSettings;
using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.IM.Server.WebApi.Filters;

public class AuthenticationFilter : IAuthenticationFilter, IFilter
{
	private readonly OpenIdTokenHandler _openIdTokenHandler;

	public bool AllowMultiple => false;

	public AuthenticationFilter()
	{
		Uri authorizationServerUri = ConfigSettingsService.Instance.AuthorizationServerUri;
		_openIdTokenHandler = new OpenIdTokenHandler(authorizationServerUri, new List<string> { "incidentmanagerserver" }, authorizationServerUri.Scheme == Uri.UriSchemeHttps);
	}

	public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
	{
		HttpRequestMessage request = context.Request;
		AuthenticationHeaderValue authorization = request.Headers.Authorization;
		if (authorization == null || authorization.Scheme != "Bearer")
		{
			return;
		}
		if (string.IsNullOrEmpty(authorization.Parameter))
		{
			context.ErrorResult = new AuthenticationFailureResult("Missing bearer token", request);
			return;
		}
		try
		{
			IPrincipal principal = await _openIdTokenHandler.ValidateToken(authorization.Parameter, cancellationToken);
			if (principal == null)
			{
				context.ErrorResult = new AuthenticationFailureResult("Invalid bearer token", request);
			}
			else
			{
				context.Principal = principal;
			}
		}
		catch (SecurityTokenValidationException arg)
		{
			LoggerProvider.Instance.Logger.LogError($"Error validating bearer token: {arg}");
			context.ErrorResult = new AuthenticationFailureResult("Invalid bearer token", request);
		}
	}

	public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}
}
