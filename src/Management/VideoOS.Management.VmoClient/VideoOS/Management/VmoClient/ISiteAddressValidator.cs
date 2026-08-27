using System;
using System.Collections.Generic;

namespace VideoOS.Management.VmoClient;

public interface ISiteAddressValidator
{
	bool IsAddressSchemeAllowed(Uri address, bool allowHttp);

	(IEnumerable<Uri> ValidAddresses, IEnumerable<Uri> InvalidAddresses) ValidateSchemas(ISiteConnectionInfoBase siteConnectionInfo);

	IEnumerable<Uri> FilterHttpAddresses(ISiteConnectionInfoBase siteConnectionInfo);
}
