using VideoOS.Management.Server.ConfigurationApi;

namespace VideoOS.Management.Server.RestApi;

public class ConfigurationApiWrapper : IConfigurationApiWrapper
{
	public IConfigurationApiSupport GetObject(string itemType)
	{
		return ConfigurationApiObjectDispatcher.GetObject(itemType);
	}
}
