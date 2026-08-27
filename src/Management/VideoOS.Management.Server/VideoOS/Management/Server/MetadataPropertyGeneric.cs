using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("MetadataProperty")]
public class MetadataPropertyGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertPropertyToGenericObject2(MetadataProperty.QueryMetadataPropertyById(requestContext, id));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "MetadataFilter" && entityName == "MetadataProperty")
		{
			return MetadataProperty.QueryMetadataPropertyIdsByFilterId(requestContext, parentId);
		}
		return null;
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertPropertiesToGenericObject2s(MetadataProperty.QueryMetadataPropertiesByIds(requestContext, ids));
	}

	private static GenericObject2 ConvertPropertyToGenericObject2(MetadataProperty property)
	{
		if (property == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", property.Id));
		list.Add(new ObjectProperty2("Name", property.Name));
		return new GenericObject2(property.Id, "MetadataProperty", list.ToArray(), null);
	}

	private static GenericObject2[] ConvertPropertiesToGenericObject2s(MetadataProperty[] properties)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (MetadataProperty property in properties)
		{
			list.Add(ConvertPropertyToGenericObject2(property));
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
