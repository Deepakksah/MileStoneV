using System;
using System.Collections.Generic;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Log;

namespace VideoOS.Management.Server;

public sealed class ViewItem
{
	public string Id { get; set; }

	public int Position { get; set; }

	public string DefinitionXml { get; set; }

	public string ViewId { get; set; }

	public CustomProperty[] CustomProperties { get; set; }

	public string ViewGroupId { get; set; }

	public string[] RelatedViewGroupIds { get; set; }

	public string MonitorId { get; set; }

	public string VideoWallId { get; set; }

	public static ViewItem ClearViewItem(IGeminiRequestContext requestContext, string viewItemId, string smartClientId)
	{
		Guid sessionId = Guid.NewGuid();
		ViewItem viewItem;
		using (requestContext.CreateComponent<ViewComponent>())
		{
			viewItem = FindViewItem(requestContext, viewItemId);
		}
		if (viewItem == null)
		{
			throw new ViewNotFoundException();
		}
		if (!string.IsNullOrEmpty(viewItem.VideoWallId))
		{
			requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "OPERATE", viewItem.VideoWallId);
			requestContext.GetService<SecurityManagerService>().CheckTimeBasedPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "OPERATE", viewItem.VideoWallId);
		}
		else if (!string.IsNullOrEmpty(viewItem.ViewGroupId))
		{
			requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.ViewGroup, "OPERATE", ServerGroup.GetRootViewGroupId(requestContext, viewItem.ViewGroupId));
		}
		using (VideoWallFunctionalityComponent videoWallFunctionalityComponent = requestContext.CreateComponent<VideoWallFunctionalityComponent>())
		{
			videoWallFunctionalityComponent.ClearViewItem(viewItemId, sessionId, smartClientId);
		}
		using (requestContext.CreateComponent<ViewComponent>())
		{
			viewItem = FindViewItem(requestContext, viewItemId);
			if (!string.IsNullOrEmpty(viewItem.VideoWallId))
			{
				Integration.FireMonitorViewChanged(requestContext, viewItem.ViewId, viewItem.MonitorId, viewItem.VideoWallId, EventAction.Updated);
			}
			else
			{
				Integration.FireViewItemChanged(requestContext, viewItem.Id, viewItem.ViewId, EventAction.Updated);
			}
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return viewItem;
	}

	public static ViewItem UpdateViewItem(IGeminiRequestContext requestContext, ViewItem viewItem)
	{
		string objectId = viewItem.Id;
		Validation.CheckObjectId(ref objectId, "viewItemId", allowNullOrEmpty: false);
		ViewItem viewItem2;
		using (requestContext.CreateComponent<ViewComponent>())
		{
			viewItem2 = FindViewItem(requestContext, objectId);
		}
		if (viewItem2 == null)
		{
			throw new ViewNotFoundException();
		}
		if (!string.IsNullOrEmpty(viewItem2.VideoWallId))
		{
			requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "OPERATE", viewItem2.VideoWallId);
			requestContext.GetService<SecurityManagerService>().CheckTimeBasedPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "OPERATE", viewItem2.VideoWallId);
		}
		else if (!string.IsNullOrEmpty(viewItem2.ViewGroupId))
		{
			requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.ViewGroup, "OPERATE", ServerGroup.GetRootViewGroupId(requestContext, viewItem2.ViewGroupId));
		}
		using (ViewComponent viewComponent = requestContext.CreateComponent<ViewComponent>())
		{
			viewComponent.UpdateViewItem(viewItem);
			viewItem = FindViewItem(requestContext, objectId);
			if (!string.IsNullOrEmpty(viewItem2.VideoWallId))
			{
				Integration.FireMonitorViewChanged(requestContext, viewItem.ViewId, viewItem2.MonitorId, viewItem.VideoWallId, EventAction.Updated);
			}
			else
			{
				Integration.FireViewItemChanged(requestContext, viewItem.Id, viewItem.ViewId, EventAction.Updated);
			}
		}
		return viewItem;
	}

	public static void ChangeViewItemId(IGeminiRequestContext requestContext, string existingId, string newId, bool fireChangeEvent = true)
	{
		ViewItem viewItem;
		using (requestContext.CreateComponent<ViewComponent>())
		{
			viewItem = FindViewItem(requestContext, existingId);
		}
		if (viewItem == null)
		{
			throw new ViewNotFoundException();
		}
		if (!string.IsNullOrEmpty(viewItem.VideoWallId))
		{
			requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "OPERATE", viewItem.VideoWallId);
			requestContext.GetService<SecurityManagerService>().CheckTimeBasedPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "OPERATE", viewItem.VideoWallId);
		}
		else if (!string.IsNullOrEmpty(viewItem.ViewGroupId))
		{
			requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.ViewGroup, "OPERATE", ServerGroup.GetRootViewGroupId(requestContext, viewItem.ViewGroupId));
		}
		using ViewComponent viewComponent = requestContext.CreateComponent<ViewComponent>();
		viewComponent.ChangeViewItemId(existingId, newId);
		if (fireChangeEvent)
		{
			if (!string.IsNullOrEmpty(viewItem.VideoWallId))
			{
				Integration.FireMonitorViewChanged(requestContext, newId, viewItem.MonitorId, viewItem.VideoWallId, EventAction.Updated);
			}
			else
			{
				Integration.FireViewItemChanged(requestContext, newId, viewItem.ViewId, EventAction.Updated);
			}
		}
	}

	private static ViewItem FindViewItem(IGeminiRequestContext requestContext, string viewItemId)
	{
		using IViewComponent viewComponent = requestContext.CreateViewComponent();
		return viewComponent.QueryViewItem(viewItemId);
	}

	public static ViewItem QueryViewItem(IGeminiRequestContext requestContext, string viewItemId)
	{
		Validation.CheckObjectId(ref viewItemId, "viewItemId", allowNullOrEmpty: false);
		ViewItem viewItem;
		using (requestContext.CreateViewComponent())
		{
			viewItem = FindViewItem(requestContext, viewItemId);
		}
		if (viewItem != null)
		{
			if (!string.IsNullOrEmpty(viewItem.VideoWallId))
			{
				if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "GENERIC_READ", viewItem.VideoWallId))
				{
					return null;
				}
			}
			else
			{
				if (string.IsNullOrEmpty(viewItem.ViewGroupId))
				{
					return null;
				}
				if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.ViewGroup, "GENERIC_READ", ServerGroup.GetRootViewGroupId(requestContext, viewItem.ViewGroupId)))
				{
					return null;
				}
			}
		}
		return viewItem;
	}

	public static string[] QueryViewItemIds(IGeminiRequestContext requestContext, string viewId)
	{
		string[] result = new string[0];
		View view = View.QueryView(requestContext, viewId);
		if (view != null)
		{
			using IViewComponent viewComponent = requestContext.CreateViewComponent();
			result = viewComponent.QueryViewItemIds(view.Id);
		}
		return result;
	}

	public static ViewItem[] QueryViewItemsByIds(IGeminiRequestContext requestContext, string[] ids)
	{
		using IViewComponent viewComponent = requestContext.CreateViewComponent();
		return FilterViewsBasedOnPermissions(requestContext, viewComponent.QueryViewItemsByIds(ids));
	}

	public static ViewItem[] QueryViewItemsAll(IGeminiRequestContext requestContext)
	{
		using IViewComponent viewComponent = requestContext.CreateViewComponent();
		return FilterViewsBasedOnPermissions(requestContext, viewComponent.QueryViewItemsAll());
	}

	private static ViewItem[] FilterViewsBasedOnPermissions(IGeminiRequestContext requestContext, ViewItem[] viewItemArray)
	{
		List<ViewItem> list = new List<ViewItem>();
		foreach (ViewItem viewItem in viewItemArray)
		{
			if (!string.IsNullOrEmpty(viewItem.VideoWallId))
			{
				if (requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "GENERIC_READ", viewItem.VideoWallId))
				{
					list.Add(viewItem);
				}
			}
			else if (!string.IsNullOrEmpty(viewItem.ViewGroupId) && requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.ViewGroup, "GENERIC_READ", ServerGroup.GetRootViewGroupId(requestContext, viewItem.ViewGroupId)))
			{
				list.Add(viewItem);
			}
		}
		return list.ToArray();
	}
}
