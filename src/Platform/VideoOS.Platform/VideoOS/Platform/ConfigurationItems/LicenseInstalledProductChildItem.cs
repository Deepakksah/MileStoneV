using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class LicenseInstalledProductChildItem : IConfigurationChildItem
{
	public string ProductDisplayName => base.Wrapper.GetPropertyValue("ProductDisplayName");

	public string PluginId => base.Wrapper.GetPropertyValue("PluginId");

	public string Slc => base.Wrapper.GetPropertyValue("Slc");

	public string ExpirationDate => base.Wrapper.GetPropertyValue("ExpirationDate");

	public string CarePlus => base.Wrapper.GetPropertyValue("CarePlus");

	public string CarePremium => base.Wrapper.GetPropertyValue("CarePremium");

	public string PuAc => base.Wrapper.GetPropertyValue("PuAc");

	public LicenseInstalledProductChildItem(ServerId serverId, ConfigurationItem licenseInstalledProductChildItem, ConfigurationApiItemWrapper parentWrapper)
		: base(serverId, licenseInstalledProductChildItem, parentWrapper)
	{
	}

	public LicenseInstalledProductChildItem(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public LicenseInstalledProductChildItem(FQID fqid)
		: base(fqid)
	{
	}
}
