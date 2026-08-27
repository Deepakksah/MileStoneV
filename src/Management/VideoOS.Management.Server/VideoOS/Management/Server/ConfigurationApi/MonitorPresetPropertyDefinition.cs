using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class MonitorPresetPropertyDefinition
{
	public static Property MonitorPresetDefinitionXmlProperty(string monitorDefinitionXml)
	{
		return new Property("DefinitionXml", monitorDefinitionXml)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyMonitorPresetDefinitionXml,
			ItemReference = false,
			ServerValidation = false,
			ToolTipTranslationId = "PropertyMonitorPresetDefinitionXmlToolTip",
			TranslationId = "PropertyMonitorPresetDefinitionXml",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}
}
