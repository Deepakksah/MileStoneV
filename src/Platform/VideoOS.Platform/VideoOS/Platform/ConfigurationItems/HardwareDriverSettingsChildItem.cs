using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class HardwareDriverSettingsChildItem : IConfigurationChildItem
{
	private ConfigurationApiProperties _properties;

	public ConfigurationApiProperties Properties => _properties;

	public HardwareDriverSettingsChildItem(ServerId serverId, ConfigurationItem hardwareDriverSettingsChildItem, ConfigurationApiItemWrapper parentWrapper)
		: base(serverId, hardwareDriverSettingsChildItem, parentWrapper)
	{
		_properties = new ConfigurationApiProperties(base.Wrapper);
	}

	public HardwareDriverSettingsChildItem(ServerId serverId, string path)
		: base(serverId, path)
	{
		_properties = new ConfigurationApiProperties(base.Wrapper);
	}

	public HardwareDriverSettingsChildItem(FQID fqid)
		: base(fqid)
	{
		_properties = new ConfigurationApiProperties(base.Wrapper);
	}
}
