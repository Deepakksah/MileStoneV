using System;
using System.Net;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.Platform.Common.OAuth;
using VideoOS.Platform.OAuth;

namespace VideoOS.Platform.SDK.OAuth;

public class MipTokenCache : IMipTokenCache, IDisposable
{
	private readonly MipTokenCacheInternal _tokenCache;

	private bool _isDisposed;

	public string Token => _tokenCache.Token;

	public ITokenCache TokenCache => _tokenCache.TokenCache;

	public MipTokenCache(Uri address)
	{
		_tokenCache = new MipTokenCacheInternal(address);
	}

	public MipTokenCache(Uri address, NetworkCredential networkCredential, bool isBasicUser)
	{
		_tokenCache = new MipTokenCacheInternal(address, networkCredential, isBasicUser);
	}

	public MipTokenCache(Uri address, string clientId, string scope, string secret)
	{
		_tokenCache = new MipTokenCacheInternal(address, clientId, scope, secret);
	}

	public bool IsTokenExpired()
	{
		return _tokenCache.IsTokenExpired();
	}

	public string GetClaimValue(string key)
	{
		return _tokenCache.GetClaimValue(key);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_isDisposed && disposing)
		{
			if (_tokenCache != null)
			{
				_tokenCache.Dispose();
			}
			_isDisposed = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
