using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ChildTaskDetail : IConfigurationItem
{
	public ConfigurationApiProperties Properties => new ConfigurationApiProperties(base.Wrapper);

	public ChildTaskDetail(ServerId serverId, ConfigurationItem task)
		: base(serverId, task)
	{
	}
}
