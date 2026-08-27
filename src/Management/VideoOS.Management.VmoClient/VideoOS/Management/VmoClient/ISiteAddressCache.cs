using System;

namespace VideoOS.Management.VmoClient;

public interface ISiteAddressCache
{
	Uri GetSiteAddress(Guid siteId);

	void SetSiteAddress(Guid siteId, Uri address);
}
