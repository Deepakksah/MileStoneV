using System;

namespace VideoOS.Management.VmoClient;

public abstract class SiteConnectionInfoBase : ISiteConnectionInfoBase
{
	public ISite Site { get; }

	public bool AllowUnsecure { get; }

	public SiteConnectionInfoBase(ISite site, bool allowUnsecure)
	{
		Site = site ?? throw new ArgumentNullException("site");
		AllowUnsecure = allowUnsecure;
	}
}
