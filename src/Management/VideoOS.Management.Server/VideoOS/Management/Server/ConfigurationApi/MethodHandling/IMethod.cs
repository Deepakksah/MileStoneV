using VideoOS.ConfigurationAPI;

namespace VideoOS.Management.Server.ConfigurationApi.MethodHandling;

public interface IMethod
{
	string Id { get; }

	ConfigurationItem Invoke(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath);
}
