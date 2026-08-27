using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Management.VmoClient;

public class SiteAddressValidator : ISiteAddressValidator
{
	public (IEnumerable<Uri> ValidAddresses, IEnumerable<Uri> InvalidAddresses) ValidateSchemas(ISiteConnectionInfoBase siteConnectionInfo)
	{
		List<Uri> valid = new List<Uri>();
		List<Uri> invalid = new List<Uri>();
		siteConnectionInfo?.Site?.Addresses?.Select((ReadOnlyAddress x) => x.ServerUrl).ToList().ForEach(delegate(Uri address)
		{
			if (IsAddressSchemeAllowed(address, siteConnectionInfo.AllowUnsecure))
			{
				valid.Add(address);
			}
			else
			{
				invalid.Add(address);
			}
		});
		return (ValidAddresses: valid, InvalidAddresses: invalid);
	}

	public bool IsAddressSchemeAllowed(Uri address, bool allowHttp)
	{
		if (Uri.UriSchemeHttps.Equals(address.Scheme, StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		return Uri.UriSchemeHttp.Equals(address.Scheme, StringComparison.OrdinalIgnoreCase) && allowHttp;
	}

	public IEnumerable<Uri> FilterHttpAddresses(ISiteConnectionInfoBase siteConnectionInfo)
	{
		return (from a in siteConnectionInfo?.Site?.Addresses?.Select((ReadOnlyAddress x) => x.ServerUrl)
			where Uri.UriSchemeHttp.Equals(a.Scheme, StringComparison.OrdinalIgnoreCase)
			select a);
	}
}
