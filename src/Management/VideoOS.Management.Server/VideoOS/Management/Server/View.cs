using System;
using System.Collections.Generic;
using System.Xml;
using VideoOS.Management.Common;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Builders;
using VideoOS.Management.Server.Log;

namespace VideoOS.Management.Server;

public sealed class View
{
	public string Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string DefinitionXml { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime LastModified { get; set; }

	public ViewItem[] ViewItems { get; set; }

	public CustomProperty[] CustomProperties { get; set; }

	public string ViewGroupId { get; set; }

	public string[] RelatedViewGroupIds { get; set; }

	public string MonitorId { get; set; }

	public string VideoWallId { get; set; }

	public static View[] QueryViewsNoSecurity(IGeminiRequestContext requestContext, string parentId)
	{
		using IViewComponent viewComponent = requestContext.CreateViewComponent();
		return viewComponent.QueryViews(parentId);
	}

	public static View[] QueryViewHeriarchyByServerGroupIdNoSecurity(IGeminiRequestContext requestContext, string serverGroupId)
	{
		using IViewComponent viewComponent = requestContext.CreateViewComponent();
		return viewComponent.QueryViewHierarchyByServerGroupId(serverGroupId);
	}

	public static View[] QueryViewAllNoSecurity(IGeminiRequestContext requestContext)
	{
		using IViewComponent viewComponent = requestContext.CreateViewComponent();
		return viewComponent.QueryViewAllNoSecurity();
	}

	public static View[] QueryViewsByIds(IGeminiRequestContext requestContext, string[] viewIds)
	{
		using IViewComponent viewComponent = requestContext.CreateViewComponent();
		return FilterViewsBasedOnPermissions(requestContext, viewComponent.QueryViewsByIds(viewIds));
	}

	public static string[] QueryViewIds(IGeminiRequestContext requestContext, string viewGroupId)
	{
		string[] result = new string[0];
		string rootViewGroupId = ServerGroup.GetRootViewGroupId(requestContext, viewGroupId);
		if (requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.ViewGroup, "GENERIC_READ", rootViewGroupId))
		{
			using IViewComponent viewComponent = requestContext.CreateViewComponent();
			result = viewComponent.QueryViewIds(viewGroupId);
		}
		return result;
	}

	public static View QueryView(IGeminiRequestContext requestContext, string viewId)
	{
		Validation.CheckObjectId(ref viewId, "viewId", allowNullOrEmpty: false);
		View view = null;
		using (IViewComponent viewComponent = requestContext.CreateViewComponent())
		{
			view = viewComponent.QueryView(viewId);
			if (view != null)
			{
				if (!string.IsNullOrEmpty(view.VideoWallId))
				{
					if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "GENERIC_READ", view.VideoWallId))
					{
						view = null;
					}
				}
				else if (!string.IsNullOrEmpty(view.ViewGroupId))
				{
					string rootViewGroupId = ServerGroup.GetRootViewGroupId(requestContext, view.ViewGroupId);
					if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.ViewGroup, requestContext.AuthenticatedUserSid, "GENERIC_READ", rootViewGroupId))
					{
						view = null;
					}
				}
				else
				{
					view = null;
				}
			}
		}
		if (view == null)
		{
			throw new ViewNotFoundException();
		}
		return view;
	}

	public static View ApplyViewToView(IGeminiRequestContext requestContext, string sourceViewId, string destinationViewId, string smartClientId)
	{
		Guid sessionId = Guid.NewGuid();
		Validation.CheckObjectId(ref sourceViewId, "sourceViewId", allowNullOrEmpty: false);
		Validation.CheckObjectId(ref destinationViewId, "destinationViewId", allowNullOrEmpty: false);
		View view = QueryView(requestContext, destinationViewId);
		View view2 = QueryView(requestContext, sourceViewId);
		if (view == null || view2 == null)
		{
			throw new ViewNotFoundException();
		}
		try
		{
			if (!string.IsNullOrEmpty(view.VideoWallId))
			{
				requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "OPERATE", view.VideoWallId);
				requestContext.GetService<SecurityManagerService>().CheckTimeBasedPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "OPERATE", view.VideoWallId);
			}
			else if (!string.IsNullOrEmpty(view.ViewGroupId))
			{
				string rootViewGroupId = ServerGroup.GetRootViewGroupId(requestContext, view.ViewGroupId);
				requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.ViewGroup, "OPERATE", rootViewGroupId);
			}
		}
		catch (SecurityObjectDoesNotExistException)
		{
			throw new ViewNotFoundException();
		}
		using (ViewComponent viewComponent = requestContext.CreateComponent<ViewComponent>())
		{
			viewComponent.ApplyViewToView(sourceViewId, destinationViewId, sessionId, smartClientId);
			view = FindView(requestContext, destinationViewId);
			if (viewComponent.IsMonitorView(destinationViewId, out var videoWallId))
			{
				Integration.FireMonitorViewChanged(requestContext, destinationViewId, view.MonitorId, videoWallId, EventAction.Updated);
				ViewGroupCache.Clear();
			}
			else
			{
				string parentIdFromId = GetParentIdFromId(requestContext, destinationViewId);
				Integration.FireViewChanged(requestContext, destinationViewId, parentIdFromId, EventAction.Updated);
				ViewGroupCache.Clear();
			}
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return view;
	}

	public static View CreateView(IGeminiRequestContext requestContext, string viewGroupId, View newView)
	{
		Validation.CheckObjectId(ref viewGroupId, "viewGroupId", allowNullOrEmpty: false);
		Validation.CheckName(newView.Name, "name");
		string description = newView.Description;
		Validation.CheckDescription(ref description, "description");
		newView.Description = description;
		try
		{
			string rootViewGroupId = ServerGroup.GetRootViewGroupId(requestContext, viewGroupId);
			requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.ViewGroup, "OPERATE", rootViewGroupId);
		}
		catch (SecurityObjectDoesNotExistException)
		{
			throw new ServerGroupNotFoundException();
		}
		using ViewComponent viewComponent = requestContext.CreateComponent<ViewComponent>();
		if (newView.ViewItems != null)
		{
			ViewItem[] viewItems = newView.ViewItems;
			foreach (ViewItem obj in viewItems)
			{
				obj.Id = GeminiResourceComponent.GetGuid(obj.Id);
			}
		}
		newView.Id = GeminiResourceComponent.GetGuid(newView.Id);
		viewComponent.CreateView(viewGroupId, newView);
		newView = FindView(requestContext, newView.Id);
		string parentIdFromId = GetParentIdFromId(requestContext, newView.Id);
		Integration.FireViewChanged(requestContext, newView.Id, parentIdFromId, EventAction.Added);
		ViewGroupCache.Clear();
		return newView;
	}

	public static View UpdateView(IGeminiRequestContext requestContext, View view)
	{
		Validation.CheckName(view.Name, "name");
		string description = view.Description;
		Validation.CheckDescription(ref description, "description");
		view.Description = description;
		View view2 = FindView(requestContext, view.Id);
		if (view2 == null)
		{
			throw new ViewNotFoundException();
		}
		try
		{
			if (!string.IsNullOrEmpty(view2.VideoWallId))
			{
				requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "OPERATE", view2.VideoWallId);
				requestContext.GetService<SecurityManagerService>().CheckTimeBasedPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "OPERATE", view2.VideoWallId);
			}
			else if (!string.IsNullOrEmpty(view2.ViewGroupId))
			{
				string rootViewGroupId = ServerGroup.GetRootViewGroupId(requestContext, view2.ViewGroupId);
				requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.ViewGroup, "OPERATE", rootViewGroupId);
			}
		}
		catch (SecurityObjectDoesNotExistException)
		{
			throw new ViewNotFoundException();
		}
		using (ViewComponent viewComponent = requestContext.CreateComponent<ViewComponent>())
		{
			viewComponent.UpdateView(view);
			view = FindView(requestContext, view.Id);
			if (viewComponent.IsMonitorView(view.Id, out var videoWallId))
			{
				Integration.FireMonitorViewChanged(requestContext, view.Id, view.MonitorId, videoWallId, EventAction.Updated);
			}
			else
			{
				string parentIdFromId = GetParentIdFromId(requestContext, view.Id);
				Integration.FireViewChanged(requestContext, view.Id, parentIdFromId, EventAction.Updated);
				ViewGroupCache.Clear();
			}
		}
		return view;
	}

	public static void DeleteView(IGeminiRequestContext requestContext, string viewId)
	{
		Validation.CheckObjectId(ref viewId, "viewId", allowNullOrEmpty: false);
		View view = FindView(requestContext, viewId);
		if (view == null)
		{
			throw new RemoveNonExistingViewException();
		}
		if (!string.IsNullOrEmpty(view.VideoWallId))
		{
			throw new InvalidOperationException();
		}
		if (!string.IsNullOrEmpty(view.ViewGroupId))
		{
			try
			{
				string rootViewGroupId = ServerGroup.GetRootViewGroupId(requestContext, view.ViewGroupId);
				requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.ViewGroup, "OPERATE", rootViewGroupId);
			}
			catch (SecurityObjectDoesNotExistException)
			{
				throw new RemoveNonExistingViewException();
			}
		}
		using ViewComponent viewComponent = requestContext.CreateComponent<ViewComponent>();
		string parentIdFromId = GetParentIdFromId(requestContext, viewId);
		viewComponent.DeleteView(viewId);
		Integration.FireViewChanged(requestContext, viewId, parentIdFromId, EventAction.Deleted);
		ViewGroupCache.Clear();
	}

	public static string GetIdFromItem(IGeminiRequestContext requestContext, string viewItemId)
	{
		using IViewComponent viewComponent = requestContext.CreateViewComponent();
		return viewComponent.GetIdFromItem(viewItemId);
	}

	private static string GetParentIdFromId(IGeminiRequestContext requestContext, string viewId)
	{
		using IServerGroupComponent serverGroupComponent = requestContext.CreateServerGroupComponent();
		return serverGroupComponent.GetServerGroupIdFromItem(viewId);
	}

	public static View ClearView(IGeminiRequestContext requestContext, string viewId, string smartClientId)
	{
		Guid sessionId = Guid.NewGuid();
		Validation.CheckObjectId(ref viewId, "viewId", allowNullOrEmpty: false);
		View view = QueryView(requestContext, viewId);
		if (view == null)
		{
			throw new ViewNotFoundException();
		}
		try
		{
			if (!string.IsNullOrEmpty(view.VideoWallId))
			{
				requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "OPERATE", view.VideoWallId);
				requestContext.GetService<SecurityManagerService>().CheckTimeBasedPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "OPERATE", view.VideoWallId);
			}
			else if (!string.IsNullOrEmpty(view.ViewGroupId))
			{
				string rootViewGroupId = ServerGroup.GetRootViewGroupId(requestContext, view.ViewGroupId);
				requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.ViewGroup, "OPERATE", rootViewGroupId);
			}
		}
		catch (SecurityObjectDoesNotExistException)
		{
			throw new ViewNotFoundException();
		}
		string viewXml = null;
		if (smartClientId != null)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(view.DefinitionXml);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("/view");
			if (!xmlNode.HasAttribute<string>("smartClientId", out var _))
			{
				xmlNode.Attributes.Append(xmlDocument.CreateAttribute("smartClientId"));
			}
			xmlNode.Attributes["smartClientId"].Value = smartClientId;
			viewXml = xmlDocument.OuterXml;
		}
		using (VideoWallFunctionalityComponent videoWallFunctionalityComponent = requestContext.CreateComponent<VideoWallFunctionalityComponent>())
		{
			videoWallFunctionalityComponent.ClearView(viewId, sessionId, viewXml, smartClientId);
		}
		View view2;
		using (ViewComponent viewComponent = requestContext.CreateComponent<ViewComponent>())
		{
			view2 = FindView(requestContext, viewId);
			if (viewComponent.IsMonitorView(viewId, out var videoWallId))
			{
				Integration.FireMonitorViewChanged(requestContext, viewId, view2.MonitorId, videoWallId, EventAction.Updated);
			}
			else
			{
				string parentIdFromId = GetParentIdFromId(requestContext, viewId);
				Integration.FireViewChanged(requestContext, viewId, parentIdFromId, EventAction.Updated);
			}
		}
		ViewGroupCache.Clear();
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return view2;
	}

	public static ViewItem[] QueryViewItemHeriarchyByServerGroupIdNoSecurity(IGeminiRequestContext requestContext, string serverGroupId)
	{
		using IViewComponent viewComponent = requestContext.CreateViewComponent();
		return viewComponent.QueryViewItemHierarchyByServerGroupId(serverGroupId);
	}

	public static ViewItem[] QueryViewItemsNoSecurity(IGeminiRequestContext requestContext, string viewId)
	{
		ViewItem[] array = new ViewItem[0];
		using IViewComponent viewComponent = requestContext.CreateViewComponent();
		string[] viewItemIds = viewComponent.QueryViewItemIds(viewId);
		return viewComponent.QueryViewItemsByIds(viewItemIds);
	}

	public static ViewItem[] QueryViewItems(IGeminiRequestContext requestContext, string viewId)
	{
		ViewItem[] result = new ViewItem[0];
		View view = QueryView(requestContext, viewId);
		if (view != null)
		{
			if (!string.IsNullOrEmpty(view.VideoWallId))
			{
				if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "GENERIC_READ", view.VideoWallId))
				{
					return result;
				}
			}
			else
			{
				if (string.IsNullOrEmpty(view.ViewGroupId))
				{
					return result;
				}
				string rootViewGroupId = ServerGroup.GetRootViewGroupId(requestContext, view.ViewGroupId);
				if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.ViewGroup, "GENERIC_READ", rootViewGroupId))
				{
					return result;
				}
			}
			return QueryViewItemsNoSecurity(requestContext, viewId);
		}
		return result;
	}

	public static ViewItem QueryViewItem(IGeminiRequestContext requestContext, string viewItemId)
	{
		using IViewComponent viewComponent = requestContext.CreateViewComponent();
		return viewComponent.QueryViewItem(viewItemId);
	}

	private static View FindView(IGeminiRequestContext requestContext, string viewId)
	{
		using IViewComponent viewComponent = requestContext.CreateViewComponent();
		return viewComponent.QueryView(viewId);
	}

	private static View[] FilterViewsBasedOnPermissions(IGeminiRequestContext requestContext, View[] viewArray)
	{
		List<View> list = new List<View>();
		foreach (View view in viewArray)
		{
			if (!string.IsNullOrEmpty(view.VideoWallId))
			{
				if (requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "GENERIC_READ", view.VideoWallId))
				{
					list.Add(view);
				}
			}
			else if (!string.IsNullOrEmpty(view.ViewGroupId) && requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.ViewGroup, "GENERIC_READ", ServerGroup.GetRootViewGroupId(requestContext, view.ViewGroupId)))
			{
				list.Add(view);
			}
		}
		return list.ToArray();
	}
}
