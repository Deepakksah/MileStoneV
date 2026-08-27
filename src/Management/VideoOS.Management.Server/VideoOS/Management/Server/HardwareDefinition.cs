namespace VideoOS.Management.Server;

public sealed class HardwareDefinition
{
	public static string QueryDefinitonXmlByHardwareId(IGeminiRequestContext requestContext, string hardwareId)
	{
		using IHardwareDefinitionComponent hardwareDefinitionComponent = requestContext.CreateHardwareDefinitionComponent();
		return hardwareDefinitionComponent.QueryDefinition(hardwareId);
	}
}
