using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.IdentityServer.Common.ServiceModel;

public class OpenIdClaimsAuthenticationManager : ClaimsAuthenticationManager
{
	private readonly IOpenIdTokenHandler _openIdTokenHandler;

	private readonly IOperationContextHelper _operationContextHelper;

	public const string Scheme = "Bearer";

	public OpenIdClaimsAuthenticationManager(IOpenIdTokenHandler openIdTokenHandler)
		: this(openIdTokenHandler, new OperationContextHelper())
	{
	}

	public OpenIdClaimsAuthenticationManager(IOpenIdTokenHandler openIdTokenHandler, IOperationContextHelper operationContextHelper)
	{
		if (openIdTokenHandler == null)
		{
			throw new ArgumentNullException("openIdTokenHandler");
		}
		if (operationContextHelper == null)
		{
			throw new ArgumentNullException("operationContextHelper");
		}
		_openIdTokenHandler = openIdTokenHandler;
		_operationContextHelper = operationContextHelper;
	}

	public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
	{
		string authorizationHeaderValue = _operationContextHelper.AuthorizationHeaderValue;
		if (authorizationHeaderValue == null)
		{
			return null;
		}
		AuthenticationHeaderValue val = default(AuthenticationHeaderValue);
		if (!AuthenticationHeaderValue.TryParse(authorizationHeaderValue, ref val))
		{
			return null;
		}
		if (!"Bearer".Equals(val.Scheme, StringComparison.OrdinalIgnoreCase))
		{
			return null;
		}
		if (!_openIdTokenHandler.CanReadToken(val.Parameter))
		{
			return null;
		}
		try
		{
			return _openIdTokenHandler.ValidateToken(val.Parameter).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
				.GetResult();
		}
		catch (SecurityTokenException)
		{
			return null;
		}
	}
}
