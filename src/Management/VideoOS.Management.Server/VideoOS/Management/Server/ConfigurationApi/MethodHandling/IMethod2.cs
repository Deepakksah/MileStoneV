using VideoOS.ConfigurationAPI;

namespace VideoOS.Management.Server.ConfigurationApi.MethodHandling;

public interface IMethod2 : IMethod
{
	ConfigurationItem Invoke2(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, IConfigurationApiSupport service);
}
