using System;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.Portable.ViewGroupItem;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.ViewGroups;

public static class ViewGroupsHelper
{
	public static bool SupportsAudio;

	private static void GetViewTree(IViewsSync views, ViewGroupTree tree, ViewParams viewParams, TimeSpan timeout)
	{
		viewParams.ViewId = tree.ItemId;
		GetSubItemsResponse views2 = views.GetViews(viewParams, timeout);
		if (views2?.SubItems == null)
		{
			return;
		}
		foreach (ViewItem viewItem in views2.SubItems.ViewItems)
		{
			Guid camID = ((viewItem.TypeItem == ViewItem.ItemType.Camera) ? viewItem.ID : Guid.Empty);
			ViewGroupTree viewGroupTree = new ViewGroupTree((ViewItemType)viewItem.TypeItem, viewItem.Name, viewItem.ID, camID, viewItem.Timeout);
			if (!IsItemLeaf((ViewItemType)viewItem.TypeItem))
			{
				GetViewTree(views, viewGroupTree, viewParams, timeout);
			}
			tree.AddSubItem(viewGroupTree);
		}
	}

	private static bool IsItemLeaf(ViewItemType viewItemType)
	{
		switch (viewItemType)
		{
		case ViewItemType.Microphone:
		case ViewItemType.Speaker:
		case ViewItemType.Metadata:
			return true;
		case ViewItemType.Camera:
			if (!SupportsAudio)
			{
				return true;
			}
			break;
		}
		return false;
	}

	public static ViewGroupTree GetPublicViews(IViewsSync views, ViewParams viewParams, TimeSpan timeout)
	{
		ViewGroupTree viewGroupTree = new ViewGroupTree(ViewItemType.Group, ViewGroupTree.PublicViewsName, ViewGroupTree.PublicViewsGuid, Guid.Empty);
		GetViewTree(views, viewGroupTree, viewParams, timeout);
		return viewGroupTree;
	}

	public static ViewGroupTree GetPrivateViews(IViewsSync views, ViewParams viewParams, TimeSpan timeout)
	{
		ViewGroupTree viewGroupTree = new ViewGroupTree(ViewItemType.Group, ViewGroupTree.PrivateViewsName, ViewGroupTree.PrivateViewsGuid, Guid.Empty);
		GetViewTree(views, viewGroupTree, viewParams, timeout);
		return viewGroupTree;
	}

	public static ViewGroupTree GetAllCamerasView(IViewsSync views, ViewParams viewParams, TimeSpan timeout)
	{
		ViewGroupTree viewGroupTree = new ViewGroupTree(ViewItemType.Group, ViewGroupTree.AllCamerasViewName, ViewGroupTree.AllCamerasFolderGuid, Guid.Empty);
		GetViewTree(views, viewGroupTree, viewParams, timeout);
		return viewGroupTree;
	}

	public static ViewGroupTree GetAllViewsAndCameras(IViewsSync views, ViewParams viewParams, TimeSpan timeout)
	{
		ViewGroupTree allSubItems = views.GetAllViewsAndCameras(viewParams, timeout).AllSubItems;
		if (!(allSubItems?.ItemId != ViewGroupTree.AllViewsAndCamerasGuid))
		{
			return allSubItems;
		}
		return null;
	}
}
