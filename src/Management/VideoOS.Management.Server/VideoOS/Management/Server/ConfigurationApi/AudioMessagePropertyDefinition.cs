using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class AudioMessagePropertyDefinition
{
	public static Property OriginalFileNameProperty(string fileName)
	{
		return new Property("OriginalFileName", fileName)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyAudioMessageOriginalFileName,
			ItemReference = false,
			ServerValidation = false,
			ToolTipTranslationId = "PropertyAudioMessageOriginalFileNameTooltip",
			TranslationId = "PropertyAudioMessageOriginalFileName",
			UIImportance = UIImportance.Hidden,
			SortKey = 100
		};
	}

	public static Property ContentProperty(string fileContent)
	{
		return new Property("Content", fileContent)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyAudioMessageContent,
			ItemReference = false,
			ServerValidation = false,
			ToolTipTranslationId = "PropertyAudioMessageContentTooltip",
			TranslationId = "PropertyAudioMessageContent",
			UIImportance = UIImportance.Hidden,
			SortKey = 200
		};
	}
}
