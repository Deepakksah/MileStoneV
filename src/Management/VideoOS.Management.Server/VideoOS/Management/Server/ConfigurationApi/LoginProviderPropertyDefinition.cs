using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class LoginProviderPropertyDefinition
{
	public static Property ClientId(string clientId)
	{
		return new Property("ClientId", clientId)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyLoginProviderClientId,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyLoginProviderClientId",
			ToolTipTranslationId = "PropertyLoginProviderClientIdTooltip",
			UIImportance = UIImportance.Default,
			SortKey = 300
		};
	}

	public static Property ClientSecret(string clientSecret)
	{
		return new Property("ClientSecret", clientSecret)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.ProperyLoginProviderClientSecret,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "ProperyLoginProviderClientSecret",
			ToolTipTranslationId = "ProperyLoginProviderClientSecretTooltip",
			UIImportance = UIImportance.Default,
			SortKey = 300
		};
	}

	public static Property ClientSecretType(string clientSecretType)
	{
		return new Property("ClientSecretType", clientSecretType)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyLoginProviderClientSecretType,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyLoginProviderClientSecretType",
			ToolTipTranslationId = "PropertyLoginProviderClientSecretTypeTooltip",
			UIImportance = UIImportance.Default,
			SortKey = 300
		};
	}

	public static Property ClientSecretHasValue(bool isValid)
	{
		return new Property("ClientSecretHasValue", isValid.ToString())
		{
			ValueType = "Tick",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyLoginProviderClientSecretHasValue,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyLoginProviderClientSecretHasValue",
			ToolTipTranslationId = "PropertyLoginProviderClientSecretHasValueToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 300
		};
	}

	public static Property CallbackPath(string callbackPath)
	{
		return new Property("CallbackPath", callbackPath)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyLoginProviderCallbackPath,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyLoginProviderCallbackPath",
			ToolTipTranslationId = "PropertyLoginProviderCallbackPathToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 300
		};
	}

	public static Property Authority(string authority)
	{
		return new Property("Authority", authority)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyLoginProviderAuthority,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyLoginProviderAuthority",
			ToolTipTranslationId = "PropertyLoginProviderAuthorityTooltip",
			UIImportance = UIImportance.Default,
			SortKey = 300
		};
	}

	public static Property Scopes(IEnumerable<string> scopes)
	{
		Property property = new Property("Scopes", scopes.ToArray());
		property.ValueType = "Array";
		property.IsSettable = true;
		property.DisplayName = ConfigurationApiResources.PropertyLoginProviderScopes;
		property.ItemReference = false;
		property.ServerValidation = false;
		property.TranslationId = "PropertyLoginProviderScopes";
		property.ToolTipTranslationId = "PropertyLoginProviderScopesTooltip";
		property.UIImportance = UIImportance.Default;
		property.SortKey = 300;
		property.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "ArrayValueType",
				Value = "String"
			}
		};
		return property;
	}

	public static Property PromptForLogin(bool promptForLogin)
	{
		return new Property("PromptForLogin", promptForLogin.ToString())
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyLoginProviderPromptForLogin,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyLoginProviderPromptForLogin",
			ToolTipTranslationId = "PropertyLoginProviderPromptForLoginTooltip",
			UIImportance = UIImportance.Default,
			SortKey = 300
		};
	}

	public static Property UserNameClaimType(string userNameClaimType)
	{
		return new Property("UserNameClaimType", userNameClaimType)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyLoginProviderUserNameClaimType,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyLoginProviderUserNameClaimType",
			ToolTipTranslationId = "PropertyLoginProviderUserNameClaimTypeTooltip",
			UIImportance = UIImportance.Default,
			SortKey = 300
		};
	}

	public static Property EnableCreationProperty(bool enabled)
	{
		return new Property("Enabled", enabled.ToString())
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyGeneralEnabled,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyGeneralEnabled",
			UIImportance = UIImportance.Default,
			SortKey = 300
		};
	}
}
