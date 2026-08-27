using System;
using System.Collections.Generic;

namespace VideoOS.Management.VmoClient;

public interface ISiteConnectionResultBuilder
{
	void AddAddressesWithInvalidScheme(IEnumerable<Uri> addressesWithInvalidScheme);

	void AddVmoClient(IVmoClient vmoClient);

	void AddSiteVersion(ServerVersion siteVersion);

	void AddSiteAddressWithError(SiteAddressWithError siteAddressWithError);

	ISiteConnectionResult Build();
}
