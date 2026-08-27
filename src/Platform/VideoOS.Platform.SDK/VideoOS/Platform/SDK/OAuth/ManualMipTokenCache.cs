using System;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.Platform.Common.OAuth;
using VideoOS.Platform.OAuth;

namespace VideoOS.Platform.SDK.OAuth;

public class ManualMipTokenCache : IMipTokenCache, IDisposable
{
	private string _accessTokenString;

	private ManualTokenCache _accessTokenCache;

	private bool _isDisposed;

	public string Token => _accessTokenString;

	public ITokenCache TokenCache => _accessTokenCache;

	public ManualMipTokenCache(string accessToken)
	{
		_accessTokenString = accessToken;
		_accessTokenCache = new ManualTokenCache(_accessTokenString);
	}

	public void UpdateAccessToken(string accessToken)
	{
		_accessTokenCache = new ManualTokenCache(accessToken);
		_accessTokenString = accessToken;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_isDisposed && disposing)
		{
			if (_accessTokenCache != null)
			{
				_accessTokenCache.Dispose();
				_accessTokenCache = null;
			}
			_isDisposed = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	public string GetClaimValue(string key)
	{
		return _accessTokenCache.GetClaimValue(key);
	}

	public bool IsTokenExpired()
	{
		return _accessTokenCache.IsTokenExpired();
	}
}
