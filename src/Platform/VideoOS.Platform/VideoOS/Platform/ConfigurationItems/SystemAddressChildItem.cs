using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class SystemAddressChildItem : IConfigurationChildItem
{
	public string Url => base.Wrapper.GetPropertyValue("Url");

	public Dictionary<string, string> AddressTypeValues => base.Wrapper.GetEnumValues("AddressType");

	public string AddressType => base.Wrapper.GetPropertyValue("AddressType");

	public SystemAddressChildItem(ServerId serverId, ConfigurationItem systemAddressChildItem, ConfigurationApiItemWrapper parentWrapper)
		: base(serverId, systemAddressChildItem, parentWrapper)
	{
	}

	public SystemAddressChildItem(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public SystemAddressChildItem(FQID fqid)
		: base(fqid)
	{
	}
}
