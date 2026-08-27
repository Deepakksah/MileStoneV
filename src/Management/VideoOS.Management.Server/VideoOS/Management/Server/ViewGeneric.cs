using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("View")]
public sealed class ViewGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2(requestContext, View.QueryView(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2s(requestContext, View.QueryViewsByIds(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "PublicViewGroup" || parentEntityName == "PrivateViewGroup")
		{
			return View.QueryViewIds(requestContext, parentId);
		}
		return new string[0];
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		throw new NotSupportedException();
	}

	public static GenericObject2 CreateView(IGeminiRequestContext requestContext, string viewGroupId, ObjectProperty2[] viewProperties, CustomProperty[] viewCustomProperties, ObjectProperty2[][] viewItemsProperties, CustomProperty[][] viewItemsCustomProperties)
	{
		View view = ConvertToView(viewProperties);
		view.CustomProperties = viewCustomProperties;
		List<ViewItem> list = new List<ViewItem>();
		for (int i = 0; i < viewItemsProperties.GetLength(0); i++)
		{
			ViewItem viewItem = ViewItemGeneric.ConvertToViewItem(viewItemsProperties[i]);
			viewItem.CustomProperties = viewItemsCustomProperties[i];
			list.Add(viewItem);
		}
		view.ViewItems = list.ToArray();
		return ConvertToGenericObject2(requestContext, View.CreateView(requestContext, viewGroupId, view));
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		View view = ConvertToView(genericObject);
		view.CustomProperties = customProperties;
		return ConvertToGenericObject2(requestContext, View.UpdateView(requestContext, view));
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		View.DeleteView(requestContext, id);
	}

	public static GenericObject2 ClearView(IGeminiRequestContext requestContext, string viewId)
	{
		return ConvertToGenericObject2(requestContext, View.ClearView(requestContext, viewId, null));
	}

	public static GenericObject2 ApplyViewToView(IGeminiRequestContext requestContext, string sourceViewId, string destinationViewId)
	{
		return ConvertToGenericObject2(requestContext, View.ApplyViewToView(requestContext, sourceViewId, destinationViewId, null));
	}

	private static GenericObject2 ConvertToGenericObject2(IGeminiRequestContext requestContext, View view)
	{
		if (view == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", view.Id));
		list.Add(new ObjectProperty2("Name", view.Name));
		list.Add(new ObjectProperty2("Description", view.Description));
		list.Add(new ObjectProperty2("CreatedDate", view.CreatedDate));
		list.Add(new ObjectProperty2("LastModified", view.LastModified));
		list.Add(new ObjectProperty2("DefinitionXml", view.DefinitionXml));
		list.Add(new ObjectProperty2("ParentServerGroupId", view.ViewGroupId));
		list.Add(new ObjectProperty2("MonitorId", view.MonitorId));
		list.Add(new ObjectProperty2("VideoWallId", view.VideoWallId));
		List<RelationProperty> list2 = new List<RelationProperty>();
		if (view.RelatedViewGroupIds != null)
		{
			string[] array = (from r in view.RelatedViewGroupIds
				select r.Trim() into r
				where !string.IsNullOrEmpty(r)
				select r).ToArray();
			bool num = ServerGroup.HasPrivateViewGroupInRelations(array);
			string text = null;
			text = ((!num) ? "PublicViewGroup" : "PrivateViewGroup");
			string privateUserViewGroupId = string.Empty;
			if (num)
			{
				privateUserViewGroupId = ServerGroup.QueryPrivateUserViewGroupId(requestContext, requestContext.UserDomainAccountName);
			}
			foreach (string item in array.Where((string r) => !ServerGroup.IsPrivateViewGroupRoot(r) && !string.Equals(r, privateUserViewGroupId, StringComparison.OrdinalIgnoreCase) && !ServerGroup.IsSpecialType(r, "6A85FFC3-C134-4338-A072-024817CB5820") && !ServerGroup.IsSpecialType(r, "430C2FE2-B233-42DA-B508-1A22EF03383D") && !ServerGroup.IsSpecialType(r, "ABF0C309-E9F6-41AF-AA1A-A48B45AA0683")))
			{
				list2.Add(new RelationProperty(text, item));
			}
		}
		if (!string.IsNullOrEmpty(view.MonitorId) && !string.IsNullOrEmpty(view.VideoWallId))
		{
			list2.Add(new RelationProperty("VideoWall", view.VideoWallId));
			list2.Add(new RelationProperty("Monitor", view.MonitorId));
		}
		return new GenericObject2(view.Id, "View", list.ToArray(), list2.ToArray());
	}

	private static GenericObject2[] ConvertToGenericObject2s(IGeminiRequestContext requestContext, View[] views)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (View view in views)
		{
			list.Add(ConvertToGenericObject2(requestContext, view));
		}
		return list.ToArray();
	}

	private static View ConvertToView(GenericObject2 genericObject)
	{
		return ConvertToView(genericObject.Properties);
	}

	private static View ConvertToView(ObjectProperty2[] properties)
	{
		View view = new View();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			view.Id = GenericObjectHelper2.GetProperty<string>(properties, "Id");
		}
		if (GenericObjectHelper2.Contains(properties, "Name"))
		{
			view.Name = GenericObjectHelper2.GetProperty<string>(properties, "Name");
		}
		if (GenericObjectHelper2.Contains(properties, "Description"))
		{
			view.Description = GenericObjectHelper2.GetProperty<string>(properties, "Description");
		}
		if (GenericObjectHelper2.Contains(properties, "CreatedDate"))
		{
			view.CreatedDate = GenericObjectHelper2.GetProperty<DateTime>(properties, "CreatedDate");
		}
		if (GenericObjectHelper2.Contains(properties, "LastModified"))
		{
			view.LastModified = GenericObjectHelper2.GetProperty<DateTime>(properties, "LastModified");
		}
		if (GenericObjectHelper2.Contains(properties, "DefinitionXml"))
		{
			view.DefinitionXml = GenericObjectHelper2.GetProperty<string>(properties, "DefinitionXml");
		}
		return view;
	}
}
