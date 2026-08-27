using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Management.VmoClient;

public class SiteConnectionResultBuilder : ISiteConnectionResultBuilder
{
	private HashSet<SiteAddressWithError> _errors = new HashSet<SiteAddressWithError>();

	private IVmoClient _vmoClient;

	private ServerVersion _siteVersion;

	public void AddAddressesWithInvalidScheme(IEnumerable<Uri> addressesWithInvalidScheme)
	{
		addressesWithInvalidScheme?.ToList().ForEach(delegate(Uri address)
		{
			_errors.Add(new SiteAddressWithError(address, new UnsecureConnectionNotAllowedException("Unsecure connection is not allowed!")));
		});
	}

	public void AddVmoClient(IVmoClient vmoClient)
	{
		_vmoClient = vmoClient;
	}

	public void AddSiteAddressWithError(SiteAddressWithError siteAddressWithError)
	{
		SiteAddressWithError siteAddressWithError2 = _errors.FirstOrDefault((SiteAddressWithError x) => x.ServerAddress.Equals(siteAddressWithError.ServerAddress));
		if (siteAddressWithError2 != null)
		{
			_errors.Remove(siteAddressWithError2);
		}
		_errors.Add(siteAddressWithError);
	}

	public void AddSiteVersion(ServerVersion siteVersion)
	{
		_siteVersion = siteVersion;
	}

	public ISiteConnectionResult Build()
	{
		if (_vmoClient == null)
		{
			return new SiteConnectionResult(_errors, _siteVersion);
		}
		return new SiteConnectionResult(_vmoClient, _vmoClient.ServerConnection.ServerUri);
	}
}
