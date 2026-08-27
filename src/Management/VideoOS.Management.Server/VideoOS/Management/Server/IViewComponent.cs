using System;

namespace VideoOS.Management.Server;

public interface IViewComponent : IDisposable
{
	void CreateView(string parentId, View view);

	string[] QueryViewIds(string viewGroupId);

	View[] QueryViewsByIds(string[] viewIds);

	View[] QueryViews(string parentId);

	View[] QueryViewHierarchyByServerGroupId(string serverGroupId);

	View[] QueryViewAllNoSecurity();

	View QueryView(string viewId);

	void UpdateView(View view);

	void DeleteView(string viewId);

	void CreateViewItem(string viewId, ViewItem viewItem);

	string[] QueryViewItemIds(string viewId);

	ViewItem QueryViewItem(string viewItemId);

	ViewItem[] QueryViewItemsByIds(string[] viewItemIds);

	ViewItem[] QueryViewItemHierarchyByServerGroupId(string serverGroupId);

	ViewItem[] QueryViewItemsAll();

	void UpdateViewItem(ViewItem viewItem);

	void ApplyViewToView(string sourceViewId, string destinationViewId, Guid sessionId, string clientId);

	void ChangeViewItemId(string existingId, string newId);

	string GetIdFromItem(string viewItemId);

	bool IsMonitorView(string viewId, out string videoWallId);
}
