using System;

namespace VideoOS.Management.VmoClient;

public class SiteConnectionInfoTokenExchange : SiteConnectionInfoBase, ISiteConnectionInfoTokenExchange, ISiteConnectionInfoBase
{
	public string TrustedJwtAccessToken { get; }

	public SiteConnectionInfoTokenExchange(ISite site, bool allowUnsecure, string trustedJwtAccessToken)
		: base(site, allowUnsecure)
	{
		TrustedJwtAccessToken = trustedJwtAccessToken ?? throw new ArgumentNullException("trustedJwtAccessToken");
	}
}
