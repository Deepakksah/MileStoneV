using VideoOS.Management.Server.BusinessObjects.MetadataSearch;

namespace VideoOS.Management.Server;

public class MetadataObjectDefinition
{
	public string Id { get; set; }

	public string ObjectRoot { get; set; }

	public static string[] QueryMetadataObjectDefinitionIds(IGeminiRequestContext requestContext)
	{
		requestContext.ValidateMetadataSearchLicense();
		requestContext.CheckReadMetadataConfigurationPermission();
		using IMetadataSearchComponent metadataSearchComponent = requestContext.CreateMetadataSearchComponent();
		return metadataSearchComponent.QueryObjectDefinitionIds();
	}

	public static MetadataObjectDefinition QueryMetadataObjectDefinitionById(IGeminiRequestContext requestContext, string id)
	{
		requestContext.ValidateMetadataSearchLicense();
		requestContext.CheckReadMetadataConfigurationPermission();
		using IMetadataSearchComponent metadataSearchComponent = requestContext.CreateMetadataSearchComponent();
		return metadataSearchComponent.QueryObjectDefinitionById(id);
	}

	public static MetadataObjectDefinition[] QueryMetadataObjectDefinitionsByIds(IGeminiRequestContext requestContext, string[] ids)
	{
		requestContext.ValidateMetadataSearchLicense();
		requestContext.CheckReadMetadataConfigurationPermission();
		using IMetadataSearchComponent metadataSearchComponent = requestContext.CreateMetadataSearchComponent();
		return metadataSearchComponent.QueryObjectDefinitionsByIds(ids);
	}
}
