using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace VideoOS.IdentityServer.Common.Handlers;

public class OpenIdTokenHandler : IOpenIdTokenHandler
{
	private readonly string[] _validAudiences;

	private readonly IConfigurationManager<OpenIdConnectConfiguration> _configurationManager;

	private readonly ISecurityTokenValidator _securityTokenHandler;

	public OpenIdTokenHandler(string idpBaseAddress, bool requireHttps = false)
		: this(new Uri(idpBaseAddress), requireHttps)
	{
	}

	public OpenIdTokenHandler(Uri idpBaseAddress, bool requireHttps = false)
		: this(idpBaseAddress, Array.Empty<string>(), requireHttps)
	{
	}

	public OpenIdTokenHandler(string idpBaseAddress, IEnumerable<string> audiences, bool requireHttps = false)
		: this(new Uri(idpBaseAddress), audiences, requireHttps)
	{
	}

	public OpenIdTokenHandler(Uri idpBaseAddress, IEnumerable<string> audiences, bool requireHttps = false)
		: this(idpBaseAddress, audiences, CreateStandardHandler(), CreateConfigurationManager(idpBaseAddress, requireHttps))
	{
		if (requireHttps && idpBaseAddress.Scheme != Uri.UriSchemeHttps)
		{
			throw new ArgumentException(FormattableString.Invariant($"Https was set as required, but the given address was {idpBaseAddress}."), "idpBaseAddress");
		}
	}

	private static ISecurityTokenValidator CreateStandardHandler()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		JwtSecurityTokenHandler val = new JwtSecurityTokenHandler();
		val.InboundClaimTypeMap.Clear();
		return (ISecurityTokenValidator)val;
	}

	private static IConfigurationManager<OpenIdConnectConfiguration> CreateConfigurationManager(Uri idpBaseAddress, bool requireHttps)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_0036: Expected O, but got Unknown
		return (IConfigurationManager<OpenIdConnectConfiguration>)(object)new ConfigurationManager<OpenIdConnectConfiguration>(idpBaseAddress.ToString().TrimEnd('/') + "/.well-known/openid-configuration", (IConfigurationRetriever<OpenIdConnectConfiguration>)new OpenIdConnectConfigurationRetriever(), (IDocumentRetriever)new HttpDocumentRetriever
		{
			RequireHttps = requireHttps
		});
	}

	public OpenIdTokenHandler(Uri idpBaseAddress, IEnumerable<string> audiences, ISecurityTokenValidator securityTokenValidator, IConfigurationManager<OpenIdConnectConfiguration> configurationManager)
	{
		if (idpBaseAddress == null)
		{
			throw new ArgumentNullException("idpBaseAddress");
		}
		if (audiences == null)
		{
			throw new ArgumentNullException("audiences");
		}
		string[] validAudiences = (audiences as string[]) ?? audiences.ToArray();
		_validAudiences = validAudiences;
		_configurationManager = configurationManager;
		_securityTokenHandler = securityTokenValidator;
	}

	public virtual Task<ClaimsPrincipal> ValidateToken(string token)
	{
		return ValidateToken(token, CancellationToken.None);
	}

	public virtual async Task<ClaimsPrincipal> ValidateToken(string token, CancellationToken cancellationToken)
	{
		if (token == null)
		{
			throw new ArgumentNullException("token");
		}
		cancellationToken.ThrowIfCancellationRequested();
		OpenIdConnectConfiguration val = await _configurationManager.GetConfigurationAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		cancellationToken.ThrowIfCancellationRequested();
		TokenValidationParameters val2 = new TokenValidationParameters
		{
			ValidIssuer = ((BaseConfiguration)val).Issuer,
			ValidAudiences = _validAudiences,
			ValidateAudience = _validAudiences.Any(),
			IssuerSigningKeys = ((BaseConfiguration)val).SigningKeys
		};
		ClaimsPrincipal claimsPrincipal;
		try
		{
			SecurityToken val3 = default(SecurityToken);
			claimsPrincipal = _securityTokenHandler.ValidateToken(token, val2, ref val3);
		}
		catch (SecurityTokenSignatureKeyNotFoundException)
		{
			_configurationManager.RequestRefresh();
			throw;
		}
		if (claimsPrincipal != null && !claimsPrincipal.HasClaim((Claim c) => c.Type.Equals("sub", StringComparison.Ordinal)))
		{
			return new SystemClaimsPrinciple(claimsPrincipal);
		}
		return claimsPrincipal;
	}

	public bool CanReadToken(string token)
	{
		return _securityTokenHandler.CanReadToken(token);
	}

	public virtual void RequestRefresh()
	{
		_configurationManager.RequestRefresh();
	}
}
