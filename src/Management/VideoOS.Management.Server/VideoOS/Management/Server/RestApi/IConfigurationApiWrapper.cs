using VideoOS.Management.Server.ConfigurationApi;

namespace VideoOS.Management.Server.RestApi;

public interface IConfigurationApiWrapper
{
	IConfigurationApiSupport GetObject(string itemType);
}
