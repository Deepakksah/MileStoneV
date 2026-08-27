using VideoOS.ConfigurationAPI;

namespace VideoOS.Management.Server.ConfigurationApi.MethodHandling;

public interface IMethodProvider
{
	string[] GetIds();

	IMethod Provide(ConfigurationItem item, string methodId);
}
