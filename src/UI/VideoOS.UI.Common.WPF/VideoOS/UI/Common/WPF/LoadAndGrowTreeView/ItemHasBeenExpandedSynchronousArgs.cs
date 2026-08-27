using System;
using System.Collections.Generic;

namespace VideoOS.UI.Common.WPF.LoadAndGrowTreeView;

public class ItemHasBeenExpandedSynchronousArgs : EventArgs
{
	private TreeViewItemData _data;

	private List<TreeViewItemData> _childItemDatas = new List<TreeViewItemData>();

	public TreeViewItemData Data
	{
		get
		{
			return _data;
		}
		set
		{
			_data = value;
		}
	}

	public List<TreeViewItemData> ChildItemDatas
	{
		get
		{
			return _childItemDatas;
		}
		set
		{
			_childItemDatas = value;
		}
	}

	public ItemHasBeenExpandedSynchronousArgs(TreeViewItemData data)
	{
		_data = data;
	}
}
