using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class LicenseOverviewAllChildItem : IConfigurationChildItem
{
	public string PluginId => base.Wrapper.GetPropertyValue("PluginId");

	public string LicenseType => base.Wrapper.GetPropertyValue("LicenseType");

	public string Activated => base.Wrapper.GetPropertyValue("Activated");

	public LicenseOverviewAllChildItem(ServerId serverId, ConfigurationItem licenseOverviewAllChildItem, ConfigurationApiItemWrapper parentWrapper)
		: base(serverId, licenseOverviewAllChildItem, parentWrapper)
	{
	}

	public LicenseOverviewAllChildItem(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public LicenseOverviewAllChildItem(FQID fqid)
		: base(fqid)
	{
	}
}
