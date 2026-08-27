using VideoOS.ConfigurationAPI;

namespace VideoOS.Management.Server.RestApi;

public class RestSerializerFactory : IRestSerializerFactory
{
	public IConfigurationItemSerializer GetRestSerializer(ConfigurationItem item)
	{
		return new ConfigurationItemSerializer(item);
	}
}
