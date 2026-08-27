using VideoOS.ConfigurationAPI;

namespace VideoOS.Management.Server.ConfigurationApi.AccessControl.Builders;

public interface IConfigurationItemBuilder
{
	ConfigurationItem BuildAccessControlItem(ConfigurationItemPath path, ConfigurationItemPath parentPath, MIPServerGroup serverGroup, MIPObject mipObject, string[] methodIds);

	ConfigurationItem BuildAccessControlItem(ConfigurationItemPath path, ConfigurationItemPath parentPath, string itemName, MIPObject mipObject, string[] methodIds);
}
