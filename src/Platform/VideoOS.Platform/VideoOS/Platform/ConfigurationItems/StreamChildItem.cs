using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class StreamChildItem : IConfigurationChildItem
{
	private ConfigurationApiProperties _properties;

	public string StreamReferenceId => base.Wrapper.GetPropertyValue("StreamReferenceId");

	public ConfigurationApiProperties Properties => _properties;

	public StreamChildItem(ServerId serverId, ConfigurationItem streamChildItem, ConfigurationApiItemWrapper parentWrapper)
		: base(serverId, streamChildItem, parentWrapper)
	{
		_properties = new ConfigurationApiProperties(base.Wrapper);
	}

	public StreamChildItem(ServerId serverId, string path)
		: base(serverId, path)
	{
		_properties = new ConfigurationApiProperties(base.Wrapper);
	}

	public StreamChildItem(FQID fqid)
		: base(fqid)
	{
		_properties = new ConfigurationApiProperties(base.Wrapper);
	}
}
