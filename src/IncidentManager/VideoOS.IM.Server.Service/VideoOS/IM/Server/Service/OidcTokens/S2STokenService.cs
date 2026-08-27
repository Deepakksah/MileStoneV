using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using VideoOS.Common.Crypto;
using VideoOS.IM.Server.Service.Annotations;
using VideoOS.IM.Server.Service.ConfigSettings;
using VideoOS.IM.Server.Service.OEM;
using VideoOS.IdentityServer.Common;
using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.IM.Server.Service.OidcTokens;

public sealed class S2STokenService : IS2STokenService
{
	private static TokenCache _tokenCache = null;

	private static readonly object _locker = new object();

	private static readonly Lazy<IS2STokenService> LazyInstance = new Lazy<IS2STokenService>(() => new S2STokenService());

	public static IS2STokenService Instance => LazyInstance.Value;

	private S2STokenService()
	{
	}

	public void InitializeTokenCache()
	{
		if (_tokenCache != null)
		{
			return;
		}
		lock (_locker)
		{
			if (_tokenCache == null)
			{
				using (SecretHandler secretHandler = new SecretHandler(new FileSecretProvider(OemResourceService.Instance.CommonApplicationDataFolder, "idp.tmp", "idp.sec")))
				{
					secretHandler.EnsureUserEncryptedSecret();
					string secret = new string(secretHandler.ReadSecret());
					_tokenCache = new TokenCache(new IdentityServerTokenHandler(new IdentityServerClientSettings(ConfigSettingsService.Instance.AuthorizationServerUri, ConfigSettingsService.Instance.ClientRegistrationId.ToString(), "client_credentials"), "managementserver", secret));
					_tokenCache.Start();
					return;
				}
			}
		}
	}

	[NotNull]
	public async Task<string> GetAccessTokenAsync()
	{
		if (_tokenCache == null)
		{
			lock (_locker)
			{
				if (_tokenCache == null)
				{
					throw new InvalidOperationException("InitializeTokenCache should be called first.");
				}
			}
		}
		return (await _tokenCache.RefreshToken().ConfigureAwait(continueOnCapturedContext: false)).RawData;
	}

	[NotNull]
	public async Task<JwtSecurityToken> GetAccessTokenAsTokenAsync()
	{
		if (_tokenCache == null)
		{
			lock (_locker)
			{
				if (_tokenCache == null)
				{
					throw new InvalidOperationException("InitializeTokenCache should be called first.");
				}
			}
		}
		return await _tokenCache.RefreshToken().ConfigureAwait(continueOnCapturedContext: false);
	}
}
