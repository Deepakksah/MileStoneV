using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class SiteAddressChildItem : IConfigurationChildItem
{
	public string Url => base.Wrapper.GetPropertyValue("Url");

	public Dictionary<string, string> AddressTypeValues => base.Wrapper.GetEnumValues("AddressType");

	public string AddressType => base.Wrapper.GetPropertyValue("AddressType");

	public SiteAddressChildItem(ServerId serverId, ConfigurationItem siteAddressChildItem, ConfigurationApiItemWrapper parentWrapper)
		: base(serverId, siteAddressChildItem, parentWrapper)
	{
	}

	public SiteAddressChildItem(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public SiteAddressChildItem(FQID fqid)
		: base(fqid)
	{
	}
}
