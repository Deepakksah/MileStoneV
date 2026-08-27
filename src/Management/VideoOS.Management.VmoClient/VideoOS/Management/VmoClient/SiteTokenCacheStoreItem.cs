using System;
using System.Collections.Generic;
using VideoOS.IdentityServer.Common;
using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.Management.VmoClient;

public class SiteTokenCacheStoreItem
{
	public Guid SiteId { get; }

	public IEnumerable<Uri> Addresses { get; }

	public Uri ConnectedAddress { get; }

	public ITokenCache TokenCache { get; }

	public IdpInfo IdpInfo { get; }

	public bool HomeSite { get; }

	public SiteTokenCacheStoreItem(Guid siteId, IEnumerable<Uri> addresses, Uri connectedAddress, ITokenCache tokenCache, IdpInfo idpInfo, bool homeSite)
	{
		SiteId = siteId;
		Addresses = addresses;
		ConnectedAddress = TrimIdpFromPath(connectedAddress);
		TokenCache = tokenCache;
		IdpInfo = idpInfo;
		HomeSite = homeSite;
	}

	private Uri TrimIdpFromPath(Uri address)
	{
		if (address == null)
		{
			return address;
		}
		string text = address.ToString();
		text = text.TrimEnd('/');
		if (text.EndsWith("/idp", StringComparison.OrdinalIgnoreCase))
		{
			text = text.Remove(text.Length - 4);
			return new Uri(text);
		}
		return address;
	}
}
