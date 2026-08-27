using System;

namespace VideoOS.UI.Common.WPF.LoadAndGrowTreeView;

public class PreviewItemCollapseArgs : EventArgs
{
	private TreeViewItemData _collapsedItem;

	public TreeViewItemData CollapsedItem
	{
		get
		{
			return _collapsedItem;
		}
		set
		{
			_collapsedItem = value;
		}
	}

	public PreviewItemCollapseArgs(TreeViewItemData collapsedItem)
	{
		_collapsedItem = collapsedItem;
	}
}
