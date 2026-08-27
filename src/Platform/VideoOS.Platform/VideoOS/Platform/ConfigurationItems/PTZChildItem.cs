using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class PTZChildItem : IConfigurationChildItem
{
	private ConfigurationApiProperties _properties;

	public ConfigurationApiProperties Properties => _properties;

	public PTZChildItem(ServerId serverId, ConfigurationItem ptzChildItem, ConfigurationApiItemWrapper parentWrapper)
		: base(serverId, ptzChildItem, parentWrapper)
	{
		_properties = new ConfigurationApiProperties(base.Wrapper);
	}

	public PTZChildItem(ServerId serverId, string path)
		: base(serverId, path)
	{
		_properties = new ConfigurationApiProperties(base.Wrapper);
	}

	public PTZChildItem(FQID fqid)
		: base(fqid)
	{
		_properties = new ConfigurationApiProperties(base.Wrapper);
	}
}
