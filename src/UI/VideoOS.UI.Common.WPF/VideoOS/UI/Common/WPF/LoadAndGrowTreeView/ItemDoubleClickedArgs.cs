using System;

namespace VideoOS.UI.Common.WPF.LoadAndGrowTreeView;

public class ItemDoubleClickedArgs : EventArgs
{
	private TreeViewItemData _item;

	public TreeViewItemData Item
	{
		get
		{
			return _item;
		}
		set
		{
			_item = value;
		}
	}

	public ItemDoubleClickedArgs(TreeViewItemData item)
	{
		_item = item;
	}
}
