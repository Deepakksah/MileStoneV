using VideoOS.ConfigurationAPI;

namespace VideoOS.Management.Server.RestApi;

public interface IRestSerializerFactory
{
	IConfigurationItemSerializer GetRestSerializer(ConfigurationItem item);
}
