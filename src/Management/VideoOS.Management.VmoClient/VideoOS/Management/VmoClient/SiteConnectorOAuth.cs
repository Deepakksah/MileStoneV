using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.IdentityServer.Common;
using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.Management.VmoClient;

public class SiteConnectorOAuth : ISiteConnectorOAuth
{
	private readonly ISiteTokenCacheStore _siteTokenCacheStore;

	private readonly ISiteIdpConnector _siteIdpConnector;

	private readonly ISiteVmoConnector _siteVmoConnector;

	private readonly IClientTokenSettings _clientTokenSettings;

	private readonly ISiteAddressCache _siteAddressCache;

	public SiteConnectorOAuth(ISiteTokenCacheStore siteTokenCacheStore, ISiteIdpConnector siteIdpConnector, ISiteVmoConnector siteVmoConnector, IClientTokenSettings clientTokenSettings, ISiteAddressCache siteAddressCache)
	{
		_siteTokenCacheStore = siteTokenCacheStore ?? throw new ArgumentNullException("siteTokenCacheStore");
		_siteIdpConnector = siteIdpConnector ?? throw new ArgumentNullException("siteIdpConnector");
		_siteVmoConnector = siteVmoConnector ?? throw new ArgumentNullException("siteVmoConnector");
		_clientTokenSettings = clientTokenSettings ?? throw new ArgumentNullException("clientTokenSettings");
		_siteAddressCache = siteAddressCache ?? throw new ArgumentNullException("siteAddressCache");
	}

	public async Task<ISiteConnectionResult> GetSiteConnectionResult(ISiteConnectionInfoTokenExchange siteConnectionInfoTokenExchange, CancellationToken cancellationToken)
	{
		if (siteConnectionInfoTokenExchange == null)
		{
			throw new ArgumentNullException("siteConnectionInfoTokenExchange");
		}
		if (siteConnectionInfoTokenExchange.Site.SiteVersion < ServerVersions.v23_3)
		{
			throw new NotSupportedException("Site doesn't support Token Exchange. Please use OAuth Windows flow or legacy flow.");
		}
		return await GetSiteConnectionResultInternal(siteConnectionInfoTokenExchange, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<ISiteConnectionResult> GetSiteConnectionResult(ISiteConnectionInfoWindows siteConnectionInfoWindows, CancellationToken cancellationToken)
	{
		if (siteConnectionInfoWindows == null)
		{
			throw new ArgumentNullException("siteConnectionInfoWindows");
		}
		bool flag = siteConnectionInfoWindows.Site.SiteVersion == null || siteConnectionInfoWindows.Site.SiteVersion < ServerVersions.v21_1;
		if (siteConnectionInfoWindows.ForceLegacy || flag)
		{
			throw new NotSupportedException("Site doesn't support OAuth. Please use legacy flow.");
		}
		return await GetSiteConnectionResultInternal(siteConnectionInfoWindows, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	private async Task<ISiteConnectionResult> GetSiteConnectionResultInternal(ISiteConnectionInfoBase siteConnectionInfo, CancellationToken cancellationToken)
	{
		ISite site = siteConnectionInfo.Site;
		SiteConnectionResultBuilder resultBuilder = new SiteConnectionResultBuilder();
		SiteAddressValidator siteAddressValidator = new SiteAddressValidator();
		IEnumerable<Uri> addresses = siteConnectionInfo.Site.Addresses.Select((ReadOnlyAddress f) => f.ServerUrl);
		resultBuilder.AddSiteVersion(siteConnectionInfo.Site.SiteVersion);
		var (enumerable, addressesWithInvalidScheme) = siteAddressValidator.ValidateSchemas(siteConnectionInfo);
		resultBuilder.AddAddressesWithInvalidScheme(addressesWithInvalidScheme);
		if (enumerable == null || enumerable.Count() == 0)
		{
			return resultBuilder.Build();
		}
		if (_siteTokenCacheStore.Contains(site.Id))
		{
			SiteTokenCacheStoreItem siteTokenCacheStoreItem = _siteTokenCacheStore.Get(site.Id);
			if (siteTokenCacheStoreItem.TokenCache != null && (siteTokenCacheStoreItem.HomeSite || enumerable.Contains(siteTokenCacheStoreItem.ConnectedAddress)) && TryConnect(site.Id, siteTokenCacheStoreItem.ConnectedAddress, siteTokenCacheStoreItem.TokenCache, resultBuilder))
			{
				return resultBuilder.Build();
			}
			_siteTokenCacheStore.Remove(site.Id);
		}
		Uri addressStoredInRegistry = _siteAddressCache.GetSiteAddress(site.Id);
		if (enumerable.Contains(addressStoredInRegistry))
		{
			IdpInfo idpInfo = await GetIdpInfo(addressStoredInRegistry, resultBuilder, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (idpInfo != null)
			{
				ITokenCache tokenCache = CreateTokenCache(addressStoredInRegistry, _clientTokenSettings.GetCalculatedClientId(siteConnectionInfo.Site.SiteVersion), siteConnectionInfo, resultBuilder);
				if (tokenCache != null)
				{
					if (TryConnect(site.Id, addressStoredInRegistry, tokenCache, resultBuilder))
					{
						Uri.UriSchemeHttps.Equals(idpInfo.Address.Scheme, StringComparison.OrdinalIgnoreCase);
						SiteTokenCacheStoreItem item = new SiteTokenCacheStoreItem(site.Id, site.Addresses.Select((ReadOnlyAddress x) => x.ServerUrl).ToList(), addressStoredInRegistry, tokenCache, idpInfo, homeSite: false);
						_siteTokenCacheStore.Add(item);
					}
					return resultBuilder.Build();
				}
			}
			else
			{
				resultBuilder.AddSiteAddressWithError(new SiteAddressWithError(addressStoredInRegistry, new IdpOfflineException("IDP is offline!")));
			}
		}
		IdpInfo idpInfo2 = await GetFastestIdpInfo(addresses, resultBuilder, siteConnectionInfo.AllowUnsecure, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		if (idpInfo2 == null)
		{
			return resultBuilder.Build();
		}
		Uri address = idpInfo2.Address;
		ITokenCache tokenCache2 = CreateTokenCache(address, _clientTokenSettings.GetCalculatedClientId(siteConnectionInfo.Site.SiteVersion), siteConnectionInfo, resultBuilder);
		if (tokenCache2 != null && TryConnect(site.Id, address, tokenCache2, resultBuilder))
		{
			Uri.UriSchemeHttps.Equals(idpInfo2.Address.Scheme, StringComparison.OrdinalIgnoreCase);
			SiteTokenCacheStoreItem item2 = new SiteTokenCacheStoreItem(site.Id, site.Addresses.Select((ReadOnlyAddress x) => x.ServerUrl), address, tokenCache2, idpInfo2, homeSite: false);
			_siteTokenCacheStore.Add(item2);
			_siteAddressCache.SetSiteAddress(site.Id, address);
			return resultBuilder.Build();
		}
		return resultBuilder.Build();
	}

	private ITokenCache CreateTokenCache(Uri address, string clientId, ISiteConnectionInfoBase siteConnectionInfo, ISiteConnectionResultBuilder resultBuilder)
	{
		Uri baseServerAddress = new Uri(address, "Idp");
		try
		{
			bool num = siteConnectionInfo is ISiteConnectionInfoTokenExchange;
			string grantType = (num ? "refresh_token" : "windows_credentials");
			IdentityServerClientSettings identityServerClientSettings = new IdentityServerClientSettings(baseServerAddress, clientId, grantType);
			IIdentityServerTokenHandler tokenHandler;
			if (num)
			{
				tokenHandler = new ExchangeTokenHandler(((ISiteConnectionInfoTokenExchange)siteConnectionInfo).TrustedJwtAccessToken, identityServerClientSettings);
			}
			else
			{
				ICredentials credentials = ((ISiteConnectionInfoWindows)siteConnectionInfo).Credentials;
				tokenHandler = ((!string.IsNullOrEmpty(credentials.GetCredential(null, null)?.UserName)) ? new IdentityServerWindowsTokenHandler(credentials, identityServerClientSettings) : new IdentityServerWindowsTokenHandler(identityServerClientSettings));
			}
			return new TokenCache(tokenHandler);
		}
		catch (Exception exceptionFound)
		{
			resultBuilder.AddSiteAddressWithError(new SiteAddressWithError(address, exceptionFound));
			return null;
		}
	}

	private async Task<IdpInfo> GetIdpInfo(Uri address, ISiteConnectionResultBuilder resultBuilder, CancellationToken cancellationToken)
	{
		try
		{
			return await _siteIdpConnector.GetIdpInfo(address, useLongTimeout: false, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (Exception exceptionFound)
		{
			resultBuilder.AddSiteAddressWithError(new SiteAddressWithError(address, exceptionFound));
			return null;
		}
	}

	private async Task<IdpInfo> GetFastestIdpInfo(IEnumerable<Uri> addresses, ISiteConnectionResultBuilder resultBuilder, bool allowUnsecure, CancellationToken cancellationToken)
	{
		IdpInfo fastestIdpInfo = null;
		(await _siteIdpConnector.GetIdpInfoOrError(addresses, useLongTimeout: true, allowUnsecure, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)).ToList().ForEach(delegate((Uri Address, IdpInfo IdpInfo, Exception Error) result)
		{
			if (result.Error != null)
			{
				resultBuilder.AddSiteAddressWithError(new SiteAddressWithError(result.Address, result.Error));
			}
			else if (!allowUnsecure && result.IdpInfo != null && result.IdpInfo.Address.Scheme.Equals(Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase))
			{
				resultBuilder.AddSiteAddressWithError(new SiteAddressWithError(result.Address, new UnsecureConnectionNotAllowedException()));
			}
			else if (result.IdpInfo != null)
			{
				fastestIdpInfo = result.IdpInfo;
			}
		});
		return fastestIdpInfo;
	}

	private bool TryConnect(Guid siteId, Uri address, ITokenCache tokenCache, ISiteConnectionResultBuilder resultBuilder)
	{
		try
		{
			IVmoClient vmoClient = _siteVmoConnector.TryConnect(siteId, address, tokenCache);
			if (vmoClient != null)
			{
				resultBuilder.AddVmoClient(vmoClient);
				return true;
			}
			return false;
		}
		catch (Exception exceptionFound)
		{
			resultBuilder.AddSiteAddressWithError(new SiteAddressWithError(address, exceptionFound));
			return false;
		}
	}
}
