using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class CustomProperties : IConfigurationItem
{
	public CustomProperties(ServerId serverId, ConfigurationItem customProperties)
		: base(serverId, customProperties)
	{
	}

	public CustomProperties(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public CustomProperties(FQID fqid)
		: base(fqid)
	{
	}
}
