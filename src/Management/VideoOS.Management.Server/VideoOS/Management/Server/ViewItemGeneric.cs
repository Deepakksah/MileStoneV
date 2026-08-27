using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("ViewItem")]
public sealed class ViewItemGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2(ViewItem.QueryViewItem(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2s(ViewItem.QueryViewItemsByIds(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "View")
		{
			return ViewItem.QueryViewItemIds(requestContext, parentId);
		}
		return new string[0];
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		throw new NotSupportedException();
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		ViewItem viewItem = ConvertToViewItem(genericObject);
		viewItem.CustomProperties = customProperties;
		return ConvertToGenericObject2(ViewItem.UpdateViewItem(requestContext, viewItem));
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		View.DeleteView(requestContext, id);
	}

	public static GenericObject2 ClearViewItem(IGeminiRequestContext requestContext, string viewItemId)
	{
		return ConvertToGenericObject2(ViewItem.ClearViewItem(requestContext, viewItemId, null));
	}

	public static void ChangeViewItemId(IGeminiRequestContext requestContext, string existingId, string newId)
	{
		ViewItem.ChangeViewItemId(requestContext, existingId, newId);
	}

	private static GenericObject2 ConvertToGenericObject2(ViewItem viewItem)
	{
		if (viewItem == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", viewItem.Id));
		list.Add(new ObjectProperty2("DefinitionXml", viewItem.DefinitionXml));
		list.Add(new ObjectProperty2("ViewId", viewItem.ViewId));
		List<RelationProperty> list2 = new List<RelationProperty>();
		if (!string.IsNullOrEmpty(viewItem.ViewId))
		{
			list2.Add(new RelationProperty("View", viewItem.ViewId));
		}
		return new GenericObject2(viewItem.Id, "ViewItem", list.ToArray(), list2.ToArray());
	}

	private static GenericObject2[] ConvertToGenericObject2s(ViewItem[] viewItems)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (ViewItem viewItem in viewItems)
		{
			list.Add(ConvertToGenericObject2(viewItem));
		}
		return list.ToArray();
	}

	private static ViewItem ConvertToViewItem(GenericObject2 genericObject)
	{
		return ConvertToViewItem(genericObject.Properties);
	}

	public static ViewItem ConvertToViewItem(ObjectProperty2[] properties)
	{
		ViewItem viewItem = new ViewItem();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			viewItem.Id = GenericObjectHelper2.GetProperty<string>(properties, "Id");
		}
		if (GenericObjectHelper2.Contains(properties, "DefinitionXml"))
		{
			viewItem.DefinitionXml = GenericObjectHelper2.GetProperty<string>(properties, "DefinitionXml");
		}
		if (GenericObjectHelper2.Contains(properties, "ViewId"))
		{
			viewItem.ViewId = GenericObjectHelper2.GetProperty<string>(properties, "ViewId");
		}
		return viewItem;
	}
}
