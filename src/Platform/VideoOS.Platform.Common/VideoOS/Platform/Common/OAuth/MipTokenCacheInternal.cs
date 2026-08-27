using System;
using System.Net;
using VideoOS.IdentityServer.Common;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.IdentityServer.Common.Utilities.Tokens;
using VideoOS.Platform.OAuth;

namespace VideoOS.Platform.Common.OAuth;

public class MipTokenCacheInternal : IMipTokenCache, IDisposable
{
	private readonly ITokenCache _tokenCache;

	private bool _isDisposed;

	public string Token
	{
		get
		{
			if (_tokenCache != null)
			{
				return _tokenCache.RefreshToken().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
					.GetResult()
					.RawData;
			}
			return string.Empty;
		}
	}

	public ITokenCache TokenCache => _tokenCache;

	public MipTokenCacheInternal(ITokenCache tokenCache)
	{
		_tokenCache = tokenCache;
	}

	public MipTokenCacheInternal(Uri address)
	{
		IdentityServerWindowsTokenHandler tokenHandler = new IdentityServerWindowsTokenHandler((NetworkCredential)CredentialCache.DefaultCredentials, new IdentityServerClientSettings(address, "GrantValidatorClient", "windows_credentials"));
		_tokenCache = new TokenCache(tokenHandler);
	}

	public MipTokenCacheInternal(Uri address, NetworkCredential networkCredential, bool isBasicUser)
	{
		_tokenCache = new TokenCache((!isBasicUser) ? ((IIdentityServerTokenHandler)new IdentityServerWindowsTokenHandler(networkCredential, new IdentityServerClientSettings(address, "GrantValidatorClient", "windows_credentials"))) : ((IIdentityServerTokenHandler)new ResourceOwnerTokenHandler(networkCredential.UserName, networkCredential.Password, new IdentityServerClientSettings(address, "GrantValidatorClient", "password"))));
	}

	public MipTokenCacheInternal(Uri address, string clientId, IIdpTokens idpTokens)
	{
		RefreshTokenHandler tokenHandler = new RefreshTokenHandler(idpTokens, new IdentityServerClientSettings(address, clientId, "refresh_token"));
		_tokenCache = new TokenCache(tokenHandler);
	}

	public MipTokenCacheInternal(Uri address, string clientId, string scope, string secret)
	{
		IdentityServerTokenHandler tokenHandler = new IdentityServerTokenHandler(new IdentityServerClientSettings(address, clientId, "password"), scope, secret);
		_tokenCache = new TokenCache(tokenHandler);
	}

	public MipTokenCacheInternal(Uri address, IMipTokenCache topSiteTokenCache)
	{
		IdentityServerClientSettings identityServerClientSettings = new IdentityServerClientSettings(address, "VmsClient");
		ExchangeTokenHandler tokenHandler = new ExchangeTokenHandler(topSiteTokenCache.Token, identityServerClientSettings);
		_tokenCache = new TokenCache(tokenHandler);
	}

	public bool IsTokenExpired()
	{
		if (_tokenCache == null)
		{
			return false;
		}
		return _tokenCache.IsTokenExpired();
	}

	public string GetClaimValue(string key)
	{
		return _tokenCache?.GetClaimValue(key);
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
