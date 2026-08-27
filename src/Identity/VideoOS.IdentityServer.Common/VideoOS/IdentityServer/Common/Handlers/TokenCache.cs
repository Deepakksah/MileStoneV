using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace VideoOS.IdentityServer.Common.Handlers;

public class TokenCache : ITokenCache, IDisposable
{
	private const string NameClaim = "name";

	private readonly IIdentityServerTokenHandler _tokenHandler;

	private volatile JwtSecurityToken _cachedAccessToken;

	private volatile JwtSecurityToken _cachedIdToken;

	private readonly SemaphoreSlim _refreshTokenLock = new SemaphoreSlim(1, 1);

	private readonly SemaphoreSlim _startTokenLock = new SemaphoreSlim(1, 1);

	private readonly JwtSecurityTokenHandler _jwtTokenHandler = new JwtSecurityTokenHandler();

	private ITimer _timer;

	private Exception _lastException;

	private volatile bool _isStarted;

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

	public TokenCache(IIdentityServerTokenHandler tokenHandler)
		: this(tokenHandler, new ThreadingTimerWrapper())
	{
	}

	public TokenCache(IIdentityServerTokenHandler tokenHandler, ITimer timer)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		_tokenHandler = tokenHandler ?? throw new ArgumentNullException("tokenHandler");
		_timer = timer;
		_timer.Elapsed += OnTimer;
	}

	public Task Start()
	{
		CheckDisposed();
		return Start(CancellationToken.None);
	}

	public Task Start(CancellationToken cancellationToken)
	{
		CheckDisposed();
		return CheckAndRefreshCachedToken(setStart: true, cancellationToken);
	}

	public Task<JwtSecurityToken> RefreshToken()
	{
		CheckDisposed();
		return RefreshToken(CancellationToken.None);
	}

	public string GetClaimValue(string key)
	{
		CheckDisposed();
		return (_cachedIdToken ?? throw new InvalidOperationException("Id token not available")).Claims.FirstOrDefault((Claim c) => c.Type == key)?.Value;
	}

	public async Task<JwtSecurityToken> RefreshToken(CancellationToken cancellationToken)
	{
		CheckDisposed();
		if (!_isStarted)
		{
			await _startTokenLock.WaitAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				if (!_isStarted)
				{
					await Start(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			finally
			{
				_startTokenLock.Release();
			}
		}
		cancellationToken.ThrowIfCancellationRequested();
		JwtSecurityToken cachedAccessToken = _cachedAccessToken;
		if (cachedAccessToken != null && ((SecurityToken)cachedAccessToken).ValidTo > DateTime.UtcNow)
		{
			return cachedAccessToken;
		}
		Exception lastException = _lastException;
		if (lastException != null)
		{
			throw lastException;
		}
		return await WaitForAuthorizationServer(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public Task Stop()
	{
		CheckDisposed();
		_isStarted = false;
		_timer?.StopTimer();
		return Task.CompletedTask;
	}

	public bool IsTokenExpired()
	{
		CheckDisposed();
		JwtSecurityToken cachedAccessToken = _cachedAccessToken;
		if (cachedAccessToken == null)
		{
			return false;
		}
		return ((SecurityToken)cachedAccessToken).ValidTo.ToUniversalTime() < DateTime.UtcNow;
	}

	private async Task<JwtSecurityToken> WaitForAuthorizationServer(CancellationToken cancellationToken)
	{
		await _refreshTokenLock.WaitAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		try
		{
			Exception lastException = _lastException;
			if (lastException != null)
			{
				throw lastException;
			}
			return _cachedAccessToken;
		}
		finally
		{
			_refreshTokenLock.Release();
		}
	}

	private async void OnTimer(object source, EventArgs e)
	{
		try
		{
			await CheckAndRefreshCachedToken(setStart: false, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
		}
	}

	private async Task CheckAndRefreshCachedToken(bool setStart, CancellationToken cancellationToken)
	{
		_ = 1;
		try
		{
			await _refreshTokenLock.WaitAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (setStart)
			{
				_isStarted = true;
			}
			try
			{
				if (_cachedAccessToken == null || setStart || ((SecurityToken)_cachedAccessToken).ValidTo - TimeSpan.FromMinutes(5.0) <= DateTime.UtcNow)
				{
					await GetAndSetTokensFromAuthorizationServer(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
					_lastException = null;
				}
			}
			catch (Exception lastException)
			{
				_lastException = lastException;
			}
			finally
			{
				TimeSpan delay = ((_lastException == null) ? TimeSpan.FromSeconds(10.0) : TimeSpan.FromSeconds(5.0));
				if (_isStarted)
				{
					_timer?.StartTimer(delay, Timeout.InfiniteTimeSpan);
				}
			}
		}
		finally
		{
			_refreshTokenLock.Release();
		}
	}

	private async Task GetAndSetTokensFromAuthorizationServer(CancellationToken cancellationToken)
	{
		TokenResponse tokenResponse = await _tokenHandler.GetTokenResponseAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		SetTokens(tokenResponse.Access_Token, tokenResponse.Id_Token);
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

	private void SetTokens(string accessToken, string idToken)
	{
		_cachedAccessToken = ReadToken(accessToken);
		if (!string.IsNullOrEmpty(idToken))
		{
			_cachedIdToken = ReadToken(idToken);
		}
	}

	private void Dispose(bool disposing)
	{
		if (_disposed)
		{
			return;
		}
		if (disposing)
		{
			if (_timer != null)
			{
				_timer.Dispose();
				_timer = null;
			}
			(_tokenHandler as IDisposable)?.Dispose();
			_refreshTokenLock.Dispose();
			_startTokenLock.Dispose();
		}
		_disposed = true;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void CheckDisposed()
	{
		if (_disposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
	}
}
