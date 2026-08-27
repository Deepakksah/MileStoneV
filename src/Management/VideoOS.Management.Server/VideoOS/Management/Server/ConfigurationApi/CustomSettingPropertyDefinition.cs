using System.Globalization;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class CustomSettingPropertyDefinition
{
	public static Property GetCustomSettingDataXmlProperty(string xml)
	{
		return new Property("DataXml", xml)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.CustomSettingDataXmlDisplayName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "CustomSettingDataXmlDisplayName",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property GetDataVersionProperty(int version)
	{
		return new Property("DataVersion", version.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Int",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.CustomSettingDataVersion,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "CustomSettingDataVersion",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property GetLastModifiedUserProperty(string userId)
	{
		return new Property("LastModifiedUser", userId)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.CustomSettingLastModifiedUser,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "CustomSettingLastModifiedUser",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}
}
