using VideoOS.Management.Server.BusinessObjects.MetadataSearch;

namespace VideoOS.Management.Server;

public class MetadataClassCandidate
{
	public string Id { get; set; }

	public string Name { get; set; }

	public static MetadataClassCandidate QueryClassCandidateById(IGeminiRequestContext requestContext, string id)
	{
		requestContext.ValidateMetadataSearchLicense();
		requestContext.CheckReadMetadataConfigurationPermission();
		using IMetadataSearchComponent metadataSearchComponent = requestContext.CreateMetadataSearchComponent();
		return metadataSearchComponent.QueryClassCandidateById(id);
	}

	public static MetadataClassCandidate[] QueryClassCandidatesByIds(IGeminiRequestContext requestContext, string[] ids)
	{
		requestContext.ValidateMetadataSearchLicense();
		requestContext.CheckReadMetadataConfigurationPermission();
		using IMetadataSearchComponent metadataSearchComponent = requestContext.CreateMetadataSearchComponent();
		return metadataSearchComponent.QueryClassCandidatesByIds(ids);
	}

	public static string[] QueryClassCandidateIdsByCategoryId(IGeminiRequestContext requestContext, string categoryId)
	{
		requestContext.ValidateMetadataSearchLicense();
		requestContext.CheckReadMetadataConfigurationPermission();
		using IMetadataSearchComponent metadataSearchComponent = requestContext.CreateMetadataSearchComponent();
		return metadataSearchComponent.QueryClassCandidateIdsByCategoryId(categoryId);
	}
}
