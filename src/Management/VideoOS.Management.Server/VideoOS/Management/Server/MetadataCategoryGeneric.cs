using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("MetadataCategory")]
public class MetadataCategoryGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertMetadataCategoryToGenericObject2(MetadataCategory.QueryCategoryById(requestContext, id));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "ManagementServer" && entityName == "MetadataCategory")
		{
			return MetadataCategory.QueryCategoryIds(requestContext);
		}
		return null;
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertMetadataCategoriesToGenericObject2s(MetadataCategory.QueryCategoriesByIds(requestContext, ids));
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		MetadataCategory metadataCategory = ConvertToMetadataCategory(genericObject);
		metadataCategory = MetadataCategory.UpdateCategory(requestContext, metadataCategory.Id, metadataCategory.Visibility);
		return ConvertMetadataCategoryToGenericObject2(metadataCategory);
	}

	private static GenericObject2 ConvertMetadataCategoryToGenericObject2(MetadataCategory metadataCategory)
	{
		if (metadataCategory == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", metadataCategory.Id));
		list.Add(new ObjectProperty2("Visibility", metadataCategory.Visibility.Value));
		return new GenericObject2(metadataCategory.Id, "MetadataCategory", list.ToArray(), null);
	}

	private static GenericObject2[] ConvertMetadataCategoriesToGenericObject2s(MetadataCategory[] metadataCategories)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (MetadataCategory metadataCategory in metadataCategories)
		{
			list.Add(ConvertMetadataCategoryToGenericObject2(metadataCategory));
		}
		return list.ToArray();
	}

	private static MetadataCategory ConvertToMetadataCategory(GenericObject2 genericObject)
	{
		MetadataCategory metadataCategory = new MetadataCategory();
		if (GenericObjectHelper2.Contains(genericObject.Properties, "Id"))
		{
			metadataCategory.Id = GenericObjectHelper2.GetProperty<string>(genericObject.Properties, "Id");
		}
		if (GenericObjectHelper2.Contains(genericObject.Properties, "Visibility"))
		{
			metadataCategory.Visibility = GenericObjectHelper2.GetProperty<bool>(genericObject.Properties, "Visibility");
		}
		return metadataCategory;
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
