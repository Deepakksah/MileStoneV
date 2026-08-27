using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class DeviceGroupPropertyDefinition
{
	public static Property BuiltInProperty(bool builtIn)
	{
		return new Property("BuiltIn", builtIn.ToString())
		{
			ValueType = "Tick",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyDeviceGroupBuiltIn,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyDeviceGroupBuiltIn"
		};
	}

	public static Property RemoveMembersProperty(bool removeMembers)
	{
		return new Property("RemoveMembers", removeMembers.ToString())
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyDeviceGroupRemoveMembers,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyDeviceGroupRemoveMembers"
		};
	}
}
