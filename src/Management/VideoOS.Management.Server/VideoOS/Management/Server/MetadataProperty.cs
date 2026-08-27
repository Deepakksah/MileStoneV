using VideoOS.Management.Server.BusinessObjects.MetadataSearch;

namespace VideoOS.Management.Server;

public class MetadataProperty
{
	public string Id { get; set; }

	public string Name { get; set; }

	public static string[] QueryMetadataPropertyIdsByFilterId(IGeminiRequestContext requestContext, string filterId)
	{
		requestContext.ValidateMetadataSearchLicense();
		requestContext.CheckReadMetadataConfigurationPermission();
		using IMetadataSearchComponent metadataSearchComponent = requestContext.CreateMetadataSearchComponent();
		return metadataSearchComponent.QueryPropertyIdsByFilterId(filterId);
	}

	public static MetadataProperty QueryMetadataPropertyById(IGeminiRequestContext requestContext, string id)
	{
		requestContext.ValidateMetadataSearchLicense();
		requestContext.CheckReadMetadataConfigurationPermission();
		using IMetadataSearchComponent metadataSearchComponent = requestContext.CreateMetadataSearchComponent();
		return metadataSearchComponent.QueryPropertyById(id);
	}

	public static MetadataProperty[] QueryMetadataPropertiesByIds(IGeminiRequestContext requestContext, string[] ids)
	{
		requestContext.ValidateMetadataSearchLicense();
		requestContext.CheckReadMetadataConfigurationPermission();
		using IMetadataSearchComponent metadataSearchComponent = requestContext.CreateMetadataSearchComponent();
		return metadataSearchComponent.QueryPropertiesByIds(ids);
	}
}
