using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Management.VmoClient;

public class SiteConnectorLegacy : ISiteConnectorLegacy
{
	private readonly ISiteAddressCache _siteAddressCache;

	private readonly ISiteVmoConnector _siteVmoConnector;

	public SiteConnectorLegacy(ISiteAddressCache siteAddressCache, ISiteVmoConnector siteVmoConnector)
	{
		if (siteAddressCache == null)
		{
			throw new ArgumentNullException("siteAddressCache");
		}
		if (siteVmoConnector == null)
		{
			throw new ArgumentNullException("siteVmoConnector");
		}
		_siteAddressCache = siteAddressCache;
		_siteVmoConnector = siteVmoConnector;
	}

	public async Task<ISiteConnectionResult> GetSiteConnectionResult(ISiteConnectionInfoWindows siteConnectionInfoWindows, CancellationToken cancellationToken)
	{
		if (siteConnectionInfoWindows == null)
		{
			throw new ArgumentNullException("siteConnectionInfoWindows");
		}
		ISite site = siteConnectionInfoWindows.Site;
		SiteConnectionResultBuilder resultBuilder = new SiteConnectionResultBuilder();
		Uri addressStoredInRegistry = _siteAddressCache.GetSiteAddress(site.Id);
		SiteAddressValidator siteAddressValidator = new SiteAddressValidator();
		IEnumerable<Uri> httpAddresses = siteAddressValidator.FilterHttpAddresses(siteConnectionInfoWindows);
		(from x in siteConnectionInfoWindows.Site.Addresses
			select x.ServerUrl into x
			where !httpAddresses.Contains(x)
			select x).ToList().ForEach(delegate(Uri uri)
		{
			resultBuilder.AddSiteAddressWithError(new SiteAddressWithError(uri, new Exception("HTTPS protocol can't be used for legacy connection")));
		});
		bool flag = site.SiteVersion == null || site.SiteVersion < ServerVersions.v21_1;
		if (httpAddresses.Contains(addressStoredInRegistry) && (siteConnectionInfoWindows.ForceLegacy || flag) && TryConnect(addressStoredInRegistry, siteConnectionInfoWindows, resultBuilder))
		{
			return resultBuilder.Build();
		}
		List<Uri> addresses = httpAddresses.Where((Uri x) => !x.Equals(addressStoredInRegistry)).ToList();
		if (await TryConnect(addresses, siteConnectionInfoWindows, resultBuilder, cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
		{
			ISiteConnectionResult siteConnectionResult = resultBuilder.Build();
			_siteAddressCache.SetSiteAddress(site.Id, siteConnectionResult.ServerAddress);
			return siteConnectionResult;
		}
		return resultBuilder.Build();
	}

	private bool TryConnect(Uri address, ISiteConnectionInfoWindows siteConnectionInfoWindows, SiteConnectionResultBuilder resultBuilder)
	{
		try
		{
			Guid id = siteConnectionInfoWindows.Site.Id;
			CredentialRetriever credentialRetriever = new CredentialRetriever(siteConnectionInfoWindows.Credentials);
			IVmoClient vmoClient = ((!credentialRetriever.ConnectAsUser) ? _siteVmoConnector.TryConnectLegacy(id, address) : _siteVmoConnector.TryConnectLegacyAsUser(id, address, credentialRetriever.UserName, credentialRetriever.Password));
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

	private async Task<bool> TryConnect(IEnumerable<Uri> addresses, ISiteConnectionInfoWindows siteConnectionInfoWindows, SiteConnectionResultBuilder resultBuilder, CancellationToken cancellationToken)
	{
		CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
		try
		{
			bool result = (await Task.WhenAll(addresses.Select((Uri x) => Task.Run(() => TryConnect(x, siteConnectionInfoWindows, resultBuilder), cts.Token)).Select(async delegate(Task<bool> tryConnectTask)
			{
				try
				{
					tryConnectTask.Wait(cts.Token);
					if (await tryConnectTask)
					{
						cts.Cancel();
						return true;
					}
					return false;
				}
				catch (OperationCanceledException) when (cts.IsCancellationRequested)
				{
					return false;
				}
			})).ConfigureAwait(continueOnCapturedContext: false)).FirstOrDefault((bool x) => x);
			cancellationToken.ThrowIfCancellationRequested();
			return result;
		}
		finally
		{
			if (cts != null)
			{
				((IDisposable)cts).Dispose();
			}
		}
	}
}
