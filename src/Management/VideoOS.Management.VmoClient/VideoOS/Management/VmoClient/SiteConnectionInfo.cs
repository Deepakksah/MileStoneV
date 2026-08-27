using System;
using System.Net;

namespace VideoOS.Management.VmoClient;

public class SiteConnectionInfo : ISiteConnectionInfo
{
	public ISite Site { get; }

	public ICredentials Credentials { get; }

	public string TrustedJwtAccessToken { get; }

	public bool AllowUnsecure { get; }

	public bool ForceLegacy { get; }

	public SiteConnectionInfo(ISiteConnectionInfoTokenExchange siteConnectionInfoTokenExchange)
	{
		if (siteConnectionInfoTokenExchange == null)
		{
			throw new ArgumentNullException("siteConnectionInfoTokenExchange");
		}
		AllowUnsecure = siteConnectionInfoTokenExchange.AllowUnsecure;
		Site = siteConnectionInfoTokenExchange.Site ?? throw new ArgumentException("Site can not be null.", "siteConnectionInfoTokenExchange");
		TrustedJwtAccessToken = siteConnectionInfoTokenExchange.TrustedJwtAccessToken ?? throw new ArgumentException("TrustedJwtAccessToken can not be null.", "siteConnectionInfoTokenExchange");
	}

	public SiteConnectionInfo(ISiteConnectionInfoWindows siteConnectionInfoWindows)
	{
		if (siteConnectionInfoWindows == null)
		{
			throw new ArgumentNullException("siteConnectionInfoWindows");
		}
		AllowUnsecure = siteConnectionInfoWindows.AllowUnsecure;
		Site = siteConnectionInfoWindows.Site ?? throw new ArgumentException("Site can not be null.", "siteConnectionInfoWindows");
		Credentials = siteConnectionInfoWindows.Credentials ?? throw new ArgumentException("Credentials can not be null.", "siteConnectionInfoWindows");
		ForceLegacy = siteConnectionInfoWindows.ForceLegacy;
	}
}
