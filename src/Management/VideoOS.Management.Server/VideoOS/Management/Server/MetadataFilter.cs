using System;
using VideoOS.Management.Server.BusinessObjects.MetadataSearch;
using VideoOS.Management.Server.Log;
using VideoOS.MetadataSearch.Common;

namespace VideoOS.Management.Server;

public class MetadataFilter
{
	public string Id { get; set; }

	public bool? Visibility { get; set; }

	public static MetadataFilter QueryFilterById(IGeminiRequestContext requestContext, string id)
	{
		requestContext.ValidateMetadataSearchLicense();
		requestContext.CheckReadMetadataConfigurationPermission();
		using IMetadataSearchComponent metadataSearchComponent = requestContext.CreateMetadataSearchComponent();
		return metadataSearchComponent.QueryFilterById(id);
	}

	public static MetadataFilter[] QueryFiltersByIds(IGeminiRequestContext requestContext, string[] ids)
	{
		requestContext.ValidateMetadataSearchLicense();
		requestContext.CheckReadMetadataConfigurationPermission();
		using IMetadataSearchComponent metadataSearchComponent = requestContext.CreateMetadataSearchComponent();
		return metadataSearchComponent.QueryFiltersByIds(ids);
	}

	public static string[] QueryFilterIdsByCategoryId(IGeminiRequestContext requestContext, string categoryId)
	{
		requestContext.ValidateMetadataSearchLicense();
		requestContext.CheckReadMetadataConfigurationPermission();
		using IMetadataSearchComponent metadataSearchComponent = requestContext.CreateMetadataSearchComponent();
		return metadataSearchComponent.QueryFilterIdsByCategoryId(categoryId);
	}

	public static MetadataFilter UpdateFilterVisibility(IGeminiRequestContext requestContext, string id, bool? visibility)
	{
		requestContext.ValidateMetadataSearchLicense();
		Validation.CheckObjectId(id, "id", allowNullOrEmpty: false);
		Validation.CheckBooleanValue(visibility, "visibility", allowNull: false);
		requestContext.CheckEditMetadataConfigurationPermission();
		MetadataFilter metadataFilter;
		using (IMetadataSearchComponent metadataSearchComponent = requestContext.CreateMetadataSearchComponent())
		{
			metadataSearchComponent.UpdateFilterVisibility(id, visibility.Value);
			metadataFilter = metadataSearchComponent.QueryFilterById(id);
		}
		if (metadataFilter != null)
		{
			using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
			if (Guid.TryParse(id, out var result))
			{
				vmoLogManager.LogEntryAuditLogger.AuditMetadataSearchFilterVisibilityChanged(id, Filters.GetValue(result), visibility.Value, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
			}
		}
		return metadataFilter;
	}
}
