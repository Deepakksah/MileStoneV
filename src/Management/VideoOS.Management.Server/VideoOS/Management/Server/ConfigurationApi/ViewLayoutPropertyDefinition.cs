using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class ViewLayoutPropertyDefinition
{
	public static Property DefinitionXmlProperty(string definitionXml, bool isSettable)
	{
		return new Property("DefinitionXml", definitionXml)
		{
			ValueType = "String",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyViewLayoutDefinitionXml,
			ServerValidation = false,
			ToolTipTranslationId = "PropertyViewLayoutDefinitionXmlToolTip",
			TranslationId = "PropertyViewLayoutDefinitionXml",
			UIImportance = UIImportance.Default
		};
	}
}
