using System;
using VideoOS.IdentityServer.Common;
using VideoOS.Management.VmoClient.Internal;

namespace VideoOS.Management.VmoClient;

public sealed class SiteConnectorFactory : ISiteConnectorFactory
{
	private readonly ISiteVmoConnector _siteVmoConnector;

	private readonly ISiteAddressCache _siteAddressCache;

	private readonly ISiteIdpConnector _siteIdpConnector;

	private readonly ISiteTokenCacheStore _siteTokenCacheStore;

	private readonly IClientTokenSettings _clientTokenSettings;

	public SiteConnectorFactory(ISiteTokenCacheStore siteTokenCacheStore, IClientTokenSettings clientTokenSettings)
		: this(new SiteVmoConnector(new ServerConnectionFactory(), new VmoClientFactory(), new VmoClientValidator()), new SiteAddressCache(), new SiteIdpConnector(IdpDiscoveryService.Instance), siteTokenCacheStore, clientTokenSettings)
	{
	}

	public SiteConnectorFactory(ISiteVmoConnector siteVmoConnector, ISiteAddressCache siteAddressCache, ISiteIdpConnector siteIdpConnector, ISiteTokenCacheStore siteTokenCacheStore, IClientTokenSettings clientTokenSettings)
	{
		_siteVmoConnector = siteVmoConnector ?? throw new ArgumentNullException("siteVmoConnector");
		_siteAddressCache = siteAddressCache ?? throw new ArgumentNullException("siteAddressCache");
		_siteIdpConnector = siteIdpConnector ?? throw new ArgumentNullException("siteIdpConnector");
		_siteTokenCacheStore = siteTokenCacheStore ?? throw new ArgumentNullException("siteTokenCacheStore");
		_clientTokenSettings = clientTokenSettings ?? throw new ArgumentNullException("clientTokenSettings");
	}

	public ISiteConnector Create()
	{
		return SiteConnector.Create(_siteVmoConnector, _siteAddressCache, _siteIdpConnector, _siteTokenCacheStore, _clientTokenSettings);
	}
}
