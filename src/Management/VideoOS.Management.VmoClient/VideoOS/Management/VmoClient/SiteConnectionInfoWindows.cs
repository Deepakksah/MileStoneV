using System;
using System.Net;

namespace VideoOS.Management.VmoClient;

public class SiteConnectionInfoWindows : SiteConnectionInfoBase, ISiteConnectionInfoWindows, ISiteConnectionInfoBase
{
	public ICredentials Credentials { get; }

	public bool ForceLegacy { get; }

	public SiteConnectionInfoWindows(ISite site, bool allowUnsecure)
		: this(site, allowUnsecure, CredentialCache.DefaultCredentials)
	{
	}

	public SiteConnectionInfoWindows(ISite site, bool allowUnsecure, bool forceLegacy)
		: this(site, allowUnsecure, CredentialCache.DefaultCredentials, forceLegacy)
	{
	}

	public SiteConnectionInfoWindows(ISite site, bool allowUnsecure, ICredentials credentials)
		: this(site, allowUnsecure, credentials, forceLegacy: false)
	{
	}

	public SiteConnectionInfoWindows(ISite site, bool allowUnsecure, ICredentials credentials, bool forceLegacy)
		: base(site, allowUnsecure)
	{
		Credentials = credentials ?? throw new ArgumentNullException("credentials");
		ForceLegacy = forceLegacy;
	}
}
