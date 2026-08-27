using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class ClaimPropertyDefinition
{
	public static Property LoginProviderIdProperty(string loginProviderId, bool isSettable)
	{
		return new Property("ClaimProvider", loginProviderId)
		{
			ValueType = "String",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyClaimLoginProviderId,
			ItemReference = false,
			ServerValidation = false,
			ToolTipTranslationId = "PropertyClaimLoginProviderIdTooltip",
			TranslationId = "PropertyClaimLoginProviderId",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property ClaimNameProperty(string claimName, bool isSettable)
	{
		return new Property("ClaimName", claimName)
		{
			ValueType = "String",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyClaimName,
			ItemReference = false,
			ServerValidation = false,
			ToolTipTranslationId = "PropertyClaimNameTooltip",
			TranslationId = "PropertyClaimName",
			UIImportance = UIImportance.Default,
			SortKey = 200
		};
	}

	public static Property ClaimValueProperty(string claimValue, bool isSettable)
	{
		return new Property("ClaimValue", claimValue)
		{
			ValueType = "String",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyClaimValue,
			ItemReference = false,
			ServerValidation = false,
			ToolTipTranslationId = "PropertyClaimValueTooltip",
			TranslationId = "PropertyClaimValue",
			UIImportance = UIImportance.Default,
			SortKey = 300
		};
	}
}
