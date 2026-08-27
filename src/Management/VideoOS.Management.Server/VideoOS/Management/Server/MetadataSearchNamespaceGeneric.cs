using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("MetadataSearchNamespace")]
public class MetadataSearchNamespaceGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertMetadataSearchNamespaceToGenericObject2(MetadataSearchNamespace.QueryMetadataSearchNamespaceById(requestContext, id));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "ManagementServer" && entityName == "MetadataSearchNamespace")
		{
			return MetadataSearchNamespace.QueryMetadataSearchNamespaceIds(requestContext);
		}
		return null;
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertMetadataSearchNamespaceToGenericObject2s(MetadataSearchNamespace.QueryMetadataSearchNamespacesByIds(requestContext, ids));
	}

	private static GenericObject2 ConvertMetadataSearchNamespaceToGenericObject2(MetadataSearchNamespace metadataSearchNamespace)
	{
		if (metadataSearchNamespace == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", metadataSearchNamespace.Id));
		list.Add(new ObjectProperty2("Prefix", metadataSearchNamespace.Prefix));
		list.Add(new ObjectProperty2("Uri", metadataSearchNamespace.Uri.ToString()));
		return new GenericObject2(metadataSearchNamespace.Id, "MetadataSearchNamespace", list.ToArray(), null);
	}

	private static GenericObject2[] ConvertMetadataSearchNamespaceToGenericObject2s(MetadataSearchNamespace[] metadataSearchNamespaces)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (MetadataSearchNamespace metadataSearchNamespace in metadataSearchNamespaces)
		{
			list.Add(ConvertMetadataSearchNamespaceToGenericObject2(metadataSearchNamespace));
		}
		return list.ToArray();
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		throw new NotSupportedException();
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		throw new NotSupportedException();
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		throw new NotSupportedException();
	}
}
