using System;
using System.Collections.Generic;
using System.Text;

namespace VideoOS.Common.Integration.Configuration;

public class Licenses
{
	private ConfigurationManager configurationManager;

	public License this[Guid id] => configurationManager.LicenseItems.GetLicenseByLicenseId(id);

	public License ServerLicense => configurationManager.LicenseItems.GetLicenseByLicenseId(License.WellKnownId.ServerLicense);

	public License LicenseOwner => configurationManager.LicenseItems.GetLicenseByLicenseId(License.WellKnownId.LicenseOwner);

	public License SitesInformation => configurationManager.LicenseItems.GetLicenseByLicenseId(License.WellKnownId.SiteInformation);

	public Licenses(ConfigurationManager configurationManager)
	{
		this.configurationManager = configurationManager;
	}

	public License Add(Guid id, byte[] data)
	{
		return configurationManager.LicenseItems.AddNew(id, data);
	}

	public void Remove(Guid id)
	{
		configurationManager.LicenseItems.Remove(id);
	}

	public License AddServerLicense(byte[] data)
	{
		return Add(License.WellKnownId.ServerLicense, data);
	}

	public License AddLicenseOwner(string data)
	{
		return Add(License.WellKnownId.LicenseOwner, Encoding.UTF8.GetBytes(data));
	}

	public License AddSitesInformation(string data)
	{
		return Add(License.WellKnownId.SiteInformation, Encoding.UTF8.GetBytes(data));
	}

	public void TryPreloadLicenses(IEnumerable<Guid> licenseIds)
	{
		configurationManager.LicenseItems.TryPreloadLicenseItems(licenseIds);
	}
}
