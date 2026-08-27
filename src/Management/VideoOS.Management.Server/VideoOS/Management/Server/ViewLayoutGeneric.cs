using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("ViewLayout")]
public class ViewLayoutGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2(ViewLayout.QueryViewLayoutById(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2s(ViewLayout.QueryViewLayoutByIds(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "ViewLayoutGroup")
		{
			return ViewLayout.QueryViewLayoutIdsByViewGroupId(requestContext, parentId);
		}
		return new string[0];
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

	private static GenericObject2 ConvertToGenericObject2(ViewLayout viewLayout)
	{
		if (viewLayout == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", viewLayout.Id));
		list.Add(new ObjectProperty2("Name", viewLayout.Name));
		list.Add(new ObjectProperty2("Description", viewLayout.Description));
		list.Add(new ObjectProperty2("DefinitionXml", viewLayout.DefinitionXml));
		list.Add(new ObjectProperty2("LastModified", viewLayout.LastModified));
		list.Add(new ObjectProperty2("CreatedDate", viewLayout.CreatedDate));
		list.Add(new ObjectProperty2("ParentServerGroupId", viewLayout.ViewLayoutGroupId));
		List<RelationProperty> list2 = new List<RelationProperty>();
		list2.Add(new RelationProperty("ViewLayoutGroup", viewLayout.ViewLayoutGroupId));
		return new GenericObject2(viewLayout.Id, "ViewLayout", list.ToArray(), list2.ToArray());
	}

	private static GenericObject2[] ConvertToGenericObject2s(ViewLayout[] viewLayouts)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (ViewLayout viewLayout in viewLayouts)
		{
			list.Add(ConvertToGenericObject2(viewLayout));
		}
		return list.ToArray();
	}
}
