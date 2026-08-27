using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace VideoOS.IdentityServer.Common.Handlers.IdentityServerToken;

public class SimpleTokenCache : ITokenCache, IDisposable
{
	private const string NameClaim = "name";

	private readonly IIdentityServerTokenHandler _tokenHandler;

	private JwtSecurityToken _cachedAccessToken;

	private volatile JwtSecurityToken _cachedIdToken;

	private readonly SemaphoreSlim _refreshTokenLock = new SemaphoreSlim(1, 1);

	private readonly JwtSecurityTokenHandler _jwtTokenHandler = new JwtSecurityTokenHandler();

	private ITokenCacheNotification _tokenCacheNotification;

	private bool _disposed;

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
			throw new NotSupportedException("SimpleTokenCache only supports UserName");
		}
	}

	public SimpleTokenCache(IIdentityServerTokenHandler tokenHandler)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		_tokenHandler = tokenHandler;
	}

	public Task<JwtSecurityToken> RefreshToken()
	{
		return RefreshToken(CancellationToken.None);
	}

	public async Task<JwtSecurityToken> RefreshToken(CancellationToken cancellationToken)
	{
		await _refreshTokenLock.WaitAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		try
		{
			if (_cachedAccessToken == null || ((SecurityToken)_cachedAccessToken).ValidTo - TimeSpan.FromMinutes(5.0) <= DateTime.UtcNow)
			{
				await GetAndSetTokensFromAuthorizationServer(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		finally
		{
			_refreshTokenLock.Release();
		}
		if (_tokenCacheNotification != null && IsTokenExpired())
		{
			_tokenCacheNotification.TokenExpired();
		}
		return _cachedAccessToken;
	}

	private async Task GetAndSetTokensFromAuthorizationServer(CancellationToken cancellationToken)
	{
		TokenResponse tokenResponse = await _tokenHandler.GetTokenResponseAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		_cachedAccessToken = ReadToken(tokenResponse.Access_Token);
		if (!string.IsNullOrEmpty(tokenResponse.Id_Token))
		{
			_cachedIdToken = ReadToken(tokenResponse.Id_Token);
		}
	}

	public bool IsTokenExpired()
	{
		JwtSecurityToken cachedAccessToken = _cachedAccessToken;
		if (cachedAccessToken == null)
		{
			return false;
		}
		return ((SecurityToken)cachedAccessToken).ValidTo.ToUniversalTime() < DateTime.UtcNow;
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

	public virtual string GetClaimValue(string key)
	{
		return (_cachedIdToken ?? throw new InvalidOperationException("Id token not available")).Claims.FirstOrDefault((Claim c) => c.Type == key)?.Value;
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

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			if (disposing)
			{
				(_tokenHandler as IDisposable)?.Dispose();
				_refreshTokenLock.Dispose();
			}
			_tokenCacheNotification = null;
			_disposed = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
