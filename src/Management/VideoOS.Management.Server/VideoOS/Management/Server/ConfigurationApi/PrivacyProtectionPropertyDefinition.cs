using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class PrivacyProtectionPropertyDefinition
{
	public static Property PrivacyMaskXml(string maskXml)
	{
		return new Property("PrivacyMaskXml", PropertyTypesHelper.GetValue(maskXml))
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyPrivacyMaskXml,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyPrivacyMaskXml",
			ToolTipTranslationId = "PropertyPrivacyMaskXmlToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}
}
