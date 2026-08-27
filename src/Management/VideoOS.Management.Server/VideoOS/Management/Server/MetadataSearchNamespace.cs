using System;
using VideoOS.Management.Server.BusinessObjects.MetadataSearch;

namespace VideoOS.Management.Server;

public class MetadataSearchNamespace
{
	public string Id { get; set; }

	public string Prefix { get; set; }

	public Uri Uri { get; set; }

	public static string[] QueryMetadataSearchNamespaceIds(IGeminiRequestContext requestContext)
	{
		requestContext.ValidateMetadataSearchLicense();
		requestContext.CheckReadMetadataConfigurationPermission();
		return Array.Empty<string>();
	}

	public static MetadataSearchNamespace QueryMetadataSearchNamespaceById(IGeminiRequestContext requestContext, string id)
	{
		requestContext.ValidateMetadataSearchLicense();
		requestContext.CheckReadMetadataConfigurationPermission();
		return null;
	}

	public static MetadataSearchNamespace[] QueryMetadataSearchNamespacesByIds(IGeminiRequestContext requestContext, string[] ids)
	{
		requestContext.ValidateMetadataSearchLicense();
		requestContext.CheckReadMetadataConfigurationPermission();
		return Array.Empty<MetadataSearchNamespace>();
	}
}
