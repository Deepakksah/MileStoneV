using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class DeviceDriverSettingsChildItem : IConfigurationChildItem
{
	private ConfigurationApiProperties _properties;

	public ConfigurationApiProperties Properties => _properties;

	public DeviceDriverSettingsChildItem(ServerId serverId, ConfigurationItem deviceDriverSettingsChildItem, ConfigurationApiItemWrapper parentWrapper)
		: base(serverId, deviceDriverSettingsChildItem, parentWrapper)
	{
		_properties = new ConfigurationApiProperties(base.Wrapper);
	}

	public DeviceDriverSettingsChildItem(ServerId serverId, string path)
		: base(serverId, path)
	{
		_properties = new ConfigurationApiProperties(base.Wrapper);
	}

	public DeviceDriverSettingsChildItem(FQID fqid)
		: base(fqid)
	{
		_properties = new ConfigurationApiProperties(base.Wrapper);
	}
}
