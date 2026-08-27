using VideoOS.Management.Server.BusinessObjects.MetadataSearch;

namespace VideoOS.Management.Server;

public class MetadataSearchPath
{
	public string Id { get; set; }

	public string Path { get; set; }

	public string PropertyId { get; set; }

	public static string[] QueryMetadataSearchPathIdsByObjectDefinitionId(IGeminiRequestContext requestContext, string objectDefinitionId)
	{
		requestContext.ValidateMetadataSearchLicense();
		requestContext.CheckReadMetadataConfigurationPermission();
		using IMetadataSearchComponent metadataSearchComponent = requestContext.CreateMetadataSearchComponent();
		return metadataSearchComponent.QuerySearchPathIdsByObjectDefinitionId(objectDefinitionId);
	}

	public static MetadataSearchPath QueryMetadataSearchPathById(IGeminiRequestContext requestContext, string id)
	{
		requestContext.ValidateMetadataSearchLicense();
		requestContext.CheckReadMetadataConfigurationPermission();
		using IMetadataSearchComponent metadataSearchComponent = requestContext.CreateMetadataSearchComponent();
		return metadataSearchComponent.QuerySearchPathById(id);
	}

	public static MetadataSearchPath[] QueryMetadataSearchPathsByIds(IGeminiRequestContext requestContext, string[] ids)
	{
		requestContext.ValidateMetadataSearchLicense();
		requestContext.CheckReadMetadataConfigurationPermission();
		using IMetadataSearchComponent metadataSearchComponent = requestContext.CreateMetadataSearchComponent();
		return metadataSearchComponent.QuerySearchPathsByIds(ids);
	}
}
