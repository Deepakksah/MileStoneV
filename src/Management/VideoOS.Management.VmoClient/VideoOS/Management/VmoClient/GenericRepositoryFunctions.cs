using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Management.VmoClient;

public class GenericRepositoryFunctions
{
	public static void ApplyLayout(IVmoRepositories repositories, Guid layoutId, Monitor monitor)
	{
		VMOWebserviceObject vMOWebserviceObject = repositories.ServerCommunication.ApplyLayout(monitor.Id.ToString(), layoutId.ToString());
		monitor.ObjectProperties = vMOWebserviceObject.Properties;
	}

	public static void ApplyCamerasToMonitorView(IVmoRepositories repositories, Monitor monitor, Guid[] cameraIds, Guid[] cameraGroupIds, int fromPosition)
	{
		VMOWebserviceObject vMOWebserviceObject = repositories.ServerCommunication.ApplyCamerasToMonitorView(monitor.Id.ToString(), cameraIds.Select((Guid i) => i.ToString()).ToArray(), cameraGroupIds.Select((Guid i) => i.ToString()).ToArray(), fromPosition);
		monitor.ObjectProperties = vMOWebserviceObject.Properties;
	}

	public static void ApplyCamerasAndLayoutToMonitorView(IVmoRepositories repositories, Monitor monitor, Guid[] cameraIds, Guid[] cameraGroupIds, int fromPosition, Guid layoutId)
	{
		VMOWebserviceObject vMOWebserviceObject = repositories.ServerCommunication.ApplyCamerasAndLayoutToMonitorView(monitor.Id.ToString(), cameraIds.Select((Guid i) => i.ToString()).ToArray(), cameraGroupIds.Select((Guid i) => i.ToString()).ToArray(), fromPosition, layoutId.ToString());
		monitor.ObjectProperties = vMOWebserviceObject.Properties;
	}

	public static void ApplyViewItemsXmlToMonitorView(IVmoRepositories repositories, Monitor monitor, string[] viewItemsXml, int fromPosition)
	{
		VMOWebserviceObject vMOWebserviceObject = repositories.ServerCommunication.ApplyViewItemXmlsToMonitorView(monitor.Id.ToString(), viewItemsXml, fromPosition);
		monitor.ObjectProperties = vMOWebserviceObject.Properties;
	}

	public static void ApplyMonitorStateToMonitor(IVmoRepositories repositories, Monitor monitor, string xmlState)
	{
		repositories.ServerCommunication.ApplyMonitorStateToMonitor(monitor.Id.ToString(), xmlState);
	}

	public static void ApplyTextAndLayoutToMonitorView(IVmoRepositories repositories, Monitor monitor, string message, int viewPosition, Guid layoutId)
	{
		VMOWebserviceObject vMOWebserviceObject = repositories.ServerCommunication.ApplyTextAndLayoutToMonitorView(monitor.Id.ToString(), message, viewPosition, layoutId.ToString());
		monitor.ObjectProperties = vMOWebserviceObject.Properties;
	}

	public static void RemoveCamerasFromMonitorView(IVmoRepositories repositories, Monitor monitor, Guid[] cameraIds, Guid[] cameraGroupIds)
	{
		repositories.ServerCommunication.RemoveCamerasFromMonitorView(monitor.Id.ToString(), cameraIds.Select((Guid i) => i.ToString()).ToArray(), cameraGroupIds.Select((Guid i) => i.ToString()).ToArray());
	}

	public static void ClearViewItem(IVmoRepositories repositories, ViewItem viewItem)
	{
		VMOWebserviceObject vMOWebserviceObject = repositories.ServerCommunication.ClearViewItem(viewItem.Id.ToString());
		viewItem.ObjectProperties = vMOWebserviceObject.Properties;
	}

	public static void ClearView(IVmoRepositories repositories, View view)
	{
		VMOWebserviceObject vMOWebserviceObject = repositories.ServerCommunication.ClearView(view.Id.ToString());
		view.ObjectProperties = vMOWebserviceObject.Properties;
	}

	public static void ApplyViewToView(IVmoRepositories repositories, Guid sourceViewId, View view)
	{
		VMOWebserviceObject vMOWebserviceObject = repositories.ServerCommunication.ApplyViewToView(sourceViewId.ToString(), view.Id.ToString());
		view.ObjectProperties = vMOWebserviceObject.Properties;
	}

	public static void CreateView(IVmoRepositories repositories, View view)
	{
		List<VmoPropertyCollection> list = new List<VmoPropertyCollection>();
		List<CustomPropertyCollection> list2 = new List<CustomPropertyCollection>();
		if (view.CustomViewItem != null)
		{
			CustomViewItem[] customViewItem = view.CustomViewItem;
			foreach (CustomViewItem customViewItem2 in customViewItem)
			{
				list.Add(customViewItem2.ObjectProperties);
				list2.Add(customViewItem2.CustomProperties);
			}
		}
		VMOWebserviceObject vMOWebserviceObject = repositories.ServerCommunication.CreateView(view.ViewGroupId, view.ObjectProperties, view.GetCustomProperties(), list.ToArray(), list2.ToArray());
		view.ObjectProperties = vMOWebserviceObject.Properties;
		view.Relations = vMOWebserviceObject.Relations;
		view.InternalId = vMOWebserviceObject.Id;
		repositories.GetGenericRepository<View>().AddToCache(view);
	}

	public static void ChangeViewItemId(IVmoRepositories repositories, ViewItem viewItem, string newId, string viewId)
	{
		repositories.ServerCommunication.ChangeViewItemId(viewItem.InternalId, newId, viewId);
		repositories.GetGenericRepository<ViewItem>().RemoveFromCache(viewItem.InternalId);
		viewItem.InternalId = newId;
		repositories.GetGenericRepository<ViewItem>().AddToCache(viewItem);
	}

	public static ChildSite AttachChildSite(IVmoRepositories repositories, Guid childSiteId, AddressCollection addresses, string childSiteVersion)
	{
		VMOWebserviceObject vMOWebserviceObject = repositories.ServerCommunication.AttachChildSite(childSiteId.ToString(), addresses, childSiteVersion);
		ChildSite childSite = new ChildSite(vMOWebserviceObject.Id, vMOWebserviceObject.Properties, vMOWebserviceObject.Relations, repositories, isInitialized: false);
		repositories.GetGenericRepository<Site>().AddToCache(childSite);
		return childSite;
	}

	public static void DetachChildSite(IVmoRepositories repositories, ChildSite childSite)
	{
		repositories.ServerCommunication.DetachSite(childSite.InternalId);
		repositories.GetGenericRepository<Site>().RemoveFromCache(childSite.InternalId);
	}

	public static MasterSite AttachMasterSite(IVmoRepositories repositories, Guid masterSiteId, AddressCollection addresses, string masterSiteVersion)
	{
		VMOWebserviceObject vMOWebserviceObject = repositories.ServerCommunication.AttachMasterSite(masterSiteId.ToString(), addresses, masterSiteVersion);
		MasterSite masterSite = new MasterSite(vMOWebserviceObject.Id, vMOWebserviceObject.Properties, vMOWebserviceObject.Relations, repositories, isInitialized: false);
		repositories.GetGenericRepository<Site>().AddToCache(masterSite);
		return masterSite;
	}

	public static void AuthorizeMasterSite(IVmoRepositories repositories, MasterSite masterSite)
	{
		VMOWebserviceObject webObj = repositories.ServerCommunication.AuthorizeMasterSite();
		repositories.GetGenericRepository<Site>().RefreshObjectInternal(masterSite, webObj);
	}

	public static SystemMonitorStateSource[] QuerySystemsMonitorStateSourcesByType(IVmoRepositories repositories, string[] types)
	{
		if (types == null || types.Length == 0)
		{
			throw new ArgumentException("The types array is either null or empty", "types");
		}
		return repositories.ServerCommunication.QuerySystemMonitorStateSourcesByTypes(types)?.Select((VMOWebserviceObject g) => new SystemMonitorStateSource(g.Id, g.Properties, g.Relations, repositories, isInitialized: true)).ToArray();
	}

	public static SystemMonitorStateSource[] QuerySystemsMonitorStateSourcesByIds(IVmoRepositories repositories, string[] ids)
	{
		if (ids == null || ids.Length == 0)
		{
			throw new ArgumentException("the ids array is either null or empty", "ids");
		}
		return repositories.ServerCommunication.QuerySystemMonitorStateSourcesByIds(ids)?.Select((VMOWebserviceObject g) => new SystemMonitorStateSource(g.Id, g.Properties, g.Relations, repositories, isInitialized: true)).ToArray();
	}

	public static RuleSource[] QueryAllRuleSources(IVmoRepositories repositories)
	{
		return repositories.ServerCommunication.QueryAllRuleSources()?.Select((VMOWebserviceObject g) => new RuleSource(g.Id, g.Properties, g.Relations, repositories, isInitialized: true)).ToArray();
	}

	public static Role CopyRole(Role sourceRole, string name, string description)
	{
		VMOWebserviceObject vMOWebserviceObject = sourceRole.Repositories.ServerCommunication.CopyRole(sourceRole.InternalId, name, description);
		Role role = new Role(vMOWebserviceObject.Id, vMOWebserviceObject.Properties, vMOWebserviceObject.Relations, sourceRole.Repositories, isInitialized: false);
		sourceRole.Repositories.GetGenericRepository<Role>().AddToCache(role);
		return role;
	}
}
