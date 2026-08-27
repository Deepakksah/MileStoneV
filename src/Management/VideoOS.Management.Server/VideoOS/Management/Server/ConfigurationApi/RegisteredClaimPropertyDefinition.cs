using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class RegisteredClaimPropertyDefinition
{
	public static Property CaseSensitive(bool caseSensitive, bool isSettable)
	{
		return new Property("CaseSensitive", caseSensitive.ToString())
		{
			ValueType = "Tick",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyRegisteredClaimCaseSensitive,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRegisteredClaimCaseSensitive",
			ToolTipTranslationId = "PropertyRegisteredClaimCaseSensitiveTooltip",
			UIImportance = UIImportance.Default,
			SortKey = 300
		};
	}

	public static Property DisplayNameCreationProperty(string displayName)
	{
		return new Property("DisplayName", displayName)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyDisplayName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyDisplayName",
			UIImportance = UIImportance.Default,
			SortKey = 300
		};
	}
}
