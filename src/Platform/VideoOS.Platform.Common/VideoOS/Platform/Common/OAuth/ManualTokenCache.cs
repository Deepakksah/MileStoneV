using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.Platform.Common.OAuth;

public class ManualTokenCache : ITokenCache, IDisposable
{
	private string _accessTokenString;

	private const string NameClaim = "name";

	private readonly JwtSecurityTokenHandler _jwtTokenHandler = new JwtSecurityTokenHandler();

	private JwtSecurityToken _cachedAccessToken;

	public string Token
	{
		get
		{
			return _accessTokenString;
		}
		set
		{
			_accessTokenString = value;
			_cachedAccessToken = ReadToken(_accessTokenString);
		}
	}

	public string UserName
	{
		get
		{
			string claimValue = GetClaimValue("name");
			if (string.IsNullOrEmpty(claimValue))
			{
				throw new InvalidOperationException("Name claim is missing from identity token");
			}
			return claimValue;
		}
	}

	public string UserAndDomainName
	{
		get
		{
			throw new NotSupportedException(string.Format("{0} only supports {1}", GetType(), "UserName"));
		}
	}

	public ManualTokenCache(string accessToken)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		_accessTokenString = accessToken;
		_cachedAccessToken = ReadToken(_accessTokenString);
	}

	public bool IsTokenExpired()
	{
		return ((SecurityToken)_cachedAccessToken).ValidTo - TimeSpan.FromMinutes(5.0) <= DateTime.UtcNow;
	}

	public string GetClaimValue(string claimId)
	{
		if (_cachedAccessToken != null)
		{
			Claim claim = _cachedAccessToken.Claims.FirstOrDefault((Claim c) => c.Type == claimId);
			if (claim != null)
			{
				return claim.Value;
			}
		}
		return "N/A";
	}

	public void Dispose()
	{
	}

	public Task<JwtSecurityToken> RefreshToken()
	{
		return RefreshToken(CancellationToken.None);
	}

	public Task<JwtSecurityToken> RefreshToken(CancellationToken cancellationToken)
	{
		return Task.Run(() => _cachedAccessToken);
	}

	public Task Start()
	{
		return Task.CompletedTask;
	}

	public Task Start(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}

	public Task Stop()
	{
		return Task.CompletedTask;
	}

	private JwtSecurityToken ReadToken(string token)
	{
		if (!((SecurityTokenHandler)_jwtTokenHandler).CanReadToken(token))
		{
			throw new IdentityServerTokenException(FormattableString.Invariant($"Unable to read token returned from Authorization server."));
		}
		SecurityToken obj = ((TokenHandler)_jwtTokenHandler).ReadToken(token);
		return (JwtSecurityToken)(object)((obj is JwtSecurityToken) ? obj : null);
	}
}
