using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.IdentityServer.Common;

namespace VideoOS.Management.VmoClient.Internal;

public sealed class SiteConnector : ISiteConnector
{
	private const int IdpDiscoveryTimeout = 3;

	private readonly ISiteConnectorOAuth _siteConnectorOAuth;

	private readonly ISiteConnectorLegacy _siteConnectorLegacy;

	public int DiscoveryTimeout => 3;

	public SiteConnector(ISiteConnectorOAuth siteConnectorOAuth, ISiteConnectorLegacy siteConnectorLegacy)
	{
		_siteConnectorOAuth = siteConnectorOAuth ?? throw new ArgumentNullException("siteConnectorOAuth");
		_siteConnectorLegacy = siteConnectorLegacy ?? throw new ArgumentNullException("siteConnectorLegacy");
	}

	public static ISiteConnector Create(ISiteTokenCacheStore siteTokenCacheStore, IClientTokenSettings clientTokenSettings)
	{
		SiteVmoConnector siteVmoConnector = new SiteVmoConnector(new ServerConnectionFactory(), new VmoClientFactory(), new VmoClientValidator());
		SiteAddressCache siteAddressCache = new SiteAddressCache();
		SiteIdpConnector siteIdpConnector = new SiteIdpConnector(IdpDiscoveryService.Instance);
		return Create(siteVmoConnector, siteAddressCache, siteIdpConnector, siteTokenCacheStore, clientTokenSettings);
	}

	public static ISiteConnector Create(ISiteVmoConnector siteVmoConnector, ISiteAddressCache siteAddressCache, ISiteIdpConnector siteIdpConnector, ISiteTokenCacheStore siteTokenCacheStore, IClientTokenSettings clientTokenSettings)
	{
		SiteConnectorOAuth siteConnectorOAuth = new SiteConnectorOAuth(siteTokenCacheStore, siteIdpConnector, siteVmoConnector, clientTokenSettings, siteAddressCache);
		SiteConnectorLegacy siteConnectorLegacy = new SiteConnectorLegacy(siteAddressCache, siteVmoConnector);
		return new SiteConnector(siteConnectorOAuth, siteConnectorLegacy);
	}

	public ISiteConnectionResult GetSiteConnection(ISiteConnectionInfoWindows siteConnectionInfoWindows)
	{
		if (siteConnectionInfoWindows == null)
		{
			throw new ArgumentNullException("siteConnectionInfoWindows");
		}
		if (siteConnectionInfoWindows.ForceLegacy || siteConnectionInfoWindows.Site.SiteVersion == null || siteConnectionInfoWindows.Site.SiteVersion < ServerVersions.v21_1)
		{
			return Task.Run(() => _siteConnectorLegacy.GetSiteConnectionResult(siteConnectionInfoWindows, CancellationToken.None)).Result;
		}
		return Task.Run(() => _siteConnectorOAuth.GetSiteConnectionResult(siteConnectionInfoWindows, CancellationToken.None)).Result;
	}

	public ISiteConnectionResult GetSiteConnection(ISiteConnectionInfoTokenExchange siteConnectionInfoTokenExchange)
	{
		if (siteConnectionInfoTokenExchange == null)
		{
			throw new ArgumentNullException("siteConnectionInfoTokenExchange");
		}
		return Task.Run(() => _siteConnectorOAuth.GetSiteConnectionResult(siteConnectionInfoTokenExchange, CancellationToken.None)).Result;
	}

	public Task<ISiteConnectionResult> GetSiteConnectionAsync(ISiteConnectionInfoWindows siteConnectionInfoWindows, CancellationToken cancellationToken)
	{
		if (siteConnectionInfoWindows == null)
		{
			throw new ArgumentNullException("siteConnectionInfoWindows");
		}
		if (siteConnectionInfoWindows.ForceLegacy || siteConnectionInfoWindows.Site.SiteVersion == null || siteConnectionInfoWindows.Site.SiteVersion < ServerVersions.v21_1)
		{
			return _siteConnectorLegacy.GetSiteConnectionResult(siteConnectionInfoWindows, cancellationToken);
		}
		return _siteConnectorOAuth.GetSiteConnectionResult(siteConnectionInfoWindows, cancellationToken);
	}

	public Task<ISiteConnectionResult> GetSiteConnectionAsync(ISiteConnectionInfoTokenExchange siteConnectionInfoTokenExchange, CancellationToken cancellationToken)
	{
		if (siteConnectionInfoTokenExchange == null)
		{
			throw new ArgumentNullException("siteConnectionInfoTokenExchange");
		}
		return _siteConnectorOAuth.GetSiteConnectionResult(siteConnectionInfoTokenExchange, cancellationToken);
	}

	public string GetUserDomainNameOrEmpty(NetworkCredential networkCredential)
	{
		return CredentialRetriever.GetUserDomainNameOrEmpty(networkCredential);
	}

	public void CheckForProtocolAllowed(string scheme, bool allowNonSecure)
	{
		if (Uri.UriSchemeHttp.Equals(scheme) && !allowNonSecure)
		{
			throw new UnsecureConnectionNotAllowedException("Unsecure connection is not allowed!");
		}
	}
}
