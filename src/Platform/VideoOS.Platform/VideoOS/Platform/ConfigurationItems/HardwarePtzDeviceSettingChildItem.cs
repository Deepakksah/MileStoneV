using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class HardwarePtzDeviceSettingChildItem : IConfigurationChildItem
{
	private ConfigurationApiProperties _properties;

	public ConfigurationApiProperties Properties => _properties;

	public HardwarePtzDeviceSettingChildItem(ServerId serverId, ConfigurationItem hardwarePtzDeviceSettingChildItem, ConfigurationApiItemWrapper parentWrapper)
		: base(serverId, hardwarePtzDeviceSettingChildItem, parentWrapper)
	{
		_properties = new ConfigurationApiProperties(base.Wrapper);
	}

	public HardwarePtzDeviceSettingChildItem(ServerId serverId, string path)
		: base(serverId, path)
	{
		_properties = new ConfigurationApiProperties(base.Wrapper);
	}

	public HardwarePtzDeviceSettingChildItem(FQID fqid)
		: base(fqid)
	{
		_properties = new ConfigurationApiProperties(base.Wrapper);
	}
}
