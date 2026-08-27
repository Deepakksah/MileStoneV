using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("MetadataObjectDefinition")]
public class MetadataObjectDefinitionGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertObjectDefinitionToGenericObject2(MetadataObjectDefinition.QueryMetadataObjectDefinitionById(requestContext, id));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "ManagementServer" && entityName == "MetadataObjectDefinition")
		{
			return MetadataObjectDefinition.QueryMetadataObjectDefinitionIds(requestContext);
		}
		return null;
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertObjectDefinitionsToGenericObject2s(MetadataObjectDefinition.QueryMetadataObjectDefinitionsByIds(requestContext, ids));
	}

	private static GenericObject2 ConvertObjectDefinitionToGenericObject2(MetadataObjectDefinition objectDefinition)
	{
		if (objectDefinition == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", objectDefinition.Id));
		list.Add(new ObjectProperty2("ObjectRoot", objectDefinition.ObjectRoot));
		return new GenericObject2(objectDefinition.Id, "MetadataObjectDefinition", list.ToArray(), null);
	}

	private static GenericObject2[] ConvertObjectDefinitionsToGenericObject2s(MetadataObjectDefinition[] objectDefinitions)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (MetadataObjectDefinition objectDefinition in objectDefinitions)
		{
			list.Add(ConvertObjectDefinitionToGenericObject2(objectDefinition));
		}
		return list.ToArray();
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		throw new NotSupportedException();
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		throw new NotSupportedException();
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		throw new NotSupportedException();
	}
}
