using System;
using VideoOS.Management.Server.BusinessObjects.MetadataSearch;
using VideoOS.Management.Server.Log;
using VideoOS.MetadataSearch.Common;

namespace VideoOS.Management.Server;

public class MetadataCategory
{
	public string Id { get; set; }

	public bool? Visibility { get; set; }

	public static MetadataCategory QueryCategoryById(IGeminiRequestContext requestContext, string id)
	{
		requestContext.ValidateMetadataSearchLicense();
		requestContext.CheckReadMetadataConfigurationPermission();
		using IMetadataSearchComponent metadataSearchComponent = requestContext.CreateMetadataSearchComponent();
		return metadataSearchComponent.QueryCategoryById(id);
	}

	public static MetadataCategory[] QueryCategoriesByIds(IGeminiRequestContext requestContext, string[] ids)
	{
		requestContext.ValidateMetadataSearchLicense();
		requestContext.CheckReadMetadataConfigurationPermission();
		using IMetadataSearchComponent metadataSearchComponent = requestContext.CreateMetadataSearchComponent();
		return metadataSearchComponent.QueryCategoriesByIds(ids);
	}

	public static string[] QueryCategoryIds(IGeminiRequestContext requestContext)
	{
		requestContext.ValidateMetadataSearchLicense();
		requestContext.CheckReadMetadataConfigurationPermission();
		using IMetadataSearchComponent metadataSearchComponent = requestContext.CreateMetadataSearchComponent();
		return metadataSearchComponent.QueryCategoryIds();
	}

	public static MetadataCategory UpdateCategory(IGeminiRequestContext requestContext, string id, bool? visibility)
	{
		requestContext.ValidateMetadataSearchLicense();
		ValidateCategoryParameters(ref id, ref visibility);
		requestContext.CheckEditMetadataConfigurationPermission();
		MetadataCategory metadataCategory = null;
		using (IMetadataSearchComponent metadataSearchComponent = requestContext.CreateMetadataSearchComponent())
		{
			metadataSearchComponent.UpdateCategoryVisibility(id, visibility.Value);
			metadataCategory = metadataSearchComponent.QueryCategoryById(id);
		}
		if (metadataCategory != null)
		{
			using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
			if (Guid.TryParse(id, out var result))
			{
				vmoLogManager.LogEntryAuditLogger.AuditMetadataSearchCategoryVisibilityChanged(id, Categories.GetValue(result), visibility.Value, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
			}
		}
		return metadataCategory;
	}

	private static void ValidateCategoryParameters(ref string id, ref bool? visibility)
	{
		Validation.CheckObjectId(ref id, "id", allowNullOrEmpty: false);
		Validation.CheckBooleanValue(visibility, "visibility", allowNull: false);
	}
}
