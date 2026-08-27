namespace VideoOS.Management.Server;

public sealed class ConfigurationDataManager
{
	public static string GetSmartClientSettingsXml(IGeminiRequestContext requestContext)
	{
		using IConfigurationDataComponent configurationDataComponent = requestContext.CreateConfigurationDataComponent();
		return configurationDataComponent.GetClientSettingsXml();
	}

	public static IdNameDescription FindHomeSiteInfo(IGeminiRequestContext requestContext)
	{
		using IConfigurationDataComponent configurationDataComponent = requestContext.CreateConfigurationDataComponent();
		return configurationDataComponent.FindHomeSiteInfo();
	}

	public static AudioMessageIdNameDescription[] FindAudioMessages(IGeminiRequestContext requestContext)
	{
		using IConfigurationDataComponent configurationDataComponent = requestContext.CreateConfigurationDataComponent();
		return configurationDataComponent.FindAudioMessages();
	}

	public static bool FindPrivacyMaskUsed(IGeminiRequestContext requestContext)
	{
		using IConfigurationDataComponent configurationDataComponent = requestContext.CreateConfigurationDataComponent();
		return configurationDataComponent.FindPrivacyMaskUsed();
	}
}
