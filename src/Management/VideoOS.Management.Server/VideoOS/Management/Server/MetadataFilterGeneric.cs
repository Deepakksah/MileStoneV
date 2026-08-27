using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("MetadataFilter")]
public class MetadataFilterGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertMetadataFilterToGenericObject2(MetadataFilter.QueryFilterById(requestContext, id));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "MetadataCategory" && entityName == "MetadataFilter")
		{
			return MetadataFilter.QueryFilterIdsByCategoryId(requestContext, parentId);
		}
		return null;
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertMetadataFiltersToGenericObject2s(MetadataFilter.QueryFiltersByIds(requestContext, ids));
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		MetadataFilter metadataFilter = ConvertGenericObject2ToMetadataFilter(genericObject);
		metadataFilter = MetadataFilter.UpdateFilterVisibility(requestContext, metadataFilter.Id, metadataFilter.Visibility);
		return ConvertMetadataFilterToGenericObject2(metadataFilter);
	}

	private static GenericObject2 ConvertMetadataFilterToGenericObject2(MetadataFilter metadataFilter)
	{
		if (metadataFilter == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", metadataFilter.Id));
		list.Add(new ObjectProperty2("Visibility", metadataFilter.Visibility.Value));
		return new GenericObject2(metadataFilter.Id, "MetadataFilter", list.ToArray(), null);
	}

	private static GenericObject2[] ConvertMetadataFiltersToGenericObject2s(MetadataFilter[] metadataFilters)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (MetadataFilter metadataFilter in metadataFilters)
		{
			list.Add(ConvertMetadataFilterToGenericObject2(metadataFilter));
		}
		return list.ToArray();
	}

	private static MetadataFilter ConvertGenericObject2ToMetadataFilter(GenericObject2 genericObject)
	{
		MetadataFilter metadataFilter = new MetadataFilter();
		if (GenericObjectHelper2.Contains(genericObject.Properties, "Id"))
		{
			metadataFilter.Id = GenericObjectHelper2.GetProperty<string>(genericObject.Properties, "Id");
		}
		if (GenericObjectHelper2.Contains(genericObject.Properties, "Visibility"))
		{
			metadataFilter.Visibility = GenericObjectHelper2.GetProperty<bool>(genericObject.Properties, "Visibility");
		}
		return metadataFilter;
	}

	GenericObject2 IGenericObjectSupport2.CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		throw new NotSupportedException();
	}

	void IGenericObjectSupport2.DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		throw new NotSupportedException();
	}
}
