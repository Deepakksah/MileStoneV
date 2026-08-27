using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("MetadataSearchPath")]
public class MetadataSearchPathGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertSearchPathToGenericObject2(MetadataSearchPath.QueryMetadataSearchPathById(requestContext, id));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "MetadataObjectDefinition" && entityName == "MetadataSearchPath")
		{
			return MetadataSearchPath.QueryMetadataSearchPathIdsByObjectDefinitionId(requestContext, parentId);
		}
		return null;
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertSearchPathsToGenericObject2s(MetadataSearchPath.QueryMetadataSearchPathsByIds(requestContext, ids));
	}

	private static GenericObject2 ConvertSearchPathToGenericObject2(MetadataSearchPath searchPath)
	{
		if (searchPath == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", searchPath.Id));
		list.Add(new ObjectProperty2("SearchPath", searchPath.Path));
		list.Add(new ObjectProperty2("PropertyId", searchPath.PropertyId));
		return new GenericObject2(searchPath.Id, "MetadataSearchPath", list.ToArray(), null);
	}

	private static GenericObject2[] ConvertSearchPathsToGenericObject2s(MetadataSearchPath[] searchPaths)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (MetadataSearchPath searchPath in searchPaths)
		{
			list.Add(ConvertSearchPathToGenericObject2(searchPath));
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
