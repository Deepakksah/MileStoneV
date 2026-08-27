using VideoOS.Management.Common;

namespace VideoOS.Management.Server.BusinessObjects.MetadataSearch;

public static class MetadataSearchGeminiRequestExtensions
{
	private const string Licensed = "1";

	public static void CheckEditMetadataConfigurationPermission(this IGeminiRequestContext requestContext)
	{
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.MetadataSearch, "EDIT_METADATA_SEARCHCONF", "/");
	}

	public static void CheckReadMetadataConfigurationPermission(this IGeminiRequestContext requestContext)
	{
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.MetadataSearch, "GENERIC_READ", "/");
	}

	public static void ValidateMetadataSearchLicense(this IGeminiRequestContext requestContext)
	{
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "SearchCategories") || !requestContext.IsAnyMetadataCategoryLicensed())
		{
			throw new FeatureNotLicensedException();
		}
	}

	private static bool IsAnyMetadataCategoryLicensed(this IGeminiRequestContext requestContext)
	{
		string value = null;
		string value2 = null;
		string value3 = null;
		if ((!GeminiApplicationCore.FeatureCacheManager.TryGetFeatureParameterValue(requestContext, "SearchCategories", "7C4765C6-BD67-45A1-9E0B-84313207379C", out value) || !(value == "1")) && (!GeminiApplicationCore.FeatureCacheManager.TryGetFeatureParameterValue(requestContext, "SearchCategories", "DE799EAA-7564-4882-8E86-B0FA85919559", out value2) || !(value2 == "1")))
		{
			if (GeminiApplicationCore.FeatureCacheManager.TryGetFeatureParameterValue(requestContext, "SearchCategories", "4E22B171-B878-4751-8184-DD87B2DD65F1", out value3))
			{
				return value3 == "1";
			}
			return false;
		}
		return true;
	}
}
